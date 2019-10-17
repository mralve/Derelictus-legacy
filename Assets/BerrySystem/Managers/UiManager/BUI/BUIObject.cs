/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public struct BUIObject
    {
        public string displayName;
        public bool useRayCast, isStatic;
        public Vector2 pivot, size, position;
        public UiManager.UiAnchorsMode uiAnchorMode;

        public void SetDefaults()
        {
            displayName = "Default display name";
            useRayCast = false;
            isStatic = false;
            pivot = new Vector2(0.5f, 0.5f);
            size = new Vector2(0, 0);
            position = new Vector2();
            uiAnchorMode = UiManager.UiAnchorsMode.MiddelCenter;
        }
    }
}