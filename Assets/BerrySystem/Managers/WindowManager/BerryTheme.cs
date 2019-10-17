/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class BerryTheme
    {
        // Berry Window settings
        public bool shadows = true;
        public Color shadowColor = new Color(0, 0, 0, 1);
        public float shadowOfset = 90;

        public Color backgroundColor = new Color(1, 1, 1, 1);
        public Color forgroundColor = new Color(0, 0, 0, 0);

        public Vector4 borderSize = new Vector4(0, 0, 0, 0);
        public Color borderColorNormal = new Color(0, 0, 0, 0);
        public Color borderColorHover = new Color(0, 0, 0, 0);
        public Color borderColorPressed = new Color(0, 0, 0, 0);

    }
}