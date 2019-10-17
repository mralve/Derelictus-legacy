/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolLineMesh : EditorTool
    {
        Vector3 cursorTransform;
        float tileSize;
        bool settingMode;
        SpriteRenderer Icon;
        Chunk targetChunk;

        public override void ToolActivation()
        {
            curTileSelector.gameObject.SetActive(false);
            curTileSelector.SetColor(new Color32(255, 255, 255, 255));
        }

        public override void ToolUpdate()
        {

        }

        public override void ToolDeActivation()
        {

        }

        public override void ToolPrimaryUse()
        {

        }

        public override void ToolSecondaryUse()
        {

        }

        public override void ToolModeSwap()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
            }

        }
    }
}