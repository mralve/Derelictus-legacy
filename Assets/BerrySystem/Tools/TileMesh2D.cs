/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ConstruiSystem
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TileMesh2D : MonoBehaviour
    {
        // TODO : Make a chunk optimizer and batch tiles.
        MeshRenderer curRenderer;
        Vector3 tilePos;
        float gridSize = 0.32f;
        Mesh curMesh;
        Vector3[] verts;
        Vector2[] uvs;
        public Vector2[] curUvs;
        int[] tris;
        public Chunk curCunk;

        public ushort currentLayer = 0;

        public GameObject spriteContainer;
        public GameObject tileLayerContainer;
        public TileMesh2D[] tileObjectLayers;

        // TileMap Settings
        public float cellSize = 0.32f;
        public Vector3 gridOffset = new Vector3(-5.12f, 0, -5.12f);
        public int gridSizeX = 32, gridSizeY = 32;
        private MaterialPropertyBlock curPropBlock;

        void Awake()
        {
            curMesh = GetComponent<MeshFilter>().mesh;
            curRenderer = GetComponent<MeshRenderer>();
            curPropBlock = new MaterialPropertyBlock();
        }

        public void StartGenerate()
        {
            // Check if this TileMesh is going to be the parrent of all other potential tile layer objects.
            GenerateLayers();
            GenerateChunk();
            gameObject.isStatic = true;
        }

        public void GenerateLayers()
        {
            if (currentLayer == 0)
            {
                // Check of the current chunk has more than one tile layer entry.
                 if (curCunk.tileLayers.Length > 1)
                {
                    //Debug.Log("Is 0, and is going to be a parrent ;)");
                    if (tileLayerContainer != null)
                    {
                        GameObject.Destroy(tileLayerContainer);
                    }
                    tileLayerContainer = new GameObject("Tile Layers");
                    tileLayerContainer.transform.SetParent(gameObject.transform);
                    tileLayerContainer.transform.localPosition = new Vector2(0, 0);
                    GameObject layerChunk;
                    TileMesh2D newChunk;
                    tileObjectLayers = new TileMesh2D[curCunk.tileLayers.Length];
                    for (int i = 0; i < curCunk.tileLayers.Length; i++)
                    {
                        if (i != 0)
                        {
                            layerChunk = new GameObject("layer " + i);
                            layerChunk.transform.SetParent(tileLayerContainer.transform);
                            layerChunk.transform.localPosition = new Vector2(0, 0);
                            newChunk = layerChunk.AddComponent<TileMesh2D>();
                            tileObjectLayers[i] = newChunk;
                            newChunk.curCunk = curCunk;
                            newChunk.currentLayer = (ushort)(i);
                            newChunk.StartGenerate();
                        }
                        else { tileObjectLayers[i] = this; }
                    }
                }
            }
        }
        // TODO : Deprecate if not needed.
        void SnapToGrid()
        {
            tilePos = this.transform.position;
            tilePos.x = (float)System.Math.Round(tilePos.x / gridSize) * gridSize - gridSize * 0.5f;
            tilePos.z = (float)System.Math.Round(tilePos.z / gridSize) * gridSize + gridSize * 0.5f;
            transform.position = new Vector3(tilePos.x * gridSize * 0.5f, 0.1f, tilePos.x);
        }

        public void GenerateChunk()
        {
            curUvs = new Vector2[4];
            curUvs[0] = new Vector2(0, 0);
            curUvs[1] = new Vector2(0, 1);
            curUvs[2] = new Vector2(1, 0);
            curUvs[3] = new Vector2(1, 1);

            //Debug.Log(chunkTiles.Length);
            verts = new Vector3[gridSizeX * gridSizeY * 4];
            uvs = new Vector2[verts.Length];
            tris = new int[gridSizeX * gridSizeY * 6];

            int v = 0, t = 0;

            float vertexOffset = cellSize * 0.5f;

            int x = 0, y = 0;
            float z = 0;

            // Start lopping thru the current chunks data and generate a mesh from that.
            if(curCunk != null)
            {
                for (int i = 0; i < 32; i++)
                {
                    //				Debug.Log(curCunk.tileLayers.Length);
                    if (curCunk.tileLayers[currentLayer][i])
                    {
                        for (int tl = 0; tl < 32; tl++)
                        {
                            Vector3 cellOffset = new Vector3(x * cellSize, 0, z);
                            if (true)
                            {
                                // Create the vertex coordinates.
                                verts[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + gridOffset + cellOffset;
                                verts[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + gridOffset + cellOffset;
                                verts[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + gridOffset + cellOffset;
                                verts[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + gridOffset + cellOffset;
                                // Create the triangles.
                                tris[t] = v;
                                tris[t + 1] = v + 1;
                                tris[t + 2] = v + 2;
                                tris[t + 3] = v + 2;
                                tris[t + 4] = v + 1;
                                tris[t + 5] = v + 3;
                                // Set the uv coordinates. TL 0.25f x 0.25f  BL 0.25f x 0.6875f BR 0.6875f x 0.6875f TR 0.6875f x 0.25
                                uvs[v] = curUvs[0]; // Bottom Left
                                uvs[v + 1] = curUvs[1]; // Left Top
                                uvs[v + 2] = curUvs[2]; // Bottom Right
                                uvs[v + 3] = curUvs[3]; // Top Right
                            }
                            // Increment the vertices and the triangles.
                            v += 4;
                            t += 6;
                            // Increment in tile data.
                            x++;
                            if (x == gridSizeX) { y += gridSizeX; x = 0; z += cellSize; }
                        }
                    }
                    else
                    {
                        // If the current data line is empty then only increment to the next vertical line. (skip 32 tiles)
                        if (curCunk.dataLayers[currentLayer].dataLine[i] == null)
                        {
                            // Increment the vertices and the triangles.
                            v += 4 * 32;
                            t += 6 * 32;
                            // Increment in tile data.
                            x += 32;
                            if (x == gridSizeX) { y += gridSizeX; x = 0; z += cellSize; }

                        }
                        else
                        {
                            // Generate tiles from the current dataLine, aka the bit array of tiles.
                            for (int tl2 = 0; tl2 < curCunk.dataLayers[currentLayer].dataLine[i].Length; tl2++)
                            {
                                Vector3 cellOffset = new Vector3(x * cellSize, 0, z);
                                if (curCunk.dataLayers[currentLayer].dataLine[i][tl2])
                                {
                                    // Create the vertex coordinates.
                                    verts[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + gridOffset + cellOffset;
                                    verts[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + gridOffset + cellOffset;
                                    verts[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + gridOffset + cellOffset;
                                    verts[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + gridOffset + cellOffset;
                                    // Create the triangles.
                                    tris[t] = v;
                                    tris[t + 1] = v + 1;
                                    tris[t + 2] = v + 2;
                                    tris[t + 3] = v + 2;
                                    tris[t + 4] = v + 1;
                                    tris[t + 5] = v + 3;
                                    // Set the uv coordinates. TL 0.25f x 0.25f  BL 0.25f x 0.6875f BR 0.6875f x 0.6875f TR 0.6875f x 0.25
                                    uvs[v] = curUvs[0]; // Bottom Left
                                    uvs[v + 1] = curUvs[1]; // Left Top
                                    uvs[v + 2] = curUvs[2]; // Bottom Right
                                    uvs[v + 3] = curUvs[3]; // Top Right
                                }
                                // Increment the vertices and the triangles.
                                v += 4;
                                t += 6;
                                // Increment in tile data.
                                x++;
                                if (x == gridSizeX) { y += gridSizeX; x = 0; z += cellSize; }
                            }
                        }
                    }
                }
            }else{ Debug.Log("ERROR!"); }
            // Pass the newly generated mesh data to it's meshrenderer. 
            UpdateMesh();
        }

        // Update the (chunk or tile layer) mesh renderer to display an updated version of z (chunk or tile layer). 
        public void UpdateMesh()
        {
            // Clear the mesh and asign new data to the renderer.
            curMesh.Clear();
            curMesh.vertices = verts;
            curMesh.triangles = tris;
            curMesh.uv = uvs;

            // Set the material(s)
            curRenderer.sharedMaterial = ScenePrimer.StandardWorld;
            curRenderer.GetPropertyBlock(curPropBlock);
            if (curCunk.textureId[currentLayer] != 0)
            {
                //Debug.Log(XCPManager.currentXCP.tileIds.Length);
                //Debug.Log(curCunk.textureId[currentLayer]);
                curPropBlock.SetTexture("_tex", XCPManager.PngToTex(XCPManager.currentXCP.tileTextures[curCunk.textureId[currentLayer]]));
            }
            else
            {
                curPropBlock.SetTexture("_tex", XCPManager.PngToTex(XCPManager.currentXCP.tileTextures[curCunk.textureId[currentLayer]]));
            }
            curPropBlock.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
            curRenderer.SetPropertyBlock(curPropBlock);

            curRenderer.sortingOrder = -1000 + currentLayer;
            curRenderer.sortingLayerName = "tilelayer";
            curRenderer.shadowCastingMode = ShadowCastingMode.Off;
            curRenderer.receiveShadows = false;
        }

        // Generate the sprites that are placed on the cunk. this must only happen once in a chunks life time.
        // TODO : Move the generation to a separate class to improve system workflow, and extensibility.
        public void GenerateSprites()
        {
            if (curCunk.sprites != null)
            {
                if (curCunk.sprites.Length != 0)
                {
                    // If the programer is not complying with the above instructions, then force a re-geneation of all sprites on the chunk.
                    // This may be only good in editing mode.
                    if (spriteContainer != null)
                    {
                        GameObject.Destroy(spriteContainer);
                        spriteContainer = new GameObject("Sprites");
                        spriteContainer.transform.SetParent(transform);
                        spriteContainer.transform.position = transform.position;
                    }
                    else
                    {
                        spriteContainer = new GameObject("Sprites");
                        spriteContainer.transform.SetParent(transform);
                        spriteContainer.transform.position = transform.position;
                    }
                    GameObject spriteObj;
                    SpriteRenderer spriteRender;
                    MaterialPropertyBlock spriteProps;
                    spriteProps = new MaterialPropertyBlock();
                    for (int i = 0; i < curCunk.sprites.Length; i++)
                    {
                        spriteObj = new GameObject(curCunk.sprites[i].spriteId.ToString());
                        spriteObj.isStatic = true;
                        spriteObj.transform.SetParent(spriteContainer.transform);
                        spriteObj.transform.localPosition = new Vector3(curCunk.sprites[i].posx, 0.2f, curCunk.sprites[i].posy);
                        spriteRender = spriteObj.AddComponent<SpriteRenderer>();
                        spriteRender.material = ScenePrimer.StandardSprite;
                        spriteRender.GetPropertyBlock(spriteProps);
                        spriteRender.transform.eulerAngles = new Vector2(90, 0);
                        spriteRender.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[curCunk.sprites[i].spriteId]);
                        spriteProps.SetTexture("_tex", spriteRender.sprite.texture);
                        spriteProps.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
                        spriteRender.SetPropertyBlock(spriteProps);
                        if (ScenePrimer.curEditorPrimer != null) { spriteObj.AddComponent<BoxCollider>().isTrigger = true; spriteObj.AddComponent<SpriteInfo>().spriteIndex = i; }
                        if (XCPManager.currentXCP.spriteTextures[curCunk.sprites[i].spriteId].colliders != null)
                        {
                            for (int col = 0; i < XCPManager.currentXCP.spriteTextures[curCunk.sprites[i].spriteId].colliders.Length; i++)
                            {
                                BoxCollider colider = spriteRender.gameObject.AddComponent<BoxCollider>();
                            }
                        }
                        spriteRender.sortingLayerName = EditorPrimer.GoodIdToBadUnityId(curCunk.sprites[i].sortLayer + 1);
                        spriteRender.sortingOrder = SessionManager.SpriteSortByPos(spriteRender);
                    }
                }
            }
        }
    }

    // SpriteInfo is a simple component to store the sprite's information for later editing.
    public class SpriteInfo : MonoBehaviour
    {
        public int spriteIndex;
    }
}
