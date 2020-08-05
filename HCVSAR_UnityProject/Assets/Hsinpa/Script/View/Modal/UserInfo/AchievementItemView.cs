using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class AchievementItemView : MonoBehaviour
    {
        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Image dotImage;

        [SerializeField]
        private Color defaultColor;

        public bool hashed = false;

        public void SetTitle(string title, bool highlight) {

            titleText.text =  (hashed && !highlight) ? GetHashText() : title;

            dotImage.enabled = highlight;

            titleText.color = (highlight) ? dotImage.color : defaultColor;
        }

        private string GetHashText() {
            return "????????????????";
        }
    }
}