using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Player : Character
    {

        public int Floor { get; set; }
        // this tells us what room the player is in
        public Room Room { get; set; }
        // Create a reference to the Encounter class so we can access its methods
        [SerializeField]
        public Encounter encounter;
        // Create a reference to the world generator
        public UIController UIController;

        public AudioEngine AudioEngine;

        [HideInInspector]
        public float textPostSpeed = UIController.textPostSpeed;

        public World world;

        void Start()
        {
            // TODO: Add player classes with different stats

            Floor = 0;
            Energy = 20;
            Attack = 20;
            Dice = 20;
            Defence = 10;
            Gold = 0;

            Inventory = new List<string>();
            // Initialize starting room
            RoomIndex = new Vector2(2, 2);
            world.PlayerPosition = RoomIndex;
            // Grab the Room from the world builder that corresponds to the player's roomindex and set room to that
            this.Room = world.Dungeon[(int)RoomIndex.x, (int)RoomIndex.y];
            // Make sure starting Room is empty
            this.Room.Empty = true;
            encounter.ResetDynamicControls();

            // Initialize the UI
            UIController.OnPlayerStatChange();
            UIController.OnPlayerInventoryChange();
            UIController.UpdateRoom(RoomIndex);

            /** Opening Cutscene **/

            //RunCutScene("open");

            /** Opening Setting. Initialize first room */

            Investigate();

        }

        public void Move(int direction)
        {
            StartCoroutine(MoveSequence(direction));
        }

        public IEnumerator MoveSequence(int direction)
        {
            // This function moves the player

            // Check if it is possible to move. If not up against a wall, then move in that direction
            switch (direction)
            {
                case 0: // Move North
                    if (RoomIndex.y > 0)
                    {
                        Journal.Instance.RegisterActiveText("You Move to the North.");
                        RoomIndex -= Vector2.up;
                    }
                    else
                    {
                        Journal.Instance.Log("There is no exit in that direction.");
                    }
                    break;
                case 1: // Move East
                    if (RoomIndex.x < world.Dungeon.GetLength(0) - 1)
                    {
                        Journal.Instance.RegisterActiveText("You Move to the East.");
                        RoomIndex += Vector2.right;
                    }
                    else
                    {
                        Journal.Instance.Log("There is no exit in that direction.");
                    }
                    break;
                case 2: // Move South
                    if (RoomIndex.y < world.Dungeon.GetLength(1) - 1)
                    {
                        Journal.Instance.RegisterActiveText("You Move to the South.");
                        RoomIndex -= Vector2.down;
                    }
                    else
                    {
                        Journal.Instance.Log("There is no exit in that direction.");
                    }
                    break;
                case 3: // Move West
                    if (RoomIndex.x > 0)
                    {
                        Journal.Instance.RegisterActiveText("You Move to the West.");
                        RoomIndex += Vector2.left;
                    }
                    else
                    {
                        Journal.Instance.Log("There is no exit in that direction.");
                    }
                    break;
                default:
                    break;
            }
           
           
            if (this.Room.RoomIndex != RoomIndex)
            {
                // If I have changed rooms, RoomIndex will be different than the Room because Room updates on Investigate()
                UIController.UpdateRoom(RoomIndex);
                world.PlayerPosition = RoomIndex;
                world.DrawMap();
                yield return new WaitForSeconds(textPostSpeed*2);
                Investigate();
            }
            // If the room the player is currently in has an enemy, break out of this function
            if (this.Room.Enemy)
            {
               yield return null;
            }

        }

        public void Investigate()
        {
            /* Method for looking in the room so we can interact with what is there */
            if (Energy > 0)
            {
                this.Room = world.Dungeon[(int)RoomIndex.x, (int)RoomIndex.y];
                /* Each time we investigate we disable all of the controls and enable
                 * The appropriate controls later
                 */
                encounter.ResetDynamicControls();

                //check to see if the room is empty.
                if (this.Room.Empty)
                {
                    //Debug.Log("Room is empty");
                    Journal.Instance.Log("You find yourself in an empty room. What do you do?");
                }
                else if (this.Room.Chest != null)
                {
                    encounter.StartChest();
                    Journal.Instance.Log("You have found a <color=yellow>chest</color>! What would you like to do?");
                }
                else if (this.Room.Enemy != null)
                {
                    Debug.Log("Room is Enemy");
                    //TODO: Add more text options 
                    Journal.Instance.Log("You are jumped by a <color=orange>"
                        + Room.Enemy.Description +
                        "</color>! You are locked in combat! What would you like to do?");
                    //An enemy is encountered. Therefore we call the encounter method for starting combat
                    encounter.StartCombat();
                }
                else if (this.Room.Exit)
                {
                    Debug.Log("Room is Exit");
                    encounter.StartExit();
                    Journal.Instance.Log("You have found the exit to the next floor. Would you like to continue?");
                }
                else
                {
                    Debug.Log("Something else is going on");
                }
                /* Reset the movement controls to active after the investigation is complete */
                encounter.enableMovementControls();
            }
            else
            {
                return;
            }
        }

        public void AddItem(string item)
        {
            // Add an item to the player's inventory
            Journal.Instance.Log("You were given item: <color=orange>" + item + "</color>");
            Inventory.Add(item);
            UIController.OnPlayerInventoryChange();
        }

        public void AddGold(int amount)
        {
            Debug.Log("player.AddGold called");
            Gold += amount;
            UIController.OnPlayerStatChange();
        }

        //TODO: Add a function for removing an inventory item

        public override void TakeDamage(int amount)
        {
            
            DamageVFX("red");

            Debug.Log(string.Format("player takes {0} damage", amount));
            base.TakeDamage(amount);
            if (Energy <= 0)
            {
                //Debug.Log("Energy was <= 0, calling Die()");
                this.Energy = 0;
                UIController.OnPlayerStatChange();
                Die();
            }
            else
            {
                UIController.OnPlayerStatChange();
            }
        }

        public override void Die()
        {
            Journal.Instance.Log("You are dead. Game Over Man!\nWould you like to try again?");
            encounter.ResetDynamicControls();

            base.Die();
            UIController.controlPanel.SetActive(false);
            UIController.yesNoPanel.SetActive(true);
        }

        public void CutsceneText(string textToLog)
	    {
		    ScriptedDialogue.Instance.stringList.Add(textToLog);
	    }

        public void RunCutScene(string cutscene)
        {
            string cutSceneSwitch = cutscene;
            switch (cutSceneSwitch)
            {
                case "open":
                    CutsceneText("Hello...");
                    CutsceneText("You’re an adventurer right?");
                    CutsceneText("Well time to do your job.");
                    CutsceneText("Your task is simple…");
                    CutsceneText("Explore this cold dark dungeon.");
                    CutsceneText("Find treasure.");  
                    CutsceneText("And locate the exit.");
                    CutsceneText("Oh and one more thing…");
                    CutsceneText("Yeah there’s spooky ghosts and monsters here."); 
                    CutsceneText("Right. Let’s get to it then…");
                    break;
                default:
                    return;
            }

            ScriptedDialogue.Instance.StartScript();
        }

        public void DamageVFX(string colorOverlay)
        {
            AudioEngine.PlaySound("PlayerTakeDamage");
            AudioEngine.PlaySound("PlayerHurt");
            UIController.GetComponent<ShakeBehavior>().TriggerShake("BG");
            UIController.StartScreenFade("red");
        }
    }
}
