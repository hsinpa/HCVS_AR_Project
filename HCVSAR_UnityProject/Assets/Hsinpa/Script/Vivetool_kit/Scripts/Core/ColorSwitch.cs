// ========================================================================== //
//
//  class ColorSwitch
//  -----
//  Purpose: Changes the color of a Graphic element based on the isOn state
//
//  Usage: Most commonly used to animate a toggle element
//
//
//  Created: 2017-12-04
//  Updated: 2017-12-04
//
//  Copyright 2017 HTC America Innovation
// 
// ========================================================================== //
using UnityEngine;
using UnityEngine.UI;

namespace Htc.ViveToolkit
{
    [RequireComponent(typeof(Graphic))]
    public class ColorSwitch : BaseBehaviour
    {
        #region Types and enums

        public enum Mode
        {
            ColorAndAlpha,
            Color,
            Alpha
        }

        #endregion

        #region Unity interface

        [SerializeField]
        private Mode mode;

        [SerializeField]
        private Color onColor = Color.white;

        [SerializeField]
        private Color offColor = Color.black;

        [SerializeField]
        private bool _isOn;
        public bool isOn
        {
            get { return _isOn; }
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    OnStateChanged();
                }
            }
        }

        // Required Component getters
        private Graphic _graphic;
        private Graphic graphic { get { if (_graphic == null) _graphic = GetComponent<Graphic>(); return _graphic; } }

        #endregion


        #region Private and protected methods

        private void OnStateChanged()
        {
            if (isOn)
                SetColor(onColor);
            else
                SetColor(offColor);
        }

        private void SetColor(Color color)
        {
            if (graphic != null)
            {
                switch (mode)
                {
                    case Mode.ColorAndAlpha:
                        graphic.color = color;
                        break;
                    case Mode.Color:
                        {
                            var col = color;
                            col.a = graphic.color.a;
                            graphic.color = col;
                        }
                        break;
                    case Mode.Alpha:
                        {
                            var col = graphic.color;
                            col.a = color.a;
                            graphic.color = col;
                        }
                        break;
                }
            }
        }

        #endregion


        #region MonoBehaviour

        private void Start()
        {
            OnStateChanged();
        }

        private void OnValidate()
        {
            OnStateChanged();
        }

        #endregion
    }
}