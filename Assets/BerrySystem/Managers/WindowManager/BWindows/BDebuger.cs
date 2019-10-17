/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class BDebugger : BerryWindow
    {
        Image[] probes;
        bool[] probesTick;
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            sizeX = 500;
            sizeY = 300;
            targetWindowRef = windowRef;
            windowShadow.uiObjName = "Shadow";
            windowShadow.uiTextureRef = Resources.Load<Sprite>("BerrySystem/UI/shadow");
            windowShadow.uiColor = new Color32(0, 0, 0, 255);
            windowShadow.uiSize = new Vector2(sizeX + 20, sizeY + 20);
            UiManager.CreateBackgroundObj(windowRef, windowShadow);

            windowRef.AddComponent<DebuggerMonoProbe>().probeUpdate = ProbeUpdate;
            windowRef.GetComponent<DebuggerMonoProbe>().probeTick = ProbeTick;

            windowPanel.uiColor = new Color32(0, 0, 255, 255);
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            UiManager.CreateBackgroundObj(windowRef, windowPanel);

            UiTextObject windowTitle = new UiTextObject();
            windowTitle.uiTextColor = new Color(255, 0, 0, 255);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiSize = new Vector2(140, 19);
            windowTitle.uiTextAlign = TextAnchor.UpperCenter;
            windowTitle.uiPosition = new Vector2(0, 130);
            UiManager.CreateTextObj(windowRef, windowTitle);

            windowRef.transform.position = new Vector2(464, -242);

            probes = new Image[1];
            probesTick = new bool[1];
            UiBackgroundObject probe = new UiBackgroundObject();

            probe.uiSize = new Vector2(32, 32);
            probe.uiColor = new Color(1, 0, 0, 1);
            probes[0] = UiManager.CreateBackgroundObj(windowRef, probe).GetComponent<Image>();
        }

        public void ProbeUpdate()
        {
        }

        public void ProbeTick()
        {
            if (probesTick[0]) { probes[0].color = new Color(1, 0, 0, 1); probesTick[0] = false; } else { probes[0].color = new Color(0.6f, 0f, 0, 1); probesTick[0] = true; }
        }

        public void CreateProbe()
        {

        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "DEBUGGER";
        }
    }

    public class DebuggerMonoProbe : MonoBehaviour
    {
        public delegate void probe();
        public probe probeUpdate;
        public probe probeTick;
        public int probeInterval = 10;
        int t;
        void FixedUpdate() { probeUpdate(); t++; if (t >= probeInterval) { t = 0; probeTick(); } }
    }
}