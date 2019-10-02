using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Allows us to call Button

/*
 * This class handles all the ways we can interact with interactable
 * Things in the game such as enemies, chests, and exits.
 * 
 * This class has to know a few things:
 * - What our player is - need a player reference to interact with player's room and inv.
 * - need an array of our dynamic controls
 */

namespace TextRPG
{
    public class Encounter : MonoBehaviour
    {
        // Need to be able to detect and assign an enemy
        public Enemy enemy { get; set; }

        [HideInInspector]
        public float textPostSpeed = UIController.textPostSpeed;

        // Reference to our player object
        [SerializeField]
        Player player;

        [SerializeField]
        UIController UIController;

        [HideInInspector]
        public bool isTextDisplaying = UIController.isTextDisplaying;

        // Create our array of buttons for our dynamic controls
        [SerializeField]
        Button[] dynamicControls;
        // Create a button for our movement controls
        [SerializeField]
        Button[] movementControls;

        [SerializeField]
        GameObject movementControlsPanel, dynamicControlsPanel;

        /* Now we will set up a delegate that listens for an event
         * called from the enemy class death method.
         * This will alert the encounter class when
         * an enemy has fired off its death method.
         */

        // First declare the delegate
        public delegate void OnEnemyDieHandler();
        // Then use that type to create the actual instance of the delegate
        public static OnEnemyDieHandler OnEnemyDie;

        private void Start()
        {
            // Whenever OnEnemyDie is called from an enemy it fires off the following method (s)
            OnEnemyDie += Loot;
        }

        /*
         * For the controls, the buttons are contextual.
         * In other words, the "attack" button will only
         * function if the player walks into a room with
         * an enemy in it.
         *
         */

        public void ResetDynamicControls()
        {
            // control for our controls that we access from player

            // loop through the array of buttons and set the interaction flag to false
            foreach (Button button in dynamicControls)
            {
                button.interactable = false;
            }
        }

        public void toggleMovementControls()
        {
            // We want the ability to turn off movement controls when encountering an enemy
            // loop through the array of buttons and set the interaction flag to the opposite of whatever it is
            foreach (Button button in movementControls)
            {
                button.interactable = !button.interactable;
            }
        }

         public void enableMovementControls()
        {
            foreach (Button button in movementControls)
            {
                button.interactable = true;
            }
        }

