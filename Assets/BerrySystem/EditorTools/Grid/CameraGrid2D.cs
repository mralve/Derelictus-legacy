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
    public class CameraGrid2D : MonoBehaviour
    {

        private GameObject lineOBJ, GridContainer, WorldOutline;

        public Color normalLineColor = new Color32(0, 210, 255, 120);
        //public Color subLineColor = new Color32(20, 196, 255, 20);
        //public Color edgeLinesColor = new Color32(255, 56, 143, 186);
        //public Color centerLineColor = new Color32(111, 255, 255, 167);

        private Color curColor;

        public int mapWidth = 32, mapHeight = 32;
        public float gridSize = 0.32f, deafultLineWidth = 0.01f;

        public Camera CameraRef;
        public Vector3 CameraVel = new Vector3(0, 0, 0), CameraPos, gridPos = new Vector3(0, 0, 0);

        public Transform[] lineRef;

        //EditorPrimer curPrimer;

        // Use this for initialization
        void Awake()
        {
            // /*curPrimer = */ScenePrimer.curPrimerParrentObj.GetComponent<EditorPrimer>();
            CameraRef = CameraManager.curCamera;
            DrawGrid();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (CameraRef == null)
            {
                CameraRef = CameraManager.curCamera;
            }
            else
            {
                CameraVel = CameraManager.curCamera.velocity; /// This is broken.!--.!--.!-- thanks unity!
				CameraPos = CameraRef.transform.position;
            }
        }

        void Update()
        {
            if (CameraVel.x != 0 || CameraVel.z != 0)
            {
            }
            gridPos.y = 0.1f;
            gridPos.x = (float)System.Math.Round(CameraPos.x / gridSize) * gridSize;
            gridPos.z = (float)System.Math.Round(CameraPos.z / gridSize) * gridSize;
            transform.position = gridPos;
        }

        public void DrawGrid()
        {
            Destroy(GridContainer);
            Destroy(WorldOutline);

            // Create the grid line's parrents (Containers)
            GridContainer = new GameObject("GridContainer");
            GridContainer.transform.SetParent(this.transform);
            GridContainer.isStatic = false;

            WorldOutline = new GameObject("WorldOutline");
            WorldOutline.transform.SetParent(this.transform);
            WorldOutline.isStatic = false;

            lineRef = new Transform[mapHeight + mapWidth + 2];

            // Generate all the grid lines in Z
            for (int z = 0; z < mapHeight + 1; z++)
            {

                curColor = normalLineColor;

                lineRef[z] = CreateLine(new Vector2(-deafultLineWidth * 0.5f - gridSize * 0.5f + gridSize * mapWidth * 0.5f * -1, gridSize * mapHeight * 0.5f * -1 + z * gridSize - gridSize * 0.5f), new Vector2(gridSize * mapWidth * 0.5f + deafultLineWidth * 0.5f - gridSize * 0.5f, gridSize * mapHeight / 2 * -1 + z * gridSize - gridSize * 0.5f), true, z, curColor).transform;
            }
            // Generate all the grid lines in X
            for (int x = 0; x < mapWidth + 1; x++)
            {

                curColor = normalLineColor;

                lineRef[x + mapHeight + 1] = CreateLine(new Vector2(mapWidth * gridSize * 0.5f * -1 + x * gridSize - gridSize * 0.5f, mapHeight * gridSize * 0.5f * -1 + -deafultLineWidth * 0.5f - gridSize * 0.5f), new Vector2(mapWidth * gridSize * 0.5f * -1 + x * gridSize - gridSize * 0.5f, mapHeight * gridSize * 0.5f + deafultLineWidth - gridSize * 0.5f), false, x, curColor).transform;

            }

        }
        GameObject CreateLine(Vector2 lineStart, Vector2 lineEnd, bool isX, int gridIndex, Color targetColor)
        {
            GameObject fastLineObj = new GameObject("GridLine" + gridIndex);
            FastLineData testLine = new FastLineData();
            testLine.linesVectors = new Vector3[2];
            testLine.linesVectors[0].x = lineStart.x;
            testLine.linesVectors[0].y = 0.01f;
            testLine.linesVectors[0].z = lineStart.y;
            testLine.linesVectors[1].x = lineEnd.x;
            testLine.linesVectors[1].y = 0.01f;
            testLine.linesVectors[1].z = lineEnd.y;
            testLine.widthMultiplyer = deafultLineWidth;
            testLine.lineMaterial = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
            testLine.lineColor = targetColor;
            fastLineObj.AddComponent<LineGenerator>().BuildMesh(testLine);
            fastLineObj.transform.SetParent(GridContainer.transform);
            return fastLineObj;
        }
    }
}
