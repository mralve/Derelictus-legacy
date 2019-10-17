/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class BXCPCreate : BerryWindow
    {
        InputField fieldObj, fieldObj1, fieldObj2;
        BItemField newField;
        Image iconPrew;
        XCP newXCPInstance;


        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            // A realy bad way to overide.
            sizeX = 520;
            sizeY = 400;

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

            UiTextObject windowTitle = new UiTextObject();
            windowTitle.uiTextColor = new Color(0, 0, 0, 255);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiSize = new Vector2(100, 19);
            windowTitle.uiTextAlign = TextAnchor.UpperCenter;
            windowTitle.uiPosition = new Vector2(0, 180);
            UiManager.CreateTextObj(windowRef, windowTitle);

            newXCPInstance = new XCP();

            UiButtonObject saveButton = new UiButtonObject();
            saveButton.uiPosition = new Vector2(210, -165);
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
            saveButton.uiButtonText.uiText = "Create";

            UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = CreateXCP;


            if (!ScenePrimer.curEditorPrimer.editorCreated)
            {
                saveButton.uiButtonText.uiText = "Cancel";
                saveButton.uiPosition = new Vector2(130, -165);
                UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = WindowTerminate;
            }
            
            saveButton.uiPosition = new Vector2(0, -165);
            saveButton.uiSize = new Vector2(90, 34);
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiButtonText.uiText = "Import icon";
            UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = IconImport;

            UiTextInputField uiMapName = new UiTextInputField();
            uiMapName.uiPosition = new Vector2(0, 116);
            uiMapName.uiSize = new Vector2(470, 34);
            uiMapName.uiButtonBackgroundObject.uiRayCast = true;
            uiMapName.uiButtonBackgroundObject.uiSize = uiMapName.uiSize;
            uiMapName.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiMapName.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiMapName.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiMapName.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiMapName.uiButtonIcon = null;
            uiMapName.fieldName = new UiTextObject();
            uiMapName.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            uiMapName.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            uiMapName.fieldName.uiSize = uiMapName.uiSize;
            uiMapName.fieldName.uiPosition += new Vector2(3, 25);
            uiMapName.fieldName.uiText = "XCP Name / Game Name";
            fieldObj = UiManager.CreateTextInputField(windowRef, uiMapName);
            fieldObj.onEndEdit.AddListener(delegate { NameUpdate(); });

            uiMapName.fieldName.uiText = "Author";
            uiMapName.uiPosition = new Vector2(0, 40);
            fieldObj1 = UiManager.CreateTextInputField(windowRef, uiMapName);
            fieldObj1.onEndEdit.AddListener(delegate { AuthorUpdate(); });

            uiMapName.fieldName.uiText = "Description";
            uiMapName.uiPosition = new Vector2(0, -40);
            fieldObj2 = UiManager.CreateTextInputField(windowRef, uiMapName);
            fieldObj2.onEndEdit.AddListener(delegate { DescriptionUpdate(); });

            UiBackgroundObject icon = new UiBackgroundObject();
            icon.uiSize = new Vector2(92, 92);
            icon.uiPosition = new Vector2(-120, -120);
            icon.uiColor = new Color32(230, 230, 230, 255);
            iconPrew = UiManager.CreateBackgroundObj(windowRef, icon).GetComponent<Image>();

            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }

        void NameUpdate()
        {
            newXCPInstance.xcpGameName = fieldObj.text;
        }
        void AuthorUpdate()
        {
            newXCPInstance.xcpAuthor = fieldObj1.text;
        }
        void DescriptionUpdate()
        {
            newXCPInstance.xcpDescription = fieldObj2.text;
        }
        void IconImport()
        {
            BImporter importer = new BImporter();
            importer.fileTypeTarget = ".png";
            importer.singelFileMode = true;
            importer.ImportCallBack = IconUpdate;
            WindowManager.CreateWindow(300, 400, importer, true);
        }
        void IconUpdate(string[] iconPath)
        {
            if (iconPath[0] == "")
            {
                Debug.LogError("Heyyyy No image here");
                return;
            }
            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(File.ReadAllBytes(iconPath[0]));
            tex.filterMode = FilterMode.Point;
            newXCPInstance.xcpIcon = XCPManager.TexToPng(tex);
            iconPrew.sprite = XCPManager.TexToSprite(tex);
        }

        void CreateXCP()
        {
            BImporter importer = new BImporter();
            importer.buttonName = "Save";
            importer.singelFileMode = true;
            importer.noFiles = true;
            importer.filePathExportMode = true;
            importer.ImportCallBack = ExportXcp;
            WindowManager.CreateWindow(300, 400, importer, true);
        }

        void ExportXcp(string[] savePath)
        {
            XCPManager.XCPExportToFile(newXCPInstance, savePath[0] + "\\" + newXCPInstance.xcpGameName + ".xcp");
            WindowTerminate();
        }


        public void CreateMapCall()
        {
            WindowManager.CreateWindow(300, 400, new BMapNew());
        }


        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "XCP Create";
        }
    }
}