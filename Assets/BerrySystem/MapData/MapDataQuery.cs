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
    public static class MapDataQuery
    {
        // Saves the mapdata
        public static MapData QueryMapData()
        {
            MapData curMapData = new MapData();

            curMapData.mapCams = new CamInstance[CameraManager.CameraRefs.Length];
            for (int i = 0; i < curMapData.mapCams.Length; i++)
            {
                curMapData.mapCams[i] = new CamInstance();
                curMapData.mapCams[i].pos = MapDataConverter.Vector3ToV3(CameraManager.CameraRefs[i].transform.position);
                curMapData.mapCams[i].rot = MapDataConverter.QuaternionToV3(CameraManager.CurrentRenderCamera.transform.rotation);
            }

            return curMapData;
        }

        /*
		public static MapData QueryMapDataAndLoad()
		{
			MapData curMapData = MapDataManager.mapData;
			CameraManager.CameraRefs = new GameObject[curMapData.mapCams.Length];
			for (int i = 0; i < curMapData.mapCams.Length; i++)
			{
				//CameraManager.SetupNewCamera()
			}
			return curMapData;
		}
		 */

        public static void DeleteMapQuery()
        {
            //MapData curMapData = new MapData();
            if (CameraManager.CameraRefs != null)
            {
                for (int i = 0; i < CameraManager.CameraRefs.Length; i++)
                {
                    GameObject.Destroy(CameraManager.CameraRefs[i]);
                }
                CameraManager.curCamera = null;
                CameraManager.CurrentRenderCamera = null;
                CameraManager.CameraRefs = null;
            }
            /*
			if(MapDataManager.mapData != null)
			{
				System.Array.Clear(MapDataManager.mapData.mapLayers, 0, MapDataManager.mapData.mapLayers.Length);
			}
			MapDataManager.mapData = null;
			 */
            //System.GC.Collect();

            if (MapDataManager.mapDataMapObj != null)
            {
                GameObject.Destroy(MapDataManager.mapDataMapObj);
                MapDataManager.mapDataMapObj = null;
            }


            // ****** DONE ******
            ScenePrimer.curPrimerComponent.PrimerMapUpdate();
        }
    }
}
