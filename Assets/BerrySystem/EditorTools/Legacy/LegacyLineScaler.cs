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
    public class LegacyLineScaler : MonoBehaviour
    {

        Camera cameraRef;
        LineRenderer lineRenderer;
        float curZoom;
        float deafultLineWidth = 1.2f;

        void Start()
        {
            cameraRef = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            curZoom = cameraRef.orthographicSize;
            float curLineWidth = 32;
            if (curZoom > 2)
            {
                curLineWidth = deafultLineWidth * curZoom / 2;
            }
            else
            {
                curLineWidth = deafultLineWidth;
            }

            lineRenderer.widthMultiplier = curLineWidth;
            //lineRenderer.SetWidth(curLineWidth,curLineWidth);

        }
    }
}
