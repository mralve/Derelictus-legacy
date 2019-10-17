/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public static class FastLine2D
    {
        public static void CreateFastLine(FastLineData lineData)
        {
            if (lineData.parentTransform == null)
            {
                GameObject lineContainer = new GameObject("LinesContainer");
                lineContainer.transform.SetParent(lineData.parentTransform);
                lineContainer.AddComponent<LineGenerator>().BuildMesh(lineData);
            }
            else
            {
                lineData.parentTransform.gameObject.AddComponent<LineGenerator>().BuildMesh(lineData);
            }
        }
    }

    public class LineGenerator : MonoBehaviour
    {
        private MeshRenderer curRenderer;
        private MaterialPropertyBlock propertyBlock;

        void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();
        }

        public void BuildMesh(FastLineData lineData)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[lineData.linesVectors.Length * 2];
            Vector3 curVector;
            Vector3 nextVector;
            for (int i = 0; i < lineData.linesVectors.Length; i++)
            {
                // Operate only if there is a next vector, The line generator needs 2 points to generate
                if (lineData.linesVectors.Length > i + 1)
                {
                    curVector = lineData.linesVectors[i];
                    nextVector = lineData.linesVectors[i + 1];
                    vertices[i] = new Vector3(curVector.x + lineData.widthMultiplyer, curVector.y, curVector.z + lineData.widthMultiplyer);
                    vertices[i + 1] = new Vector3(nextVector.x + lineData.widthMultiplyer, nextVector.y, nextVector.z + lineData.widthMultiplyer);
                    vertices[i + 2] = new Vector3(curVector.x - lineData.widthMultiplyer, curVector.y, curVector.z - lineData.widthMultiplyer);
                    vertices[i + 3] = new Vector3(nextVector.x - lineData.widthMultiplyer, nextVector.y, nextVector.z - lineData.widthMultiplyer);
                }
            }

            mesh.vertices = vertices;
            int multiplayer = lineData.linesVectors.Length - 1;
            int[] triangles = new int[6 * multiplayer];
            int curVert = 0;
            for (int i = 0; i < lineData.linesVectors.Length - 1; i++)
            {
                if (i != 0)
                {
                    curVert = curVert + 6;
                }
                triangles[curVert] = 0;
                triangles[curVert + 1] = 2;
                triangles[curVert + 2] = 3;

                triangles[curVert + 3] = 3;
                triangles[curVert + 4] = 1;
                triangles[curVert + 5] = 0;
            }

            mesh.triangles = triangles;
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            curRenderer = gameObject.AddComponent<MeshRenderer>();
            curRenderer.material = lineData.lineMaterial;
            curRenderer.receiveShadows = false;
            curRenderer.sortingLayerName = EditorPrimer.GoodIdToBadUnityId(0);
            curRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", lineData.lineColor);
            curRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public class FastLineData
    {
        public ushort sortLayer = 0;
        public Transform parentTransform;
        public Vector3[] linesVectors;
        public float[] lineWidth;
        public float widthMultiplyer;
        public Material lineMaterial;
        public Color lineColor;
    }
}
