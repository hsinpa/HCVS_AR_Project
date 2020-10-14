using Hsinpa.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class AppStartModal : Modal
    {
        [SerializeField]
        private Button confirmBtn;

        public void SetConfirmBtnEvent(System.Action p_action) {
            confirmBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(() => p_action());
        }

    }
}