/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;


namespace ConstruiSystem
{
    public class BerryWindow
    {
        // The window theme is a set of styling instructions of the window to draw from.
        public BerryTheme windowTheme = new BerryTheme();

        // The window title is the windows tile on screen.
        public string windowTitleName = "window";

        // Window UI short name used for options.
        public string windowShortName = "Short name";

        // The window size is the windows spawn in size.
        public Vector2 windowSize = new Vector2(100, 100);

        // The window shadow is deafult window shadow object witch is displays being the window.
        public UiBackgroundObject windowShadow = new UiBackgroundObject();

        // The window panel is the windows standard background.
        public UiBackgroundObject windowPanel = new UiBackgroundObject();
        // 
        public GameObject targetWindowRef;
        public bool isSepareateFocus, isSubWindow;

        // Windowcreate is deafult function for creating the window.
        public virtual void WindowCreate(int sizeX, int sizeY, GameObject windowRef = null)
        {

        }
        // WindowTerminate is the deafult termination function, this is the function witch effectively kills the window. 
        public virtual void WindowTerminate()
        {
            ScenePrimer.curEditorPrimer.takesInput = true;
            if (targetWindowRef != null)
            {
                targetWindowRef.AddComponent<QuickUiAnimator>().PlayScaleAnim(new Vector2(1, 1), new Vector2(0f, 0f), false, false, 3.5f);
                targetWindowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 0), new Vector2(0, -50), false, false, 800f, false, WindowDestroy);
            }
        }
        public virtual void WindowDestroy()
        {
            if (!isSubWindow)
            {
                // Enable scrolling. 
                if (ScenePrimer.curPrimerComponent.curZoomComp != null)
                {
                    ScenePrimer.curPrimerComponent.curZoomComp.zoomingEnabled = true;
                }
                ScenePrimer.curPrimerComponent.disableInput = false;
            }
            // Switch to the current window inheritance parrent mode.
            if (isSepareateFocus)
            {
                GameObject.Destroy(targetWindowRef.transform.parent.gameObject);
            }
            else
            {
                GameObject.Destroy(targetWindowRef);
            }
        }
        // Window move handles the movement of the window.
        public virtual void WindowMove()
        {

        }
        // Overide this and set windowShortName to the windows name in the Ui.
        public virtual string WindowGrabName()
        {
            return "window ui name";

        }
    }
}