// ========================================================================== //
//
//  class Console
//  -----
//  Purpose: Represents a console text field
//
//  Usage: Place with a Text component
//
//
//  Created: 2017-12-06
//  Updated: 2017-12-06
//
//  Copyright 2017 HTC America Innovation
// 
// ========================================================================== //
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Htc.ViveToolkit
{
    [RequireComponent(typeof(Text))]
    public class Console : MonoBehaviour
    {
        static Console instance;

        static List<string> texts = new List<string>();

        Text _textField;
        Text textField { get { if (_textField == null) _textField = GetComponent<Text>(); return _textField; } }

        public int maximumLines = 10;

        public static void Log(string text)
        {
            if (instance != null)
            {
                texts.Add(text);

                while (texts.Count > instance.maximumLines)
                    texts.RemoveAt(0);

                instance.textField.text = string.Join("\n", texts.ToArray());
            }
        }

        public static void Clear()
        {
            if (instance != null)
            {
                texts.Clear();
                instance.textField.text = string.Empty;
            }
        }

        void Awake()
        {
            instance = this;
            Log("[Console initialized]");
        }
    }
}