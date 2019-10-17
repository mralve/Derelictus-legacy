/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */
 
using UnityEngine;

namespace ConstruiSystem
{
    public class BorderObject : MonoBehaviour
    {
        // An refrence to the border parrent object.
        public GameObject borderRef;

        // The reference to the border componet.
        public LineRenderer lineRnedereRef;

        // This creates a border with a set of dimensions.
        public void CreateBorderField(GameObject parrentObject, Color32 borderColor, float borderWidth, int borderX, int borderY)
        {
            if (parrentObject == null) { parrentObject = new GameObject("BorderContainer"); }
            if (gameObject.GetComponent<LineRenderer>() == null)
            {
                lineRnedereRef = parrentObject.AddComponent<LineRenderer>();
            }


        }

        // Here we can update the border
        public void UpdateBorder()
        {

        }

    }
}