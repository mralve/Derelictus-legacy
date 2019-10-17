/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolTileFill : EditorTool
    {
        Vector3 cursorTransform;
        public bool clearChunkFill;
        float tileSize;
        public override void ToolActivation()
        {
            ScenePrimer.curEditorPrimer.curSpriteMode = false;
            UpdateColor();
            tileSize = MapDataManager.mapDataTileSize;
            usesDrag = true;
        }

        public override void ToolDeActivation()
        {
        }

        public override void ToolPrimaryUse()
        {
            cursorTransform = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);
            cursorTransform.x = (float)System.Math.Round(cursorTransform.x / tileSize) * tileSize - tileSize * 0.5f;
            cursorTransform.z = (float)System.Math.Round(cursorTransform.z / tileSize) * tileSize + tileSize * 0.5f;
            SessionManager.FillChunk(cursorTransform, ScenePrimer.curEditorPrimer.curLayer, ScenePrimer.curEditorPrimer.curTileId, clearChunkFill);
        }

        public override void ToolSecondaryUse()
        {

        }

        public void UpdateColor()
        {
            if (clearChunkFill)
            {
                curTileSelector.SetColor(new Color32(255, 150, 0, 255));
            }
            else
            {
                curTileSelector.SetColor(new Color32(0, 255, 0, 255));
            }
        }

        public override void ToolModeSwap()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                clearChunkFill = !clearChunkFill;
                UpdateColor();
            }
        }
    }
}