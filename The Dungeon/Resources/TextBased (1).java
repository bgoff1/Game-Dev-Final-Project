/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package textbased;

import static java.lang.Integer.parseInt;
import static java.lang.Math.max;
import static java.lang.Math.pow;
import static java.lang.Math.round;
import java.util.Random;
import java.util.Scanner;

/**
 *
 * @author Caleb
 */
public class TextBased {

    /**
     * @param args the command line arguments
     */
    
    //<editor-fold desc="Variables">

        //<editor-fold desc="SystemObjects">
        static Scanner in = new Scanner(System.in);
        static Random rand = new Random();
        //</editor-fold>

        //<editor-fold desc="GameVariables">
        static int monsterLevels = 0;
        static int monstersDefeated = 0;
        static int totalGoldCollected= 0;
        
        static int itemInShop = 0;

        static String[] enemies = { "Skeleton", "Zombie", "Goblin", "Orc", "Spectre" };
        static String[] bosses = { "Giant", "Ogre", "Vampire" };
        static int maxEnemyHealth = 50;
        static int enemyAttackDamage = 25;
        static int maxEnemyExperience = 100;
        static boolean enemyIsBoss = false;
        
        static int experienceNeeded;
        
        //options: 1 - weapon, 2-armor, 3-cloak, 4-dagger, 5-Spellbook, 6-bow, 7-shield, 8-druidic amulet
        static int[] possibleRogueOptions = {1, 2, 3, 4, 8};
        static int[] possibleWarriorOptions = {1, 2, 3, 7, 8};
        static int[] possibleWizardOptions = {1, 2, 3, 5, 8};
        static int[] possibleArcherOptions = {1, 2, 3, 6, 8};
        static int[] possibleDruidOptions = {1, 2, 3, 8, 9};
        //</editor-fold>

        //<editor-fold desc="Player variables">
        static int maxHealthPotions =5;
        static int maxHealth=100;
        static int currentHealth=100;
        static int attackDamage = 50;
        static int numHealthPotions = 3;
        static int playerGold = 50;
        static int escapeRopes = 0;
        static int critChance = 5; //percent
        static boolean magicWeaponOwned = false; //+10% damage
        static boolean magicArmorOwned = false; //-10% damage taken
        static boolean magicCloakOwned = false; //+10% dodge chance
        static boolean magicDaggerOwned = false; //rogue only - 20% chance for extra 50% attack
        static boolean magicSpellbookOwned = false; //wizard only - 10% chance for random spell to be cast
        static boolean magicBowOwned = false; //archer only - 10% chance for headshot (2x damage)
        static boolean magicShieldOwned = false; //warrior only - +10% block, reflects 20% of all incoming damage
        static boolean magicPendantOwned = false; //+5% crit chance
        static boolean magicStaffOwned = false; //druid only - gets to choose 2 aspects
        
        //</editor-fold>

        //<editor-fold desc="DifficultyVariables">
        static int healthPotionHealAmount = 30;
        static int healthPotionDropChance = 35; //Percentage
        static int attackMedium = 7;
        static int healthMedium = 20;
        static int potionGainStandard = 10;
        static int enemyDifficultyStandard = 10;
        static int experienceNeededMultiplier = 300;
        //</editor-fold>

        //<editor-fold desc="LevelingVariables">
        static int level=1;
        static int currentExperience = 0;

        static int playerClass = 0;
        static int dodgeBlockChance = 0;
        //</editor-fold>
        //<editor-fold desc="SpellVariables">
        static int maxSpellSlots = 0;
        static int currentSpellSlots = 0;
        //</editor-fold>

        static boolean running = true;
        //</editor-fold>
        
