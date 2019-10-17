/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class EditorXDP : BerryWindow
    {
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            // A realy bad way to overide.
            sizeX = 400;
            sizeY = 400;

            windowShadow.uiObjName = "Shadow";
            windowShadow.uiTextureRef = Resources.Load<Sprite>("BerrySystem/UI/shadow");
            windowShadow.uiColor = new Color32(17, 20, 20, 255);
            windowShadow.uiSize = new Vector2(sizeX + 90, sizeY + 90);
            UiManager.CreateBackgroundObj(windowRef, windowShadow);

            windowPanel.uiColor = new Color32(13, 15, 15, 255);
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            windowPanel.uiRayCast = true;
            UiManager.CreateBackgroundObj(windowRef, windowPanel);

            UiBackgroundObject editorIcon = new UiBackgroundObject();
            editorIcon.uiTextureRef = Resources.Load<Sprite>("BerrySystem/Icons/xdp");
            editorIcon.uiObjName = "EditorIcon";
            editorIcon.uiColor = new Color32(219, 219, 219, 230);
            editorIcon.uiPosition = new Vector2(0, 60);
            editorIcon.uiSize = new Vector2(150, 150);
            UiManager.CreateBackgroundObj(windowRef, editorIcon);

            UiTextObject editorNameText = new UiTextObject();
            editorNameText.uiTextColor = new Color32(239, 239, 255, 255);
            editorNameText.uiSize = new Vector2(300, 32);
            editorNameText.uiPosition = new Vector2(0, -70);
            editorNameText.uiTextSize = 25;
            editorNameText.uiText = "XDP";
            editorNameText.uiTextAlign = TextAnchor.MiddleCenter;
            UiManager.CreateTextObj(windowRef, editorNameText);

            editorNameText.uiText = "Service not ready...";
            editorNameText.uiPosition = new Vector2(0, -100);
            editorNameText.uiTextSize = 18;
            UiManager.CreateTextObj(windowRef, editorNameText);

            UiTextObject credit = new UiTextObject();
            credit.uiTextColor = new Color32(239, 239, 255, 255);
            credit.uiSize = new Vector2(160, 16);
            credit.uiPosition = new Vector2(0, -150);
            credit.uiTextSize = 12;
            credit.uiText = "Copyright Xnomoto 2018.";
            credit.uiTextAlign = TextAnchor.MiddleCenter;
            UiManager.CreateTextObj(windowRef, credit).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            if (!ScenePrimer.curEditorPrimer.editorCreated)
            {
                UiButtonObject saveButton = new UiButtonObject();
                saveButton.uiSize = new Vector2(70, 34);
                saveButton.uiButtonBackgroundObject.uiRayCast = true;
                saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;

                saveButton.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
                saveButton.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
                saveButton.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
                saveButton.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
                saveButton.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
                saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
                saveButton.uiButtonText = new UiTextObject();
                saveButton.uiButtonText.uiSize = saveButton.uiSize;
                saveButton.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
                saveButton.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;

                saveButton.uiButtonText.uiText = "Close";
                saveButton.uiPosition = new Vector2(140, -165);
                UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = WindowTerminate;
            }
            // Add a scale fade in.
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
            //windowRef.AddComponent<QuickUiAnimator>().PlayScaleAnim(new Vector2(0, 0), new Vector2(1,1), false, false, 2f);

        }



        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Xnomoto Dev Platform";
        }
    }
}