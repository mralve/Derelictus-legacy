/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class BerryLineMesh : MonoBehaviour
    {
        // Mesh Fill
        public bool meshFill;
        public Color32 meshFillColor;
        public Material meshFillMaterial;

        // Line Knobs
        public bool disableKnobs;
        public Color32 knobColor;
        public Material knobsMaterial;

        // Lines
        public bool disableLines;
        public Color32 linesColor;
        public Material linesMaterial;

    }
}