        public void toggleSetActive(GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        public void StartCombat()
        {
            // Assign the enemy found to our enemy instance
            this.enemy = player.Room.Enemy;

            // Enable the Flee and Attack Buttons and disable other UI
            dynamicControls[0].interactable = true;
            dynamicControls[1].interactable = true;
            toggleSetActive(movementControlsPanel);
            toggleSetActive(dynamicControlsPanel);
            //toggleSetActive(UIController.EnemyStatsPanel);
            toggleMovementControls();
            enemy.Energy = enemy.MaxEnergy;
            /* When combat is started, pass the enemy that was just assigned to enemy
             * into the UI controller so it can update the stats in the UI
             */
            UIController.OnEnemyUpdate(this.enemy);
        }

        public void EndCombat()
        {
            toggleSetActive(movementControlsPanel);
            toggleSetActive(dynamicControlsPanel);
        }

        public void StartChest()
        {
            AudioEngine.Instance.PlaySound("chestDetected");
            // Method to allow chest encounter controls
            dynamicControls[3].interactable = true;

        }

        public void OpenChest()
        {
            StartCoroutine(IOpenChest());
        }
        public IEnumerator IOpenChest()
        {
            Journal.Instance.RegisterActiveText("You carefully attempt to open the chest...");
            yield return new WaitForSeconds(textPostSpeed*2);

            Chest chest = player.Room.Chest;
            // check if the chest is a trap. If so, take damage
            if (chest.Trap)
            {
                AudioEngine.Instance.PlaySound("openChestTrap");
                player.TakeDamage(5);
                Journal.Instance.Log("It's a trap! You take <color=red>5</color> damage.");
                // Remove the chest
                player.Room.Empty = true;
                //player.Investigate();
            }
            else if (chest.Heal)
            {
                AudioEngine.Instance.PlaySound("openChestHeal");
                player.TakeDamage(-7);
                Journal.Instance.Log("The chest contained a healing spell. You gained <color=green>7</color> energy!");
                player.Room.Empty = true;
            }
            else if (chest.Enemy)
            {
                /* If the chest contains an enemy, first we create the enemy*/
                player.Room.Enemy = chest.Enemy;
                Journal.Instance.Log("The chest contained a " + player.Room.Enemy.Description + "!");
            }
            else
            {
                AudioEngine.Instance.PlaySound("openChestGood");
                Journal.Instance.Log(
                    "You open the chest and find a <color=orange>" + chest.Item +
                    "</color> and <color=yellow>" + chest.Gold + "g.</color>");
                player.Gold += chest.Gold;
                player.AddItem(chest.Item);
                UIController.OnPlayerStatChange();
                UIController.OnPlayerInventoryChange();
                player.Room.Empty = true;
            }

            //remove the chest and set the open chest button to false
            player.Room.Chest = null;
            dynamicControls[3].interactable = false;
            yield return new WaitForSeconds(textPostSpeed);
            player.Investigate();
        }

        public void StartExit()
        {
            // Method for interacting with Exits
            dynamicControls[2].interactable = true;
        }
        public void ExitFloor()
        {

            Journal.Instance.Log("You made it out of the dungeon alive!" +
                "\nWell done. Would you like to play again?");

            UIController.controlPanel.SetActive(false);
            UIController.yesNoPanel.SetActive(true);
            //StartCoroutine(player.world.GenerateFloor());
            //player.Floor += 1;
        }

        /*
         * Handler for attacking. This method will be called
         * from the Attack button
         *
         */
        public void AttackStart()
        {
            // string enemyDescription = string.Format("<color=orange>{0}</color>", enemy.Description);
            StartCoroutine(Attack());
        }
        public IEnumerator Attack()
        {
            if (player.Energy > 0)
            {
                // Disable fight controls to let the fight play out
                dynamicControls[0].interactable = false;
                dynamicControls[1].interactable = false;

                // Calculate dice rolls
                int playerRoll = RollDice(player);
                

                /***** Begin Player Attack *******/

                Journal.Instance.RegisterActiveText("You launch a furious attack at the " + enemy.Description + ".");
                
                while(UIController.isTextDisplaying == true)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(textPostSpeed*2);
                Journal.Instance.Log(string.Format("Roll: <color=#00BFFF>{0}</color>", playerRoll));

                // Roll dice to see if attack hits
                if (playerRoll > enemy.Defence)
                {
                    Journal.Instance.Log("Hit!");
                    int playerDamageAmount = (int)(Random.Range(1, player.Attack));

                    EnemyTakeDamageVFX();
                    yield return new WaitForSeconds(textPostSpeed*2);

                    // Write the combat to the journal
                    Journal.Instance.Log("You deal <color=purple>" + playerDamageAmount +
                        "</color> damage to the <color=orange>"
                        + enemy.Description + "</color>\n");

                    enemy.TakeDamage(playerDamageAmount);
                }
                else
                {
                    Journal.Instance.Log("Miss! - No Damage.\n");
                }
            }
            /***** Begin Enemy Attack *******/
            if (enemy.Energy > 0)
            {
                // yield return new WaitForSeconds(textPostSpeed);
                // Journal.Instance.Log("The enemy readies it's attack...");
                yield return new WaitForSeconds(textPostSpeed*2);

                int enemyRoll = RollDice(enemy);

                Journal.Instance.Log("The <color=orange>" + enemy.Description + "</color> attacks you viciously!\n");
                yield return new WaitForSeconds(textPostSpeed);

                Journal.Instance.Log(string.Format("Roll: <color=#00BFFF>{0}</color>", enemyRoll));

                if (enemyRoll > player.Defence)
                {
                    Journal.Instance.Log("Hit!");
                    int enemyDamageAmount = (int)(Random.Range(1, enemy.Attack));

                    Journal.Instance.Log("The enemy counters and deals <color=red>" + enemyDamageAmount + "</color> damage!");
                    player.TakeDamage(enemyDamageAmount);
                }
                else
                {
                    Journal.Instance.Log("Miss! - No Damage");
                }

                if (player.Energy > 0)
                {
                    dynamicControls[0].interactable = true;
                    dynamicControls[1].interactable = true;
                }
            }
        }

        public void EnemyTakeDamageVFX()
        {
            UIController.GetComponent<ShakeBehavior>().TriggerShake("EE");
            UIController.StartScreenFade("ERed");
            AudioEngine.Instance.PlaySound("EnemyTakeDamage");
            AudioEngine.Instance.PlaySound("EnemyHurt");

        }

        public void Flee()
        {
            StartCoroutine(IFlee());
        }

        public IEnumerator IFlee()
        {
            
            // Player takes damage upon fleeing
            int enemyDamageAmount = (int)(Random.Range(1, enemy.Attack));
            
            Journal.Instance.RegisterActiveText(string.Format("You attempt to run from the {0}...", enemy.Description));
            
            // TODO: Put in a mechanic for determining if the attempt is succesful.
            yield return new WaitForSeconds(textPostSpeed*2);
            // Log to journal
            Journal.Instance.Log("You manage to flee but take " +
                "<color=red>" + enemyDamageAmount + "</color> damage! ");

            // Player takes damage
            player.TakeDamage(enemyDamageAmount);

            //null out the enemy
            player.Room.Enemy = null;
            //Set the room back to empty
            player.Room.Empty = true;

            // Remove enemy from room when fleeing
            UIController.OnEnemyUpdate(null);

            // Reset the room at the end of flee by using investigate
            EndCombat();
            toggleMovementControls();
            yield return new WaitForSeconds(textPostSpeed);
            player.Investigate();
        }
        public void Loot()
        {
            StartCoroutine(ILoot());
        }
        public IEnumerator ILoot()
        {
            //Loot method for when an enemy dies. To be called by the OnEnemyDie Delegate

            //Log to the journal
            Journal.Instance.Log(string.Format("You kill the <color=orange>{0}</color>!.", this.enemy.Description));
            yield return new WaitForSeconds(textPostSpeed*2);

            Journal.Instance.Log(string.Format("Searching the carcass, you find a <color=orange>{0}</color> and " +
                "<color=yellow>{1}g!</color>",
                 this.enemy.Inventory[0], this.enemy.Gold));

            player.AddItem(this.enemy.Inventory[0]);
            player.AddGold(this.enemy.Gold);

            // Now remove the enemy from the room
            player.Room.Enemy = null;
            // Flag the room as empty
            player.Room.Empty = true;
            // Update the UI (empty it)
            
            UIController.OnEnemyUpdate(null);

            //reset the room so we can move again
            EndCombat();
            toggleMovementControls();
            yield return new WaitForSeconds(textPostSpeed);
            player.Investigate();
        }

        public int RollDice(Character character)
        {
            int result = Random.Range(1, character.Dice);
            return result;
        }

        public void OnDestroy()
        {
            OnEnemyDie -= Loot;
        }
    }
}