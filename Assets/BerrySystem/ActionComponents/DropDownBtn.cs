/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class DropDownButton : ActionComponent
    {

        public bool destroy = true, all = true;
        public DropDownOption optionData;

        public override void AwakeActionComponent()
        {
            this.GetComponent<UiIntractable>().curAC = this;
        }

        public override void Click()
        {
            if (optionData.itemPressMethod != null) { optionData.itemPressMethod(); UiManager.DestroyAllFocus(); return; }
            if (ACClick != null) { ACClick(); }
            if (optionData.targetNewWindow != null)
            {
                if (!optionData.destroy)
                {
                    UiManager.DestroyAllFocus();
                }
                if (optionData.hack)
                {
                    WindowManager.CreateWindow(optionData.freeIndex, 300, optionData.targetNewWindow, true, optionData.skipInstanceRef);
                }
                else
                {
                    WindowManager.CreateWindow(300, 300, optionData.targetNewWindow, true, optionData.skipInstanceRef);
                }
            }
            else
            {

            }
        }
    }
}