/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections;
using System;
using System.IO;
//using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConstruiSystem
{
    public static class MapDataManager
    {

        // Map file info.
        public static string mapDataFilePath = "";
        public static string mapDataFileName;
        public static float mapDataTileSize;
        public static bool mapDataIsSaved;

        // Current instances of the actrive map data
        public static XCP xcpInstance;
        public static int mapDataXCPIndex = -1;
        public static int mapDataCurrentLayer;
        public static int mapDataMainLayer;

        // Current Map Object
        public static GameObject mapDataMapObj;


        // Tries to open and load a map file on users computer
        /*
		public static void MapDataOpenMap(string mapName = "", string mapPath = "")
		{
			switch (MapDataPrepForChange())
			{
				case -1:
				break;
				case 0:
					MapDataSaveMap();
					MapDataQuery.DeleteMapQuery();
				goto case 1;
				case 1:
					GlobalToolManager.disabelTools();
					MapDataQuery.DeleteMapQuery();
					SessionManager.SessionManagerClearRefs();

					mapData = MapDataImportFromFile(mapPath, mapName + ".berrymap");
					
					mapDataFileName = mapName;
					mapDataTileSize = 0.32f;
					UiManager.DestroyAllFocus();
					
					if(mapData == null){ Debug.LogError("WOOOPSI"); }
					MapDataCreateMapObj();

				break;
			}
		}
		 */

        public static void MapDataOpenXCPMap(int mapIndex)
        {
            if (XCPManager.currentXCP.xpcMaps[mapIndex] == null) { Debug.LogError("WWWWWNWOWPWEWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW"); }
            //if(mapIndex == mapDataXCPIndex) {UiManager.DestroyAllFocus(); Debug.Log("Sorry Dave but i can't let you do that."); return;}

            GlobalToolManager.DisabelTools();
            MapDataQuery.DeleteMapQuery();
            SessionManager.SessionManagerClearRefs();

            mapDataXCPIndex = mapIndex;

            mapDataFileName = XCPManager.currentXCP.xpcMaps[mapIndex].map;
            mapDataTileSize = 0.32f;
            ScenePrimer.curPrimerComponent.PrimerMapUpdate();
            UiManager.DestroyAllFocus();

            if (XCPManager.currentXCP.xpcMaps[mapDataXCPIndex] == null) { Debug.LogError("WOOOPSI"); }
            MapDataCreateMapObj();
        }


        // Saves the mapdata to a file on disk.
        public static void MapDataSaveMap(string mapName = "", string mapPath = "")
        {
            if (mapName == "")
            {
                mapName = mapDataFileName;
            }
            if (mapPath == "")
            {
                if (mapDataFilePath != "")
                {
                    MapDataSave(mapName, mapDataFilePath);
                }
                else
                {
                    //UiManager.CreateUiFocusObj(true).AddComponent<EditorSaveMapAs>().Click();
                }
            }
            else
            {
                MapDataSave(mapName, mapPath);
            }
        }

        // Grab map data and start making tha map and it's objects from the data.
        public static void MapDataCreateMapObj()
        {
            // Create the map data object.
            if (mapDataMapObj == null)
            {
                mapDataMapObj = new GameObject("Map " + XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].map);
                for (int i = 0; i < XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers.Length; i++)
                {
                    // Create eathc layer as a object.
                    XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].layerObjRef = new GameObject(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].layerName);
                    XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].layerObjRef.transform.SetParent(mapDataMapObj.transform);

                    // Create the map layers.
                    if (XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters != null)
                    {
                        for (int cc = 0; cc < XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters.Length; cc++)
                        {
                            GameObject newChunkCluster = new GameObject(cc.ToString());
                            newChunkCluster.transform.position = MapDataConverter.V2ToVector2(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].posId);
                            newChunkCluster.transform.SetParent(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].layerObjRef.transform);
                            XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].clusterObjRef = newChunkCluster;

                            if (XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks != null)
                            {
                                for (int c = 0; c < XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks.Length; c++)
                                {
                                    GameObject newChunk = new GameObject(c.ToString());
                                    newChunk.transform.SetParent(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].clusterObjRef.transform);
                                    newChunk.transform.position = new Vector3(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks[c].posId.x, 0, XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks[c].posId.y);
                                    newChunk.AddComponent<MeshFilter>();
                                    newChunk.AddComponent<MeshRenderer>();
                                    TileMesh2D cTileMap = newChunk.AddComponent<TileMesh2D>();
                                    cTileMap.curCunk = XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks[c];
                                    cTileMap.gridOffset = new Vector3(-5.12f, 0, -5.12f);
                                    cTileMap.gridSizeX = 32;
                                    cTileMap.gridSizeY = 32;
                                    cTileMap.cellSize = 0.32f;
                                    cTileMap.StartGenerate();
                                    cTileMap.GenerateSprites();
                                    XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].chunkClusters[cc].chunks[c].chunkObjRef = newChunk;
                                }
                            }
                        }
                    }

                    // Check if this is the main layer.
                    if (XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].mainLayer)
                    {
                        mapDataCurrentLayer = i;
                        mapDataMainLayer = i;
                    }
                    else
                    {
                        XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[i].layerObjRef.SetActive(false);

                    }
                }
            }
            SwitchLayer(mapDataMainLayer, true);
            CameraManager.UpdateCurrentCamera();
        }

        public static void MapDataDeconstructMapObj(bool clearMapData = false)
        {
            GameObject.Destroy(mapDataMapObj);
            mapDataMapObj = null;
        }

        public static void MapDataReConstructMapObj()
        {
            MapDataDeconstructMapObj();
            MapDataCreateMapObj();
        }

        private static void MapDataSave(string mapName = "", string mapPath = "")
        {
            mapDataFilePath = mapPath;
            mapDataFileName = mapName;
            XCPManager.currentXCP.xpcMaps[mapDataXCPIndex] = MapDataQuery.QueryMapData();
            MapDataExportToFile(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex], mapPath, mapDataFileName + ".berrymap");
            UiManager.ShowNotification("Map Saved", 0);
            ScenePrimer.curPrimerComponent.PrimerMapUpdate();
            mapDataIsSaved = true;
        }

        // Creates a empty map that is only stored in temp memory.
        public static void MapDataCreateEmptyMap(string mapName = "New Map")
        {
            GlobalToolManager.DisabelTools();
            MapDataQuery.DeleteMapQuery();
            SessionManager.SessionManagerClearRefs();

            if (XCPManager.currentXCP.xpcMaps == null)
            {
                XCPManager.currentXCP.xpcMaps = new MapData[1];
                XCPManager.currentXCP.xpcMaps[0] = new MapData().CreateEmptyMapData(mapName, EditorPrimer.editorVersionNumberInternal, "Derelictus");
                mapDataXCPIndex = 0;
            }
            else
            {
                Array.Resize(ref XCPManager.currentXCP.xpcMaps, XCPManager.currentXCP.xpcMaps.Length + 1);
                XCPManager.currentXCP.xpcMaps[XCPManager.currentXCP.xpcMaps.Length - 1] = new MapData().CreateEmptyMapData(mapName, EditorPrimer.editorVersionNumberInternal, "Derelictus"); ;
                mapDataXCPIndex = XCPManager.currentXCP.xpcMaps.Length - 1;
            }
            mapDataFileName = mapName;
            mapDataTileSize = 0.32f;
            SessionManager.CreateMapLayer("Layer 0", true, true);
            ScenePrimer.curPrimerComponent.PrimerMapUpdate();
            UiManager.DestroyAllFocus();
            MapDataCreateMapObj();
        }

        // Notifies the user that a change to the mapdata has been requested and gives the user to cansel the request, or to save the map data.
        public static int MapDataPrepForChange(string message = "")
        {
            if (XCPManager.currentXCP.xpcMaps[mapDataXCPIndex] != null)
            {
                return MapDataPromptUserOfMapChange();
            }
            else
            {
                return 1;
            }
        }

        public static int MapDataPromptUserOfMapChange(string message = "")
        {
            return 1;// UiManager.createMessageBox(message, true, true, true, false, false);
        }

        // Switches the current layer to the layer with that index.
        public static void SwitchLayer(int layerIndex, bool instantSwitch = false)
        {
            XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[mapDataCurrentLayer].layerObjRef.SetActive(false);
            mapDataCurrentLayer = layerIndex;
            XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[layerIndex].layerObjRef.SetActive(true);
            if (ScenePrimer.curEditorPrimer != null)
            {
                GameObject.Destroy(ScenePrimer.curGamePrimer);
                if (EditorPrimer.SpawnIcon != null)
                {
                    EditorPrimer.SpawnIcon.transform.position = MapDataConverter.V3ToVector3(XCPManager.currentXCP.xpcMaps[mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn);
                }
            }
            CameraManager.UpdateCurrentCamera();
        }

        // File System

        public static bool IsValidPath(string mapPath = "")
        {
            if (mapPath == "")
            {
                return false;
            }
            return System.IO.Directory.Exists(mapPath);
        }

        // Save a file to the users OS and export the mapdata to the file.
        public static bool MapDataExportToFile(MapData fileData, string filePath = null, string fileName = "untitled.berrymap")
        {
            // Set the export time on the map
            fileData.exportTime = DateTime.Now.ToString();
            fileData.map = mapDataFileName;
            Debug.Log(fileData.exportTime);

            // Check the file path if it is valid!
            if (filePath == null)
            {
                filePath = Application.persistentDataPath;
            }
            string fullFilePath = filePath + "/" + fileName;

            using (Stream curStream = File.Create(fullFilePath))
            {
                BinaryFormatter curBinaryFormater = new BinaryFormatter();
                curBinaryFormater.Serialize(curStream, fileData);
                curStream.Close();
            }
            return true;

        }

        // Load a file from the users OS and extract the mapdata from the file.
        /*
		public static MapData MapDataImportFromFile(string filePath = null, string fileName = "untitled.berrymap")
		{
			if(filePath == null)
			{
				filePath = Application.persistentDataPath;
			}
			string fullFilePath = filePath + "/" + fileName;

			if(File.Exists(fullFilePath))
			{
				FileStream stream = new FileStream(filePath + "/" + fileName, FileMode.Open);
				BinaryFormatter curBinaryFormater = new BinaryFormatter();
				
				MapData fileData = curBinaryFormater.Deserialize(stream) as MapData;
				stream.Close();
				
				mapData = fileData;
				mapDataFilePath = filePath;
				mapDataFileName = fileName;
				return fileData;
			}else{
				return null;
			}
		}
		 */
    }
    public class MapDataManagerMono : MonoBehaviour
    {
        public IEnumerator LayerFadeSwitch(IEnumerator cor)
        {
            while (cor.MoveNext())
                yield return cor.Current;
        }
    }
}