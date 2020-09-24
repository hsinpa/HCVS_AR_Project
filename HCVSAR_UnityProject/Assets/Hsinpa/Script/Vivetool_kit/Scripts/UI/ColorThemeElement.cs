// ========================================================================== //
//
//  class ColorThemeElement
//  -----
//  Purpose: Drive the color/color block of a Graphic/Selectable component
//      from one of the color theme presets
//
//
//  Created: 2017-04-04
//  Updated: 2017-04-04
//
//  Copyright 2017 Yu-hsien Chang
// 
// ========================================================================== //
using UnityEngine;
using UnityEngine.UI;

namespace Htc.ViveToolkit.UI
{
    public class ColorThemeElement : BaseBehaviour
    {
        #region Types and enums

        public enum Target
        {
            Graphic,
            Selectable
        }

        #endregion


        #region Unity interface

        [Tooltip("Select the target component")]
        [SerializeField]
        private Target _target;
        public Target target
        {
            get { return _target; }
            private set { _target = value; }
        }

        [SerializeField]
        private string _colorName;
        public string colorName
        {
            get { return _colorName; }
            private set { _colorName = value; }
        }

        #endregion


        #region Properties and fields

        private Selectable _selectable;
        private Selectable selectable { get { if (_selectable == null) _selectable = GetComponent<Selectable>(); return _selectable; } }

        private Graphic _graphic;
        private Graphic graphic { get { if (_graphic == null) _graphic = GetComponent<Graphic>(); return _graphic; } }

        public Color color { get; set; }
        public ColorBlock colorBlock { get; set; }

        #endregion


        #region Public methods

        public void Refresh()
        {
            if (enabled)
            {
                switch (target)
                {
                    case Target.Graphic:
                        if (graphic != null && !graphic.color.Equals(color))
                            graphic.color = color;
                        break;

                    case Target.Selectable:
                        if (selectable != null && !selectable.colors.Equals(colorBlock))
                            selectable.colors = colorBlock;
                        break;
                }
            }
        }

        #endregion


        #region Private and protected methods

        #endregion


        #region MonoBehaviour

        private void Awake()
        {
            // Self destruction on awake
            Destroy(this);
        }

        private void Start()
        {
            // Keep this to show the enabled toggle
        }

        #endregion
    }
}