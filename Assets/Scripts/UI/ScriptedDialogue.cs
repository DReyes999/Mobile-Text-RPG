using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TextRPG
{
    public class ScriptedDialogue : MonoBehaviour 
    {
        
        public static ScriptedDialogue Instance {get; set;}
        public Text _textField; //logTextField;
        public TMP_Text logTextField;
        public GameObject continueButton, CutScenePanel; //typeWriterPanel;
        public UIController UIController;
        
        public List<string> stringList = new List<string>();
        
        public float delay = UIController.textDisplaySpeed;
        [HideInInspector]
        public bool isTextDisplaying = false;
        private bool isPlayerInScriptedText = false;
        private bool isEndOfList = false;
        [HideInInspector]
        public bool pressedContinue = false;
        [HideInInspector]
        public KeyCode DialogueInput = KeyCode.Space;
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            Destroy(this.gameObject);
            else
            Instance = this;
        }
        
        public void FlipPressedContinue()
        {
            pressedContinue = true;
        }
        
        public void HideContinueButton()
        {
            continueButton.SetActive(false);
        }
        
        public void ShowContinueButton()
        {
            continueButton.SetActive(true);
        }
        
        public void EndScriptedText()
        {
            
            UIController.CutSceneFade(CutScenePanel);
        }
        public void StartScript()
        {
            if (!isPlayerInScriptedText)
            {
                //Debug.Log("Start Script Called");
                isPlayerInScriptedText = true;
                HideContinueButton();
                StartCoroutine(StartScriptedText());
            }
        }
        
        private IEnumerator StartScriptedText()
        {
            int listLength = stringList.Count;
            //Debug.Log(stringList.Count);
            int currentScriptIndex = 0;
            isEndOfList = false;
            
            while (currentScriptIndex < listLength)
            {
                if(!isTextDisplaying)
                {
                    isTextDisplaying = true;
                    HideContinueButton();
                    //Debug.Log(currentScriptIndex);
                    StartCoroutine(DisplayText(stringList[currentScriptIndex]));
                    currentScriptIndex++;
                    
                    if (currentScriptIndex >= listLength)
                    {
                        isEndOfList = true;
                    }
                }
                
                yield return 0;
            }
            
            Debug.Log("end of Scripted Text");
            stringList.Clear();
            isPlayerInScriptedText = false;
            
        }
        
        private IEnumerator DisplayText(string textToDisplay)
        {
            _textField.text = "";
            //Debug.Log(textToDisplay);
            
            for (int i = 0; i < textToDisplay.Length; i++)
            {
                logTextField.text += textToDisplay[i];
                yield return new WaitForSeconds(delay);
            }
            
            ShowContinueButton();
            
            while (true)
            {
                if(pressedContinue)
                {
                    //Debug.Log("text button was pressed");
                    break;
                }
                yield return 0;
            }
            
            isTextDisplaying = false;
            pressedContinue = false;
            logTextField.text += textToDisplay + "\n";
            
            if (isEndOfList)
            {
                HideContinueButton();
                EndScriptedText();
                
            }
            logTextField.text = "";
            
            yield return 0;
            
        }
    }

}