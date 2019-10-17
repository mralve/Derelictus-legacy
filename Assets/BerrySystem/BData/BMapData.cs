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

    // Keep class names and variable names as small as possible for small map file sizes. :P
    [System.Serializable]
    public struct BMapData
    {
        public string mapName;
        public string exportTime;
        public float editorExportVersion;
        public BCam[] mapCams;
        public int activeCam;
        public Layer[] mapLayers;
    }

    [System.Serializable]
    public struct BLayer
    {
        public string layerName;
        public V3 bgColor;
        public V3 forColor;
        public bool mainLayer;
        public ChunkCluster[] chunkClusters;
    }

    [System.Serializable]
    public struct BImg
    {

    }

    [System.Serializable]
    public struct BChunkCluster
    {
        public int index;
        public V2 posId;
        public Chunk[] chunks;
    }

    [System.Serializable]
    public struct BTextureId
    {
        public int texId;
        public V2 e0;
        public V2 e1;
        public V2 e2;
        public V2 e4;
    }

    [System.Serializable]
    public struct BCam
    {
        public float zoom;
        public V3 pos;
        public V3 rot;
    }

    [System.Serializable]
    public struct Bv2
    {
        public float x;
        public float y;
    }

    [System.Serializable]
    public struct Bv3
    {
        public float x;
        public float y;
        public float z;
    }

}