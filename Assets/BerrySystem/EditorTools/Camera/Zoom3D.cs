/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace ConstruiSystem
{
    public class Zoom3D : MonoBehaviour
    {

        public float zoomSensitivity = 0.3f, curZoomSensitivity, zoomSpeed = 8.77f, zoomMin = 0.005f, zoomMax = 4000, zoom;
        private float mouseScroll;
        public bool zoomingEnabled = true;
        Camera curCamera;


        void Start()
        {
            curCamera = GetComponent<Camera>();
            zoom = curCamera.orthographicSize;
            curZoomSensitivity = zoomSensitivity;
        }

        void Update()
        {
            if (zoomingEnabled)
            {
                mouseScroll = Input.GetAxis("Mouse ScrollWheel") * curZoomSensitivity;
                curZoomSensitivity = zoomSensitivity + zoom * 0.4f;
                if (mouseScroll <= 0 || mouseScroll >= 0)
                {
                    zoom -= mouseScroll;
                }
                zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
                curCamera.orthographicSize = Mathf.Lerp(curCamera.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
            }
        }
    }
}