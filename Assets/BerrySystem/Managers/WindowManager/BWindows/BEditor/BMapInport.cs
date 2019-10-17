/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class BMapImport : BerryWindow
    {
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            // A realy bad way to overide.
            sizeX = 400;
            sizeY = 280;

            // Create a window shadow.
            windowShadow.uiObjName = "Shadow";
            windowShadow.uiTextureRef = Resources.Load<Sprite>("BerrySystem/UI/shadow");
            windowShadow.uiColor = new Color32(255, 255, 255, 255);
            windowShadow.uiSize = new Vector2(sizeX + 90, sizeY + 90);
            UiManager.CreateBackgroundObj(windowRef, windowShadow);

            // Create the window panel.
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            windowPanel.uiRayCast = true;
            UiManager.CreateBackgroundObj(windowRef, windowPanel).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            // Window content...
            UiTextObject windowTitle = new UiTextObject();
            windowTitle.uiTextColor = new Color(0, 0, 0, 255);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiSize = new Vector2(77, 19);
            windowTitle.uiTextAlign = TextAnchor.MiddleCenter;
            windowTitle.uiPosition = new Vector2(0, 120);
            UiManager.CreateTextObj(windowRef, windowTitle);

            UiTextInputField uiMapName = new UiTextInputField();
            uiMapName.uiPosition = new Vector2(0, 30);
            uiMapName.uiSize = new Vector2(362, 34);
            uiMapName.uiButtonBackgroundObject.uiRayCast = true;
            uiMapName.uiButtonBackgroundObject.uiSize = uiMapName.uiSize;
            uiMapName.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiMapName.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiMapName.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiMapName.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiMapName.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            uiMapName.uiButtonIcon.uiSize = new Vector2(uiMapName.uiSize.x - 2, uiMapName.uiSize.y - 2);
            uiMapName.fieldName = new UiTextObject();
            uiMapName.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            uiMapName.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            uiMapName.fieldName.uiSize = uiMapName.uiSize;
            uiMapName.fieldName.uiPosition += new Vector2(3, 25);
            uiMapName.fieldName.uiText = "Map Name";
            InputField feildObj0 = UiManager.CreateTextInputField(windowRef, uiMapName);

            uiMapName.fieldName.uiText = "Map File path";
            uiMapName.uiPosition = new Vector2(0, -50);
            InputField feildObj1 = UiManager.CreateTextInputField(windowRef, uiMapName);

            UiButtonObject saveButton = new UiButtonObject();
            saveButton.uiPosition = new Vector2(156, -103);
            saveButton.uiSize = new Vector2(50, 34);
            saveButton.uiButtonBackgroundObject.uiRayCast = true;
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;

            saveButton.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            saveButton.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            saveButton.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            saveButton.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            saveButton.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiButtonText = new UiTextObject();
            saveButton.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            saveButton.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            saveButton.uiButtonText.uiText = "Open";

            BMapImportBtn curBSaveMapAs = UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<BMapImportBtn>();
            curBSaveMapAs.targetFeild0 = feildObj0;
            curBSaveMapAs.targetFeild1 = feildObj1;

            UiTextObject ErrorTitle = new UiTextObject();
            ErrorTitle.uiTextColor = new Color(255, 0, 0, 255);
            ErrorTitle.uiText = "There must be a name for the map!";
            ErrorTitle.uiSize = new Vector2(262, 21);
            ErrorTitle.uiPosition = new Vector2(-50, 0);
            curBSaveMapAs.error0 = UiManager.CreateTextObj(windowRef, ErrorTitle);
            curBSaveMapAs.error0.SetActive(false);

            UiBackgroundObject errorBg = new UiBackgroundObject();
            errorBg.uiColor = new Color(1, 0, 0, 0.1f);
            errorBg.uiPosition = new Vector2(50, 30);
            errorBg.uiSize = new Vector2(364, 36);
            UiManager.CreateBackgroundObj(curBSaveMapAs.error0, errorBg);

            UiTextObject ErrorTitle1 = new UiTextObject();
            ErrorTitle1.uiTextColor = new Color(255, 0, 0, 255);
            ErrorTitle1.uiText = "There must be a valid file path so the map can be saved!!";
            ErrorTitle1.uiSize = new Vector2(262, 21);
            ErrorTitle1.uiPosition = new Vector2(-50, -80);
            curBSaveMapAs.error1 = UiManager.CreateTextObj(windowRef, ErrorTitle1);
            curBSaveMapAs.error1.SetActive(false);

            UiBackgroundObject errorBg1 = new UiBackgroundObject();
            errorBg1.uiColor = new Color(1, 0, 0, 0.1f);
            errorBg1.uiPosition = new Vector2(50, 30);
            errorBg1.uiSize = new Vector2(364, 36);
            UiManager.CreateBackgroundObj(curBSaveMapAs.error1, errorBg1);

            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Import Map";
        }
    }

    public class BMapImportBtn : ActionComponent
    {
        public InputField targetFeild0, targetFeild1;
        public GameObject error0, error1;
        bool ok, ok1;
        public override void AwakeActionComponent()
        {
            this.GetComponent<UiIntractable>().curAC = this;
        }

        public override void Click()
        {
            if (targetFeild0.text == "")
            {
                error0.SetActive(true);
                ok = false;
            }
            else
            {
                error0.SetActive(false);
                ok = true;
            }

            if (MapDataManager.IsValidPath(targetFeild1.text))
            {
                error1.SetActive(false);
                ok1 = true;
            }
            else
            {
                error1.SetActive(true);
                ok1 = false;
            }

            if (ok && ok1)
            {
                UiManager.DestroyAllFocus();
                Debug.LogError("NOT IMPLEMENTED!");
                //MapDataManager.MapDataOpenMap(targetFeild0.text, targetFeild1.text);
            }

        }
    }
}