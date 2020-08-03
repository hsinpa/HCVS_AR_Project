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

        private void SetTitle(string title, bool highlight) {
            titleText.text = title;
            dotImage.enabled = highlight;

            titleText.color = (highlight) ? dotImage.color : defaultColor;
        }
    }
}