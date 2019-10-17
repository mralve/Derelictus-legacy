/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class DefocusBtn : ActionComponent
    {
        public bool killInput, fadeOut = true, oldAnimation = false;
        public GameObject focusInstanceObjRef;
        public BerryWindow windowReference;

        public override void AwakeActionComponent()
        {
            this.GetComponent<UiIntractable>().curAC = this;
            ScenePrimer.curPrimerComponent.disableInput = true;
            GlobalToolManager.DisableInput(true);
        }
        public override void Click()
        {
            //UiManager.DestroyAllFocus();
            if (windowReference != null) { windowReference.WindowTerminate(); }
            gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(1, 0, false, true, 3.5f, true, UiManager.DestroyAllFocus);
            //GameObject.Destroy(gameObject);
            /*
			if(windowReference != null)
			{
				// Just send a signal to the window before death.
				windowReference.WindowTerminate();
			}
			if(fadeOut)
			{
				this.transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(1, 0, false, true, 9f);
			}else{
			}
			 */
            //if(killInput){GlobalToolManager.globalToolManager.disableInput = false; ScenePrimer.curPrimerComponent.disableInput = false;}

        }
    }
}