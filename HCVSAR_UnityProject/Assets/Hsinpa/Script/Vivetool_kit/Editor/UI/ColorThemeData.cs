// ========================================================================== //
//
//  class ColorThemeData
//  -----
//  Purpose: Serializable class storing all Color Theme data
//
//
//  Created: 2017-04-04
//  Updated: 2017-04-09
//
//  Copyright 2017 Yu-hsien Chang
// 
// ========================================================================== //
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Htc.ViveToolkit.UI
{
    public class ColorThemeData : ScriptableObject
    {
        public const string AssetName = "ColorTheme";

        [System.Serializable]
        public class NamedColor
        {
            public string name = "New Color";
            public Color color = Color.white;
        }

        [System.Serializable]
        public class NamedColorBlock
        {
            public string name = "New ColorBlock";
            public ColorBlock colorBlock = ColorBlock.defaultColorBlock;
        }

        public List<NamedColor> colors = new List<NamedColor>();
        public List<NamedColorBlock> colorBlocks = new List<NamedColorBlock>();
    }
}