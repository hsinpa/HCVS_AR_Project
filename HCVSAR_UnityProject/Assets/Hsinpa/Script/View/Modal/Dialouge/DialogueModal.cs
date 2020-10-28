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
        private Dropdown _dropDownMenu;
        public Dropdown dropDownMenu => _dropDownMenu;

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

        public void SetDropDown(string[] options) {
            dropDownMenu.ClearOptions();

            if (options != null && options.Length > 0) {
                dropDownMenu.gameObject.SetActive(true);

                var optionItems = options.Select(x => new Dropdown.OptionData(x)).ToList();
                dropDownMenu.AddOptions(optionItems);
            }
        }

        private void RegisterButtons(ButtonType[] allowBtns, System.Action<ButtonType> btnEvent) {
            int btnlength = allowBtns.Length;

            for (int i = 0; i < btnlength; i++) {
                var DialogueButtonObj = buttons.Find(x => x.type == allowBtns[i]);

                DialogueButtonObj.gameObject.SetActive(true);

                DialogueButtonObj.SetBtnEvent((ButtonType type) =>
                {
                    Modals.instance.Close();

                    btnEvent(type);
                });
            }
        }

        private void ResetContent() {
            DecorateSideImage(null);

            dropDownMenu.gameObject.SetActive(false);

            buttons.ForEach(x => x.gameObject.SetActive(false));
        }


    }
}