using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextRPG
{
    public class TypeWriterEffect : MonoBehaviour
    {

        public float delay = 0.001f; //speed to wait before showing each new char
        public string fullText; // What will be shown when the text has been completely displayed
        private string currentText = "";
        public Text activeTextField, logTextField;
        public static TypeWriterEffect Instance { get; set; }

        List<string> textBuffer = new List<string>();

        bool done = true;

        void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);
            else
                Instance = this;
        }
        
        void Start()
        {
            //StartCoroutine(ShowText());
        }

        /********* Method 1 **************/

        public void StartShowText()
        {
            /* Take the text and put it in a buffer */
            
            

            /* Feed each member of the list into showtext */
            /* Remove from the list when done */
            StartCoroutine(ShowText());
        }

        IEnumerator ShowText()
        {
            textBuffer.Add(fullText);

            while (textBuffer.Count > 0)
            {
                done = false;
                for (int i = 0; i < textBuffer.Count; i++)
                {
                    Debug.Log(string.Format("textbuffer count: {0}", textBuffer.Count));
                    logTextField.text += textBuffer[i];
                    Debug.Log("Waiting");
                    yield return new WaitForSeconds(2);
                    textBuffer.Remove(textBuffer[i]);
                    Debug.Log(string.Format("textbuffer count: {0}", textBuffer.Count));
                }

            }

            //for (int i = 0; i < fullText.Length; i++)
            //{
            //    currentText = fullText.Substring(0, i);
            //    activeTextField.text = currentText;
            //    yield return new WaitForSeconds(delay);
            //}

            //yield return new WaitForSeconds(2);
            //logTextField.text += fullText;
        }
    }
}
