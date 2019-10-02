using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace TextRPG
{
    public class Journal : MonoBehaviour
    {
        [SerializeField]
        Text activeText;

        public TextMeshProUGUI logText;
        public static Journal Instance { get; set; }
        public UIController UIController;

        [HideInInspector]
        public float textDisplaySpeed = UIController.textDisplaySpeed,
                     textPostSpeed = UIController.textPostSpeed;

        //public bool isTextdisplaying = UIController.isTextDisplaying;

        // Use this for initialization
        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }

        
        public void Log(string text)
        {
            /* Public method which allows us to pass a string of text to journal
            * And then add that string of text to our text element in the UI*/
           
            this.logText.text += "\n" + text + "\n";
        }

        public void RegisterActiveText(string text)
        {
            StartCoroutine(IRegisterActiveText(text));
            UIController.player.encounter.toggleMovementControls();
        }

        public IEnumerator IRegisterActiveText(string text)
        {
            textDisplaySpeed = UIController.textDisplaySpeed;
            
            
            UIController.isTextDisplaying = true;

            // Register the player action in the active text box
            StartCoroutine(DisplayActiveText(text));

            while(UIController.isTextDisplaying == true)
            {
             yield return null;
             
            }
            // Wait before adding it to the journal
            yield return new WaitForSeconds(textPostSpeed);
            Log(this.activeText.text);

            // Clear the active text field
            this.activeText.text = "";
            
        }

         IEnumerator DisplayActiveText(string textToDisplay)
         {
             Debug.Log("textdispayspeed = " + UIController.textDisplaySpeed);
             for (int i = 0; i < textToDisplay.Length; i++)
            {
                
                this.activeText.text += string.Format("<color=yellow>{0}</color>", textToDisplay[i]);
                if (textDisplaySpeed >0)
                    yield return new WaitForSecondsRealtime(textDisplaySpeed);

            }
            UIController.isTextDisplaying = false;
         }

    }
}
