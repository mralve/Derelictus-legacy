/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public static class WindowManager
    {
        // The actual window container object that is the parrent of the windows.
        public static GameObject windowContainer;

        // A reference to the windows outer border.
        public static BorderObject windowOuterBorder;

        // An array of window instances.
        public static GameObject[] windows;

        // The current window in focus.
        public static GameObject uiFocusInstance;

        // the diffrent window types built in.
        public enum StandardWindowTypes { console }

        // CreateWindowOfTypes is a easy way to create windows, it takes care the setup for you.
        // CreateWindowOfTypes also returns the created window so that setup can be done with less code.
        public static void CreateWindowOfType()
        {

        }

        // CreateWindow is the manual way of creating a window.
        public static GameObject CreateWindow(int sizeX, int sizeY, BerryWindow windowType, bool spearateInstace = true, bool disableExitBg = false, bool skipInstanceRef = false)
        {
            // Disable scrolling. ( UiManager destroy all focus and BerryWindow will take care of re-enabling scrolling )
            if (ScenePrimer.curPrimerComponent.curZoomComp != null)
            {
                ScenePrimer.curPrimerComponent.curZoomComp.zoomingEnabled = false;
                ScenePrimer.curPrimerComponent.curZoomComp.mouseScroll = 0;
            }

            ScenePrimer.curPrimerComponent.disableInput = true;

            // Create the window object and set it's parrent to be the target focus object.
            GameObject newWindow = new GameObject(windowType.windowTitleName);
            uiFocusInstance = UiManager.CreateUiFocusObj(spearateInstace, disableExitBg, true, windowType, false, skipInstanceRef);
            newWindow.transform.SetParent(uiFocusInstance.transform);
            windowType.isSepareateFocus = spearateInstace;


            // Create the actual window Object
            windowType.WindowCreate(sizeX, sizeY, newWindow);

            return newWindow;
        }

    }
}