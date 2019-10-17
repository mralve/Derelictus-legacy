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
    public class BEntityEditor : BerryWindow
    {
        public bool creationMode;
        BEnt newEntity;

        public int targetEntID = -1;

        UiIntractable openBtn;
        UiIntractable editBtn;
        Image spritePrew;
        Sprite img;

        Text entType;

        public delegate void CallBack(BEnt newEnt);
        public CallBack CreateNew;

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
            windowTitle.uiSize = new Vector2(180, 19);
            windowTitle.uiText = WindowGrabName();
            windowTitle.uiTextAlign = TextAnchor.UpperCenter;
            windowTitle.uiPosition = new Vector2(0, 180);
            UiManager.CreateTextObj(windowRef, windowTitle);

            if (creationMode)
            {
                newEntity = new BEnt();
                newEntity.entName = "new ent";
                newEntity.entType = 1;
                newEntity.entHealth = 100;
                newEntity.entSprites = XCPManager.SpritesToPngs(Resources.LoadAll<Sprite>("BerrySystem/Textures/Topdown/newEnt"));
            }
            else
            {
                if (targetEntID == -1)
                {
                    Debug.LogError("Error! taget ent is -1! Set a valid ID");
                }
            }

            UiTextInputField uiMapName = new UiTextInputField();
            uiMapName.uiPosition = new Vector2(143.7f, 115);
            uiMapName.uiSize = new Vector2(200, 34);
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
            uiMapName.fieldName.uiText = "Entity Name";
            uiMapName.uiButtonText = new UiTextObject();

            if (creationMode)
            {
                uiMapName.uiButtonText.uiText = newEntity.entName;
            }
            else
            {
                uiMapName.uiButtonText.uiText = XCPManager.currentXCP.entities[targetEntID].entName;
            }

            InputField entProperty = UiManager.CreateTextInputField(windowRef, uiMapName);
            entProperty.onEndEdit.AddListener(delegate { EntitySetName(entProperty.text); });

            if (creationMode)
            {
                uiMapName.uiButtonText.uiText = newEntity.entHealth.ToString();
            }
            else
            {
                uiMapName.uiButtonText.uiText = XCPManager.currentXCP.entities[targetEntID].entHealth.ToString();
            }

            uiMapName.fieldName.uiText = "Entity Health";
            uiMapName.uiPosition = new Vector2(143.7f, 3.25f);
            entProperty = UiManager.CreateTextInputField(windowRef, uiMapName);
            entProperty.contentType = InputField.ContentType.DecimalNumber;
            entProperty.onEndEdit.AddListener(delegate { EntitySetName(entProperty.text); });

            UiButtonObject editorFileMenu = new UiButtonObject();
            editorFileMenu.uiObjName = "editorFileMenu";
            editorFileMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorFileMenu.uiPosition = new Vector2(143.7f, 70f);
            editorFileMenu.uiSize = new Vector2(200, 34);
            editorFileMenu.uiStaticObj = true;
            editorFileMenu.uiButtonBackgroundObject.uiColor = new Color32(155, 155, 155, 100);

            editorFileMenu.uiButtonText = null;

            editorFileMenu.uiButtonIcon = new UiBackgroundObject();
            editorFileMenu.uiButtonIcon.uiTextureRef = ScenePrimer.curEditorPrimer.editorIcons[18];
            editorFileMenu.uiButtonIcon.uiColor = new Color(0, 0, 0, 0.5f);
            editorFileMenu.uiButtonIcon.uiSize = new Vector2(17, 17);
            editorFileMenu.uiButtonIcon.uiPosition = new Vector2(85, 0);

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(200, 34);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 70);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            UiManager.CreateButton(windowRef, editorFileMenu).gameObject.AddComponent<DropDownButton>().ACClick = ItemSelect;
            
            if(creationMode)
            {
                windowTitle.uiText = "Set Entity type";
            }else{
                windowTitle.uiText = newEntity.entType.ToString();
            }
            windowTitle.uiTextAlign = TextAnchor.UpperLeft;
            windowTitle.uiPosition = new Vector2(139.3f, 67.94f);
            entType = UiManager.CreateTextObj(windowRef, windowTitle).GetComponent<Text>();

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

            if (creationMode)
            {
                saveButton.uiButtonText.uiText = "Create";
                openBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
                openBtn.onMouseClickEvent = EntityCreate;
            }
            else
            {
                saveButton.uiButtonText.uiText = "Close";
                openBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
                openBtn.onMouseClickEvent = WindowTerminate;
            }

            saveButton.uiButtonText.uiText = "Animation editor";
            saveButton.uiSize = new Vector2(120, 34);
            saveButton.uiButtonText.uiSize = saveButton.uiSize;
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiPosition = new Vector2(-186, -165);
            editBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            editBtn.onMouseClickEvent = StartSpriteAnimator;

            saveButton.uiButtonText.uiText = "Texture manager";
            saveButton.uiSize = new Vector2(120, 34);
            saveButton.uiButtonText.uiSize = saveButton.uiSize;
            saveButton.uiButtonBackgroundObject.uiSize = saveButton.uiSize;
            saveButton.uiButtonIcon.uiSize = new Vector2(saveButton.uiSize.x - 2, saveButton.uiSize.y - 2);
            saveButton.uiPosition = new Vector2(-30, -165);
            editBtn = UiManager.CreateButton(windowRef, saveButton).GetComponent<UiIntractable>();
            editBtn.onMouseClickEvent = StartTextureManager;

            UiBackgroundObject transparacyBg = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(269, 269);
            transparacyBg.uiPosition = new Vector2(-109f, 12f);
            transparacyBg.uiColor = new Color32(255, 255, 255, 255);
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            spritePrew.material = Resources.Load("BerrySystem/Shaders/bgTran", typeof(Material)) as Material;

            UiBackgroundObject sprite = new UiBackgroundObject();
            transparacyBg.uiSize = new Vector2(260, 260);
            transparacyBg.uiPosition = new Vector2(-109f, 12f);
            if (img == null)
            {
                transparacyBg.uiColor = new Color32(255, 255, 255, 0);
            }
            else
            {
                transparacyBg.uiColor = new Color32(255, 255, 255, 255);
            }
            spritePrew = UiManager.CreateBackgroundObj(windowRef, transparacyBg).GetComponent<Image>();
            spritePrew.sprite = img;
            spritePrew.preserveAspect = true;

            //
            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(0, 20), new Vector2(0, 0), false, false, 120f);
        }

        public void ItemSelect()
        {
            UiDropDown dropDown = new UiDropDown();
            dropDown.uiPosition = new Vector2(143.7f, 53f);
            dropDown.uiAnchorMode = UiManager.UiAnchorsMode.MiddelCenter;
            dropDown.uiSize = new Vector2(200, 34);
            dropDown.uiObjName = "DropDown";
            dropDown.dropDownOptions = new DropDownOption[3];
            dropDown.dropDownOptions[0].Name = "Player";
            dropDown.dropDownOptions[0].itemPressMethod = ItemSetPlayer;
            dropDown.dropDownOptions[1].Name = "Enemy is this AI?";
            dropDown.dropDownOptions[1].itemPressMethod = ItemSetBadAI;
            dropDown.dropDownOptions[2].Name = "A* AI WIP";
            UiManager.CreateDropDown(targetWindowRef, dropDown);
        }

        public void ItemSetPlayer()
        {
            entType.text = "Player";
            EntitySetType(0);
        }

        public void ItemSetBadAI()
        {
            entType.text = "Is this AI? (GENERAL ENEMY)";
            EntitySetType(1);
        }

        public void EntitySetName(string newEntName)
        {
            if (creationMode)
            {
                newEntity.entName = newEntName;
            }
            else
            {
                XCPManager.currentXCP.entities[targetEntID].entName = newEntName;
            }
        }

        public void EntitySetType(int type)
        {
            if(creationMode)
            {
                newEntity.entType = type;
            }else{
                XCPManager.currentXCP.entities[targetEntID].entType = type;
            }
        }

        public void EntityCreate()
        {
            CreateNew(newEntity);
            WindowTerminate();
        }

        public void StartSpriteAnimator()
        {
            WindowManager.CreateWindow(0, 0, new BAnimationEditor(), true);
        }

        public void StartTextureManager()
        {
            WindowManager.CreateWindow(0, 0, new BTextureManager(), true);
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Entity Editor";
        }
    }
}