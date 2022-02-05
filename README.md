# Saturn-Fable-Online
Unity3D MMO project snippets

Demonstration of MMO spell and talent system. Basic architecture is as follows:

A spellHit is created when a spell is cast. It is then populated with damage, range, crit chance, etc. based on:
 * attacker's talents, gear, status effects
 * defender's talents, gear, status effects

It does this by calling all the virtual functions for each talent, gear, and status effect on caster and defender. These all inherit from SpellMod. The complete list of available overrides for SpellMods (as talents, gear, status effects):

* on calculate hit chance
* on calculate (flat bonuses)
* on calculate (percentage bonuses)
* on calculate spell type (fire,water, holy, etc)
* on hit
* on crit
* on damage
* on heal
* on kill
* on not hit
* on parry
* on dodge
* on block
* on on use charge
* on gain charge
* on summon
* on dispel
* on status effect expire
* on consume status effect
 
 Just define a SpellMod in the form of a talent, spell, or piece of gear and override one of these "on" functions to have it called at appropriate time, with the spellHit passed as an argument so you can make modifications to it.
 
 Examples:
 * a talent that procs an ability when the attacker crits
 * a status effect that makes all hostile spells cast on the defender take longer to cast
 * a talent that restores energy when you cast a fire spell

Possible values to modify:
  * power
  * cost
  * cast time
  * hit chance
  * crit chance
  * crit damage bonus
  * dodge chance
  * block chance
  * evade chance
  * parry chance
  * duration
  * interval
  * range
  * radius
  * cooldown
  * number of stacks
  * max number of stacks
  * whether to inturrupt spell on movement (boolean)
  * spell type
  * projectile speed
  * number of mosters to summon
  * number of strikes
  * leech health percent
  * reflect damage
  * charge gained
  * charge used

  reusable TalentMod which increases damage vs immobilized targe
        
        public class IncreaseDamage_Percent_VsImmobilized : TalentMod
        {
        
          private IncreaseDamage_Percent_VsImmobilized() : base(default) { } //private to hide argless constructor

          readonly float amount;
          readonly float baseAmount;
          readonly float amountPerLevel;
          
          //constructor. This class will automatically be added to a list of spells and assigned a spell ID
          //baseAmount is how much to add on point 1
          //amountPerLevel is how much to add for each additional point
          public IncreaseDamage_Percent_VsImmobilized(float baseAmount, float amountPerLevel, int currentLevel = -1) : base(currentLevel)
          {
              this.baseAmount = baseAmount;
              this.amountPerLevel = amountPerLevel;
              this.amount = 1 + (baseAmount + (amountPerLevel * (currentLevel - 1)));
          }
          
          //called when calculating offensive ability's stats on caster after hit along with all over percentage-based modifiers
          //have to make distinction betweeen flat and percentage so that all flat are called first and percentage afterwards
          
          public override void CalcCastPerOffensive_PostHit(spellHit spellHit)
          {
              //if the target exists, the spell being cast is not a heal, and the target is immobilized
              if(spellHit.target != null && !spellHit.isHeal && spellHit.target.GetOrDefault<bool>(SyncSpeed.immobilized))
                  spellHit.power = (int)(spellHit.power * amount); //modify damage
          }

          //used to create one instance of this class for each talent level and keep them in memory
          public override TalentMod Constructor(TalentMod self, int currentLevel)
          {
              var casted = (IncreaseDamage_Percent_VsImmobilized)self;
              return new IncreaseDamage_Percent_VsImmobilized(casted.baseAmount, casted.amountPerLevel, currentLevel);
          }
          
        } //end class IncreaseDamage_Percent
        
        
        
   now to define a talent that uses this TalentMod
   
        public class T_WarriorIncreasedDamagevsImmobilized : GridTalentBlack
        {

          public override string nameFormatted => "Draconian Measures";
          public override string description => "increased damage vs immobilized targets";
          public override string iconName => nameFormatted;


          const float increasePerLevel = 0.1f; //how much bonus damage to deal
          
          public T_WarriorIncreasedDamagevsImmobilized() : base(3, null) //this talent has 3 possible points for 10% extra damage each
          {

          }
          
          //populate what this talent does, called when application starts
          protected override void SetDefaults(out List<TalentMod> spellMods, out List<StatMod> permanentStatMods, out List<EntityMod> entityMods, out TeamMods teamMods)
          {

              //list of modifiers to spells, (hit chance, crit chance, etc)
              spellMods = new List<TalentMod>()
              {
                  //use the TalentMod we created here. Can easily add other effects here as well
                  
                  new OnCalcOffensive_BaseTalentMods.IncreaseDamage_Percent_VsImmobilized(increasePerLevel, increasePerLevel)
              };

              //list of modifiers to stats (max health, max energy, etc)
              //will safely recalculate the stat value after added to prevent add/remove errors
              permanentStatMods = new List<StatMod>()
              {
              };

              //modifiers to be added to talent owner (chance on hit, chance on crit, chance on dodge, etc)
              entityMods = new List<EntityMod>()
              {
                //there is a class called ChanceMod which defines casting spells from a chance on hit, dodge, crit, etc
              };

              //modifiers to be applied to entire team
              teamMods = new TeamMods(
                  new List<SpellModEntry>() { }, //faction SpellMods
                  new Dictionary<int, List<SpellModEntry>>() { }, //faction SpellMods for entity typeID
                  new List<StatModEntry>(), //faction StatMods
                  new Dictionary<int, List<StatModEntry>>() { } //faction StatMods for entity typeID
              );
          }

          //calculate text description based on current talent level
          public override string GetLevelDescription(int currentLevel)
          {
              //use percentage-based description
              return CalcDescriptionPer(ref currentLevel, increasePerLevel) + " increased damage";
          }

        } //end talent T_WarriorIncreasedDamagevsImmobilized

The reason why this works is because C# is extremely well optimized for empty functions. Therefore, it doesn't take a lot of performance to loop over every possible modifier to the spellHit on both attacker and defender. It also allows you to recycle code between talents, spells, and gear.
