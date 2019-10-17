/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstruiSystem
{
    public class GenericFunc
    {
        public float delta = Time.deltaTime;

        public float SmoothLerpFloat(float startValue, float targetValue, float speed)
        {
            float time = 0.0f;
            float value = startValue;
            while (time <= 1f)
            {
                time += delta / speed;
                value = Mathf.Lerp(startValue, targetValue, Mathf.SmoothStep(0, 1, time));
                Debug.Log(value);
            }
            return value;
        }
    }
}
