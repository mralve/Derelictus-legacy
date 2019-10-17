/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;

namespace ConstruiSystem
{
    public static class CameraManager
    {
        public static GameObject[] CameraRefs;
        public static Camera curCamera;
        public static GameObject CurrentRenderCamera;

        //CameraRect
        public static Resolution LastScreenRes;
        public static Vector2 rectPos = new Vector2(0, 0);
        public static Vector2 rectSizeSubtract = new Vector2(0, 64);
        public static Vector2 rectSizeTarget = new Vector2(1, 1);

        public static GameObject SetupNewCamera(GameObject parentObj = null, string CameraObjName = "Camera", bool setActive = true, bool setCurrent = true, float cameraOrthoSize = 1, Color backgroundColorFill = new Color(), Vector3 spawnPos = new Vector3(), Vector3 startRotation = new Vector3(), bool useRectManager = false)
        {
            GameObject newCamera = new GameObject(CameraObjName);
            Camera curCamRef = newCamera.AddComponent<Camera>();
            newCamera.transform.position = spawnPos;
            newCamera.transform.Rotate(startRotation);
            curCamRef.backgroundColor = backgroundColorFill;
            curCamRef.allowHDR = false;
            curCamRef.allowMSAA = false;
            curCamRef.orthographic = true;
            curCamRef.nearClipPlane = 1;
            curCamRef.farClipPlane = 3;
            curCamRef.orthographicSize = 1;
            curCamRef.depthTextureMode = DepthTextureMode.None;
            curCamRef.renderingPath = RenderingPath.Forward;
            if (parentObj != null)
            {
                newCamera.transform.SetParent(parentObj.transform);
            }
            if (setCurrent)
            {
                curCamera = curCamRef;
                //curCamRef.backgroundColor = MapDataConverter.V3ToColor32( MapDataManager.mapData.mapLayers[MapDataManager.mapDataCurrentLayer].bgColor);
                CurrentRenderCamera = newCamera;
                if (CurrentRenderCamera != null)
                {
                    CurrentRenderCamera.SetActive(false);
                }
            }
            AddCameraReference(newCamera);
            newCamera.SetActive(setActive);
            if (useRectManager)
            {
                newCamera.AddComponent<CameraRectManager>();
            }
            CurrentRenderCamera = newCamera;
            return newCamera;
        }

        public static void AddCameraReference(GameObject cameraRef)
        {
            if (cameraRef.GetComponent<Camera>() != null)
            {
                if (CameraRefs != null)
                {
                    Array.Resize(ref CameraRefs, CameraRefs.Length + 1);
                    CameraRefs[CameraRefs.Length - 1] = cameraRef;
                }
                else
                {
                    CameraRefs = new GameObject[1];
                    CameraRefs[0] = cameraRef;
                }
            }
        }

        public static void UpdateCurrentCamera()
        {
            if (curCamera != null)
            {
                curCamera.backgroundColor = MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor);
                RenderSettings.ambientLight = MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor);
            }
        }

        public static void UpdateCameraRect()
        {
            float width = LastScreenRes.width - rectSizeSubtract.x;
            width = width / LastScreenRes.width;

            float height = LastScreenRes.height - rectSizeSubtract.y;
            height = height / LastScreenRes.height;

            Rect newTargetRect = new Rect();
            newTargetRect.x = rectPos.x;
            newTargetRect.y = rectPos.y;
            newTargetRect.width = width;
            newTargetRect.height = height;
            if (curCamera == null)
            {
                if (CurrentRenderCamera != null)
                {
                    curCamera = CurrentRenderCamera.GetComponent<Camera>();
                }
            }
            curCamera.rect = newTargetRect;
        }
    }

    public class CameraRectManager : MonoBehaviour
    {
        public void FixedUpdate()
        {

            if (Screen.height != CameraManager.LastScreenRes.height)
            {
                CameraManager.LastScreenRes.height = Screen.height;
                CameraManager.UpdateCameraRect();
            }
            if (Screen.width != CameraManager.LastScreenRes.width)
            {
                CameraManager.LastScreenRes.width = Screen.width;
                CameraManager.UpdateCameraRect();
            }
        }
    }

}
