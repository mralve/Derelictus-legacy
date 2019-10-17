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
    public class BXCPManager : BerryWindow
    {
        InputField feildObj;
        Text curXPCName;
        Text curXPCAuthor;
        Text curXPCDescript;
        Text curXPCVer;
        Text noMaps;
        BItemField newField;
        UiIntractable openBtn;
        UiIntractable newBtn;
        Image iconPrew;
        ItemPress selectedItem;

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
            saveButton.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            saveButton.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            saveButton.uiButtonText.uiText = "Open";

            openBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            openBtn.gameObject.SetActive(false);
            openBtn.onMouseClickEvent = OpenMap;

            saveButton.uiButtonText.uiText = "New Map";
            saveButton.uiSize = new Vector2(100, 34);
            saveButton.uiPosition = new Vector2(130, -165);
            newBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            newBtn.onMouseClickEvent = NewMap;
            if (XCPManager.currentXCP == null)
            {
                newBtn.gameObject.SetActive(false);
            }

            UiBackgroundObject xpcInfoPanel = new UiBackgroundObject();
            xpcInfoPanel.uiPosition = new Vector2(-108.07f, -15.12f);
            xpcInfoPanel.uiSize = new Vector2(284.07f, 340.45f);
            xpcInfoPanel.uiColor = new Color(0.9f, 0.9f, 0.9f, 1);
            UiManager.CreateBackgroundObj(windowRef, xpcInfoPanel);

            saveButton.uiButtonText.uiText = "Load";
            saveButton.uiPosition = new Vector2(-8, -162);
            UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = XCPLoad;

            saveButton.uiButtonText.uiText = "Create";
            saveButton.uiPosition = new Vector2(-85, -162);
            DropDownOption xcpCreate = new DropDownOption(); // Move over to the new way of doing things....
            xcpCreate.targetNewWindow = new BXCPCreate();

            xcpCreate.destroy = true;
            UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().optionData = xcpCreate;

            if (ScenePrimer.curEditorPrimer.editorCreated)
            {
                saveButton.uiButtonText.uiText = "Save";
                saveButton.uiPosition = new Vector2(-205, -162);
                xcpCreate.destroy = true;
                UiManager.CreateButton(windowRef, saveButton).gameObject.AddComponent<DropDownButton>().ACClick = SaveXCP;
            }

            UiBackgroundObject icon = new UiBackgroundObject();
            icon.uiSize = new Vector2(92, 92);
            icon.uiPosition = new Vector2(-197.8f, 101.7f);
            icon.uiColor = new Color32(243, 243, 243, 255);
            iconPrew = UiManager.CreateBackgroundObj(windowRef, icon).GetComponent<Image>();

            UiTextObject xcpTitle = new UiTextObject();
            if (XCPManager.currentXCP == null)
            {
                xcpTitle.uiText = "NO XCP LOADED !";
            }
            else
            {
                xcpTitle.uiText = XCPManager.currentXCP.xcpGameName;
            }
            xcpTitle.uiTextSize = 15;
            xcpTitle.uiTextColor = new Color(0, 0, 0, 1);
            xcpTitle.uiPosition = new Vector2(-59.55f, 127.11f);
            xcpTitle.uiSize = new Vector2(170.72f, 34);
            curXPCName = UiManager.CreateTextObj(windowRef, xcpTitle).GetComponent<Text>();
            xcpTitle.uiText = "";
            xcpTitle.uiTextSize = 13;
            xcpTitle.uiPosition = new Vector2(-59.55f, 104.4f);
            curXPCAuthor = UiManager.CreateTextObj(windowRef, xcpTitle).GetComponent<Text>();
            xcpTitle.uiPosition = new Vector2(-157.5f, -126.8f);
            curXPCVer = UiManager.CreateTextObj(windowRef, xcpTitle).GetComponent<Text>();
            xcpTitle.uiSize = new Vector2(269, 34);
            xcpTitle.uiPosition = new Vector2(-110, 28);
            curXPCDescript = UiManager.CreateTextObj(windowRef, xcpTitle).GetComponent<Text>();

            if (XCPManager.currentXCP != null)
            {
                curXPCName.text = XCPManager.currentXCP.xcpGameName;
                curXPCAuthor.text = XCPManager.currentXCP.xcpAuthor;
                curXPCDescript.text = XCPManager.currentXCP.xcpDescription;
                curXPCVer.text = "V." + XCPManager.currentXCP.xcpFormVersion.ToString();
                iconPrew.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.xcpIcon);
            }

            UiItemFeild uiFileBrowser = new UiItemFeild();
            uiFileBrowser.uiPosition = new Vector2(146.78f, 9.19f);
            uiFileBrowser.uiSize = new Vector2(209.7f, 291.82f);
            uiFileBrowser.uiColor = new Color32(239, 239, 239, 255);
            newField = UiManager.CreateItemsFeild(windowRef, uiFileBrowser);

            xcpTitle.uiPosition = new Vector2(200f, 127.11f);
            xcpTitle.uiSize = new Vector2(170.72f, 34);
            xcpTitle.uiText = "No maps!";
            noMaps = UiManager.CreateTextObj(windowRef, xcpTitle).GetComponent<Text>();


            GenerateMapView();
            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }

        public void NewMap()
        {
            WindowManager.CreateWindow(300, 400, new BMapNew());
        }

        public void GenerateMapView()
        {
            if (newField != null)
            {
                if (XCPManager.currentXCP != null)
                {
                    newField.hNr = 3;
                    if (XCPManager.currentXCP.xpcMaps != null)
                    {
                        noMaps.gameObject.SetActive(false);
                        newField.items = null;
                        newField.items = new FieldItem[XCPManager.currentXCP.xpcMaps.Length];
                        if (newField.items.Length == 0)
                        {
                            GameObject.Destroy(newField.container);

                        }
                        else
                        {
                            for (int i = 0; i < newField.items.Length; i++)
                            {
                                newField.items[i].index = i;
                                newField.items[i].useIcon = true;
                                newField.items[i].selectable = true;
                                newField.items[i].itemPressMethod = ItemHandler;
                                if (i == XCPManager.currentXCP.mainMapIndex)
                                {
                                    newField.items[i].itemIcon = Resources.Load<Sprite>("BerrySystem/Icons/MMain");
                                }
                                else
                                {
                                    newField.items[i].itemIcon = Resources.Load<Sprite>("BerrySystem/Icons/MAP");
                                }
                                newField.items[i].itemDisplayText = XCPManager.currentXCP.xpcMaps[i].map;
                            }
                            newField.GenerateViewItems();
                        }
                    }
                    else
                    {
                        GameObject.Destroy(newField.container);
                        noMaps.gameObject.SetActive(true);
                    }
                }
            }
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
                    openBtn.gameObject.SetActive(true);
                }
                else
                {
                    selectedItem = handler;
                    selectedItem.selected = true;
                    openBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                selectedItem.selected = false;
                selectedItem.curLocAC.selectIntractable(false);
                selectedItem = null;
                openBtn.gameObject.SetActive(false);
            }
        }

        public void OpenMap()
        {
            UiManager.DestroyAllFocus();
            MapDataManager.MapDataOpenXCPMap(selectedItem.itemTarget.index);
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

        public void XCPLoad()
        {
            BImporter importer = new BImporter();
            importer.fileTypeTarget = ".xcp";
            importer.singelFileMode = true;
            importer.ImportCallBack = LoadXCP;
            WindowManager.CreateWindow(0, 0, importer, true, true);
        }

        public void LoadXCP(string[] file)
        {
            XCPManager.XCPImportFromFile(file[0]);
            if (XCPManager.currentXCP != null)
            {
                curXPCName.text = XCPManager.currentXCP.xcpGameName;
                curXPCAuthor.text = XCPManager.currentXCP.xcpAuthor;
                curXPCDescript.text = XCPManager.currentXCP.xcpDescription;
                curXPCVer.text = "V." + XCPManager.currentXCP.xcpFormVersion.ToString();
                iconPrew.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.xcpIcon);
                newBtn.gameObject.SetActive(true);
                newField.items = null;
                GenerateMapView();
            }
        }

        public void SaveXCP()
        {
            if (XCPManager.currentXCP != null)
            {
                XCPManager.XCPExportToFile(XCPManager.currentXCP, XCPManager.importPath);
                WindowTerminate();
            }
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "XCP Manager";
        }
    }
}