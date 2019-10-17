/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class BMapSave : BerryWindow
    {
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            if (MapDataManager.mapDataFilePath == "")
            {
                BMapSaveAs newSave = new BMapSaveAs();
                newSave.WindowCreate(sizeX, sizeY, windowRef);
            }
            else
            {

                // A realy bad way to overide.
                sizeX = 200;
                sizeY = 200;

                // Create the window panel.
                windowPanel.uiSize = new Vector2(sizeX, sizeY);
                windowPanel.uiColor = new Color32(0, 255, 0, 200);
                windowPanel.uiRayCast = true;

                // Window content...
                UiTextObject mapSaved = new UiTextObject();
                mapSaved.uiText = "Map has been saved!";

                // Create 
                UiManager.CreateBackgroundObj(windowRef, windowPanel).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0.2f, 1, true, false, 2f);
            }
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Save Map";
        }
    }
}