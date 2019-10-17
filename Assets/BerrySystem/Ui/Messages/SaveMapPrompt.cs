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
    public class SaveMapPrompt : ActionComponent
    {
        public void Prompt(GameObject TargetFocusCanvas, string message = "You are about to leave this masterpiece un-saved!!")
        {
            /*
			GameObject curFocus;
			if(TargetFocusCanvas == null){
				curFocus = UiManager.CreateUiFocusObj(true);
			}else{
				curFocus = TargetFocusCanvas;
			}
			GameObject warning = UiManager.CreateBackgroundObj(curFocus, "EditorInfoPanelShadow", null, new Vector2(0,0), new Vector2(450, 180), new Color(0.5f,0,0,0.4f), UiManager.UiAnchorsMode.FillStretch, false);
			warning.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 3f);
			GameObject shadow = UiManager.CreateBackgroundObj(curFocus, "EditorInfoPanelShadow", Resources.Load<Sprite>("BerrySystem/UI/shadow"), new Vector2(0,0), new Vector2(450, 180), new Color(0,0,0,0.7f), UiManager.UiAnchorsMode.MiddelCenter);
			GameObject curPanel = UiManager.CreateBackgroundObj(shadow, "EditorInfoPanel", null, new Vector2(0,0), new Vector2(400, 150), new Color(1,1,1,1), UiManager.UiAnchorsMode.MiddelCenter, true);
			shadow.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);
			shadow.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, -20), new Vector2(0, 0), false, false, 120f);
			UiManager.CreateTextObj(curPanel, "INFO", message, 14, TextAnchor.MiddleCenter, new Vector2(0,0), new Vector2(40, 40), new Color(0,0,0,1), UiManager.UiAnchorsMode.MiddelStretchHorizontal);
			InteractiveBg dropDownBg = new InteractiveBg();
			dropDownBg.hoverColor = new Color(0, 0.78f, 1f, 0.35f);
			dropDownBg.pressedColor = new Color(0, 0.55f, 0.89f, 0.35f);
			UiManager.CreateButton(curPanel, "Info", null, "Save", 14, TextAnchor.MiddleCenter, dropDownBg, new Color(0,0,0,1), new Color(0,0,0), new Vector2(160, -59), new Vector2(70, 32), UiManager.UiAnchorsMode.TopLeft);
			UiManager.CreateButton(curPanel, "Info", null, "Don't Save", 14, TextAnchor.MiddleCenter, dropDownBg, new Color(0,0,0,1), new Color(0,0,0), new Vector2(75, -59), new Vector2(100, 32), UiManager.UiAnchorsMode.TopLeft);
			UiManager.CreateButton(curPanel, "Info", null, "Cansel", 14, TextAnchor.MiddleCenter, dropDownBg, new Color(0,0,0,1), new Color(0,0,0), new Vector2(-7, -59), new Vector2(66, 32), UiManager.UiAnchorsMode.TopLeft);
			 */
        }
    }
}