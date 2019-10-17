/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class NewLayer : BerryWindow
    {
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            SessionManager.CreateMapLayer("Layer " + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length, false, false);
            SessionManager.CreateNewMapLayerObject(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length - 1);
            UiManager.DestroyAllFocus();
        }

        public override void WindowTerminate()
        {
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Kill Preview Session";
        }
    }
}