/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolPosition : EditorTool
    {
        GameObject toolWindow;
        SpriteRenderer Icon;
        public Sprite toolActiveSetterIcon;
        public Sprite toolPlacedSetterIcon;
        float tileSize;

        public event ToolEventHandler onToolPrimaryUse;
        public override void ToolActivation()
        {
            UiManager.DestroyAllFocus();
            BPlacerTool toolWindowType = new BPlacerTool();
            toolWindowType.targetTool = this;
            toolWindow = WindowManager.CreateWindow(200, 100, toolWindowType, true, true);
            if (toolActiveSetterIcon != null)
            {

            }
            GlobalToolManager.globalToolManager.ToolSelectorDeactivate();
            curTileSelector.tileSelectionEnabled = false;
            tileSize = MapDataManager.mapDataTileSize;

            Icon = EditorPrimer.SpawnIconRenderer;
            Icon.transform.position = MapDataConverter.V3ToVector3(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn);
            Icon.transform.position += new Vector3(0, 0.4f, 0);
            usesDrag = true;
        }

        public override void ToolDeActivation()
        {
            Icon.sprite = Resources.Load<Sprite>("BerrySystem/Textures/Topdown/alphaSpawn1");
            GameObject.Destroy(toolWindow);
        }

        public override void ToolPrimaryUse()
        {
            if (onToolPrimaryUse != null) { onToolPrimaryUse.Invoke(); }
            toolPrimaryTargetPosition = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);
            toolPrimaryTargetPosition.x = (float)System.Math.Round(toolPrimaryTargetPosition.x, 3);
            toolPrimaryTargetPosition.z = (float)System.Math.Round(toolPrimaryTargetPosition.z, 3);
            //toolPrimaryTargetPosition.z += 0.3f;
            toolPrimaryTargetPosition.y = 0.4f;
            Icon.transform.position = toolPrimaryTargetPosition;
            Icon.sortingOrder = SessionManager.SpriteSortByPos(Icon);
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn = MapDataConverter.Vector3ToV3(toolPrimaryTargetPosition);
        }

        public override void ToolSecondaryUse()
        {

        }

        public void Update()
        {
            Debug.Log("hello world");
            Icon.transform.position = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);
            if (toolActiveSetterIcon)
            {

            }
        }
    }
}