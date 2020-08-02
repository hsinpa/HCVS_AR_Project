using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View {
    public class LoginInputComponent : MonoBehaviour
    {
        [SerializeField]
        private Text titleTxt;

        public InputField _inputField;

        public void SetTitle(string titleString) {
            if (titleTxt != null)
                titleTxt.text = titleString;
        }

        public void Erase() {
            _inputField.text = "";
        }

    }
}
