using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;

namespace Expect.View
{

    public class SituationMissionView : BaseView
    {
        [Header("Situation Text")]
        [SerializeField]
        private Text situationText;

        public void SituationView(string message)
        {
            situationText.text = message;
        }
    }
}
