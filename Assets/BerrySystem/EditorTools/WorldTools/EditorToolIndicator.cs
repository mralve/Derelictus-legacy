/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolIndicator : ActionComponent
    {
        public EditorTool targetTool;
        public bool isIndicatorBtn;

        public Image toolIndicatorImg;
        public Sprite toolActivatedIcon;
        public Sprite toolDeActivatedIcon;

        public override void AwakeActionComponent()
        {
            toolIndicatorImg = curUiIntractable.targetIcon.GetComponent<Image>();
        }

        public void UpdateIndicatorIcon(bool ToolState)
        {
            if (ToolState)
            {
                toolIndicatorImg.sprite = toolActivatedIcon;
            }
            else
            {
                toolIndicatorImg.sprite = toolDeActivatedIcon;
            }
        }

        public override void Click()
        {
            GlobalToolManager.globalToolManager.SetTool(targetTool);
        }
    }
}