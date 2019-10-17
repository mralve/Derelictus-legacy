/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class UiObj : MonoBehaviour
    {
        GameObject parrentObj;
        string objName;
        Vector2 pos;
        Vector2 size;
        bool staticObj;
        public enum UiAnchorsMode
        {
            FillStretch,
            TopLeft,
            TopCenter,
            TopRight,
            TopStretchHorizontal,
            TopStretchVertical,
            MiddelLeft,
            MiddelLeftStretchVertical,
            MiddelCenter,
            MiddelRight,
            MiddelStretchHorizontal,
            MiddelStretchVertical,
            BottomLeft,
            BottomCenter,
            BottomRight,
            BottomStretchHorizontal,
            BottomStretchVertical
        }
    }
}