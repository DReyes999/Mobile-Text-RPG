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

        /* Public method which allows us to pass a string of text to journal
         * And then add that string of text to our text element in the UI*/
        public void Log(string text)
        {
            //this.logText.text += "\n" + text;
            this.logText.text += "\n" + text + "\n";
            
            /* Typewrite Effect Code */
            //logText.GetComponent<TypeWriterEffect>().fullText = text;
            //TypeWriterEffect.Instance.StartShowText();
        }

        public void RegisterActiveText(string text)
        {
            StartCoroutine(IRegisterActiveText(text));
        }

        public IEnumerator IRegisterActiveText(string text)
        {
            //Debug.Log("Ienumerator called");
            UIController.isTextDisplaying = true;
            // Register the player action in the active text box
            //this.activeText.text = string.Format("<color=yellow>{0} </color>", text);
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
            //yield return new WaitForSeconds(1);
        }

         IEnumerator DisplayActiveText(string textToDisplay)
         {
             
             for (int i = 0; i < textToDisplay.Length; i++)
            {
                
                this.activeText.text += string.Format("<color=yellow>{0}</color>", textToDisplay[i]);
                yield return new WaitForSeconds(textDisplaySpeed);

            }
            UIController.isTextDisplaying = false;
         }

    }
}
