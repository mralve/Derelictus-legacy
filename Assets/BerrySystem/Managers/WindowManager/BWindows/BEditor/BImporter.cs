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
    public class BImporter : BerryWindow
    {
        InputField feildObj;
        BItemField newField;
        public string fileTypeTarget = ".png";
        public bool singelFileMode;
        public bool noFiles;
        public bool filePathExportMode;
        public string buttonName = "Import";

        public delegate void BImporterCallBack(string[] imports);
        public BImporterCallBack ImportCallBack;

        string[] importFiles;
        int[] iFEmpty;
        ItemPress selectedItem;
        GameObject saveButtonObj;

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

            UiItemFeild uiFileBrowser = new UiItemFeild();
            uiFileBrowser.uiPosition = new Vector2(0, -13);
            uiFileBrowser.uiSize = new Vector2(470, 256);
            uiFileBrowser.uiColor = new Color32(239, 239, 239, 255);
            newField = UiManager.CreateItemsFeild(windowRef, uiFileBrowser);

            
            // Window content...
            UiTextInputField uiMapName = new UiTextInputField();
            uiMapName.uiPosition = new Vector2(0, 136);
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
            uiMapName.fieldName.uiText = "Textrue filepath";
            feildObj = UiManager.CreateTextInputField(windowRef, uiMapName);
            feildObj.onEndEdit.AddListener(delegate { GoTo(); });

            GenerateFeild(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

            UiButtonObject saveButton = new UiButtonObject();
            saveButton.uiPosition = new Vector2(210, -165);
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
            saveButton.uiButtonText.uiText = buttonName;
            saveButtonObj = UiManager.CreateButton(windowRef, saveButton).gameObject;
            saveButtonObj.AddComponent<DropDownButton>().ACClick = Import;

            DropDownOption btnCan = new DropDownOption();
            btnCan.targetNewWindow = new BImporter();
            btnCan.destroy = true;

            saveButton.uiButtonText.uiText = "Cancel";
            saveButton.uiPosition = new Vector2(150, -165);
            UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = WindowTerminate;
            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }

        public void GenerateFeild(string path)
        {
            if (Directory.Exists(path))
            {
                feildObj.text = path;
                string[] strings = Directory.GetFiles(feildObj.text, "*" + fileTypeTarget);
                string[] folders = Directory.GetDirectories(feildObj.text);

                if (noFiles)
                {
                    newField.items = new FieldItem[folders.Length + 1];
                }
                else
                {
                    newField.items = new FieldItem[strings.Length + folders.Length + 1];
                }

                if (Directory.GetParent(path) == null)
                {
                    newField.items[0].hidden = true;
                }
                else
                {
                    newField.items[0].itemIcon = ScenePrimer.curEditorPrimer.editorIcons[29];
                    newField.items[0].itemDisplayText = "Up";
                    newField.items[0].filePath = Directory.GetParent(path).FullName;
                    newField.items[0].itemPressMethod = GoInsideFolder;
                }

                for (int i = 0; i < folders.Length; i++)
                {
                    newField.items[i + 1].itemIcon = ScenePrimer.curEditorPrimer.editorIcons[3];
                    newField.items[i + 1].itemDisplayText = Path.GetFileNameWithoutExtension(folders[i]);
                    newField.items[i + 1].filePath = folders[i];
                    newField.items[i + 1].itemPressMethod = GoInsideFolder;
                    newField.items[i + 1].useIcon = true;
                }
                if (!noFiles)
                {
                    for (int i = 0; i < strings.Length; i++)
                    {
                        Texture2D tex = new Texture2D(1, 1);
                        newField.items[folders.Length + 1 + i].useIcon = true;
                        tex.LoadImage(File.ReadAllBytes(strings[i]));
                        tex.filterMode = FilterMode.Point;
                        if (fileTypeTarget == ".png" || fileTypeTarget == ".jpg")
                        {
                            newField.items[folders.Length + 1 + i].itemIcon = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                        }
                        else
                        {
                            if (fileTypeTarget == ".xcp")
                            {
                                newField.items[folders.Length + 1 + i].itemIcon = Resources.Load<Sprite>("BerrySystem/Icons/xcp 1");
                            }
                            else
                            {
                                newField.items[folders.Length + 1 + i].itemIcon = ScenePrimer.curEditorPrimer.editorIcons[13];
                            }
                        }
                        newField.items[folders.Length + 1 + i].itemDisplayText = Path.GetFileNameWithoutExtension(strings[i]);
                        newField.items[folders.Length + 1 + i].filePath = strings[i];
                        newField.items[folders.Length + 1 + i].itemPressMethod = ItemHandler;
                        newField.items[folders.Length + 1 + i].selectable = true;
                    }
                }
                else
                {

                }
                newField.GenerateViewItems();
            }
            else
            {
            }
        }

        void Import()
        {
            if (filePathExportMode)
            {
                importFiles = new string[1];
                importFiles[0] = feildObj.text;
            }
            if (ImportCallBack != null)
            {
                ImportCallBack(importFiles);
                WindowTerminate();
            }
        }

        public void GoTo()
        {
            GenerateFeild(feildObj.text);
        }
        public void GoInsideFolder(string path, bool selectedMode, ItemPress handler, bool headLess, int index)
        {
            //Debug.Log(filepath);
            GenerateFeild(path);
        }

        public void ItemHandler(string path, bool selectedMode, ItemPress handler, bool headLess, int index)
        {
            if (selectedItem != handler)
            {
                if (selectedItem != null)
                {
                    selectedItem.selected = false;
                    selectedItem.curLocAC.selectIntractable(false);
                    selectedItem = handler;
                    selectedItem.selected = true;
                    selectedItem.curLocAC.selectIntractable(true);
                    AddFile(path, selectedMode, handler);
                    saveButtonObj.gameObject.SetActive(true);
                }
                else
                {
                    selectedItem = handler;
                    selectedItem.selected = true;
                    AddFile(path, selectedMode, handler);
                    saveButtonObj.gameObject.SetActive(true);
                }
            }
            else
            {
                selectedItem.selected = false;
                selectedItem.curLocAC.selectIntractable(false);
                selectedItem = null;
                AddFile(path, false, handler);
                saveButtonObj.gameObject.SetActive(false);
            }
        }

        public void AddFile(string filepath, bool mode, ItemPress handler)
        {
            if (singelFileMode)
            {
                if (importFiles == null)
                {
                    importFiles = new string[1];
                }
                if (mode)
                {
                    importFiles[0] = filepath;
                    return;
                }
                else
                {
                    importFiles[0] = "";
                    return;
                }

            }
            else
            {

                if (importFiles == null)
                {
                    importFiles = new string[0];
                }
            }

            for (int i = 0; i < importFiles.Length; i++)
            {
                if (filepath == importFiles[i])
                {
                    if (mode)
                    {
                        return;
                    }
                    else
                    {
                        importFiles[i] = "";
                        if (iFEmpty == null)
                        {
                            iFEmpty = new int[1];
                            iFEmpty[0] = i;
                        }
                    }
                }
            }

        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "File browser";
        }
    }
}