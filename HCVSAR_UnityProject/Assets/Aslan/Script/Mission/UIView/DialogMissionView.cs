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

        public void DialogView(string name, string message, Sprite sprite)
        {
            nameText.text = name;
            messageText.text = message;
            characterImage.sprite = sprite;
        }
        
    }
}
