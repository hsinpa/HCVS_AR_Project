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

        [HideInInspector]
        public bool isHash;

        public void SetTitle(string titleString, bool isHash = false) {
            this.isHash = isHash;

            if (titleTxt != null)
                titleTxt.text = titleString;

            _inputField.contentType = (isHash) ? InputField.ContentType.Password : InputField.ContentType.Standard;
        }

        public void Erase() {
            _inputField.text = "";
        }

    }
}
