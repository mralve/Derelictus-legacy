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
    public class FastTileSelector : MonoBehaviour
    {

        public bool tileSelectionEnabled = true;
        public Color tileSelectionColorDisabled = new Color(0, 0, 0, 0);
        public Color tileSelectionColorEnabled = new Color(255, 255, 255, 100);
        public Color outsideGrid = new Color(255, 255, 255, 100);

        public Camera EditorCameraReference;
        private Vector3 cursorTransform;

        public float gridSize = 0.32f;

        public GameObject editorGrid;
        public int mapWidth;
        public int mapHeight;

        public LineGenerator curLineRenderer;
        public GameObject selectorShadow;

        void Start()
        {
            EditorCameraReference = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
            mapHeight = 32; // GET THE TILE SIZE FROM LOADED MAP!
            mapWidth = 32;
            if (curLineRenderer == null)
            {
                curLineRenderer = gameObject.AddComponent<LineGenerator>();
                FastLineData testLine = new FastLineData();
                testLine.linesVectors = new Vector3[2];
                testLine.linesVectors[0] = new Vector3(-0.32f, 0, 0);
                testLine.linesVectors[1] = new Vector3(0.32f, 0, 0);
                testLine.widthMultiplyer = 0.05f;
                testLine.lineMaterial = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
                testLine.lineColor = new Color();
                curLineRenderer.BuildMesh(testLine);
            }
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

            if (tileSelectionEnabled)
            {
                cursorTransform = EditorCameraReference.ScreenToWorldPoint(Input.mousePosition);
                cursorTransform.y = 0.1f;
                cursorTransform.x = (float)System.Math.Round(cursorTransform.x / gridSize) * gridSize - gridSize * 0.5f;
                cursorTransform.z = (float)System.Math.Round(cursorTransform.z / gridSize) * gridSize + gridSize * 0.5f;
                transform.position = new Vector3(cursorTransform.x * gridSize * 0.5f, 0.1f, cursorTransform.x);
            }
        }
    }
}
