/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;
using System.IO;

namespace ConstruiSystem
{
    public class BTexturePanel : BerryWindow
    {

        BItemField newField;
        ItemPress selectedItem;
        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            targetWindowRef = windowRef;
            // A realy bad way to overide.
            sizeX = 200;
            sizeY = Screen.height - 64;

            // Create the window panel.
            windowPanel.uiSize = new Vector2(sizeX, sizeY);
            windowPanel.uiAnchorMode = UiManager.UiAnchorsMode.MiddelLeftStretchVertical;
            windowPanel.uiPosition = new Vector2(Screen.width / 2 * -1 + sizeX - 68, -32);
            windowPanel.uiRayCast = true;
            GameObject panel = UiManager.CreateBackgroundObj(windowRef, windowPanel);
            panel.transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            // Window content...
            UiButtonObject editorFileMenu = new UiButtonObject();
            editorFileMenu.uiObjName = "editorFileMenu";
            editorFileMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorFileMenu.uiPosition = new Vector2(100, -24);
            editorFileMenu.uiSize = new Vector2(185, 32);
            editorFileMenu.uiStaticObj = true;
            editorFileMenu.uiButtonBackgroundObject.uiColor = new Color32(155, 155, 155, 100);

            editorFileMenu.uiButtonText = new UiTextObject();
            editorFileMenu.uiButtonText.uiSize = new Vector2(185, 32);
            editorFileMenu.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorFileMenu.uiButtonText.uiText = "Import Texture";
            editorFileMenu.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);

            editorFileMenu.uiButtonIcon = null;

