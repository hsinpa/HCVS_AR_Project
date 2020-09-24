// ========================================================================== //
//
//  class BaseBehaviour
//  -----
//  Purpose: Base class of all ViveToolkit UI classes
//
//  Usage:
//    - Removes the script field in the inspector
//    - Implements IBaseBehaviour for mocking
//
//
//  Created: 2017-07-17
//  Updated: 2017-07-17
//
//  Copyright 2017 HTC America Innovation
// 
// ========================================================================== //
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Htc.ViveToolkit
{
    public class BaseUIBehaviour : UIBehaviour, IBaseBehaviour
    {
        // CanvasGroup
        private readonly List<CanvasGroup> canvasGroupCache = new List<CanvasGroup>();
        private bool _groupsAllowInteraction = true;
        protected bool groupsAllowInteraction
        {
            get { return _groupsAllowInteraction; }
            private set { _groupsAllowInteraction = value; }
        }

        protected override void OnCanvasGroupChanged()
        {
            bool flag = true;
            Transform transform = base.transform;
            while (transform != null)
            {
                transform.GetComponents<CanvasGroup>(canvasGroupCache);
                bool flag2 = false;
                for (int i = 0; i < canvasGroupCache.Count; i++)
                {
                    if (!canvasGroupCache[i].interactable)
                    {
                        flag = false;
                        flag2 = true;
                    }
                    if (canvasGroupCache[i].ignoreParentGroups)
                    {
                        flag2 = true;
                    }
                }
                if (flag2)
                {
                    break;
                }
                transform = transform.parent;
            }
            if (flag != groupsAllowInteraction)
            {
                groupsAllowInteraction = flag;
            }
        }
    }
}