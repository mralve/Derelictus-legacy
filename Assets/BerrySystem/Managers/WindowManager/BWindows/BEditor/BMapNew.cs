/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class BMapNew : BerryWindow
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
            InputField feildObj = UiManager.CreateTextInputField(windowRef, uiMapName);
            /*
            uiMapName.fieldName.uiText = "Map File path";
            uiMapName.uiPosition = new Vector2(0, -40);
            UiManager.CreateTextInputField(windowRef, uiMapName);
             */

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
            saveButton.uiButtonText.uiText = "Create";

            MapNewBtn curMapNewBtn = UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<MapNewBtn>();
            curMapNewBtn.targetField = feildObj;

            if (!ScenePrimer.curEditorPrimer.editorCreated)
            {
                saveButton.uiButtonText.uiText = "Cancel";
                saveButton.uiPosition = new Vector2(100, -103);
                UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = WindowTerminate;
            }

            // Window content...
            UiTextObject ErrorTitle = new UiTextObject();
            ErrorTitle.uiTextColor = new Color(255, 0, 0, 255);
            ErrorTitle.uiText = "There must be a name for a new map!";
            ErrorTitle.uiSize = new Vector2(262, 21);
            ErrorTitle.uiPosition = new Vector2(-50, 0);
            curMapNewBtn.Error = UiManager.CreateTextObj(windowRef, ErrorTitle);
            curMapNewBtn.Error.SetActive(false);

            UiBackgroundObject errorBg = new UiBackgroundObject();
            errorBg.uiColor = new Color(1, 0, 0, 0.1f);
            errorBg.uiPosition = new Vector2(50, 30);
            errorBg.uiSize = new Vector2(364, 36);
            UiManager.CreateBackgroundObj(curMapNewBtn.Error, errorBg);


            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }
        public override void WindowTerminate()
        {
            ScenePrimer.curEditorPrimer.takesInput = false;
            targetWindowRef.AddComponent<QuickUiAnimator>().PlayScaleAnim(new Vector2(1, 1), new Vector2(0f, 0f), false, false, 3.5f);
            targetWindowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 0), new Vector2(0, -50), false, false, 800f, false, WindowDestroy);
        }
        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "New Map";
        }
    }
    public class MapNewBtn : ActionComponent
    {
        public InputField targetField;
        public GameObject Error;
        public override void AwakeActionComponent()
        {
            this.GetComponent<UiIntractable>().curAC = this;
        }
        public override void Click()
        {
            if (targetField.text == "")
            {
                Error.SetActive(true);
            }
            else
            {
                // Clear the passed prewive game
                if (ScenePrimer.curGamePrimer != null)
                {
                    if (ScenePrimer.curGamePrimer.primerParrentObj != null)
                    {
                        GameObject.Destroy(ScenePrimer.curGamePrimer.primerParrentObj);
                        ScenePrimer.curGamePrimer = null;
                    }
                    ScenePrimer.curGamePrimer = null;
                }
                UiManager.DestroyAllFocus();
                MapDataManager.MapDataCreateEmptyMap(targetField.text);
                if (ScenePrimer.curEditorPrimer != null)
                {
                    if (!ScenePrimer.curEditorPrimer.editorCreated)
                    {
                        ScenePrimer.curEditorPrimer.PrimerInitialize();
                    }
                    else
                    {
                        ScenePrimer.curPrimerComponent.PrimerCreateCamera();
                    }
                }
                else { ScenePrimer.curSceneprimer.PrimerStartEditor(); }

            }
        }
    }
}