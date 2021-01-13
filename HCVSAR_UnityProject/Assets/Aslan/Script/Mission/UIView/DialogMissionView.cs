using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Expect.View
{
    public class DialogMissionView : BaseView
    {
        [Header("Dialog Text")]
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text messageText;
        [SerializeField]
        private Image characterImage;
        [SerializeField]
        private AudioSource typeSound;

        [HideInInspector]
        public string currentText = "";
        [HideInInspector]
        public bool isTyping;

        private float delay = 0.08f;

        public void DialogView(string name, string message, Sprite sprite)
        {
            isTyping = true;

            nameText.text = name;
            //messageText.text = message;
            StartCoroutine(TypeText(message));
            characterImage.sprite = sprite;
        }

        IEnumerator TypeText(string message)
        {
            if (!isTyping) yield break;

            for (int i = 0; i <= message.Length; i++)
            {
                currentText = message.Substring(0, i);
                messageText.text = currentText;
                typeSound.Play();
                yield return new WaitForSeconds(delay);
            }

            isTyping = false;
        }

        public void TypeInit()
        {
            StopAllCoroutines();
            isTyping = false;
            currentText = "";
            messageText.text = currentText;
            typeSound.Pause();
        }

        public void DialogViewForMission6(string name, string message, Sprite sprite)
        {
            isTyping = true;

            nameText.text = name;
            messageText.text = message;
            characterImage.sprite = sprite;
        }
    }
}
