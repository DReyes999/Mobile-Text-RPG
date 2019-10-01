using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TextRPG
{
    public class TypeWriterEffect02 : MonoBehaviour
    {
        public Text displayedText;
        public Text inputText;
        public Text italicText;

        private string outputString = null;
        private int i = -1;
        private bool done;
        private bool changed;

        public float charsPerSec = 10;

        private void Awake()
        {
            BeginTypeWrite(inputText.text);
        }
        void Update()
        {
            //if(!done)
            //{
            //    displayedText.text = TypeWrite(inputText.text);
            //}
            //else
            //{
            //    if(!changed)
            //    {
            //        displayedText.text = italicText.text;
            //        changed = true;
            //    }
            //}

            //if (!done)
            //{
            //    if (!changed)
            //    {
            //        displayedText.text = italicText.text;
            //        changed = true;
            //    }
            //}
        }

        //private string TypeWrite(string text)
        //{
        //    i++;
        //    char[] characters = text.ToCharArray();
        //    outputString = outputString + characters[i].ToString();
        //    if (i == (characters.Length - 1))
        //    {
        //            done = true;
        //    }
        //    return outputString;
        //}

        private void BeginTypeWrite(string text)
        {
            char[] characters = text.ToCharArray();
            StartCoroutine(Type(characters, charsPerSec));
        }

        private IEnumerator Type(char[] chars, float charsPerSec)
        {
            i++;
            outputString = outputString + chars[i].ToString();
            displayedText.text = outputString;
            yield return new WaitForSeconds(1 / charsPerSec);

            if (i == chars.Length - 1)
            {
                done = true;
            }
            else
            {
                StartCoroutine(Type(chars, charsPerSec));
            }

        }
    }
}
