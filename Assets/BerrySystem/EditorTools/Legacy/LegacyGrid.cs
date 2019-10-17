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
    public class LegacyGrid : MonoBehaviour
    {

        private GameObject lineOBJ;
        private GameObject GridContainer;
        private GameObject WorldOutline;

        public Color normalLineColor = new Color32(0, 192, 255, 77);
        public Color edgeLinesColor = new Color32(255, 56, 143, 186);
        public Color centerLineColor = new Color32(111, 255, 255, 167);

        private Color curColor;
        private bool isMapEdgeLine;

        public int mapWidth = 45;
        public int mapHeight = 45;
        public float gridSize = 0.96f;
        public float deafultLineWidth = 0.01f;

        public bool combineLinesToMesh;

        public Camera CameraRef;

        EditorPrimer curPrimer;


        // Use this for initialization
        void Start()
        {
            curPrimer = ScenePrimer.curPrimerParrentObj.GetComponent<EditorPrimer>();
            CameraRef = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
            DrawGrid();
        }

        // Update is called once per frame
        void Update()
        {
            //manageLineScale();
            if (Input.GetButtonUp("gridToggel"))
            {
                if (GridContainer.activeInHierarchy)
                {
                    GridContainer.SetActive(false);
                }
                else
                {
                    GridContainer.SetActive(true);
                }
                curPrimer.curGridBtn.IconUpdate();
            }
        }
        void DrawGrid()
        {
            Destroy(GridContainer);
            Destroy(WorldOutline);

            // Switch to the new and faster grid.
            if (combineLinesToMesh)
            {

                // TODO: Front to new line renderer
            }
            else
            {

                // Create the grid line's parrents (Containers)
                GridContainer = new GameObject("XEditor_GridContainer");
                GridContainer.transform.SetParent(this.transform);
                GridContainer.isStatic = true;

                WorldOutline = new GameObject("XEditor_WorldOutline");
                WorldOutline.transform.SetParent(this.transform);
                WorldOutline.isStatic = true;
            }

            // Generate all the grid lines in Z
            for (int z = 0; z < mapHeight + 1; z++)
            {

                if (z == mapHeight / 2)
                {
                    curColor = centerLineColor;
                    isMapEdgeLine = false;
                }
                else
                {
                    if (z % 2 == 0)
                    {
                        curColor = normalLineColor;
                        isMapEdgeLine = false;
                    }
                    else
                    {
                        curColor = normalLineColor;
                        isMapEdgeLine = false;
                    }
                }

                if (z == 0 || z == mapHeight)
                {
                    curColor = edgeLinesColor;
                    isMapEdgeLine = true;
                }

                CreateLine(new Vector2(-deafultLineWidth / 2 - gridSize / 2, z * gridSize - gridSize / 2), new Vector2(mapWidth * gridSize + deafultLineWidth / 2 - gridSize / 2, z * gridSize - gridSize / 2), true, z, curColor, isMapEdgeLine);

            }
            // Generate all the grid lines in X
            for (int x = 0; x < mapWidth + 1; x++)
            {
                if (x == mapHeight / 2)
                {
                    curColor = centerLineColor;
                    isMapEdgeLine = false;
                }
                else
                {
                    if (x % 2 == 0)
                    {
                        curColor = normalLineColor;
                        isMapEdgeLine = false;
                    }
                    else
                    {
                        curColor = normalLineColor;
                        isMapEdgeLine = false;
                    }
                }
                if (x == 0 || x == mapWidth)
                {
                    curColor = edgeLinesColor;
                    isMapEdgeLine = true;
                }

                CreateLine(new Vector2(x * gridSize - gridSize / 2, -deafultLineWidth / 2 - gridSize / 2), new Vector2(x * gridSize - gridSize / 2, mapHeight * gridSize + deafultLineWidth / 2 - gridSize / 2), false, x, curColor, isMapEdgeLine);

            }

        }

        void CreateLine(Vector2 lineStart, Vector2 lineEnd, bool isX, int gridIndex, Color targetColor, bool isMapBorderLine)
        {

            if (combineLinesToMesh)
            {

                // TODO: Front to new line renderer
            }
            else
            {

                lineOBJ = new GameObject("gridLine" + gridIndex);
                LineRenderer curLineObj = lineOBJ.AddComponent<LineRenderer>();
                curLineObj.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
                curLineObj.startColor = targetColor;
                curLineObj.endColor = targetColor;
                curLineObj.startWidth = deafultLineWidth;
                curLineObj.endWidth = deafultLineWidth;
                curLineObj.SetPosition(0, new Vector3(lineStart.x, 0.05f, lineStart.y));
                curLineObj.SetPosition(1, new Vector3(lineEnd.x, 0.05f, lineEnd.y));

                lineOBJ.AddComponent<LegacyLineScaler>();
                if (isMapBorderLine)
                {
                    lineOBJ.transform.SetParent(WorldOutline.transform);

                }
                else
                {
                    lineOBJ.transform.SetParent(GridContainer.transform);

                }

                lineOBJ.isStatic = true;
                lineOBJ = null;
            }
        }

        void ManageLineScale()
        {

            if (combineLinesToMesh)
            {

                // TODO: Front to new line renderer
            }
            else
            {
                float curZoom = CameraRef.orthographicSize;

                int lines = GridContainer.transform.childCount;
                float newLineWidth;

                for (int i = 0; i < lines; i++)
                {
                    Transform curGridLine = GridContainer.transform.GetChild(i);

                    if (curZoom > 2)
                    {
                        newLineWidth = curZoom / 240;
                    }
                    else
                    {
                        newLineWidth = deafultLineWidth;
                    }
                    LineRenderer curLineRenderer = curGridLine.GetComponent<LineRenderer>();
                    curLineRenderer.startWidth = newLineWidth;
                    curLineRenderer.endWidth = newLineWidth;

                }
            }

        }

    }
}
