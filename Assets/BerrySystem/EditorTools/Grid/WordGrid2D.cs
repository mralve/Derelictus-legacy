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
    public class WorldGrid2D : MonoBehaviour
    {

        //private GameObject lineOBJ;
        private GameObject GridContainer;
        private GameObject WorldOutline;

        public Color normalLineColor = new Color32(0, 200, 255, 120);
        //public Color subLineColor = new Color32(20, 196, 255, 20);
        //public Color edgeLinesColor = new Color32(255, 56, 143, 186);
        //public Color centerLineColor = new Color32(111, 255, 255, 167);

        private Color curColor;

        public int mapWidth = 17;
        public int mapHeight = 17;
        public float gridSize = 10.24f;
        public float deafultLineWidth = 0.02f;

        public Camera CameraRef;
        public Vector3 CameraVel = new Vector3(0, 0, 0);
        public Vector3 CameraPos;

        public Vector3 gridPos = new Vector3(0, 0, 0);

        public Transform[] lineRef;

        //EditorPrimer curPrimer;

        // Use this for initialization
        void Awake()
        {
            /*curPrimer = ScenePrimer.curPrimerParrentObj.GetComponent<EditorPrimer>();*/
            CameraRef = CameraManager.curCamera;
            DrawGrid();
            UpdateGridPos();
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
                CameraVel = CameraRef.velocity;
                CameraPos = CameraRef.transform.position;
                if (CameraVel.x != 0 || CameraVel.z != 0)
                {
                    UpdateGridPos();
                }
            }
        }

        void Update()
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0)
            {
            }

            /*
			//manageLineScale();
			if(Input.GetButtonUp("gridToggel"))
			{
				if(GridContainer.activeInHierarchy)
				{
					GridContainer.SetActive(false);
				}else{
					GridContainer.SetActive(true);
				}
				//curPrimer.curGridBtn.IconUpdate();
			}
			 */
        }

        void UpdateGridPos()
        {
            gridPos.y = 0.1f;
            gridPos.x = (float)System.Math.Round(CameraPos.x / 174.08f) * 174.08f;
            gridPos.z = (float)System.Math.Round(CameraPos.z / 174.08f) * 174.08f;
            gridPos += new Vector3(gridSize * 0.5f, 0, gridSize * 0.5f);
            gridPos -= new Vector3(0.16f, 0, 0.16f);
            transform.position = gridPos;
        }

        void DrawGrid()
        {
            Destroy(GridContainer);
            Destroy(WorldOutline);

            // Create the grid line's parrents (Containers)
            GridContainer = new GameObject("WorldGridContainer");
            GridContainer.transform.SetParent(this.transform);
            GridContainer.isStatic = false;

            WorldOutline = new GameObject("WorldGridOutline");
            WorldOutline.transform.SetParent(this.transform);
            WorldOutline.isStatic = false;

            lineRef = new Transform[mapHeight + mapWidth + 2];

            // Generate all the grid lines in Z
            for (int z = 0; z < mapHeight + 1; z++)
            {

                curColor = normalLineColor;

                lineRef[z] = CreateLine(new Vector2(-deafultLineWidth * 0.5f - gridSize * 0.5f + gridSize * mapWidth * 0.5f * -1, gridSize * mapHeight * 0.5f * -1 + z * gridSize - gridSize * 0.5f), new Vector2(gridSize * mapWidth * 0.5f + deafultLineWidth * 0.5f - gridSize * 0.5f, gridSize * mapHeight / 2 * -1 + z * gridSize - gridSize * 0.5f), true, z, curColor, true).transform;
            }
            // Generate all the grid lines in X
            for (int x = 0; x < mapWidth + 1; x++)
            {

                curColor = normalLineColor;

                lineRef[x + mapHeight + 1] = CreateLine(new Vector2(mapWidth * gridSize * 0.5f * -1 + x * gridSize - gridSize * 0.5f, mapHeight * gridSize * 0.5f * -1 + -deafultLineWidth * 0.5f - gridSize * 0.5f), new Vector2(mapWidth * gridSize * 0.5f * -1 + x * gridSize - gridSize * 0.5f, mapHeight * gridSize * 0.5f + deafultLineWidth - gridSize * 0.5f), false, x, curColor, false).transform;

            }

        }
        GameObject CreateLine(Vector2 lineStart, Vector2 lineEnd, bool isX, int gridIndex, Color targetColor, bool useX)
        {
            GameObject fastLineObj = new GameObject("WorldGridLine" + gridIndex);
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
            //fastLineObj.AddComponent<GridLineScaler>().useX = useX;
            fastLineObj.transform.SetParent(GridContainer.transform);
            return fastLineObj;
        }
    }
}
