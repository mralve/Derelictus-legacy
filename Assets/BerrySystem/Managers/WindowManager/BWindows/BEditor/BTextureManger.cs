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
    public class BTextureManager : BerryWindow
    {
        public int TargetSprite = -1;
        Text noMaps;
        BItemField newField;
        UiIntractable openBtn;
        UiIntractable newBtn;
        UiIntractable editBtn;
        UiIntractable editDoneBtn;
        Image spritePrew;
        SpriteAnimation[] animations;
        ItemPress selectedItem;
        Image sortPointObj, ColiderBoxObj;

        Slider Slider0;

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
            windowPanel.uiColor = new Color32(15, 15, 15, 255);
            windowPanel.uiRayCast = true;
            UiManager.CreateBackgroundObj(windowRef, windowPanel).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            UiBackgroundObject xpcInfoPanel = new UiBackgroundObject();
            xpcInfoPanel.uiPosition = new Vector2(142, 13);
            xpcInfoPanel.uiSize = new Vector2(205, 285);
            xpcInfoPanel.uiColor = new Color32(24, 24, 24, 255);
            UiManager.CreateBackgroundObj(windowRef, xpcInfoPanel);

            UiTextObject windowTitle = new UiTextObject();
            windowTitle.uiTextColor = new Color(1, 1, 1, 1);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiSize = new Vector2(180, 19);
            windowTitle.uiTextAlign = TextAnchor.UpperCenter;
            windowTitle.uiPosition = new Vector2(0, 180);
            UiManager.CreateTextObj(windowRef, windowTitle);

            UiButtonObject saveButton = new UiButtonObject();
            saveButton.uiPosition = new Vector2(210, -165);
            saveButton.uiSize = new Vector2(70, 34);
            saveButton.uiButtonBackgroundObject.uiRayCast = true;
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;

            saveButton.uiButtonBackgroundObject.uiColor = new Color32(25, 25, 25, 255);
            saveButton.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            saveButton.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            saveButton.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            saveButton.uiButtonIcon.uiColor = new Color32(24, 24, 24, 255);
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiButtonText = new UiTextObject();
            saveButton.uiButtonText.uiTextColor = new Color32(255, 255, 255, 200);
            saveButton.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            saveButton.uiButtonText.uiText = "Close";

            openBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            openBtn.onMouseClickEvent = WindowTerminate;

            // The old way of doing things... but this needs to be node fast.
            UiButtonObject editorFileMenu = new UiButtonObject();
            editorFileMenu.uiObjName = "editorFileMenu";
            editorFileMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorFileMenu.uiPosition = new Vector2(-168, -165);
            editorFileMenu.uiSize = new Vector2(145, 32);
            editorFileMenu.uiStaticObj = true;
            editorFileMenu.uiButtonBackgroundObject.uiColor = new Color32(155, 155, 155, 100);

            editorFileMenu.uiButtonText = new UiTextObject();
            editorFileMenu.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorFileMenu.uiButtonText.uiText = "Import Texture";
            editorFileMenu.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);
            editorFileMenu.uiButtonText.uiSize = new Vector2(100, 30);

            editorFileMenu.uiButtonIcon = new UiBackgroundObject();
            editorFileMenu.uiButtonIcon.uiTextureRef = ScenePrimer.curEditorPrimer.editorIcons[18];
            editorFileMenu.uiButtonIcon.uiColor = new Color(0, 0, 0, 0.5f);
            editorFileMenu.uiButtonIcon.uiSize = new Vector2(17, 17);
            editorFileMenu.uiButtonIcon.uiPosition = new Vector2(58, 0);

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(145, 32);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 70);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            UiManager.CreateButton(windowRef, editorFileMenu).gameObject.AddComponent<editorTextureDropDown>();

            saveButton.uiPosition = new Vector2(58, -111);
            saveButton.uiSize = new Vector2(34, 34);
            saveButton.uiButtonText.uiText = "";

            UiBackgroundObject transparacyBg = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(284.0696f, 284.0696f);
            transparacyBg.uiPosition = new Vector2(-108.0697f, 13f);
            transparacyBg.uiColor = new Color32(255, 255, 255, 200);
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            spritePrew.material = Resources.Load("BerrySystem/Shaders/bgTranDark", typeof(Material)) as Material;

            UiBackgroundObject sprite = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(260, 260);
            transparacyBg.uiPosition = new Vector2(-109f, 12f);
            transparacyBg.uiColor = new Color32(255, 255, 255, 0);
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            //spritePrew.sprite = img;
            spritePrew.preserveAspect = true;

            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
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

                        newField.items = new FieldItem[XCPManager.currentXCP.xpcMaps.Length];
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


        public void AnimationToggel()
        {

        }


        public void AnimationLoop()
        {

        }

        public void SpriteRemove()
        {

        }

        public void SortUpdate()
        {
            XCPManager.currentXCP.spriteTextures[TargetSprite].sortPoint = Slider0.value;
            sortPointObj.transform.localPosition = new Vector3(-109f, Slider0.value * 260 - 120, 0);
        }

        public void StartSpriteAnimator()
        {

        }

        public void SpriteEditDone()
        {
            editDoneBtn.gameObject.SetActive(false);
            Slider0.gameObject.SetActive(false);
            editBtn.gameObject.SetActive(true);
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Texture Manger";
        }
        public class editorTextureDropDown : ActionComponent
        {
            public override void Click()
            {
                UiDropDown dropDown = new UiDropDown();
                dropDown.uiPosition = new Vector2(-168.1f, -181.01f);
                dropDown.uiAnchorMode = UiManager.UiAnchorsMode.MiddelCenter;
                dropDown.uiSize = new Vector2(145, 185);
                dropDown.uiObjName = "DropDown";
                dropDown.dropDownOptions = new DropDownOption[2];
                dropDown.dropDownOptions[0].Name = "Import sprite sheet";
                //dropDown.dropDownOptions[0].targetNewWindow = new NewLayer();
                dropDown.dropDownOptions[1].Name = "Import texture";
                UiManager.CreateDropDown(this.gameObject, dropDown);
            }
            public ListObj curDropDown;

            public override void AwakeActionComponent()
            {
                this.GetComponent<UiIntractable>().curAC = this;
            }
        }
    }

    
}