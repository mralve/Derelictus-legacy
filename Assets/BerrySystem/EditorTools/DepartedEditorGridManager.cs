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
    public class EditorGridManager : MonoBehaviour
    {

        EditorPrimer curPrimer;
        public GameObject curGrid;
        public Camera curCamera;
        public GridLineRef[] gridLinePool;
        public int gridSizeCount;
        public Material lineMaterial;
        public float lineLength = 1;
        bool linesGenerated = false;
        float lastGridSizeCalculatedFor = 4;
        float tileSize = 0.32f;

        void Awake()
        {
            curPrimer = ScenePrimer.curPrimerParrentObj.GetComponent<EditorPrimer>();
            curGrid = new GameObject("Grid");
            curGrid.transform.SetParent(this.transform);
            curPrimer.editorGridObj = curGrid;
            curCamera = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
            gridSizeCount = 8;
            lineMaterial = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
        }

        void Update()
        {
            if (Input.GetButtonUp("gridToggel"))
            {
                if (curGrid.activeInHierarchy)
                {
                    curGrid.SetActive(false);
                }
                else
                {
                    curGrid.SetActive(true);
                }
                curPrimer.curGridBtn.IconUpdate();
            }
            if (Input.GetAxis("Mouse ScrollWheel") >= 0.03f || Input.GetAxis("Mouse ScrollWheel") <= -0.03f)
            {
                CalculateGrid();
            }
        }

        void OnEnable()
        {
            CalculateGrid();
        }

        public void CalculateGrid()
        {
            if (gridLinePool == null)
            {
                if (gridSizeCount % 2 != 0)
                {
                    gridSizeCount++;
                }
                gridSizeCount = gridSizeCount * 2;
                gridLinePool = new GridLineRef[gridSizeCount];
                for (int i = 0; i < gridLinePool.Length; i++)
                {
                    GameObject curObj = new GameObject("GL_" + i);
                    curObj.transform.SetParent(curGrid.transform);
                    gridLinePool[i] = new GridLineRef();
                    gridLinePool[i].ObjRef = curObj;
                }
            }

            if (curGrid.activeInHierarchy)
            {
                if (linesGenerated != true)
                {
                    linesGenerated = true;
                    for (int i = 0; i < gridLinePool.Length; i++)
                    {
                        LineRenderer curLine = gridLinePool[i].ObjRef.AddComponent<LineRenderer>();
                        gridLinePool[i].LineRendererRef = curLine;
                        curLine.widthMultiplier = 0.01f;
                        curLine.material = lineMaterial;
                        curLine.useWorldSpace = false;
                        Vector3[] linePos = new Vector3[2];
                        if (i >= gridLinePool.Length / 2)
                        {
                            linePos[0] = new Vector3(lineLength, 0, 0);
                            linePos[1] = new Vector3(-lineLength, 0, 0);
                        }
                        else
                        {
                            linePos[0] = new Vector3(0, 0, lineLength);
                            linePos[1] = new Vector3(0, 0, -lineLength);
                        }
                        curLine.SetPositions(linePos);
                    }
                }
                if (curCamera.orthographicSize % 4 >= 3.5f)
                {
                    ReDrawGrid();
                }
                if (lastGridSizeCalculatedFor >= 2 || curCamera.orthographicSize > 2)
                {
                    ReDrawGrid();
                }
            }
        }

        public void ReDrawGrid()
        {
            //Debug.Log( curCamera.orthographicSize + " > " + curCamera.orthographicSize % 4);
            lastGridSizeCalculatedFor = curCamera.orthographicSize;
            float hPos = tileSize * gridLinePool.Length / 2;
            hPos = hPos + tileSize / 2;
            float vPos = tileSize * gridLinePool.Length / 2;
            vPos = vPos + tileSize / 2;
            //Vector3[] linePos = new Vector3[2];
            for (int i = 0; i < gridLinePool.Length; i++)
            {
                if (i > gridLinePool.Length / 2)
                {
                    gridLinePool[i].ObjRef.transform.position = new Vector3(0, 0, hPos);
                    hPos = hPos + tileSize;
                    /*
                                        linePos[0] = new Vector3(0, 0, vPos / 2 - 0.005f);
                                        linePos[1] = new Vector3(0, 0, -vPos / 2 + 0.005f);
                                        gridLinePool[i].LineRendererRef.SetPositions(linePos);
                     */

                }
                else
                {
                    gridLinePool[i].ObjRef.transform.position = new Vector3(vPos, 0, 0);
                    vPos = vPos + tileSize;
                    /*
					linePos[0] = new Vector3(hPos - 0.005f, 0, 0);
					linePos[1] = new Vector3(-hPos + 0.005f, 0, 0);
					gridLinePool[i].LineRendererRef.SetPositions(linePos);
					 */
                }
            }

            //Debug.Log("Andrew drew a penis to the president of the united states of america");
        }

        public void ReGrabCamera()
        {
            curCamera = CameraManager.CurrentRenderCamera.GetComponent<Camera>();
        }
    }

    public class GridLineRef
    {
        public GameObject ObjRef;
        public LineRenderer LineRendererRef;
    }
}
