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
    public class BEntityPanel : BerryWindow
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
            editorFileMenu.uiButtonText.uiText = "Create Entity";
            editorFileMenu.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);

            editorFileMenu.uiButtonIcon = null;

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(185, 32);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 70);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            UiManager.CreateButton(panel, editorFileMenu).gameObject.AddComponent<DropDownButton>().ACClick = CreateEntityCreator;

            UiItemFeild uiFileBrowser = new UiItemFeild();
            uiFileBrowser.uiPosition = new Vector2(100, -24);
            uiFileBrowser.uiSize = new Vector2(200, -50);
            uiFileBrowser.uiColor = new Color32(239, 239, 239, 255);
            uiFileBrowser.uiAnchorMode = UiManager.UiAnchorsMode.MiddelLeftStretchVertical;
            newField = UiManager.CreateItemsFeild(panel, uiFileBrowser);
            newField.useNumberShortCut = true;

            GenerateMapView();

            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(-230, 0), new Vector2(0, 0), false, false, 1800f);
        }
        public void GenerateMapView()
        {
            if (XCPManager.currentXCP.entities != null)
            {
                if (newField != null)
                {
                    if (XCPManager.currentXCP != null)
                    {
                        newField.hNr = 3;
                        if (XCPManager.currentXCP.xpcMaps != null)
                        {

                            newField.items = new FieldItem[XCPManager.currentXCP.entities.Length];
                            for (int i = 0; i < newField.items.Length; i++)
                            {
                                newField.items[i].index = i;
                                if(XCPManager.currentXCP.entities[i].entSprites.Length != 0)
                                {
                                    newField.items[i].useIcon = true;
                                    newField.items[i].itemIcon = XCPManager.PngToSprite(XCPManager.currentXCP.entities[i].entSprites[0]);
                                }
                                newField.items[i].selectable = true;
                                newField.items[i].itemPressMethod = ItemHandler;
                                newField.items[i].itemRightPressMethod = CreateSpriteEditor;
                            }
                            newField.GenerateViewItems();
                        }
                    }
                }
            }
        }
        /*
        public void EntityPanel()
        {
                UiDropDown dropDown = new UiDropDown();
                dropDown.uiPosition = new Vector2(132.502f, -104);
                dropDown.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
                dropDown.uiSize = new Vector2(185, 185);
                dropDown.uiObjName = "DropDown";
                dropDown.dropDownOptions = new DropDownOption[1];
                dropDown.dropDownOptions[0].Name = " Create NPC ";
                dropDown.dropDownOptions[0].itemPressMethod = CreateEntityCreator;
                UiManager.CreateDropDown(targetWindowRef, dropDown);
        }
         */


        public void ItemHandler(string path, bool selectedMode, ItemPress handler, bool headLess, int index)
        {
            if (headLess)
            {
                GlobalToolManager.globalToolManager.SetTool(new EditorToolTilePen());
                GlobalToolManager.SendToolUpdate();
                ScenePrimer.curEditorPrimer.curSpriteId = newField.items[index].index;
                ScenePrimer.curEditorPrimer.curSpriteMode = true;
                ScenePrimer.curEditorPrimer.UpdateLayer(3);
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
                    GlobalToolManager.globalToolManager.SetTool(new EditorToolTilePen());
                    GlobalToolManager.SendToolUpdate();
                    ScenePrimer.curEditorPrimer.curSpriteId = selectedItem.itemTarget.index;
                    ScenePrimer.curEditorPrimer.curSpriteMode = true;
                    ScenePrimer.curEditorPrimer.UpdateLayer(3);
                    WindowTerminate();
                }
                else
                {
                    selectedItem = handler;
                    selectedItem.selected = true;
                    selectedItem.curLocAC.selectIntractable(true);
                    GlobalToolManager.globalToolManager.SetTool(new EditorToolTilePen());
                    GlobalToolManager.SendToolUpdate();
                    ScenePrimer.curEditorPrimer.curSpriteId = selectedItem.itemTarget.index;
                    ScenePrimer.curEditorPrimer.curSpriteMode = true;
                    ScenePrimer.curEditorPrimer.UpdateLayer(3);
                    WindowTerminate();
                }
            }
            else
            {
                selectedItem.selected = true;
                selectedItem.curLocAC.selectIntractable(true);
                ScenePrimer.curEditorPrimer.curSpriteId = selectedItem.itemTarget.index;
                ScenePrimer.curEditorPrimer.UpdateLayer(3);
                GlobalToolManager.SendToolUpdate();
            }
        }

        public void CreateSpriteEditor(string path, bool selectedMode, ItemPress handler, bool headLess, int index)
        {
            BEntityEditor SEditor = new BEntityEditor();
            SEditor.isSubWindow = true;
            SEditor.creationMode = false;
            SEditor.targetEntID = index;
            WindowManager.CreateWindow(0, 0, SEditor, true);
        }

        public void CreateEntityCreator()
        {
            BEntityEditor entityCreator = new BEntityEditor();
            entityCreator.CreateNew = EntityCreate;
            entityCreator.creationMode = true;
            WindowManager.CreateWindow(0, 0, entityCreator, true, false, true);
        }

        public void EntityCreate( BEnt newEnt )
        {
            if(XCPManager.currentXCP.entities == null)
            {
                XCPManager.currentXCP.entities = new BEnt[1];
                XCPManager.currentXCP.entities[0] = newEnt;
            }else{
                Array.Resize(ref XCPManager.currentXCP.entities, XCPManager.currentXCP.entities.Length + 1);
                XCPManager.currentXCP.entities[XCPManager.currentXCP.entities.Length] = newEnt;
            }
            GenerateMapView();
        }

        public void Import(string[] imports)
        {
            int passedLength = 0;
            if (imports != null)
            {
                if (XCPManager.currentXCP.spriteTextures == null)
                {
                    XCPManager.currentXCP.spriteTextures = new Png[imports.Length];
                }
                else
                {
                    passedLength = XCPManager.currentXCP.spriteTextures.Length;
                    Array.Resize(ref XCPManager.currentXCP.spriteTextures, XCPManager.currentXCP.spriteTextures.Length + imports.Length);
                }

                Texture2D tex = new Texture2D(1, 1);
                for (int i = 0; i < imports.Length; i++)
                {
                    tex.LoadImage(File.ReadAllBytes(imports[0]));
                    XCPManager.currentXCP.spriteTextures[passedLength + i] = XCPManager.TexToPng(tex);
                }
            }
            GenerateMapView();
        }

        public override void WindowTerminate()
        {
            targetWindowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 0), new Vector2(-230, 0), false, false, 1800f, false, WindowDestroy);
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Layer Settings";
        }
    }
}