    public static void chooseDifficulty() {
        boolean difficultyChosen = false;
        while(!difficultyChosen) {
        System.out.println("Select your difficulty level." +
                            "\n\t1. Sandbox (If you die, you just weren't paying attention)" +
                            "\n\t2. Easy (Pay attention to your HP and drink a potion every once in a while and you'll be fine)" +
                            "\n\t3. Medium (This is where the challenge begins. Smart spell usage and good rolls go a long way)" +
                            "\n\t4. Hard (You will constantly have no potions and no spell slots)" +
                            "\n\t5. Nightmare (Good luck!)");
        //<editor-fold desc="Difficulty">
        String inputDifficulty = in.nextLine();
        switch (inputDifficulty) {
            case "1":
                attackMedium+=8;
                healthMedium+=10;
                potionGainStandard+=10;
                enemyDifficultyStandard-=5;
                healthPotionHealAmount=50;
                healthPotionDropChance=45;
                experienceNeededMultiplier=250;
                difficultyChosen=true;
                break;
            case "2":
                attackMedium+=3;
                healthMedium+=5;
                potionGainStandard+=5;
                enemyDifficultyStandard-=3;
                healthPotionHealAmount=40;
                healthPotionDropChance=40;
                experienceNeededMultiplier=275;
                difficultyChosen=true;
                break;
            case "3":
                difficultyChosen=true;
                break;
            case "4":
                attackMedium -= 1;
                healthMedium-=5;
                potionGainStandard-=3;
                enemyDifficultyStandard+=3;
                healthPotionHealAmount=25;
                healthPotionDropChance=30;
                experienceNeededMultiplier=325;
                difficultyChosen=true;
                break;
            case "5":
                attackMedium -= 2;
                healthMedium -=7;
                potionGainStandard -=5;
                enemyDifficultyStandard+=5;
                healthPotionHealAmount=20;
                healthPotionDropChance=25;
                experienceNeededMultiplier=350;
                difficultyChosen=true;
                break;
            default:
                System.out.println("\tInvalid command!");
                break;
        }

        }
        //</editor-fold>
        
    }
    
    
    public static int fight(int enemyLevel, int enemyHealth, String enemy, int tempDodgeBlockChance) {
        boolean mageArmorOn = false;
        boolean enemyBurning = false;
        boolean rejuvinating = false;
        boolean huntersMarkOn = false;
        int aspect=0;
        
        int roundOfBattle = 1;
         //<editor-fold desc="ChoosingAspect">
        if(playerClass==5)
        {
            boolean fightAspectChosen = false;
            while(!fightAspectChosen) {
                System.out.println("# What aspect would you like to enter for this fight? #");
                if(magicStaffOwned) {
                    System.out.println("# 4. Lion-Bear form (take less damage, deal massive amounts of damage) #");
                    System.out.println("# 5. Owl-Lion form (massively increased damage, more potent spellcasting) #");
                    System.out.println("# 6. Owl-Bear form (more potent spellcasting, take less damage, deal slightly more damage) #");
                }
                else{
                    System.out.println("# 1. Bear form (take less damage, deal slightly more damage) #");
                    System.out.println("# 2. Lion form (massively increased damage) #");
                    System.out.println("# 3. Owl form (more potent spellcasting) #");
                }
                
                
                String aspectInput = in.nextLine();
                if(null == aspectInput){
                    System.out.println("\tInvalid command!");
                }
                else switch (aspectInput) {
                    case "1":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    case "2":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    case "3":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    case "4":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    case "5":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    case "6":
                        fightAspectChosen = true;
                        aspect = parseInt(aspectInput);
                        break;
                    default:
                        System.out.println("\tInvalid command!");
                        break;
                }
            
            }
        }
        //</editor-fold>
        
        FIGHT:
        while (enemyHealth > 0) {
            //<editor-fold desc="BurningEnemy">
            if(enemyBurning)
            {
                int damageDealt = (rand.nextInt(attackDamage)+1)/4;
                enemyHealth -= damageDealt;
                if(enemyHealth > 0) {
                    System.out.println("\t>The " + enemy + " is taking damage every round!" +
                            "\n\t>It takes " + damageDealt + " damage, and has " + enemyHealth + " HP remaining.");
                }
                else {
                    break;
                }
            }
            //</editor-fold>
            
            //<editor-fold desc="Rejuvination">
            if(rejuvinating) {
                int damageHealed = healthPotionHealAmount/4;
                if(aspect==3||aspect==5||aspect==6) {
                    damageHealed *= 1.25;
                }
                currentHealth += damageHealed;
                if(currentHealth > maxHealth) {
                    currentHealth = maxHealth;
                }
                System.out.println("\t>You are rejuvinating, and heal for " + damageHealed + " damage, up to a max of " + maxHealth + ".");
            }
            //</editor-fold>
            //<editor-fold desc="Menu">
            System.out.println("\tRound: " + roundOfBattle);
            System.out.println("\tYour HP: " + currentHealth);
            System.out.println("\t" + enemy + "'s HP: " + enemyHealth);
            System.out.println("\n\tWhat would you like to do?");
            System.out.println("\t1. Attack");
            System.out.println("\t2. Drink health potion");
            if(level >= 3) {
                System.out.println("\t3. Cast a spell");
            }
            if(escapeRopes > 0) {
                System.out.println("\t4. Use escape rope to run away");
            }
            if("Treasure Goblin".equals(enemy)) {
                System.out.println("\tThe Treasure Goblin will not fight you, but you have " + (11-roundOfBattle) + " rounds to kill it before it gets away!");
            }
            //</editor-fold>

            //<editor-fold desc="FightOptions">
            String input = in.nextLine();
            switch (input) {
                case "1":
                    //<editor-fold desc="Attack">
                    int damageDealt = (int)round(rand.nextGaussian()*(attackDamage/6)+(attackDamage/2));
                    int damageTaken = (int)round(rand.nextGaussian()*(enemyAttackDamage/6)+(enemyAttackDamage/2));
                    int moreDamageDealt = 0;
                    
                    //<editor-fold desc="AspectDamagingFactor">
                    if(aspect == 1||aspect==4||aspect==6) {
                        damageDealt *= 1.15;
                        damageTaken *= 0.85;
                    }
                    else if(aspect == 2||aspect==4||aspect==5) {
                        damageDealt *= 1.3;
                    }
                    //</editor-fold>
                    //<editor-fold desc="CheckHuntersMark">
                    if(damageDealt < (int)(1.5*(attackDamage/2))&&huntersMarkOn) {
                        damageDealt = (int)1.5*(attackDamage/2);
                    }
                    //</editor-fold>
                    //<editor-fold desc="MageArmorOn">
                    if(mageArmorOn) {
                        damageTaken /= 2;
                    }
                    //</editor-fold>
                    //<editor-fold desc="MagicItemsOwned">
                    if(magicWeaponOwned) {
                        damageDealt *= 1.1;
                    }
                    if(magicArmorOwned) {
                        damageTaken *= .9;
                    }

                    //</editor-fold>
                    //<editor-fold desc="Dodge">
                    if((rand.nextInt(100)<tempDodgeBlockChance)||(playerClass==4&&roundOfBattle==1)||enemy == "Treasure Goblin") {
                        damageTaken=0;
                    }
                    //</editor-fold>
                    //<editor-fold desc="BossIncreasedDamage">
                    if(enemyIsBoss)
                    {
                        if(enemy == "Giant")
                        {
                            damageTaken *= 1.25;
                        }
                        else if (enemy == "Ogre")
                        {
                            damageTaken *= 1.5;
                        }
                    }
                    //</editor-fold>
                    //<editor-fold desc="MagicBow">
                    if(magicBowOwned && (rand.nextInt(100)<10)) {
                        damageDealt*=2;
                        System.out.println("\t> Headshot!");
                    }
                    //</editor-fold>
                    if(rand.nextInt(100) < critChance) {
                        damageDealt *= 2;
                        System.out.println("\t> You CRITICALLY strike the " + enemy + " for " + damageDealt + " damage.");
                    }
                    else {
                        System.out.println("\t> You strike the " + enemy + " for " + damageDealt + " damage.");
                    }
                    //<editor-fold desc="MagicDagger">
                    if(magicDaggerOwned && (rand.nextInt(100)<20)) {
                        moreDamageDealt = rand.nextInt(attackDamage/2)+1;
                        System.out.println("\t> Your magical dagger whips by, also slicing the " + enemy + " for " + moreDamageDealt + " damage.");
                    }
                    //</editor-fold>
                    //<editor-fold desc="DodgeBlockTreasureGoblinOutput">
                    if(damageTaken==0) {
                        if(playerClass==4&&roundOfBattle==1)
                        {
                            System.out.println("\t> You fire the first shot, taking no damage in retaliation!");
                        }
                        else if(magicShieldOwned) {
                            moreDamageDealt = rand.nextInt(attackDamage/4)+1;
                            System.out.println("\t> You blocked, and your shield instead deals " + moreDamageDealt + " damage!");
                        }
                        else if(enemy == "Treasure Goblin") {}
                        else {
                            System.out.println("\t> You " + (playerClass == 1 ? "block" : "dodge") + ", taking no damage in retaliation!");
                        }
                    }
                    
                    //</editor-fold>
                    //<editor-fold desc="DamageTakenOutput">
                    else {
                        System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                        if(magicShieldOwned) {
                            moreDamageDealt = damageTaken/5;
                            System.out.println("\t> Your shield deals back " + moreDamageDealt + " of the damage you took!");
                        }
                        if(enemyIsBoss && enemy == "Vampire")
                        {
                            System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                            enemyHealth += damageTaken/2;
                        }
                    }
                    //</editor-fold>

                    enemyHealth -= (damageDealt+moreDamageDealt);
                    currentHealth -= damageTaken;

                    //<editor-fold desc="PlayerDeath">
                    if (currentHealth < 1) {
                        System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                        break FIGHT;
                    }
                    //</editor-fold>
                    roundOfBattle++;
                    break;
                    //</editor-fold>
                case "2":
                    //<editor-fold desc="Potion">
                    if (numHealthPotions > 0) {
                        if(currentHealth == maxHealth)
                        {
                            System.out.println("\tYou are at full health! Your potion will do nothing.");
                        }
                        else {
                            currentHealth += healthPotionHealAmount;
                            if (currentHealth > maxHealth) {
                                currentHealth = maxHealth;
                            }
                            numHealthPotions--;
                            System.out.println("\t> You drink a health potion, healing yourself for " + healthPotionHealAmount + "."
                                    + "\n\t> You now have " + currentHealth + " HP."
                                    + "\n\t> You have " + numHealthPotions + " health potions left.");
                        }

                    } else {
                        System.out.println("\t> You have no health potions left! Defeat enemies for a chance to get a health potion!");
                    }
                    break;
                    //</editor-fold>
                case "3":
                    //<editor-fold desc="Spell">
                    if(level <3) {
                        System.out.println("\t You don't know any spells yet!");
                        continue;
                    }
                    else if(currentSpellSlots==0) {
                        System.out.println("\t You don't have any available spell slots!");
                        continue;
                    }
                    else {
                        switch (playerClass) {
                            case 1:
                                //<editor-fold desc="WarriorSpells">
                                System.out.println("\t> Your spells:");
                                System.out.println("\t> 1. Lethal Strike (deal extra damage)");
                                System.out.println("\t> 2. Shield Wall (increase block chance for this battle)");
                                System.out.println("\t> 3. Bloodthirst (deal quadruple damage, take double damage)");
                                if(level >= 10) {System.out.println("\t> 4. Rend (damage over time)");}
                                String input1 = in.nextLine();
                                switch (input1) {
                                    case "1":
                                        //<editor-fold desc="LethalStrike">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        
                                        damageDealt = (int)round(1.5*(rand.nextInt(attackDamage)+1));
                                        damageTaken = rand.nextInt(enemyAttackDamage + 10 * (enemyLevel - 1))+1;
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(rand.nextInt(100)<tempDodgeBlockChance) {
                                            damageTaken=0;
                                        }
                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }


                                        System.out.println("\t> You magically strike the " + enemy + " for " + damageDealt + " damage.");
                                        if(damageTaken==0) {
                                            if(magicShieldOwned) {
                                                moreDamageDealt = rand.nextInt(attackDamage/4)+1;
                                                System.out.println("\t> You blocked, and your shield instead deals " + moreDamageDealt + " damage!");
                                            }
                                            else {
                                                System.out.println("\t> You block, taking no damage in retaliation!");
                                            }

                                        }
                                        else if(enemy == "Treasure Goblin") {}
                                        else {
                                            System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                                            if(magicShieldOwned) {
                                                moreDamageDealt = damageTaken/5;
                                                System.out.println("\t> Your shield deals back " + moreDamageDealt + " of the damage you took!");
                                            }
                                            if(enemyIsBoss && enemy == "Vampire")
                                            {
                                                System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                                enemyHealth += damageTaken/2;
                                            }
                                        }

                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        currentHealth -= damageTaken;

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "2":
                                        //<editor-fold desc="ShieldWall">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        tempDodgeBlockChance+=40;
                                        System.out.println("\t> Your shield grows, increasing your block chance by 40%.");
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "3":
                                        //<editor-fold desc="Bloodthirst">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        
                                        damageDealt = 4*(rand.nextInt(attackDamage)+1);
                                        damageTaken = 2*(rand.nextInt(enemyAttackDamage+1));
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(rand.nextInt(100)<tempDodgeBlockChance) {
                                            damageTaken=0;
                                        }
                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }




                                        System.out.println("\t> In a mad rage, you strike the " + enemy + " for " + damageDealt + " damage.");
                                        if(damageTaken==0) {
                                            if(magicShieldOwned) {
                                                moreDamageDealt = rand.nextInt(attackDamage/4)+1;
                                                System.out.println("\t> You blocked, and your shield instead deals " + moreDamageDealt + " damage!");
                                            }
                                            else {
                                                System.out.println("\t> You block, taking no damage in retaliation!");
                                            }
                                        }
                                        else if(enemy == "Treasure Goblin") {}
                                        else {
                                            System.out.println("\t> In your rage, you receive " + damageTaken + " in retaliation!");
                                            if(magicShieldOwned) {
                                                moreDamageDealt = damageTaken/5;
                                                System.out.println("\t> Your shield deals back " + moreDamageDealt + " of the damage you took!");
                                            }
                                            if(enemyIsBoss && enemy == "Vampire")
                                            {
                                                System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                                enemyHealth += damageTaken/2;
                                            }
                                        }

                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        currentHealth -= damageTaken;

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "4":
                                        //<editor-fold desc="Rend">
                                        if(level >= 10) {
                                            currentSpellSlots--;
                                            System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                            enemyBurning = true;
                                            System.out.println("\t> The " + enemy + " is bleeding out.");
                                        }
                                        else {
                                            System.out.println("\tInvalid command!");
                                        }
                                        continue FIGHT;
                                        //</editor-fold>
                                    default:
                                        System.out.println("\tInvalid command!");
                                        continue FIGHT;
                                }
                                //</editor-fold>
                            case 2:
                                //<editor-fold desc="WizardSpells">
                                System.out.println("\t> Your spells:");
                                System.out.println("\t> 1. Fireball (deal extra damage)");
                                System.out.println("\t> 2. Mage Armor (take half damage for this battle)");
                                System.out.println("\t> 3. Immolate (deal damage every round)");
                                System.out.println("\t> 4. Restoration (heal)");
                                System.out.println("\t> 5. Polymorph (lower enemy's level)");
                                if(level >= 10) { System.out.println("\t> 6. Ice Blast (deal damage and freeze enemy for turn)"); }
                                String input2 = in.nextLine();
                                int randomSpell;
                                switch (input2) {
                                    case "1":
                                        //<editor-fold desc="Fireball">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        damageDealt = (int)round(1.5*(rand.nextInt(attackDamage)+1));
                                        damageTaken = rand.nextInt(enemyAttackDamage)+1;
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }

                                        if((rand.nextInt(100)<tempDodgeBlockChance)) {
                                            damageTaken=0;
                                        }

                                        System.out.println("\t> You shoot a ball of fire at the " + enemy + " for " + damageDealt + " damage.");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    int maxHealAmount = healthPotionHealAmount*2/3;
                                                    int healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        if(damageTaken ==0 ) {
                                            System.out.println("\t> You dodge, taking no damage in retaliation!");
                                        }
                                        else if(enemy != "Treasure Goblin") {
                                            System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                                        }
                                        else if(enemyIsBoss && enemy == "Vampire")
                                        {
                                            System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                            enemyHealth += damageTaken/2;
                                        }

                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        currentHealth -= damageTaken;

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "2":
                                        //<editor-fold desc="MageArmor">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        mageArmorOn = true;
                                        System.out.println("\t> A shimmering glow surrounds you as a magical armor molds to your body.");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    enemyHealth -= moreDamageDealt;
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    int maxHealAmount = healthPotionHealAmount*2/3;
                                                    int healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "3":
                                        //<editor-fold desc="Immolate">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        enemyBurning = true;
                                        System.out.println("\t> The " + enemy + " is engulfed in flames.");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    enemyHealth -= moreDamageDealt;
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    int maxHealAmount = healthPotionHealAmount*2/3;
                                                    int healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "4":
                                        //<editor-fold desc="Restoration">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        int maxHealAmount = healthPotionHealAmount*2/3;
                                        int healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                        currentHealth += healAmount;
                                        if(currentHealth > maxHealth) {
                                            currentHealth = maxHealth;
                                        }
                                        System.out.println("\t> You are healed for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    enemyHealth -= moreDamageDealt;
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    maxHealAmount = healthPotionHealAmount*2/3;
                                                    healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "5":
                                        //<editor-fold desc="Polymorph">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        enemyLevel -= 3;
                                        if (enemyLevel < 1)
                                        {
                                            enemyLevel = 1;
                                        }
                                        System.out.println("\t> The enemy visibly becomes weaker.");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    enemyHealth -= moreDamageDealt;
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    maxHealAmount = healthPotionHealAmount*2/3;
                                                    healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "6":
                                        //<editor-fold desc="IceBlast">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        damageDealt = (int)round(1.25*(rand.nextInt(attackDamage)+1));
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        //</editor-fold>

                                        

                                        System.out.println("\t> You fire an icy blast at the " + enemy + " for " + damageDealt + " damage, freezing the enemy in place for a moment.");
                                        //<editor-fold desc="MagicSpellbook">
                                        if(magicSpellbookOwned&&(rand.nextInt(100)<10)) {
                                            randomSpell=rand.nextInt(5);
                                            switch(randomSpell) {
                                                case 1:
                                                    //<editor-fold desc="Fireball">
                                                    moreDamageDealt = (int)round((rand.nextInt(attackDamage)+1)*1.5);
                                                    System.out.println("\t> Your Spellbook shoots out a fireball randomly, dealing " + moreDamageDealt + " damage!");
                                                    break;
                                                    //</editor-fold>
                                                case 2:
                                                    //<editor-fold desc="MageArmor">
                                                    mageArmorOn = true;
                                                    System.out.println("\t> Your Spellbook surrounds you with a shimmering mage armor!");
                                                    break;
                                                    //</editor-fold>
                                                case 3:
                                                    //<editor-fold desc="Immolate">
                                                    enemyBurning=true;
                                                    System.out.println("Your Spellbook lights the " + enemy + " on fire!");
                                                    break;
                                                    //</editor-fold>
                                                case 4: 
                                                    //<editor-fold desc="Restoration">
                                                    maxHealAmount = healthPotionHealAmount*2/3;
                                                    healAmount = rand.nextInt(10) + maxHealAmount - 10;
                                                    currentHealth += healAmount;
                                                    if(currentHealth > maxHealth) {
                                                        currentHealth = maxHealth;
                                                    }
                                                    System.out.println("\t> Your Spellbook heals you for " + healAmount + " damage, and are now at " + currentHealth + ". (To a maximum of " + maxHealth + ".)");
                                                    break;
                                                    //</editor-fold>
                                                case 5:
                                                    //<editor-fold desc="Polymorph">
                                                    enemyLevel -= 3;
                                                    if (enemyLevel < 1)
                                                    {
                                                        enemyLevel = 1;
                                                    }
                                                    System.out.println("Your Spellbook polymorphs the " + enemy + ", weakening them.");
                                                    break;
                                                    //</editor-fold>
                                            }
                                        }
                                        //</editor-fold>
                                        
                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    default:
                                        System.out.println("\tInvalid command!");
                                        continue FIGHT;
                                }
                                //</editor-fold>
                            case 3:
                                //<editor-fold desc="RogueSpells">
                                System.out.println("\t> Your spells:");
                                System.out.println("\t> 1. Strike from the Shadows (deal extra damage, take no damage)");
                                System.out.println("\t> 2. Elusive (increase dodge chance for this battle)");
                                System.out.println("\t> 3. Sneak away (run from the fight)");
                                String input3 = in.nextLine();
                                switch (input3) {
                                    case "1":
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        //<editor-fold desc="StrikeFromShadows">
                                        damageDealt = (int) round(1.5*(rand.nextInt(attackDamage)+1));
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        //</editor-fold>



                                        System.out.println("\t> You strike the " + enemy + " from the shadows for " + damageDealt + " damage, taking no damage yourself.");
                                        if(magicDaggerOwned && (rand.nextInt(100)<20)) {
                                            moreDamageDealt = rand.nextInt(attackDamage/2)+1;
                                            System.out.println("\t> Your magical dagger whips by, also slicing the " + enemy + " for " + moreDamageDealt + " damage.");
                                        }
                                        enemyHealth -= (damageDealt+moreDamageDealt);

                                        roundOfBattle++;
                                        continue FIGHT;
                                    //</editor-fold>
                                    case "2":
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        //<editor-fold desc="Elusive">
                                        tempDodgeBlockChance+=40;
                                        System.out.println("\t> You start to blend into your surroundings, increasing your block chance by 40%.");
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "3":
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        //<editor-fold desc="SneakAway">
                                        break FIGHT;
                                        //</editor-fold>
                                    default:
                                        System.out.println("\tInvalid command!");
                                        continue FIGHT;
                                }
                                //</editor-fold>
                            case 4:
                                //<editor-fold desc="ArcherSpells">
                                System.out.println("\t> Your spells:");
                                System.out.println("\t> 1. Poison Arrow (deal damage every round)");
                                System.out.println("\t> 2. Explosive Shot (deal extra damage)");
                                System.out.println("\t> 3. Ice Arrow (take another shot for no retaliation damage)");
                                System.out.println("\t> 4. Hunter's Mark (always deal at least 75% of your attack damage)");
                                String input4 = in.nextLine();
                                switch (input4) {
                                    case "1":
                                        //<editor-fold desc="PoisonArrow">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        enemyBurning = true;
                                        System.out.println("\t> The enemy begins to look sickly.");
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "2":
                                        //<editor-fold desc="ExplosiveShot">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        
                                        damageDealt = (int)round(1.5*(rand.nextInt(attackDamage)+1));
                                        //<editor-fold desc="CheckHuntersMark">
                                        if(damageDealt < (1.5*(attackDamage/2))&&huntersMarkOn) {
                                            damageDealt = (int)1.5*(attackDamage/2);
                                        }
                                        //</editor-fold>
                                        damageTaken = rand.nextInt(enemyAttackDamage)+1;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(playerClass==4&&roundOfBattle==1) {
                                            damageTaken=0;
                                        }
                                        if((rand.nextInt(100)<tempDodgeBlockChance)) {
                                            damageTaken=0;
                                        }

                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }

                                         if(magicBowOwned && (rand.nextInt(100)<10)) {
                                            damageDealt*=2;
                                            System.out.println("\t> Headshot!");
                                        }

                                        enemyHealth -= damageDealt;
                                        currentHealth -= damageTaken;

                                        System.out.println("\t> You fire an explosive bolt at the " + enemy + " for " + damageDealt + " damage.");
                                        if(damageTaken==0 && roundOfBattle==1) {
                                            System.out.println("\t> You fire the first shot, taking no damage in retaliation!");
                                        }
                                        else if(damageTaken==0)
                                        {
                                            System.out.println("You dodge, taking no damage in retaliation!");
                                        }
                                        else if(enemy == "Treasure Goblin") {}
                                        else {
                                            System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                                            if(enemyIsBoss && enemy == "Vampire")
                                            {
                                                System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                                enemyHealth += damageTaken/2;
                                            }
                                        }

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "3":
                                        //<editor-fold desc="IceArrow">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        
                                        damageDealt = (int)round(1.25*(rand.nextInt(attackDamage)+1));
                                        //<editor-fold desc="CheckHuntersMark">
                                        if(damageDealt < (1.5*(attackDamage/2))&&huntersMarkOn) {
                                            damageDealt = (int)1.5*(attackDamage/2);
                                        }
                                        //</editor-fold>
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        //</editor-fold>
                                         if(magicBowOwned && (rand.nextInt(100)<10)) {
                                            damageDealt*=2;
                                            System.out.println("\t> Headshot!");
                                        }

                                        enemyHealth -= damageDealt;

                                        System.out.println("\t> You fire an icy arrow at the " + enemy + " for " + damageDealt + " damage, freezing the enemy in place for a moment.");

                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "4":
                                        //<editor-fold desc="HuntersMark">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        System.out.println("\t> You magically mark your enemy for death!");
                                        huntersMarkOn = true;
                                        //</editor-fold>
                                    default:
                                        System.out.println("\tInvalid command!");
                                        continue FIGHT;
                                }
                                //</editor-fold>
                            case 5:
                                //<editor-fold desc="DruidSpells">
                                System.out.println("\t> Your spells:");
                                System.out.println("\t> 1. Rejuvination (heal damage every round)");
                                System.out.println("\t> 2. Solar Flare (deal extra damage early in a fight)");
                                System.out.println("\t> 3. Moonfire (deal extra damage the longer the fight has lasted)");
                                String input5 = in.nextLine();
                                switch (input5) {
                                    case "1":
                                        //<editor-fold desc="Rejuvination">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        rejuvinating = true;
                                        System.out.println("\t> You are restoring health at the beginning of every turn.");
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "2":
                                        //<editor-fold desc="Solar Flare">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        damageDealt = (int)round(max(2-(0.25*(roundOfBattle-1)), 1)*(rand.nextInt(attackDamage)+1)); //max(2-(0.25*(roundOfBattle-1)), 1)
                                        damageTaken = rand.nextInt(enemyAttackDamage)+1;
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(aspect == 3||aspect==5||aspect==6) {
                                            damageDealt *= 1.25;
                                        }
                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }

                                        if((rand.nextInt(100)<tempDodgeBlockChance)) {
                                            damageTaken=0;
                                        }

                                        System.out.println("\t> You blast solar energy at the " + enemy + " for " + damageDealt + " damage.");
                                        
                                        if(damageTaken ==0 ) {
                                            System.out.println("\t> You dodge, taking no damage in retaliation!");
                                        }
                                        else if(enemy != "Treasure Goblin") {
                                            System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                                        }
                                        else if(enemyIsBoss && enemy == "Vampire")
                                        {
                                            System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                            enemyHealth += damageTaken/2;
                                        }

                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        currentHealth -= damageTaken;

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                    case "3":
                                        //<editor-fold desc="Moonfiree">
                                        currentSpellSlots--;
                                        System.out.println("\t> Your remaining spell slots: " + currentSpellSlots);
                                        damageDealt = (int)round(max(1+(0.25*(roundOfBattle-1)), 1)*(rand.nextInt(attackDamage)+1)); //max(2-(0.25*(roundOfBattle-1)), 1)
                                        damageTaken = rand.nextInt(enemyAttackDamage)+1;
                                        moreDamageDealt = 0;
                                        //<editor-fold desc="MagicItemsOwned">
                                        if(magicWeaponOwned) {
                                            damageDealt *= 1.1;
                                        }
                                        if(magicArmorOwned) {
                                            damageTaken *= .9;
                                        }
                                        //</editor-fold>
                                        if(aspect == 3||aspect==5||aspect==6) {
                                            damageDealt *= 1.25;
                                        }
                                        if(enemyIsBoss)
                                        {
                                            if(enemy == "Giant")
                                            {
                                                damageTaken *= 1.25;
                                            }
                                            else if (enemy == "Ogre")
                                            {
                                                damageTaken *= 1.5;
                                            }
                                        }

                                        if((rand.nextInt(100)<tempDodgeBlockChance)) {
                                            damageTaken=0;
                                        }

                                        System.out.println("\t> You blast solar energy at the " + enemy + " for " + damageDealt + " damage.");
                                        
                                        if(damageTaken ==0 ) {
                                            System.out.println("\t> You dodge, taking no damage in retaliation!");
                                        }
                                        else if(enemy != "Treasure Goblin") {
                                            System.out.println("\t> You receive " + damageTaken + " in retaliation!");
                                        }
                                        else if(enemyIsBoss && enemy == "Vampire")
                                        {
                                            System.out.println("\t> The Vampire steals the life from you, healing for " + damageTaken/2 + " HP.");
                                            enemyHealth += damageTaken/2;
                                        }

                                        enemyHealth -= (damageDealt+moreDamageDealt);
                                        currentHealth -= damageTaken;

                                        if (currentHealth < 1) {
                                            System.out.println("\t>You have taken too much damage, you are too weak to go on!");
                                            break FIGHT;
                                        }
                                        roundOfBattle++;
                                        continue FIGHT;
                                        //</editor-fold>
                                }
                                //</editor-fold>
                        }

                    }
                    //</editor-fold>
                case "4":
                    //<editor-fold desc="EscapeRope">
                    if(escapeRopes == 0)
                    {
                        System.out.println("\t You don't have any escape ropes!");
                        continue;
                    }
                    else {
                        escapeRopes--;
                        System.out.println("\t You run away!");
                        break FIGHT;
                    }
                    //</editor-fold>
                default:
                    System.out.println("\tInvalid command!");
                    break;
            }
            //</editor-fold>
        }
        return enemyHealth;
    }
    
    public static void levelUp() {
        //<editor-fold desc="LevelUp">
            System.out.println("--------------------------------------------------------------------------------------------------");
            if (currentExperience >= experienceNeeded) {
                //<editor-fold desc="Level">
                level += 1;
                System.out.println(" # Congratulations! You leveled up to level " + level + "! #");
                //</editor-fold>

                //<editor-fold desc="ClassPick">
                while (level == 3 && playerClass == 0) {
                    System.out.println(" # You have reached a level of competency that allows you to choose a class. Choosing one of the below classes locks provides a short description of the class, and is not final. #");
                    System.out.println("\t1. Warrior");
                    System.out.println("\t2. Wizard");
                    System.out.println("\t3. Rogue");
                    System.out.println("\t4. Archer");
                    System.out.println("\t5. Druid");
                    String input = in.nextLine();
                    switch (input) {
                        case "1":
                            System.out.println("\tThe Warrior excels in hand-to hand combat." +
                                        "\n\tThe Warrior gains extra maximum health and attack damage at every level up. Health potions also heal for more." +
                                        "\n\tThe Warrior also has a chance to completely block attacks." +
                                        "\n\tThe Warrior gains access to Lethal Strike (damaging spell), Shield Wall (increases block chance), and Bloodthirst (deal massive damage, take extra damage).");
                            System.out.println("# Would you like to choose the Warrior? #\n1. Yes \n2. Back");
                            String input2 = in.nextLine();
                            switch (input2) {
                                case "1":
                                    playerClass=1;
                                    dodgeBlockChance+=10;
                                    maxSpellSlots = 2;
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                case "2":
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                default:
                                    System.out.println("\tInvalid command!");
                                    break;
                            }
                            break;
                        case "2":
                            System.out.println("\tThe Wizard is a master of the magical arts." +
                                        "\n\tHealth potions heal for twice as much as usual, but the wizard gains less maximum health per level." +
                                        "\n\tWhat the Wizard lacks in passive combat abilities, it makes up for in spells." +
                                        "\n\tThe Wizard gains access to Fireball (damaging spell), Mage Armor (shielding ability), Immolate (damage every round), Restoration (healing spell), and Polymorph (lower the enemy level by 3).");
                            System.out.println("# Would you like to choose the Wizard? #\n1. Yes \n2. Back");
                            String input3 = in.nextLine();
                            switch (input3) {
                                case "1":
                                    playerClass=2;
                                    maxSpellSlots=3;
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                case "2":
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                default:
                                    System.out.println("\tInvalid command!");
                                    break;
                            }
                            break;
                        case "3":
                            System.out.println("\tThe Rogue is a sneaky dungeon delver, constantly playing in the shadows." +
                                        "\n\tThe Rogue gains damage extra attack damage (faster than the Warrior!), but does not gain as much maximum health as any of the other classes." +
                                        "\n\tThe Rogue has an innate chance to dodge an attack completely, and monsters have a higher chance of dropping a health potion." +
                                        "\n\tThe Rogue gains access to Strike from the Shadows (damaging spell and takes no retaliation damage), Elusive (increases dodge chance), and Sneak Away (can run from a fight).");
                            System.out.println("# Would you like to choose the Rogue? #\n1. Yes \n2. Back");
                            String input4 = in.nextLine();
                            switch (input4) {
                                case "1":
                                    playerClass=3;
                                    healthPotionDropChance+=15;
                                    dodgeBlockChance+=10;
                                    maxSpellSlots=2;
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                case "2":
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                default:
                                    System.out.println("\tInvalid command!");
                                    break;
                            }
                            break;
                        case "4":
                            System.out.println("\tThe Archer fires from afar, staying safe for as long as possible." +
                                        "\n\tThe Archer attacks once per battle from afar, taking no retaliation damage." +
                                        "\n\tThe Archer levels up 15% faster." +
                                        "\n\tThe Archer gains access to Poison Arrow (damage every round), Explosive Shot (damaging spell), and Ice Arrow (allows for another attack without retaliation damage).");
                            System.out.println("# Would you like to choose the Archer? #\n1. Yes \n2. Back");
                            String input5 = in.nextLine();
                            switch (input5) {
                                case "1":
                                    playerClass=4;
                                    maxSpellSlots=2;
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                case "2":
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                default:
                                    System.out.println("\tInvalid command!");
                                    break;
                            }
                            break;
                            case "5":
                            System.out.println("\tThe Druid is one with nature, transforming into different animals for different benefits for each fight." +
                                        "\n\tThe Druid chooses a form to specialize in every level, increasing their gains in that specific area." +
                                        "\n\tThe Druid gains access to bear form (more maximum health and more damage), Lion form (massive increase in damage), and Owl form (increase in spell damage and potency)." + 
                                        "\n\tThe Druid also has access to Rejuvination (healing over time), Solar Flare (damaging spell that does more at the beginning of a fight),"+
                                        " and Moonfire (damaging spell that does more the longer a fight has lasted");
                            System.out.println("# Would you like to choose the Druid? #\n1. Yes \n2. Back");
                            String input6 = in.nextLine();
                            switch (input6) {
                                case "1":
                                    playerClass=5;
                                    maxSpellSlots=2;
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                case "2":
                                    System.out.println("--------------------------------------------------------------------------------------------------");
                                    break;
                                default:
                                    System.out.println("\tInvalid command!");
                                    break;
                            }
                            break;
                        default:
                            System.out.println("\tInvalid command!");
                            break;
                    }
                }
                //</editor-fold>
                
                //<editor-fold desc="DruidExtra">
                int druidExtra=0;
                if(playerClass==5) {
                    boolean aspectChosen= false;
                    while(!aspectChosen) {
                        System.out.println("# Which aspect would you like to specialize in? #");
                        System.out.println("# 1. Bear form (extra HP) #");
                        System.out.println("# 2. Lion form (extra damage) #");
                        if(level%2 == 0) {
                            System.out.println("# 3. Owl form (extra spell slot) #");
                        }
                        String input = in.nextLine();
                        switch(input) {
                            case "1":
                                druidExtra = 1;
                                aspectChosen = true;
                                break;
                            case "2":
                                druidExtra = 2;
                                aspectChosen = true;
                                break;
                            case "3":
                                if(level%2==0) {
                                    druidExtra = 3;
                                    aspectChosen = true;
                                }
                                else {
                                    System.out.println("\tInvalid command!");
                                }
                                break;
                            default:
                                System.out.println("\tInvalid command!");
                                break;
                        }
                    }
                }
                //</editor-fold>

                //<editor-fold desc="SpellSlots">
                if(level == 4) {
                    maxSpellSlots++;
                    System.out.println(" # You gain an extra spell slot, up to " + maxSpellSlots + ", and all of your spell slots are refreshed. #");
                }
                else if(level > 4) {
                    if(playerClass==2 || druidExtra ==3) {
                        if(level%2==0)
                        {
                            maxSpellSlots++;
                            System.out.println(" # You gain an extra spell slot, up to " + maxSpellSlots + ", and all of your spell slots are refreshed. #");
                        }
                        else {
                            System.out.println(" # All of your spell slots are refreshed! You have " + maxSpellSlots + ". #");
                        }
                    }
                    else {
                        if(level%3==0)
                        {
                            maxSpellSlots++;
                            System.out.println(" # You gain an extra spell slot, up to " + maxSpellSlots + ", and all of your spell slots are refreshed. #");
                        }
                        else {
                            System.out.println(" # All of your spell slots are refreshed! You have " + maxSpellSlots + ". #");
                        }
                    }
                }
                currentSpellSlots=maxSpellSlots;
                //</editor-fold>

                //<editor-fold desc="MaxHealth">
                //healthMedium = 20
                int healthGained = healthMedium*(level-1);
                if(playerClass==1||druidExtra==1) {
                    healthGained = (healthMedium+5) * (level - 1); //warrior
                }
                else if(playerClass==3) {
                    healthGained = (healthMedium-5)*(level-1); //rogue
                }
                else if(playerClass==2) {
                    healthGained = (healthMedium-10)*(level-1); //wizard
                }
                maxHealth += healthGained;
                currentHealth = maxHealth;
                System.out.println(" # You gain " + healthGained + " maximum health and are restored to full HP. #");
                //</editor-fold>

                //<editor-fold desc="AttackDamage">
                //attackMedium = 7
                int damageGained = (attackMedium-2)*(level-1);
                if(playerClass==1) {
                    damageGained = attackMedium*(level-1); //warrior
                }
                else if(playerClass==3||druidExtra==2) {
                    damageGained = (attackMedium+2)*(level-1); //rogue
                }
                else if(playerClass==2) {
                    damageGained = (attackMedium-3)*(level-1); //wizard
                }
                attackDamage += damageGained;
                System.out.println(" # You gain " + damageGained + " attack damage. Your new attack damage is " + attackDamage + ". #");
                //</editor-fold>

                //<editor-fold desc="PotionHealing">
                int healthPotionGain = potionGainStandard*(level-1);
                if(playerClass ==1) {
                    healthPotionGain = (potionGainStandard+5)*(level-1);
                }
                if(level==3 && playerClass ==2){
                    healthPotionHealAmount *= 2;
                    System.out.println(" # Your wizarding abilities doubled the base heal amount of healing potions, to " + healthPotionHealAmount + ". #");
                }
                if(playerClass==2) {
                    healthPotionGain *= 2;
                }
                healthPotionHealAmount += healthPotionGain;
                System.out.println(" # Health potions now heal for " + healthPotionGain + " HP more, for a total of " + healthPotionHealAmount + ". #");
                //</editor-fold>

                //<editor-fold desc="Experience">
                currentExperience -= experienceNeeded;
                experienceNeeded = experienceNeededMultiplier*(int) pow(2, level-1);
                if(playerClass==4){
                    experienceNeeded = (int)round((.85*(double)experienceNeededMultiplier)*pow(2, level-1));
                }
                //</editor-fold>

                //<editor-fold desc="HarderEnemies">
                maxEnemyHealth = maxEnemyHealth+enemyDifficultyStandard*(level-1);
                enemyAttackDamage = enemyAttackDamage+enemyDifficultyStandard*(level-1);
                //</editor-fold>
                
                //<editor-fold desc="MoreSpells">
                if(level == 10) {
                    switch(playerClass) {
                        case 1:
                            System.out.println(" # You have unlocked a new spell! #");
                            System.out.println(" # Rend (enemy bleeds out over time) #");
                            break;
                        case 2:
                            System.out.println(" # You have unlocked a new spell! #");
                            System.out.println(" # Icy Blast (damaging spell that freezes the enemy for the round) #");
                            break;
                        case 3:
                            System.out.println(" # You have unlocked a new spell! #");
                            System.out.println(" # Deadly Poison (enemy is poisoned over time) #");
                            break;
                        case 4:
                            System.out.println(" # You have unlocked a new spell! #");
                            System.out.println(" # Hunter's Mark (you always do at least 75% of your attack damage) #");
                            break;
                        case 5:
                            System.out.println(" # You have unlocked a new spell! #");
                            System.out.println(" # Barkskin (take 50% less damage) #");
                            break;
                    }
                }
                //</editor-fold>
            }

            else {
                System.out.println(" # Experience to level up: " + (experienceNeeded-currentExperience) + " #");
            }
            System.out.println(" # Total gold: " + playerGold + " #");
            System.out.println("--------------------------------------------------------------------------------------------------");
            //</editor-fold>
    }
    
    public static void main(String[] args) {
        System.out.println("Welcome to the Dungeon!");
        
        chooseDifficulty();
        experienceNeeded = experienceNeededMultiplier; //300*2^(level-1)

        while(running) {
            System.out.println("--------------------------------------------------------------------------------------------------");

            //<editor-fold desc="FightVariables">
            int enemyLevel = level;
            int enemyHealth = rand.nextInt(30)+maxEnemyHealth-30;
            String enemy = enemies[rand.nextInt(enemies.length)];
            int tempDodgeBlockChance = dodgeBlockChance;
            if(monstersDefeated%10==0) {
                //boss fight
                enemyIsBoss = true;
                enemy = bosses[rand.nextInt(bosses.length)];
                switch (enemy) {
                    case "Giant":
                        enemyHealth *= 2;
                        break;
                    case "Ogre":
                        enemyHealth *= 1.75;
                        break;
                    default:
                        enemyHealth *= 1.5;
                        break;
                }
                System.out.println("\t# You have encountered a boss! #");
            }
            else if(rand.nextInt(100)<2) {
                enemy = "Treasure Goblin";
                enemyHealth *= 3;
            }
            System.out.println("\t# Level " + enemyLevel + " " + enemy +" appeared! #\n");
            
            
            //</editor-fold>
            enemyHealth = fight(enemyLevel, enemyHealth, enemy, tempDodgeBlockChance);


            //<editor-fold desc="Death">
            if(currentHealth < 1) {
                System.out.println("--------------------------------------------------------------------------------------------------");
                System.out.println("You limp out of the dungeon, weak from battle.");
                break;
            }
            //</editor-fold>

            if(enemyHealth <= 0) { //checking if a rogue ran away
            //<editor-fold desc="DefeatedEnemy">
            System.out.println("--------------------------------------------------------------------------------------------------");
            System.out.println(" # Level " + enemyLevel + " " + enemy + " was defeated! # ");
            monsterLevels += enemyLevel;
            monstersDefeated++;
            int experienceGained = maxEnemyExperience*(enemyLevel);
            int goldGained = 10*enemyLevel;
            if(enemyIsBoss) {
                experienceGained *= 1.5;
                goldGained *= 3;
            }
            else if(enemy == "Treasure Goblin") {
                experienceGained *=3;
                goldGained *= 10;
            }
            System.out.println(" # Experience gained: " + experienceGained + "! #");
            System.out.println(" # Gold gained: " + goldGained + "! #");
            currentExperience += experienceGained;
            playerGold += goldGained;
            totalGoldCollected += goldGained;
            if(enemyIsBoss) {
                enemyIsBoss = false;
            }
            System.out.println("--------------------------------------------------------------------------------------------------");
            
            //</editor-fold>
            levelUp();
            }
            //<editor-fold desc="AfterFight">
            System.out.println("--------------------------------------------------------------------------------------------------");
            System.out.println(" # You have " + currentHealth + " HP left. #");
            //<editor-fold desc="PotionDrop">
            if(rand.nextInt(100)<healthPotionDropChance && enemyHealth<=0) { //checking if a rogue ran away
                numHealthPotions++;
                if (numHealthPotions>maxHealthPotions) {
                    numHealthPotions=maxHealthPotions;
                    System.out.println(" # The " + enemy + " dropped a health potion, but you already are carrying the maximum amount. # ");
                }
                else {
                    System.out.println(" # The " + enemy + " dropped a health potion! # ");
                }
                System.out.println(" # You now have " + numHealthPotions + " health potion(s). #");
            }
            //</editor-fold>

            //<editor-fold desc="AfterFightOptions">
            System.out.println("--------------------------------------------------------------------------------------------------");
            System.out.println("What would you like to do now?");
            System.out.println("1. Continue fighting.");
            System.out.println("2. Visit the shop." + (((monstersDefeated%5==0)&&(level>=3)&&(monstersDefeated>=15))?" There is a new magic item available!":""));
            System.out.println("3. Exit dungeon");
            //</editor-fold>

            String input = in.nextLine();

            while(!input.equals("1") && !input.equals("2") && !input.equals("3")) {
                System.out.println("Invalid command!");
                input = in.nextLine();
            }
            System.out.println("--------------------------------------------------------------------------------------------------");
            //<editor-fold desc="Continue">
            if(input.equals("1")) {
                System.out.println("You continue on your adventure!");
            }
            //</editor-fold>
            //<editor-fold desc="Shop">
            else if (input.equals("2")) {
                System.out.println("You walk back towards the entrance of the dungeon, where a man is selling supplies.");
                //<editor-fold desc="ShopVariables">
                int potionsInShop = 5;
                int weaponUpgradesInShop = 5;
                int armorUpgradesInShop = 5;
                int spellSlotBuyBackInShop = 3;
                int escapeRopeAvailableInShop = 1;
                boolean inShop = true;
                int prevItem = 0;
                //<editor-fold desc="MagicItem">
                if((monstersDefeated%5==0)&&(level>=3)&&(monstersDefeated>=15)){
                    switch(playerClass) {
                        case 1:
                            prevItem = itemInShop;
                            while(itemInShop == prevItem) {
                                itemInShop = possibleWarriorOptions[rand.nextInt(4)];
                            }
                            break;
                        case 2:
                            prevItem = itemInShop;
                            while(itemInShop == prevItem) {
                                itemInShop = possibleWizardOptions[rand.nextInt(4)];
                            }
                            break;
                        case 3:
                            prevItem = itemInShop;
                            while(itemInShop == prevItem) {
                                itemInShop = possibleRogueOptions[rand.nextInt(4)];
                            }
                            break;
                        case 4:
                            prevItem = itemInShop;
                            while(itemInShop == prevItem) {
                                itemInShop = possibleArcherOptions[rand.nextInt(4)];
                            }
                            break;
                        case 5:
                            prevItem = itemInShop;
                            while(itemInShop == prevItem) {
                                itemInShop = possibleDruidOptions[rand.nextInt(4)];
                            }
                    }
                }
                //</editor-fold>
                //</editor-fold>
                while(inShop) {
                    //<editor-fold desc="ShopOptions">
                    System.out.println("### Gold available: " + playerGold + " ###");
                    System.out.println("### Wares available: ###");
                    System.out.println("### 1. Potion of Healing (" + potionsInShop + " available) - 25g each ###");
                    System.out.println("### 2. Weapon Improvement (" + weaponUpgradesInShop + " available) - 25g each - Current attack damage:" + attackDamage + " ###");
                    System.out.println("### 3. Armor Improvement (" + armorUpgradesInShop + " available) - 25g each - Current max health: " + maxHealth + " ###");
                    if(level >= 3)
                    {
                        System.out.println("### 4. Refresh Spell Slot (" + spellSlotBuyBackInShop + " available - 75g each) ###");
                    }
                    if(level >= 5)
                    {
                        System.out.println("### 5. Escape Rope (" + escapeRopeAvailableInShop + " available - 150g each) ###");
                    }
                    //<editor-fold desc="MagicItem">
                    if(itemInShop != 0)
                    {
                        switch(itemInShop) {
                            case 1:
                                System.out.println("### 6. Magic Weapon (+10% to all attacks)" + (!magicWeaponOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 2:
                                System.out.println("### 6. Magic Armor (-10% to all damage taken)" + (!magicArmorOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 3:
                                System.out.println("### 6. Magic Cloak (+10% to " + ((playerClass==1)?"block":"dodge") + ")" + (!magicCloakOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 4:
                                System.out.println("### 6. Magic Dagger (Rogue Only)(20% for extra off-hand attack - 50% damage)" + (!magicDaggerOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 5:
                                System.out.println("### 6. Magic Spellbook (Wizard Only)(+10% on spell cast to cast a random Wizard spell)" + (!magicSpellbookOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 7:
                                System.out.println("### 6. Magic Shield (Warrior Only)(+10% to block)" + (!magicShieldOwned?"(available)":"(already owned)") + " 500g ###");
                                break;
                            case 6:
                                System.out.println("### 6. Magic Bow (Archer Only)(+10% for headshot - x2 damage)" + (!magicBowOwned?"(available)":"(already owned)") + " ###");
                                break;
                            case 8:
                                System.out.println("### 6. Magic Pendant (+5% crit chance - x2 damage)" + (!magicPendantOwned?"(available)":"(already owned)") + " ###");
                                break;
                            case 9:
                                System.out.println("### 6. Magic Staff (Druid Only)(Choose from 2 stances at the beginning of a fight)" + (!magicStaffOwned?"(available)":"(already owned)") + " ###");
                        }
                    }
                    //</editor-fold>
                    System.out.println("### 7. Leave Shop ###");
                    //</editor-fold>
                    String shopInput = in.nextLine();
                    switch (shopInput) {
                        case "1":
                            //<editor-fold desc="Potion">
                            if(playerGold >= 25 && numHealthPotions < 5 && potionsInShop>0) {
                                playerGold -= 25;
                                potionsInShop--;
                                numHealthPotions++;
                                System.out.println("\t> You bought a health potion!");
                            }
                            else if(playerGold < 25) {
                                System.out.println("\t> Not enough gold!");
                            }
                            else if(numHealthPotions >= 5) {
                                System.out.println("\t> You can't carry any more potions!");
                            }
                            else if(potionsInShop < 1) {
                                System.out.println("\t> There are no more potions in the shop!");
                            }
                            break;
                            //</editor-fold>
                        case "2":
                            //<editor-fold desc="Weapon">
                            if(playerGold >= 25 && weaponUpgradesInShop>0) {
                                attackDamage++;
                                weaponUpgradesInShop--;
                                playerGold -= 25;
                                System.out.println("\t> Your attack increased to " + attackDamage + "!");
                                
                            }
                            else if (playerGold < 25){
                                System.out.println("\t> Not enough gold!");
                            }
                            else if(weaponUpgradesInShop < 1) {
                                System.out.println("\t> There are no more weapon upgrades in the shop!");
                            }
                            break;
                            //</editor-fold>
                        case "3":
                            //<editor-fold desc="Armor">
                            if(playerGold >= 25 && armorUpgradesInShop>0) {
                                maxHealth += 5;
                                currentHealth +=5;
                                armorUpgradesInShop--;
                                playerGold -= 25;
                                System.out.println("\t> Your health increased to " + maxHealth + "!");
                            }
                            else if(playerGold < 25) {
                                System.out.println("\t> Not enough gold!");
                            }
                            else if(armorUpgradesInShop < 1) {
                                System.out.println("\t> There are no more armor upgrades in the shop!");
                            }
                            break;
                            //</editor-fold>
                        case "4":
                            //<editor-fold desc="SpellSlot">
                            if(level >= 3 && spellSlotBuyBackInShop >0 && currentSpellSlots<maxSpellSlots && playerGold >= 75) {
                                currentSpellSlots++;
                                playerGold-=75;
                                spellSlotBuyBackInShop--;
                                System.out.println("\t> You gained one spell slot back! (You now have "+ currentSpellSlots + ".)");
                            }
                            else if(level < 3) {
                                System.out.println("Invalid command!");
                            }
                            else if(spellSlotBuyBackInShop == 0) {
                                System.out.println("\t> There are no more spell slot buybacks in the shop!");
                            }
                            else if(playerGold < 75) {
                                System.out.println("\t> Not enough gold!");
                            }
                            else if(currentSpellSlots==maxSpellSlots) {
                                System.out.println("\t> You are already at your maximum spell slots!");
                            }
                            break;
                            //</editor-fold>
                        case "5":
                            //<editor-fold desc="EscapeRope">
                            if(level >= 5 && escapeRopeAvailableInShop >0 && playerGold >= 150) {
                                escapeRopes++;
                                playerGold-=150;
                                escapeRopeAvailableInShop--;
                                System.out.println("\t> You gained one escape rope!");
                            }
                            else if(level < 5) {
                                System.out.println("Invalid command!");
                            }
                            else if(escapeRopeAvailableInShop == 0) {
                                System.out.println("\t> There are no more escape ropes in the shop!");
                            }
                            else if(playerGold < 150) {
                                System.out.println("\t> Not enough gold!");
                            }
                            break;
                            //</editor-fold>
                        case "6":
                            //<editor-fold desc="MagicItem">
                            //<editor-fold desc="TooLowLevel">
                            if(itemInShop == 0)
                            {
                                System.out.println("### Invalid command! ###");
                                break;
                            }
                            //</editor-fold>
                            else {
                                switch(itemInShop) {
                                    case 1:
                                        //<editor-fold desc="Weapon">
                                        if(magicWeaponOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicWeaponOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical weapon! ###");
                                        }
                                        break;
                                        //</editor-fold>
                                    case 2:
                                        //<editor-fold desc="Armor">
                                        if(magicArmorOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicArmorOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought magical armor! ###");
                                        }
                                        break;
                                        //</editor-fold>
                                    case 3:
                                        //<editor-fold desc="Cloak">
                                        if(magicCloakOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicCloakOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical cloak! ###");
                                            dodgeBlockChance += 10;
                                        }
                                        break;
                                        //</editor-fold>
                                    case 4:
                                        //<editor-fold desc="Dagger">
                                        if(magicDaggerOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicDaggerOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical dagger! ###");
                                        }
                                        break;
                                        //</editor-fold>
                                    case 5:
                                        //<editor-fold desc="Spellbook">
                                        if(magicSpellbookOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicSpellbookOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical Spellbook! ###");
                                        }
                                        break;
                                        //</editor-fold>
                                    case 6:
                                        //<editor-fold desc="Bow">
                                        if(magicBowOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicBowOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical bow! ###");
                                        }
                                        break;
                                        //</editor-fold>
                                    case 7:
                                        //<editor-fold desc="Shield">
                                        if(magicShieldOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicShieldOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical shield! ###");
                                            dodgeBlockChance += 10;
                                        }
                                        break;
                                        //</editor-fold>
                                    case 8:
                                        //<editor-fold desc="Pendant">
                                        if(magicPendantOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicPendantOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical pendant! ###");
                                            critChance += 5;
                                        }
                                        break;
                                        //</editor-fold>
                                    case 9:
                                        //<editor-fold desc="Staff">
                                        if(magicStaffOwned) {
                                            System.out.println("### You already own this item! ###");
                                        }
                                        else if(playerGold < 500)
                                        {
                                            System.out.println("### Not enough gold! ###");
                                        }
                                        else {
                                            magicStaffOwned=true;
                                            playerGold-=500;
                                            System.out.println("### You bought a magical staff! ###");
                                        }
                                        //</editor-fold>
                                }
                            }
                            break;
                            //</editor-fold>
                        case "7":
                            inShop=false;
                            break;
                        default:
                            System.out.println("Invalid command!");
                            break;
                    }
                }
            }
            //</editor-fold>
            //<editor-fold desc="Quit">
            else if (input.equals("3")) {
                System.out.println("# You exit the dungeon, successful from your adventures! #");
                break;
            }
            //</editor-fold>
            System.out.println("--------------------------------------------------------------------------------------------------");
            //</editor-fold>
        }

        //<editor-fold desc="EndGame">
        System.out.println("--------------------------------------------------------------------------------------------------");
        System.out.println("# Your level: " + level  + " #");
        System.out.println("# Monsters defeated: " + monstersDefeated + " #");
        System.out.println("# Average level of monsters: " + (double)monsterLevels/monstersDefeated + " #");
        System.out.println("# Total gold collected: " + totalGoldCollected + " #");
        System.out.println("--------------------------------------------------------------------------------------------------");
        System.out.println("--------------------------------------------------------------------------------------------------");
        System.out.println("######################");
        System.out.println("# THANKS FOR PLAYING #");
        System.out.println("######################");
        //</editor-fold>
    }
}