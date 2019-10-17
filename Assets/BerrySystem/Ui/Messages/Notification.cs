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
    public class Notification : ActionComponent
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
			GameObject shadow = UiManager.CreateBackgroundObj(curFocus, "EditorInfoPanelShadow", Resources.Load<Sprite>("BerrySystem/UI/shadow"), new Vector2(-110,-90), new Vector2(320, 40), new Color(0,0,0,0), UiManager.UiAnchorsMode.TopRight);
			GameObject curPanel = UiManager.CreateBackgroundObj(shadow, "EditorInfoPanel", null, new Vector2(0,0), new Vector2(200, 32), new Color(1,1,1,0.8f), UiManager.UiAnchorsMode.TopRight, true);
			shadow.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);
			UiManager.CreateTextObj(curPanel, "INFO", message, 14, TextAnchor.MiddleCenter, new Vector2(0,0), new Vector2(40, 40), new Color(0,0,0,1), UiManager.UiAnchorsMode.MiddelStretchHorizontal);
			StartCoroutine(startDelay());
			 */
        }

        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(2);
            this.transform.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(1, 0, false, true, 6f);
        }
    }
}