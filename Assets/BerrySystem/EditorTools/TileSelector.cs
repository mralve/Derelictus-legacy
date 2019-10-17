/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace ConstruiSystem
{
    public class TileSelector : MonoBehaviour
    {

        public bool tileSelectionEnabled = true;
        public Color tileSelectionColorDisabled = new Color(0, 0, 0, 0), tileSelectionColorEnabled = new Color(255, 255, 255, 100), outsideGrid = new Color(255, 255, 255, 100);

        public Camera EditorCameraReference;
        private Vector3 cursorTransform;

        public float gridSize = 0.32f;

        public GameObject editorGrid;
        public int mapWidth, mapHeight;

        public LineRenderer curLineRenderer;
        public GameObject selectorShadow;

        void Awake()
        {
            mapHeight = 32; // GET THE TILE SIZE FROM LOADED MAP!
            mapWidth = 32;
            EditorCameraReference = CameraManager.curCamera;
            if (curLineRenderer == null)
            {
                curLineRenderer = gameObject.AddComponent<LineRenderer>();
                curLineRenderer.receiveShadows = false;
                curLineRenderer.widthMultiplier = 0.01f;
                curLineRenderer.useWorldSpace = false;
                curLineRenderer.positionCount = 4;
                Vector3[] linePoisons = new Vector3[4];
                linePoisons[0] = new Vector3(0.32f + 0.005f, 0, 0.005f);
                linePoisons[1] = new Vector3(0.32f + 0.005f, 0, -0.32f - 0.005f);
                linePoisons[2] = new Vector3(-0.005f, 0, -0.32f + -0.005f);
                linePoisons[3] = new Vector3(-0.005f, 0, 0.005f);

                curLineRenderer.SetPositions(linePoisons);
                curLineRenderer.loop = true;
                curLineRenderer.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
            }
        }
        /*
		 */
        public void UpdateCamObj()
        {
            EditorCameraReference = CameraManager.curCamera;
        }

        public void SetColor(Color32 targetColor)
        {
            curLineRenderer.startColor = targetColor;
            curLineRenderer.endColor = targetColor;
        }

        public void SetTileSelectorActive(bool setActive)
        {
            if (setActive)
            {
                curLineRenderer.enabled = true;
                tileSelectionEnabled = true;
            }
            else
            {
                curLineRenderer.enabled = false;
                tileSelectionEnabled = false;
            }
        }

        void Update()
        {
            cursorTransform = EditorCameraReference.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log(SessionManager.TileNrId(new Vector3(cursorTransform.x + 4.96f + 0.16f, cursorTransform.y, cursorTransform.z + 4.96f + 0.16f)));
            /*
			 */
            cursorTransform.x = (float)System.Math.Round(cursorTransform.x / gridSize) * gridSize - gridSize * 0.5f;
            cursorTransform.z = (float)System.Math.Round(cursorTransform.z / gridSize) * gridSize + gridSize * 0.5f;
            cursorTransform.y = 0.2f;
            /*
			cursorTransform = SessionManager.ChunkClusterPoissonId(cursorTransform);
			 */
            //Debug.Log(SessionManager.ChunkPoissonId(cursorTransform));
            transform.position = cursorTransform;
        }
    }
}
