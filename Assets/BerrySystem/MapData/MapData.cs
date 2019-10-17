/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace ConstruiSystem
{

    // Keep class names and variable names as small as possible for small map file sizes. :P
    [System.Serializable]
    public class MapData
    {
        public string map;
        public string game;
        public string exportTime;
        public int editorExportVersion = 0;
        public CamInstance[] mapCams;
        public int activeCam;
        public Layer[] mapLayers;

        public MapData CreateEmptyMapData(string mapName = "NewMap", float editorVersion = -1, string gameName = "Derelictus")
        {
            if (editorVersion == -1)
            {
                Debug.Log("Editor version not set! ");
            }

            MapData newMap = new MapData();
            newMap.map = mapName;
            newMap.game = gameName;
            newMap.mapLayers = new Layer[0];

            CamInstance newCam = new CamInstance();
            newCam.zoom = 1;
            newMap.mapCams = new CamInstance[1];
            newMap.mapCams[0] = newCam;

            return newMap;
        }
    }

    [System.Serializable]
    public class Layer
    {
        public string layerName;
        public V3 layerSpawn;
        public Col bgColor;
        public Col forColor;
        public bool mainLayer;
        public ushort[] layerStack;
        public ChunkCluster[] chunkClusters;
        [System.NonSerialized]
        public GameObject layerObjRef;

    }

    [System.Serializable]
    public struct Spr
    {
        public float posx;
        public float posy;
        public ushort spriteId;
        public ushort sortLayer;
    }

    [System.Serializable]
    public class ChunkCluster
    {
        public int index;
        public V2 posId;
        public Chunk[] chunks;

        [System.NonSerialized]
        public GameObject clusterObjRef;
    }


    // BitArray[] layers,  

    [System.Serializable]
    public class Chunk
    {
        public V2 posId;  // Optimization, remove saving each chunk position and give ChunkCluster the job to place the Chunk relative to it's own position, them 
        public ushort[] textureId;
        public BitArray[] tileLayers;
        public DataLayer[] dataLayers;
        public Spr[] sprites;
        public CoEnt[] entities;

        [System.NonSerialized]
        public GameObject chunkObjRef;

        [System.NonSerialized]
        public TileMesh2D meshObjRef;
    }

    [System.Serializable] // Change this to a struct to optimize.
    public struct DataLayer
    {
        public BitArray[] dataLine;
    }

    [System.Serializable]
    public struct Tile
    {
        public short tileId;
    }

    // Quick impalement!! create a popper implementation, map file size is bad for saving each uv cordinate for each obj!
    // TODO: Create some sort of texture ID master file to keep track of all uvs for each ID.

    [System.Serializable]
    public struct TextureId
    {
        private static XCP safeXCP;
        public V2 e0;
        public V2 e1;
        public V2 e2;
        public V2 e4;
        public string texId;
    }

    [System.Serializable]
    public struct CamInstance
    {
        public float zoom;
        public V3 pos;
        public V3 rot;
    }

    [System.Serializable]
    public struct V2
    {
        public float x;
        public float y;
    }

    [System.Serializable]
    public struct V3
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public struct Col
    {
        public byte x;
        public byte y;
        public byte z;
    }

    [System.Serializable]
    public struct Sv3
    {
        public short x;
        public short y;
        public short z;
    }

}