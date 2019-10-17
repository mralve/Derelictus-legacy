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
    public class BItemEditor : BerryWindow
    {
        public int TargetSprite = -1;
        Text noMaps;
        BItemField newField;
        UiIntractable openBtn;
        UiIntractable newBtn;
        UiIntractable editBtn;
        UiIntractable editDoneBtn;
        Image spritePrew;
        Sprite img;
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
            windowPanel.uiRayCast = true;
            UiManager.CreateBackgroundObj(windowRef, windowPanel).transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, false, 9f);

            UiBackgroundObject xpcInfoPanel = new UiBackgroundObject();
            xpcInfoPanel.uiPosition = new Vector2(-108.07f, -15.12f);
            xpcInfoPanel.uiSize = new Vector2(284.07f, 340.45f);
            xpcInfoPanel.uiColor = new Color(0.9f, 0.9f, 0.9f, 1);
            UiManager.CreateBackgroundObj(windowRef, xpcInfoPanel);

            UiTextObject windowTitle = new UiTextObject();
            windowTitle.uiTextColor = new Color(0, 0, 0, 255);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiSize = new Vector2(180, 19);
            windowTitle.uiTextAlign = TextAnchor.UpperCenter;
            windowTitle.uiPosition = new Vector2(0, 180);
            UiManager.CreateTextObj(windowRef, windowTitle);

            windowTitle.uiTextAlign = TextAnchor.MiddleLeft;
            windowTitle.uiText = "Sprite ID : " + TargetSprite;
            windowTitle.uiPosition = new Vector2(131, 140);
            UiManager.CreateTextObj(windowRef, windowTitle);

            if (TargetSprite == -1)
            {
                windowTitle.uiText = "Pixsel scale : ?";
            }
            else
            {
                img = XCPManager.PngToSprite(XCPManager.currentXCP.itemTextures[TargetSprite]);
                windowTitle.uiText = "Pixsel scale : " + XCPManager.currentXCP.itemTextures[TargetSprite].pixScale;
            }

            windowTitle.uiSize = new Vector2(236, 19);
            windowTitle.uiPosition = new Vector2(158.79f, 120);
            UiManager.CreateTextObj(windowRef, windowTitle);
            if (TargetSprite == -1)
            {
                windowTitle.uiText = "Resolution : " + " H : ?" + "px  W : ?" + " px";
            }
            else
            {
                windowTitle.uiText = "Resolution : " + " H : " + img.texture.height + "px  W : " + img.texture.width + " px";
            }
            windowTitle.uiSize = new Vector2(236, 19);
            windowTitle.uiPosition = new Vector2(158.79f, 100);
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
            saveButton.uiButtonText.uiText = "Close";

            openBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            openBtn.onMouseClickEvent = WindowTerminate;

            saveButton.uiButtonText.uiText = "Remove";
            saveButton.uiSize = new Vector2(100, 34);
            saveButton.uiPosition = new Vector2(130, -165);
            newBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            newBtn.onMouseClickEvent = SpriteRemove;

            saveButton.uiButtonText.uiText = "Remove";
            saveButton.uiButtonBackgroundObject.uiColor = new Color32(255, 0, 0, 255);
            saveButton.uiButtonBackgroundObject.normalColor = new Color32(255, 0, 0, 255);
            saveButton.uiButtonBackgroundObject.hoverColor = new Color32(255, 0, 0, 100);
            saveButton.uiButtonIcon.uiColor = new Color32(255, 255, 255, 50);
            saveButton.uiSize = new Vector2(100, 34);
            saveButton.uiPosition = new Vector2(130, -125);
            newBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            newBtn.onMouseClickEvent = SpriteRemove;
            newBtn.gameObject.SetActive(false);

            saveButton.uiButtonText.uiText = "Import Texture";
            saveButton.uiSize = new Vector2(120, 34);
            saveButton.uiButtonText.uiSize = saveButton.uiSize;
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiPosition = new Vector2(-186, -165);
            editBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            editBtn.onMouseClickEvent = SpriteEdit;

            UiBackgroundObject transparacyBg = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(269, 269);
            transparacyBg.uiPosition = new Vector2(-109f, 12f);
            transparacyBg.uiColor = new Color32(255, 255, 255, 255);
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            spritePrew.material = Resources.Load("BerrySystem/Shaders/tran", typeof(Material)) as Material;

            UiBackgroundObject sprite = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(260, 260);
            transparacyBg.uiPosition = new Vector2(-109f, 12f);
            transparacyBg.uiColor = new Color32(255, 255, 255, 255);
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            spritePrew.sprite = img;
            spritePrew.preserveAspect = true;

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


        public void SpriteRemove()
        {

        }

        public void SortUpdate()
        {
            XCPManager.currentXCP.spriteTextures[TargetSprite].sortPoint = Slider0.value;
            sortPointObj.transform.localPosition = new Vector3(-109f, Slider0.value * 260 - 120, 0);
        }

        public void SpriteEdit()
        {
            editDoneBtn.gameObject.SetActive(true);
            Slider0.gameObject.SetActive(true);
            editBtn.gameObject.SetActive(false);
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
            return windowShortName = "Item Editor";
        }
    }
}