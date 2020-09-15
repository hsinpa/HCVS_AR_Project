using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

namespace Expect.View
{
    public class GetToolView : BaseView
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Image image;
        [SerializeField]
        private Text text;

        public delegate void OnButtonConfirm();
        public event OnButtonConfirm Confirm;

        public void ToolView(string message, Sprite sprite)
        {
            text.text = message;
            image.sprite = sprite;

            button.onClick.AddListener(() => Confirm());
        }

        public void RemoveListeners()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
