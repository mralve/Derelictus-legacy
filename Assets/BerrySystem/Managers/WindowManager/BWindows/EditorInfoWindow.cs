/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class BEditorInfoWindow : BerryWindow
    {
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            // A realy bad way to overide.
            sizeX = 400;
            sizeY = 400;

            windowShadow.uiObjName = "Shadow";
            windowShadow.uiTextureRef = Resources.Load<Sprite>("BerrySystem/UI/shadow");
            windowShadow.uiColor = new Color32(255, 255, 255, 255);
            windowShadow.uiSize = new Vector2(sizeX + 90, sizeY + 90);
            UiManager.CreateBackgroundObj(windowRef, windowShadow);

            windowPanel.uiColor = new Color32(243, 243, 243, 255);
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            UiManager.CreateBackgroundObj(windowRef, windowPanel);

            UiBackgroundObject editorIcon = new UiBackgroundObject();
            editorIcon.uiTextureRef = Resources.Load<Sprite>("BerrySystem/Icons/Main");
            editorIcon.uiObjName = "EditorIcon";
            editorIcon.uiColor = new Color32(219, 219, 219, 230);
            editorIcon.uiPosition = new Vector2(10, 50);
            editorIcon.uiSize = new Vector2(140, 140);
            UiManager.CreateBackgroundObj(windowRef, editorIcon);

            UiTextObject editorNameText = new UiTextObject();
            editorNameText.uiTextColor = new Color32(0, 0, 0, 255);
            editorNameText.uiSize = new Vector2(150, 32);
            editorNameText.uiPosition = new Vector2(0, -70);
            editorNameText.uiTextSize = 25;
            editorNameText.uiText = "Construi";
            editorNameText.uiTextAlign = TextAnchor.MiddleCenter;
            UiManager.CreateTextObj(windowRef, editorNameText);

            editorNameText.uiText = "A L P H A 2";
            editorNameText.uiPosition = new Vector2(0, -100);
            editorNameText.uiTextSize = 18;
            UiManager.CreateTextObj(windowRef, editorNameText);

            /*
            UiTextObject credit = new UiTextObject();
            credit.uiTextColor = new Color32(0, 0, 0, 255);
            credit.uiSize = new Vector2(160, 16);
            credit.uiPosition = new Vector2(0, -150);
            credit.uiTextSize = 12;
            credit.uiText = "Made By Alve Larsson";
            credit.uiTextAlign = TextAnchor.MiddleCenter;
            UiManager.CreateTextObj(windowRef, credit).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);
             */

            // Add a scale fade in.
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
            //windowRef.AddComponent<QuickUiAnimator>().PlayScaleAnim(new Vector2(0, 0), new Vector2(1,1), false, false, 2f);

        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Editor";
        }
    }
}