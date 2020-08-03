using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class DialogueModal : Modal
    {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Text contentText;

        [SerializeField]
        private Image decorateImage;

        public enum ButtonType {
            Accept, Cancel, Close
        }

        public void DecorateSideImage(Sprite sprite) {
            decorateImage.gameObject.SetActive(sprite != null);
            decorateImage.sprite = sprite;
        }

        private List<DialogueButtonObj> buttons = new List<DialogueButtonObj>();

        private void Awake()
        {
            buttons = GetComponentsInChildren<DialogueButtonObj>().ToList();
        }

        public void SetDialogue(string title, string content, ButtonType[] allowBtns, System.Action<ButtonType> btnEvent) {
            ResetContent();

            titleText.text = title;
            contentText.text = content;

            RegisterButtons(allowBtns, btnEvent);
        }

        private void RegisterButtons(ButtonType[] allowBtns, System.Action<ButtonType> btnEvent) {
            int btnlength = allowBtns.Length;

            for (int i = 0; i < btnlength; i++) {
                var DialogueButtonObj = buttons.Find(x => x.type == allowBtns[0]);

                DialogueButtonObj.gameObject.SetActive(true);

                DialogueButtonObj.SetBtnEvent((ButtonType type) =>
                {
                    btnEvent(type);

                    Modals.instance.Close();
                });
            }
        }

        private void ResetContent() {
            DecorateSideImage(null);

            buttons.ForEach(x => x.gameObject.SetActive(false));
        }


    }
}