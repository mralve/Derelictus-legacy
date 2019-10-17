/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class GamePrimer : PrimerComponent
    {
        public GameObject targetPlayerObj, gameUI;
        public PlayerTopDown targetPlayEntityAgent;

        public override void PrimerInitialize()
        {
            primerModeName = "Game";
            GlobalToolManager.DisabelTools();
            gameUI = UiManager.CreateCanvas("gameUI", primerParrentObj);
            CreatePlayer();
            GL.Clear(true, true, new Color(0, 0, 0, 0));
        }

        public override string PrimerGrabName()
        {
            return primerModeName = "Game Primer";
        }

        public virtual void CreatePlayer()
        {
            targetPlayerObj = new GameObject("Player");
            targetPlayerObj.transform.SetParent(primerParrentObj.transform);
            targetPlayerObj.transform.position = MapDataConverter.V3ToVector3(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn);
            targetPlayerObj.transform.position += new Vector3(0, 0.5f);
            targetPlayEntityAgent = targetPlayerObj.AddComponent<PlayerTopDown>();
            targetPlayEntityAgent.CreateEntity("PlayerGameObject");
            targetPlayEntityAgent.playerCameraObj.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
            GL.Clear(true, true, new Color(0, 0, 0, 1), 1);
        }

        public override void PrimerSwitchEvent()
        {
            //targetPlayEntityAgent.playerCameraObj.GetComponent<Camera>().backgroundColor = MapDataConverter.colToColor32( XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor);
            //CameraManager.curCamera.backgroundColor = MapDataConverter.colToColor32( XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor);
            GL.Clear(true, true, new Color(0, 0, 0, 1), 1);
            targetPlayEntityAgent.playerCameraObj.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        }

        public override void PrimerSaveSession()
        {

        }

        public override void PrimerTerminate()
        {

        }
    }
}
