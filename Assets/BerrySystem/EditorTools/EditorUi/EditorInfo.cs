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
    public class EditorInfo : ActionComponent
    {
        EditorPrimer curEditorObj;

        public override void AwakeActionComponent()
        {
            // Tell the current actionComponent that we are here!
            curEditorObj = ScenePrimer.curPrimerParrentObj.GetComponent<EditorPrimer>();
            this.GetComponent<UiIntractable>().curAC = this;
        }

        public override void Click()
        {
            GameObject curFocus = UiManager.CreateUiFocusObj();

            // Create the panels background shadow.
            UiBackgroundObject editorInfoPanelShadow = new UiBackgroundObject();
            editorInfoPanelShadow.uiObjName = "editorInfoPanelShadow";
            editorInfoPanelShadow.uiTextureRef = Resources.Load<Sprite>("BerrySystem/UI/shadow");
            editorInfoPanelShadow.uiSize = new Vector2(450, 350);
            editorInfoPanelShadow.uiColor = new Color(0, 0, 0, 0.7f);
            GameObject shadow = UiManager.CreateBackgroundObj(curFocus, editorInfoPanelShadow);

            // Create the actual panel.
            UiBackgroundObject editorInfoPanel = new UiBackgroundObject();
            editorInfoPanel.uiObjName = "EditorInfoPanel";
            editorInfoPanel.uiSize = new Vector2(400, 300);
            editorInfoPanel.uiColor = new Color(1, 1, 1, 1);
            editorInfoPanel.uiRayCast = true;
            GameObject curPanel = UiManager.CreateBackgroundObj(curFocus, editorInfoPanel);

            // Play the pop in intro animation.
            shadow.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);
            shadow.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, -20), new Vector2(0, 0), false, false, 120f);

            // Create the panel logo.
            UiBackgroundObject editorLogo = new UiBackgroundObject();
            editorLogo.uiObjName = "editorLogo";
            editorLogo.uiTextureRef = Resources.Load<Sprite>("BerrySystem/Icons/mapBerryIcon");
            editorLogo.uiSize = new Vector2(250, 250);
            editorLogo.uiColor = new Color(1, 1, 1, 0.1f);
            UiManager.CreateBackgroundObj(curFocus, editorLogo);

            // Editor name!
            UiTextObject editorName = new UiTextObject();
            editorName.uiObjName = "editorName";
            editorName.uiText = "MapBerry";
            editorName.uiTextSize = 30;
            editorName.uiTextAlign = TextAnchor.MiddleCenter;
            editorName.uiSize = new Vector2(40, 40);
            editorName.uiTextColor = new Color();
            editorName.uiAnchorMode = UiManager.UiAnchorsMode.MiddelStretchHorizontal;
            UiManager.CreateTextObj(curPanel, editorName);

            // Editor Stage Version
            UiTextObject editorStageVersion = new UiTextObject();
            editorStageVersion.uiObjName = "editorStageVersion";
            editorStageVersion.uiText = "A L P H A";
            editorStageVersion.uiTextSize = 16;
            editorStageVersion.uiTextAlign = TextAnchor.MiddleCenter;
            editorStageVersion.uiPosition = new Vector2(0, -10);
            editorStageVersion.uiSize = new Vector2(40, 40);
            editorStageVersion.uiTextColor = new Color();
            editorStageVersion.uiAnchorMode = UiManager.UiAnchorsMode.MiddelStretchHorizontal;
            UiManager.CreateTextObj(curPanel, editorStageVersion);

            // Editor Version
            UiTextObject editorVersion = new UiTextObject();
            editorVersion.uiObjName = "editorVersion";
            editorVersion.uiText = curEditorObj.editorVersionNumberDisplay;
            editorVersion.uiTextSize = 12;
            editorVersion.uiTextAlign = TextAnchor.MiddleCenter;
            editorVersion.uiPosition = new Vector2(0, -30);
            editorVersion.uiSize = new Vector2(40, 40);
            editorVersion.uiTextColor = new Color();
            editorVersion.uiAnchorMode = UiManager.UiAnchorsMode.MiddelStretchHorizontal;
            UiManager.CreateTextObj(curPanel, editorVersion);

            // editor dev/author
            UiTextObject editorDev = new UiTextObject();
            editorDev.uiObjName = "editorDev";
            editorDev.uiText = "Created by Alve Larsson. zurra.alve@gmail.com";
            editorDev.uiTextSize = 9;
            editorDev.uiTextAlign = TextAnchor.MiddleCenter;
            editorDev.uiPosition = new Vector2(0, -86);
            editorDev.uiSize = new Vector2(20, 20);
            editorDev.uiTextColor = new Color();
            editorDev.uiAnchorMode = UiManager.UiAnchorsMode.MiddelStretchHorizontal;
            UiManager.CreateTextObj(curPanel, editorDev);

            // editor Copyright
            UiTextObject editorCopyright = new UiTextObject();
            editorCopyright.uiObjName = "editorCopyright";
            editorCopyright.uiText = "Copyright (C) Xnomoto Software - All Rights Reserved";
            editorCopyright.uiTextSize = 9;
            editorCopyright.uiTextAlign = TextAnchor.MiddleCenter;
            editorCopyright.uiPosition = new Vector2(0, -100);
            editorCopyright.uiSize = new Vector2(20, 20);
            editorCopyright.uiTextColor = new Color();
            editorCopyright.uiAnchorMode = UiManager.UiAnchorsMode.MiddelStretchHorizontal;
            UiManager.CreateTextObj(curPanel, editorCopyright);

            this.transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(1, 0, false, true, 8f);
        }
    }
}