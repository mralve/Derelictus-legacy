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
    public class GridLineScaler : MonoBehaviour
    {

        Camera cameraRef;
        //LineRenderer lineRenderer;
        float curZoom, deafultLineWidth = 1.2f, curLineWidth = 32;
        public bool useX;

        void Start()
        {
            cameraRef = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
            //lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                curZoom = cameraRef.orthographicSize;
                if (curZoom > 2)
                {
                    curLineWidth = deafultLineWidth * curZoom * 0.5f;
                }
                else
                {
                    curLineWidth = deafultLineWidth;
                }
                if (useX)
                {
                    this.transform.localScale = new Vector3(curLineWidth, 1, 1);
                }
                else
                {
                    this.transform.localScale = new Vector3(1, 1, curLineWidth);
                }
            }
        }
    }
}
