using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class DialogueButtonObj : MonoBehaviour
    {
        [SerializeField]
        private DialogueModal.ButtonType _type;

        [SerializeField]
        private Button button;

        public DialogueModal.ButtonType type => _type;

        public void SetBtnEvent(System.Action<DialogueModal.ButtonType> action) {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() => action(type));
        }
    }
}