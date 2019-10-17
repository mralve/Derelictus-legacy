/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorTool
    {

        public TileSelector curTileSelector;
        public EditorToolIndicator curToolIndicator;
        public bool isToolActive;
        public bool inBrushMode = true, usesDrag;
        public bool isToolLocked, careAbutUi = true;
        public bool isTileBased;
        public Vector3 toolPrimaryTargetPosition;

        public delegate void ToolEventHandler();
        public event ToolEventHandler onToolActivation;
        public event ToolEventHandler onToolDeActivation;
        public event ToolEventHandler onToolSecondaryUse;

        // Tool activation
        public void PreToolActivation()
        {
            if (curToolIndicator != null)
            {
                curToolIndicator.toolIndicatorImg.sprite = curToolIndicator.toolActivatedIcon;
                curToolIndicator.UpdateIndicatorIcon(true);
                curToolIndicator.targetTool = this;
            }
            isToolActive = true;
            ToolActivation();
            if (onToolActivation != null) { onToolActivation.Invoke(); }
        }


        // Tool de-activation
        public void PreToolDeActivation()
        {
            if (curToolIndicator != null) { curToolIndicator.toolIndicatorImg.sprite = curToolIndicator.toolDeActivatedIcon; }
            //            curToolIndicator.UpdateIndicatorIcon(false);
            isToolActive = false;
            ToolDeActivation();
            if (onToolDeActivation != null) { onToolDeActivation.Invoke(); }
        }

        // Tool Tool Secondary Use
        public void PreToolSecondaryUse()
        {
            ToolSecondaryUse();
            if (onToolSecondaryUse != null) { onToolSecondaryUse.Invoke(); }
        }

        // Tool Tool Primary Use
        public void PreToolPrimaryUse()
        {
            ToolPrimaryUse();
        }

        public virtual void ToolActivation() { }
        public virtual void ToolDeActivation() { }
        public virtual void ToolUpdate() { }
        public virtual void ToolPrimaryUse() { }
        public virtual void ToolSecondaryUse() { }
        // The mode swap gets called when the user presses down the Tab key.
        public virtual void ToolModeSwap() { }
        public virtual void ToolLock(bool lockState)
        {
            isToolLocked = lockState;
        }
    }
}