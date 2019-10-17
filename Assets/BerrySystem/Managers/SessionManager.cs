/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace ConstruiSystem
{
    public static class SessionManager
    {

        public static BPlayerController[] PlayerControllers;
        public static BPlayerController curPlayerController;

        //public static Layer[] curMapLayers;
        public static int curLayerIndex = 0, mapWidth = 32, mapHight = 32;

        public static Vector4 lastSelectedChunkPos = new Vector4(0, 0, 0, 0), lastSelectedChunkClusterPos = new Vector4(0, 0, 0, 0);

        public static int lastSelectedLayer = -1, lastSelectedLayerChunk = -1;
        public static Chunk lastSelectedChunk;
        public static ChunkCluster lastSelectedChunkCluster;

        static float chunkSize = 10.24f, chunkClusterSize = chunkSize * clusterHightAndWidth;
        static int clusterHightAndWidth = 17;

        public static void PlaceTile(Vector3 worldPosition, int layer = 0, int tileId = 0, bool spawn = true, bool remove = false, bool registerToHistory = true)
        {
            GetTile(worldPosition, GetChunk(worldPosition, GetChunkCluster(worldPosition, spawn), spawn), spawn, layer, tileId, remove);
        }

        public static void FillChunk(Vector3 worldPosition, int layer = 0, int tileFillId = 1, bool clear = false)
        {
            if (lastSelectedChunk != null || clear != true)
            {
                ChunkFill(GetChunk(worldPosition, GetChunkCluster(worldPosition, !clear), !clear, clear), layer, tileFillId, clear);
            }
        }

        // TODO Optimize so that we use Struct instead of classes.

        public static void SessionManagerClearRefs()
        {
            lastSelectedChunkPos = new Vector4(0, 0, 0, 0);
            lastSelectedChunkClusterPos = new Vector4(0, 0, 0, 0);
            lastSelectedChunk = null;
            lastSelectedChunkCluster = null;
        }

        // Get a reference to a chunk cluster by world position, then query the tiles to get the desired chunk cluster.
        public static ChunkCluster GetChunkCluster(Vector3 worldPosition, bool createChunkClusterIfNotFound = false)
        {
            // Convert and store the given position to a valid ChunkClusterPoisson.
            worldPosition = ChunkClusterPoissonId(worldPosition);

            // Check if last selected Chunk Cluster poisson is the same, if it is then just return the same Chunk Cluster. 
            if (lastSelectedLayer == MapDataManager.mapDataCurrentLayer)
            {
                if (lastSelectedChunkCluster != null)
                {
                    if (lastSelectedChunkCluster.posId.x == worldPosition.x && lastSelectedChunkCluster.posId.y == worldPosition.z)
                    {
                        return lastSelectedChunkCluster;
                    }
                }
            }

            curLayerIndex = lastSelectedLayer = MapDataManager.mapDataCurrentLayer;
            // Create a berrySystem vector 2 [ v2 ] 

            V2 targetPosId = new V2();
            targetPosId.x = worldPosition.x;
            targetPosId.y = worldPosition.z;

            // Get the current layer and get all the chunk clusters on that maplayer and then scan for the correct posId.
            if (XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters != null)
            {
                for (int i = 0; i < XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters.Length; i++)
                {
                    if (XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters[i].posId.x == targetPosId.x && XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters[i].posId.y == targetPosId.y)
                    {
                        return lastSelectedChunkCluster = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters[i];
                    }
                    if (i == XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters.Length - 1 && createChunkClusterIfNotFound)
                    {
                        // Create a new chunk cluster entire in mapdata.
                        Array.Resize(ref XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters, XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters.Length + 1);
                        ChunkCluster newCluster = new ChunkCluster();
                        newCluster.index = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters.Length - 1;
                        newCluster.posId = targetPosId;

                        // Create an actual gameobject in the level.
                        newCluster.clusterObjRef = new GameObject(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters.Length - 1 + " chunkCluster");
                        newCluster.clusterObjRef.transform.parent = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].layerObjRef.transform;
                        newCluster.clusterObjRef.isStatic = true;

                        // Applay the new ChunkCluster Array to the current layer.
                        return lastSelectedChunkCluster = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters[i + 1] = newCluster;
                    }
                }
            }
            else
            {
                if (createChunkClusterIfNotFound)
                {
                    // Create a new chunk cluster entire in mapdata
                    ChunkCluster[] newChunkCluster = new ChunkCluster[1];
                    newChunkCluster[0] = new ChunkCluster();
                    newChunkCluster[0].posId = targetPosId;
                    newChunkCluster[0].index = 0;

                    // Create an actual gameobject in the level.
                    newChunkCluster[0].clusterObjRef = new GameObject("0" + " chunkCluster");
                    newChunkCluster[0].clusterObjRef.transform.parent = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].layerObjRef.transform;
                    newChunkCluster[0].clusterObjRef.isStatic = true;

                    // Applay the new ChunkCluster Array to the current layer.
                    XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters = newChunkCluster;

                    return lastSelectedChunkCluster = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[curLayerIndex].chunkClusters[0];
                }
            }
            return null;
        }

        // Get a reference to a chunk, then query the tiles to get the desired chunk.
        public static Chunk GetChunk(Vector3 worldPosition, ChunkCluster chunkCluster, bool createChunkIfNotFound, bool clear = false)
        {
            if (chunkCluster != null)
            {
                // Convert and store the given position to a valid Chunk Poisson.
                worldPosition = ChunkPoissonId(worldPosition);

                if (lastSelectedLayerChunk == MapDataManager.mapDataCurrentLayer)
                {
                    if (lastSelectedChunk != null)
                    {
                        if (lastSelectedChunk.posId.x == worldPosition.x && lastSelectedChunk.posId.y == worldPosition.z)
                        {
                            return lastSelectedChunk;
                        }
                    }
                }

                lastSelectedLayer = lastSelectedLayerChunk = MapDataManager.mapDataCurrentLayer;

                V2 targetPosId = new V2();
                targetPosId.x = worldPosition.x;
                targetPosId.y = worldPosition.z;

                if (chunkCluster.chunks != null)
                {
                    for (int i = 0; i < chunkCluster.chunks.Length; i++)
                    {
                        if (chunkCluster.chunks[i].posId.x == worldPosition.x && chunkCluster.chunks[i].posId.y == worldPosition.z)
                        {
                            lastSelectedChunk = chunkCluster.chunks[i];
                            return lastSelectedChunk;
                        }
                        if (i == chunkCluster.chunks.Length - 1 && createChunkIfNotFound)
                        {
                            // Create a new chunk entire in mapdata.
                            Array.Resize(ref chunkCluster.chunks, chunkCluster.chunks.Length + 1);
                            Chunk newChunk = new Chunk();
                            newChunk.posId = targetPosId;

                            // Create an actual chunk GameObject in the level.
                            newChunk.chunkObjRef = new GameObject(chunkCluster.chunks.Length - 1 + " chunk");
                            newChunk.chunkObjRef.isStatic = true;
                            newChunk.chunkObjRef.transform.position = worldPosition;
                            newChunk.meshObjRef = newChunk.chunkObjRef.AddComponent<TileMesh2D>();
                            newChunk.chunkObjRef.transform.parent = chunkCluster.clusterObjRef.transform;

                            // Applay the new ChunkCluster Array to the current layer.
                            return lastSelectedChunk = chunkCluster.chunks[chunkCluster.chunks.Length - 1] = newChunk;
                        }
                    }
                }
                else
                {
                    if (createChunkIfNotFound)
                    {
                        // Create a new chunk cluster entire in mapdata.
                        Chunk newChunk = new Chunk();
                        newChunk.posId = targetPosId;

                        // Create an actual gameobject in the level.  
                        newChunk.chunkObjRef = new GameObject("0" + " chunk");
                        newChunk.chunkObjRef.isStatic = true;
                        newChunk.chunkObjRef.transform.position = worldPosition;
                        newChunk.meshObjRef = newChunk.chunkObjRef.AddComponent<TileMesh2D>();
                        newChunk.chunkObjRef.transform.parent = chunkCluster.clusterObjRef.transform;

                        // Applay the new ChunkCluster Array to the current layer.
                        chunkCluster.chunks = new Chunk[1];
                        return lastSelectedChunk = chunkCluster.chunks[0] = newChunk;
                    }
                }
            }
            return null;
        }



        // Get a reference to a chunk, then query the tiles to get the desired tile.
        public static void GetTile(Vector3 worldPos, Chunk targetChunk, bool createTileIfNotFound, int layer, int newTileId, bool removeIfFound = false)
        {
            if (targetChunk == null && removeIfFound) { return; }

            if (targetChunk.tileLayers == null)
            {
                if (createTileIfNotFound)
                {
                    targetChunk.tileLayers = new BitArray[1];
                    targetChunk.textureId = new ushort[1];
                    targetChunk.textureId[0] = (ushort)newTileId;
                    targetChunk.dataLayers = new DataLayer[1];
                    targetChunk.dataLayers[0].dataLine = new BitArray[32];
                }
            }

            if (targetChunk.tileLayers.Length - 1 <= layer)
            {
                Array.Resize(ref targetChunk.tileLayers, layer + 1);
                Array.Resize(ref targetChunk.textureId, layer + 1);
                Array.Resize(ref targetChunk.dataLayers, layer + 1);
            }

            Sv3 tileId = TileVector(worldPos);

            // Remove the target tile at the churrent layer.

            if (removeIfFound)
            {
                if (targetChunk.tileLayers[layer] == null || targetChunk == null)
                {
                    return;
                }
                if (targetChunk.tileLayers[layer][tileId.y])
                {
                    // If the vertical flag is true = 1
                    // Set the vertical line to 0 and generate a data line for this index.

                    targetChunk.tileLayers[layer][tileId.y] = false;
                    targetChunk.dataLayers[layer].dataLine[tileId.y] = new BitArray(32, true);
                    targetChunk.dataLayers[layer].dataLine[tileId.y][tileId.x] = false;

                }
                else
                {

                    // If the vertical flag is false = 0
                    if (targetChunk.dataLayers[layer].dataLine[tileId.y] != null)
                    {
                        targetChunk.dataLayers[layer].dataLine[tileId.y][tileId.x] = false;
                    }
                    else { return; }

                }

                if (targetChunk.meshObjRef == null)
                {
                    targetChunk.meshObjRef = targetChunk.chunkObjRef.GetComponent<TileMesh2D>();
                }

                if(targetChunk.meshObjRef.tileObjectLayers != null)
                {
                    if (targetChunk.meshObjRef.tileObjectLayers[layer] != null)
                    {
                        targetChunk.meshObjRef.tileObjectLayers[layer].currentLayer = (ushort)layer;
                        targetChunk.meshObjRef.tileObjectLayers[layer].curCunk = targetChunk;
                        targetChunk.meshObjRef.tileObjectLayers[layer].StartGenerate();
                    }
                }else{
                    targetChunk.meshObjRef.StartGenerate();
                }

            }
            if (createTileIfNotFound)
            {
                // Get the current chunk and check if the current target layer dose exist inside 
                // the map data, if not then register a new layer.
                if (targetChunk.tileLayers[layer] == null)
                {
                    
                    targetChunk.tileLayers[layer] = new BitArray(32);
                    targetChunk.textureId[layer] = (ushort)newTileId;
                }

                // 
                if (!targetChunk.tileLayers[layer][tileId.y])
                {
                    if (targetChunk.dataLayers[layer].dataLine == null)
                    {
                        targetChunk.dataLayers[layer].dataLine = new BitArray[32];
                    }
                    if (targetChunk.dataLayers[layer].dataLine[tileId.y] != null)
                    {
                        if (targetChunk.dataLayers[layer].dataLine[tileId.y].Length < tileId.x)
                        {
                            targetChunk.dataLayers[layer].dataLine[tileId.y].Length = tileId.x;
                        }
                    }
                    else
                    {
                        targetChunk.dataLayers[layer].dataLine[tileId.y] = new BitArray(32);
                    }

                    targetChunk.dataLayers[layer].dataLine[tileId.y].Set(tileId.x, true);

                    if (targetChunk.dataLayers[layer].dataLine[tileId.y].Length == 32)
                    {
                        for (int i = 0; i < 32; i++)
                        {
                            if (targetChunk.dataLayers[layer].dataLine[tileId.y][i] == false)
                            {
                                break;
                            }
                            if (i == targetChunk.dataLayers[layer].dataLine[tileId.y].Length - 1)
                            {
                                targetChunk.dataLayers[layer].dataLine[tileId.y] = null;
                                targetChunk.tileLayers[layer][tileId.y] = true;
                            }
                        }
                    }
                }

                // Get the cunk tilemesh component and check if the parrent mesh has any children an then find
                // if the current tile id exists as a layer.

                if (targetChunk.meshObjRef == null)
                {
                    targetChunk.meshObjRef = targetChunk.chunkObjRef.GetComponent<TileMesh2D>();
                }
                if (layer != 0)
                {
                    if (targetChunk.meshObjRef.tileObjectLayers == null)
                    {
                        targetChunk.meshObjRef.GenerateLayers();
                    }

                    if(targetChunk.tileLayers.Length != targetChunk.meshObjRef.tileObjectLayers.Length)
                    {
                        targetChunk.meshObjRef.GenerateLayers();
                    }
                    //Debug.Log(targetChunk.meshObjRef);
                    //Debug.Log(targetChunk.meshObjRef.tileObjectLayers.Length);
                    //Debug.Log(layer);
                    //Debug.Log(targetChunk.tileLayers.Length);
                    if (targetChunk.meshObjRef.tileObjectLayers[layer] == null)
                    {
                        targetChunk.meshObjRef.GenerateLayers();
                    }
                }

                if (targetChunk.meshObjRef.tileObjectLayers == null)
                {
                    //targetChunk.meshObjRef.currentLayer = (ushort) 0;
                    targetChunk.meshObjRef.curCunk = targetChunk;
                    targetChunk.meshObjRef.StartGenerate();
                }
                else
                {

                    if (targetChunk.meshObjRef.tileObjectLayers[layer] != null)
                    {
                        targetChunk.meshObjRef.tileObjectLayers[layer].currentLayer = (ushort)layer;
                        targetChunk.meshObjRef.tileObjectLayers[layer].curCunk = targetChunk;
                        targetChunk.meshObjRef.tileObjectLayers[layer].StartGenerate();
                    }
                }

            }

        }

        /* FUNCTION END ---
         */

        public static void ChunkFill(Chunk targetChunk, int layer = 0, int tileFillId = 0, bool clear = false)
        {
            if (targetChunk == null && clear) { return; }

            if (targetChunk.tileLayers == null)
            {
                if (!clear)
                {
                    if (layer == 0)
                    {
                        targetChunk.tileLayers = new BitArray[1];
                        targetChunk.tileLayers[0] = new BitArray(32);
                        targetChunk.textureId = new ushort[1];
                        targetChunk.dataLayers = new DataLayer[1];
                        targetChunk.dataLayers[0].dataLine = new BitArray[32];
                    }
                    else
                    {
                        targetChunk.tileLayers = new BitArray[layer + 1];
                        targetChunk.tileLayers[layer] = new BitArray(32);
                        targetChunk.textureId = new ushort[layer + 1];
                        targetChunk.dataLayers = new DataLayer[layer + 1];
                        targetChunk.dataLayers[layer].dataLine = new BitArray[32];
                    }
                }
                else { return; }
            }
            if (targetChunk != null)
            {
                targetChunk.textureId[layer] = (ushort)tileFillId;

                if (clear)
                {
                    targetChunk.tileLayers[layer] = new BitArray(32);
                    targetChunk.dataLayers[layer].dataLine = new BitArray[32];
                }
                else { targetChunk.tileLayers[layer] = new BitArray(32, true); }
            }
            if (targetChunk.meshObjRef == null)
            {
                targetChunk.meshObjRef = targetChunk.chunkObjRef.GetComponent<TileMesh2D>();
            }
            targetChunk.meshObjRef.currentLayer = (ushort)layer;
            targetChunk.meshObjRef.curCunk = targetChunk;
            targetChunk.meshObjRef.StartGenerate();
            return;
        }

        // just take a vector3 (Poisson) and round it to a universal Chunk Cluster position form.
        public static Vector3 ChunkClusterPoissonId(Vector3 worldPosition)
        {
            worldPosition.x = (float)System.Math.Round(worldPosition.x / 174.08f) * 174.08f;
            worldPosition.z = (float)System.Math.Round(worldPosition.z / 174.08f) * 174.08f;
            worldPosition += new Vector3(10.24f * 0.5f, 0, 10.24f * 0.5f);
            worldPosition -= new Vector3(0.16f, 0, 0.16f);
            return worldPosition;
        }

        // just take a vector3 (Poisson) and round it to a universal Chunk position form.
        public static Vector3 ChunkPoissonId(Vector3 worldPosition)
        {
            worldPosition.x = (float)System.Math.Round(0.016f + worldPosition.x / chunkSize) * chunkSize;
            worldPosition.y = 0;
            worldPosition.z = (float)System.Math.Round(0.013821f + worldPosition.z / chunkSize) * chunkSize;
            return worldPosition;
        }

        /*
		public static Vector3 ChunkClusterMiniId(Vector3 worldPosition)
		{
			worldPosition.x = (int)System.Math.Round(0.016f + worldPosition.x / chunkClusterSize) * chunkClusterSize;
			worldPosition.z = (int)System.Math.Round(0.013821f + worldPosition.z / chunkClusterSize) * chunkClusterSize;
			worldPosition.x = worldPosition.x * tileSize / clusterHightAndWidth;
			worldPosition.x = worldPosition.x * tileSize + 0.006f;
			worldPosition.z = worldPosition.z * tileSize / clusterHightAndWidth;
			worldPosition.z = worldPosition.z * tileSize + 0.006f;
			return worldPosition;
		}
		 */


        /*
		public static Vector3 TileChunkPoissonId(Vector3 worldPosition)
		{
			worldPosition.x = (float)System.Math.Round(0.016f + worldPosition.x / chunkSize) * chunkSize;
			worldPosition.x -= chunkSize * 0.5f;
			worldPosition.y = 0;
			worldPosition.z = (float)System.Math.Round(0.013821f + worldPosition.z / chunkSize) * chunkSize;
			worldPosition.z -= chunkSize * 0.5f;
			return worldPosition;
		}

		 */
        public static Vector3 TilePoissonId(Vector3 worldPosition)
        {
            // This is just a dirty quick sulosion, this need a new one, this one cant go over 48 or else there will be overshoot.
            worldPosition.x = (float)System.Math.Round(worldPosition.x / 0.32f) * 0.32f;
            worldPosition.x = worldPosition.x * 3.128256f;
            worldPosition.x = worldPosition.x + 1;
            worldPosition.z = (float)System.Math.Round(worldPosition.z / 0.32f) * 0.32f;
            worldPosition.z = worldPosition.z * 3.128256f;
            worldPosition.z = worldPosition.z + 1;
            return worldPosition;
        }

        public static Vector3 ObjPosition(Vector3 worldPosition)
        {
            Vector3 offset = ChunkPoissonId(worldPosition);
            worldPosition.x -= offset.x;
            worldPosition.y = 0.2f;
            worldPosition.z -= offset.z;

            return worldPosition;
        }

        public static int SpriteSortByPos(SpriteRenderer targetSprite, float correction = 0.0102f)
        {
            if (targetSprite == null || targetSprite.sprite == null) { return -1; }
            float worldPosition = targetSprite.transform.position.z;
            worldPosition -= (float)System.Math.Round(worldPosition / 174.08f) * 174.08f;
            worldPosition += 10.24f * 0.5f;
            worldPosition -= 0.16f;
            worldPosition -= 174.08f;
            worldPosition -= targetSprite.sprite.pivot.y * correction;
            int pos;
            if (worldPosition > 0)
            {
                worldPosition *= 64;
                pos = (int)worldPosition + 1;
            }
            else
            {

                worldPosition *= 32;
                pos = (int)worldPosition * -1;
            }
            return pos;
        }


        public static int TileNrId(Vector3 worldPosition)
        {
            Vector3 offset = ChunkPoissonId(worldPosition) / 0.32f;

            worldPosition.x = (int)Math.Ceiling(worldPosition.x / 0.32f) + mapWidth * 0.5f;
            worldPosition.x -= offset.x;

            worldPosition.z = (int)Math.Ceiling(worldPosition.z / 0.32f) + mapWidth * 0.5f;
            worldPosition.z -= offset.z;

            worldPosition.x = worldPosition.x + mapWidth * worldPosition.z - mapWidth;

            return (int)worldPosition.x;
        }

        public static Sv3 TileVector(Vector3 worldPosition)
        {
            Vector3 offset = ChunkPoissonId(worldPosition) / 0.32f;

            worldPosition.x = (int)Math.Ceiling(worldPosition.x / 0.32f) + mapWidth * 0.5f;
            worldPosition.x -= offset.x;

            worldPosition.z = (int)Math.Ceiling(worldPosition.z / 0.32f) + mapWidth * 0.5f;
            worldPosition.z -= offset.z;


            Sv3 target = new Sv3();
            target.x = (short)worldPosition.x;
            target.y = (short)worldPosition.z;
            target.y -= 1;

            return target;
        }

        // PlayerController handeling
        // Create and register a new BPlayerController then return the new BPlayerController.
        public static BPlayerController CreateBPlayerController(GameObject targetGameObj, bool setToCurrent = true)
        {
            if (curPlayerController != null)
            {
                curPlayerController.ControllerUpdateMode(!setToCurrent);
            }
            else { PlayerControllers = new BPlayerController[0]; }
            Array.Resize(ref PlayerControllers, PlayerControllers.Length + 1);
            return PlayerControllers[PlayerControllers.Length - 1] = targetGameObj.AddComponent<BPlayerController>();
        }

        public static Layer CreateMapLayer(string layerName, bool clearLayers = false, bool setToMain = false)
        {
            if (clearLayers)
            {
                XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers = null;
            }

            Layer curNewLayer = new Layer();
            curNewLayer.layerName = layerName;
            curNewLayer.mainLayer = setToMain;
            curNewLayer.bgColor = MapDataConverter.Color32ToCol(new Color(0, 0.023f, 0.05f, 1));
            curNewLayer.forColor = MapDataConverter.Color32ToCol(new Color32(255, 255, 255, 255));

            if (XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers == null)
            {
                XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers = new Layer[1];
                curLayerIndex = 0;
                return XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[0] = curNewLayer;
            }

            Array.Resize(ref XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers, XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length + 1);
            return XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length - 1] = curNewLayer;
        }

        public static void CreateNewMapLayerObject(int layerIndex)
        {
            GameObject NewLayer = new GameObject(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[layerIndex].layerName);
            NewLayer.transform.SetParent(MapDataManager.mapDataMapObj.transform);
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[layerIndex].layerObjRef = NewLayer;
            NewLayer.SetActive(false);
        }
    }
}
