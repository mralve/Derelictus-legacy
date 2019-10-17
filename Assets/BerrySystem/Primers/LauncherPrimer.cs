/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class LauncherPrimer : PrimerComponent
    {
        public GameObject launcherUI;

        public override void PrimerInitialize()
        {
            // Set the launchers window size. (This needs to be small so it can fit on smaller screens)
            Screen.SetResolution(545, 200, false);

            // Set the currently initialized primer info.
            primerModeName = "Launcher";
            launcherUI = UiManager.CreateCanvas("LauncherUi", null, true, true, 0, true);

            UiBackgroundObject launcherBg = new UiBackgroundObject();
            launcherBg.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            launcherBg.uiTextureRef = Resources.Load<Sprite>("BerrySystem/Icons/launcher_img");
            launcherBg.uiPosition = new Vector2(0, -102);
            launcherBg.uiSize = new Vector2(490, 200);
            UiManager.CreateBackgroundObj(launcherUI, launcherBg);

            UiTextObject launcherText = new UiTextObject();
            launcherText.uiText = "Derelictus.";
            launcherText.uiTextSize = 25;
            launcherText.uiSize = new Vector2(200, 50);
            launcherText.uiPosition = new Vector2(0, 30);
            launcherText.uiTextAlign = TextAnchor.MiddleCenter;
            UiManager.CreateTextObj(launcherUI, launcherText);

            launcherText.uiTextSize = 18;
            launcherText.uiText = "Indev 0.1";
            launcherText.uiPosition = new Vector2(0, 0);
            UiManager.CreateTextObj(launcherUI, launcherText);

            UiButtonObject launcherPlayBtn = new UiButtonObject();
            launcherPlayBtn.uiAnchorMode = UiManager.UiAnchorsMode.BottomRight;
            launcherPlayBtn.uiPosition = new Vector2(-32, 32);
            launcherPlayBtn.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            launcherPlayBtn.uiButtonIcon.uiRayCast = true;
            UiManager.CreateButton(launcherUI, launcherPlayBtn);

        }

        public override void PrimerTerminate()
        {
            Destroy(launcherUI);
        }
    }
}
