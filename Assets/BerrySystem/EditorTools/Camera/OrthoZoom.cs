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
    public class OrthoZoom : MonoBehaviour
    {

        public Camera curCam;
        public GenericFunc curGen;

        void Awake()
        {
            curCam = this.GetComponent<Camera>();
            curGen = new GenericFunc();
        }

        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float velocity = Input.GetAxis("Mouse ScrollWheel");
                Zoom(curCam.orthographicSize, curCam.orthographicSize + velocity, 4004);
            }
        }

        void Zoom(float startValue, float targetValue, float speed)
        {
            float time = 0.0f;
            while (time <= 1f)
            {
                time += Time.deltaTime / speed;
                curCam.orthographicSize = Mathf.SmoothStep(-1, 1, time);
                //Debug.Log(time);
            }
        }
    }
}
