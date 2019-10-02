using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * UI Controller is where we define our events for the UI.
 * We talk to those events using the Encounter or Player class depending on what we're doing.
 * For example when the player or enemy takes damage, this Class 
 * will ensure that the UI is updated to reflect that.
 * 
 */

namespace TextRPG
{
    public class UIController : MonoBehaviour
    {
        // Reference to player
        [SerializeField]
        public Player player;

        // Reference to various text fields we need to update
        [SerializeField]
        Text playerENGText,
            playerDEFText,
            playerGOLDText,
            enemyENGText,
            enemyDEFText,
            enemyNAMEText,
            playerInventoryText,
            roomIndexText;

        public GameObject controlPanel, yesNoPanel, EnemyStatsPanel, redOverlay, eRedOverlay;

        /* Defining a public delegate for an event handling system */

        /* We need to be able to reference the player information
         * and so we create a parameter that is passed to the delegate. 
         * The  methods subscribed to that delegate must also have this signature.
         * 
         * If we have multiple players we might pass this parameter as described above.
         * Since the player will always be player we are going to pass it as a public variable
         */
        public delegate void OnPlayerUpdateHandler();
        public static OnPlayerUpdateHandler OnPlayerStatChange;
        public static OnPlayerUpdateHandler OnPlayerInventoryChange;

        public delegate void OnEnemyUpdateHandler(Enemy enemy);
        public static OnEnemyUpdateHandler OnEnemyUpdate;

        public Image black, red, eRed;
        public Animator blackAnim, redAnim, cutSceneAnim, eRedAnim;

        public static float textDisplaySpeed = 0.001f, textPostSpeed = 1.0f;

        [HideInInspector]
        public static bool isTextDisplaying = false;


        void Awake()
        {
            OnPlayerStatChange += UpdatePlayerStats;
            OnPlayerInventoryChange += UpdatePlayerInventory;
            OnEnemyUpdate += UpdateEnemyStats;
        }

        public void UpdatePlayerStats()
        {
            //Debug.Log("UpdatePlayerStats called");
            // TODO: Turn health into a graphical bar. Put stats in separate UI
            playerENGText.text = player.Energy.ToString();
            playerDEFText.text = player.Defence.ToString();
            playerGOLDText.text = player.Gold.ToString();
        }

        public void UpdatePlayerInventory()
        {
            //TODO: Add functionality for removing items from the inventory. Use a list or dictionaries.
            playerInventoryText.text = "Inventory: ";
            foreach (string item in player.Inventory)
            {
                playerInventoryText.text += "\n" + item + "\n";
            }
        }

        public void UpdateEnemyStats(Enemy enemy)
        {
            if (enemy)
            {
                enemyENGText.text = enemy.Energy.ToString();
                enemyDEFText.text = enemy.Defence.ToString();
                enemyNAMEText.text = enemy.Description;
            }
            else
            {
                enemyENGText.text = "";
                enemyDEFText.text = "";
                enemyNAMEText.text = "";
            }
            
        } 

        public void UpdateRoom(Vector2 room)
        {
            roomIndexText.text = string.Format("{0},{1}", (int)room.x, (int)room.y);
        }

        public void QuitGame()
        {
            Debug.Log("Game was Quit");
            Application.Quit();
        }

        public void GoMainMenu()
        {
            SceneManager.LoadScene("TitleScreen");
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene("Game");
        }

        public void OnDestroy()
        {
            OnPlayerStatChange -= UpdatePlayerStats;
            OnPlayerInventoryChange -= UpdatePlayerInventory;
            OnEnemyUpdate -= UpdateEnemyStats;
        }

        public void ToggleSetActive(GameObject gameObject)
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        public void ToggleInteractable(Button button)
        {
            button.interactable = (!button.interactable);
        }

        
        public void StartScreenFade(string typeOfFade)
        {
            StartCoroutine(Fading(typeOfFade));
        }

        IEnumerator Fading(string typeOfFade)
        {
            switch (typeOfFade)
            {
                case "red":
                    ToggleSetActive(redOverlay);
                    redAnim.SetTrigger("FadeOut");
                    yield return new WaitUntil(() => red.color.a == 0);
                    while (red.color.a > 0)
                    {
                        yield return null;
                    }
                    redAnim.SetTrigger("FadeOut");
                    ToggleSetActive(redOverlay);
                    break;
                case "ERed":
                    ToggleSetActive(eRedOverlay);
                    eRedAnim.SetTrigger("FadeOut");
                    yield return new WaitUntil(() => eRed.color.a == 0);
                    while (eRed.color.a > 0)
                    {
                        yield return null;
                    }
                    eRedAnim.SetTrigger("FadeOut");
                    ToggleSetActive(eRedOverlay);
                    break;
                case "black":
                    blackAnim.SetBool("Fade", !blackAnim.GetBool("fade"));
                    yield return new WaitUntil(() => black.color.a == 1);
                    break;
                
            }
            
            // anim.SetBool("Fade", !anim.GetBool("fade"));
            // yield return new WaitUntil(() => black.color.a == 1);
        }

        public void CutSceneFade(GameObject cutscenepanel)
        {
            StartCoroutine(CutSceneFading(cutscenepanel));
            
        }

        IEnumerator CutSceneFading(GameObject cutscenepanel)
        {
            cutSceneAnim.SetBool("CutSceneFade", !cutSceneAnim.GetBool("CutSceneFade"));
            yield return new WaitForSeconds(2);
            ToggleSetActive(cutscenepanel);
        }

        public void AdjustTextDisplaySpeed(float newspeed)
        {
            textDisplaySpeed = newspeed;
        }
    }
}
