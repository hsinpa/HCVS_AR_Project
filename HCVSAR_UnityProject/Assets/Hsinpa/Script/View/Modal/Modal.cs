using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.View
{
    public class Modal : BaseView
    {
        /// <summary>
        /// Don't call this function, except from Modal.cs
        /// </summary>
        /// <param name="isShow"></param>
        public override void Show(bool isShow)
        {
            base.Show(isShow);

            if (isShow)
                Modals.instance.EnableBackground(enableModalBG);
        }
    }
}