            /*
            editorFileMenu.uiButtonIcon = new UiBackgroundObject();
            editorFileMenu.uiButtonIcon.uiTextureRef = ScenePrimer.curEditorPrimer.editorIcons[18];
            editorFileMenu.uiButtonIcon.uiColor = new Color(0, 0, 0, 0.5f);
            editorFileMenu.uiButtonIcon.uiSize = new Vector2(17, 17);
            editorFileMenu.uiButtonIcon.uiPosition = new Vector2(78, 0);
             */

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(185, 32);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 70);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            UiManager.CreateButton(panel, editorFileMenu).gameObject.AddComponent<DropDownButton>().ACClick = CreateTextureImporter;

            UiItemFeild uiFileBrowser = new UiItemFeild();
            uiFileBrowser.uiPosition = new Vector2(100, -24);
            uiFileBrowser.uiSize = new Vector2(200, -50);
            uiFileBrowser.uiColor = new Color32(239, 239, 239, 255);
            uiFileBrowser.uiAnchorMode = UiManager.UiAnchorsMode.MiddelLeftStretchVertical;
            newField = UiManager.CreateItemsFeild(panel, uiFileBrowser);

            GenerateMapView();

            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(-230, 0), new Vector2(0, 0), false, false, 1800f);
        }
        public void GenerateMapView()
        {
            if (XCPManager.currentXCP.tileTextures != null)
            {
                if (ScenePrimer.curEditorPrimer.curTileId == -1)
                {
                    ScenePrimer.curEditorPrimer.curTileId = 0;
                }

                if (newField != null)
                {
                    if (XCPManager.currentXCP != null)
                    {
                        newField.hNr = 3;
                        newField.useNumberShortCut = true;
                        if (XCPManager.currentXCP.xpcMaps != null)
                        {

                            newField.items = new FieldItem[XCPManager.currentXCP.tileTextures.Length];
                            for (int i = 0; i < newField.items.Length; i++)
                            {
                                if (ScenePrimer.curEditorPrimer.curTileId == i)
                                {
                                    newField.items[i].startSelect = true;
                                }
                                newField.items[i].index = i;
                                newField.items[i].useIcon = true;
                                newField.items[i].selectable = true;
                                newField.items[i].itemIcon = XCPManager.PngToSprite(XCPManager.currentXCP.tileTextures[i]);
                                newField.items[i].itemPressMethod = ItemHandler;
                                //newField.items[i].itemDisplayText = "item";
                            }
                            newField.GenerateViewItems();
                        }
                    }
                }
            }
        }

        public void ItemHandler(string path, bool selectedMode, ItemPress handler, bool headLess, int index)
        {
            if (headLess)
            {
                ScenePrimer.curEditorPrimer.curTileId = newField.items[index].index;
                ScenePrimer.curEditorPrimer.curSpriteMode = false;
                WindowTerminate();
                return;
            }
            if (selectedItem != handler)
            {
                if (selectedItem != null)
                {
                    selectedItem.selected = false;
                    selectedItem.curLocAC.selectIntractable(false);
                    selectedItem = handler;
                    selectedItem.selected = true;
                    selectedItem.curLocAC.selectIntractable(true);
                    ScenePrimer.curEditorPrimer.curTileId = selectedItem.itemTarget.index;
                    ScenePrimer.curEditorPrimer.curSpriteMode = false;
                }
                else
                {
                    selectedItem = handler;
                    selectedItem.selected = true;
                    selectedItem.curLocAC.selectIntractable(true);
                    ScenePrimer.curEditorPrimer.curTileId = selectedItem.itemTarget.index;
                    ScenePrimer.curEditorPrimer.curSpriteMode = false;
                }
            }
            else
            {
                selectedItem.selected = true;
                selectedItem.curLocAC.selectIntractable(true);
            }
        }

        public void CreateTextureImporter()
        {
            BImporter textureImporter = new BImporter();
            textureImporter.isSubWindow = true;
            textureImporter.singelFileMode = true;
            textureImporter.fileTypeTarget = ".png";
            textureImporter.ImportCallBack = Import;
            WindowManager.CreateWindow(0, 0, textureImporter, true, false, true);
        }

        public void Click()
        {
            UiDropDown dropDown = new UiDropDown();
            dropDown.uiPosition = new Vector2(132.502f, -104);
            dropDown.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            dropDown.uiSize = new Vector2(185, 185);
            dropDown.uiObjName = "DropDown";
            dropDown.dropDownOptions = new DropDownOption[2];
            dropDown.dropDownOptions[0].Name = " Import Tile";
            dropDown.dropDownOptions[0].itemPressMethod = CreateTextureImporter;
            //dropDown.dropDownOptions[0].skipInstanceRef = true;
            dropDown.dropDownOptions[1].Name = "[ WIP ] Import TileSet";
            //dropDown.dropDownOptions[1].targetNewWindow = new BImporter();
            dropDown.dropDownOptions[1].skipInstanceRef = true;
            /*
            dropDown.dropDownOptions[2].Name = " Import Sprite";
            dropDown.dropDownOptions[2].targetNewWindow = new BImporter();
            dropDown.dropDownOptions[2].skipInstanceRef = true;
             */
            UiManager.CreateDropDown(targetWindowRef, dropDown);
        }

        public void Import(string[] imports)
        {
            int passedLength = 0;
            if (imports != null)
            {
                if (XCPManager.currentXCP.tileTextures == null)
                {
                    XCPManager.currentXCP.tileTextures = new Png[imports.Length];
                }
                else
                {
                    passedLength = XCPManager.currentXCP.tileTextures.Length;
                    Array.Resize(ref XCPManager.currentXCP.tileTextures, XCPManager.currentXCP.tileTextures.Length + imports.Length);
                }

                Texture2D tex = new Texture2D(0, 0);
                for (int i = 0; i < imports.Length; i++)
                {
                    tex.LoadImage(File.ReadAllBytes(imports[0]));
                    XCPManager.currentXCP.tileTextures[passedLength + i] = XCPManager.TexToPng(tex);
                }
            }
            GenerateMapView();
        }

        public override void WindowTerminate()
        {
            if (targetWindowRef != null)
            {
                targetWindowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 0), new Vector2(-230, 0), false, false, 1800f, false, WindowDestroy);
            }
            else { WindowDestroy(); }
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Layer Settings";
        }
    }
}