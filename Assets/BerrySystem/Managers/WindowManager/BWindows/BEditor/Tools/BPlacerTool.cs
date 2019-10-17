/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ConstruiSystem
{
    public class BPlacerTool : BerryWindow
    {
        public Text infoTextComp;
        public EditorToolPosition targetTool;
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;

            // Create the window panel.
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            windowPanel.uiPosition = new Vector2(Screen.width / 2 - sizeX * 0.5f - 16, Screen.height / 2 * -1 + sizeY - 32);
            windowPanel.uiRayCast = true;
            GameObject panel = UiManager.CreateBackgroundObj(windowRef, windowPanel);
            panel.transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            UiTextObject infoText = new UiTextObject();
            infoText.uiTextAlign = TextAnchor.MiddleCenter;
            infoText.uiSize = new Vector2(sizeX, sizeY);
            infoText.uiTextColor = new Color(0, 0, 0, 1);
            infoText.uiTextSize = 13;

            infoTextComp = UiManager.CreateTextObj(panel, infoText).GetComponent<Text>();

            targetTool.onToolPrimaryUse += SetText;

            SetText();

            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(20, 0), new Vector2(0, 0), false, false, 120f);
        }

        public void SetText()
        {
            infoTextComp.text = "Set spawn point.                   New  : X : "
            + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn.x + " Y : "
            + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn.y + " Z : "
            + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn.z + " ";
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Layer Settings";
        }
    }
}