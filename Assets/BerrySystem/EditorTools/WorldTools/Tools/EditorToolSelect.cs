/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolSelect : EditorTool
    {
        Vector3 cursorTransform;
        float tileSize;

        public override void ToolActivation()
        {
            curTileSelector.SetColor(new Color32(255, 255, 255, 255));
            curTileSelector.curLineRenderer.material = Resources.Load("BerrySystem/Shaders/Select", typeof(Material)) as Material;
            tileSize = MapDataManager.mapDataTileSize;
        }

        public override void ToolDeActivation()
        {
            curTileSelector.curLineRenderer.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
        }

        public override void ToolPrimaryUse()
        {
            cursorTransform = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);

            cursorTransform.x = (float)System.Math.Round(cursorTransform.x / tileSize) * tileSize - tileSize * 0.5f;
            cursorTransform.z = (float)System.Math.Round(cursorTransform.z / tileSize) * tileSize + tileSize * 0.5f;
        }

        public override void ToolSecondaryUse()
        {

        }
        /*
		public void Update()
		{
			//if(Input.GetMouseButtonDown(0)){ToolPrimaryUse();}
		}
		 */
    }
}