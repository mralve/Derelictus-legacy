/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class BLayerPanel : BerryWindow
    {
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

            // Editor File Menu
            UiButtonObject editorFileMenu = new UiButtonObject();
            editorFileMenu.uiObjName = "editorFileMenu";
            editorFileMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorFileMenu.uiPosition = new Vector2(100, -24);
            editorFileMenu.uiSize = new Vector2(185, 32);
            editorFileMenu.uiStaticObj = true;
            editorFileMenu.uiButtonBackgroundObject.uiColor = new Color32(155, 155, 155, 100);

            editorFileMenu.uiButtonText = new UiTextObject();
            editorFileMenu.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorFileMenu.uiButtonText.uiText = "Layers";
            editorFileMenu.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);

            editorFileMenu.uiButtonIcon = new UiBackgroundObject();
            editorFileMenu.uiButtonIcon.uiTextureRef = ScenePrimer.curEditorPrimer.editorIcons[18];
            editorFileMenu.uiButtonIcon.uiColor = new Color(0, 0, 0, 0.5f);
            editorFileMenu.uiButtonIcon.uiSize = new Vector2(17, 17);
            editorFileMenu.uiButtonIcon.uiPosition = new Vector2(78, 0);

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(185, 32);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 70);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            UiManager.CreateButton(panel, editorFileMenu).gameObject.AddComponent<editorLayersButton>();

            UiTextInputField uiLayerName = new UiTextInputField();
            uiLayerName.uiPosition = new Vector2(100, -90);
            uiLayerName.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            uiLayerName.uiSize = new Vector2(185, 32);
            uiLayerName.uiButtonBackgroundObject.uiRayCast = true;
            uiLayerName.uiButtonBackgroundObject.uiSize = uiLayerName.uiSize;
            uiLayerName.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiLayerName.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiLayerName.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiLayerName.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiLayerName.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            uiLayerName.uiButtonIcon.uiSize = new Vector2(uiLayerName.uiSize.x - 2, uiLayerName.uiSize.y - 2);
            uiLayerName.uiButtonText = new UiTextObject();
            uiLayerName.uiButtonText.uiText = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerName;
            uiLayerName.fieldName = new UiTextObject();
            uiLayerName.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            uiLayerName.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            uiLayerName.fieldName.uiSize = uiLayerName.uiSize;
            uiLayerName.fieldName.uiPosition += new Vector2(3, 25);
            uiLayerName.fieldName.uiText = "Layer Name";
            InputField inLayerName = UiManager.CreateTextInputField(panel, uiLayerName);
            inLayerName.onEndEdit.AddListener(delegate { SetLayerName(inLayerName.text); });

            UiTextInputField uiLayerBgColor = new UiTextInputField();
            uiLayerBgColor.uiPosition = new Vector2(100, -189);
            uiLayerBgColor.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            uiLayerBgColor.uiSize = new Vector2(185, 98);
            uiLayerBgColor.uiButtonBackgroundObject.uiRayCast = true;
            uiLayerBgColor.uiButtonBackgroundObject.uiSize = uiLayerBgColor.uiSize;
            uiLayerBgColor.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiLayerBgColor.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiLayerBgColor.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiLayerBgColor.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiLayerBgColor.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            uiLayerBgColor.uiButtonIcon.uiSize = new Vector2(uiLayerBgColor.uiSize.x - 2, uiLayerBgColor.uiSize.y - 2);
            uiLayerBgColor.fieldName = new UiTextObject();
            uiLayerBgColor.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            uiLayerBgColor.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            uiLayerBgColor.fieldName.uiSize = uiLayerBgColor.uiSize;
            uiLayerBgColor.fieldName.uiPosition += new Vector2(3, 25);
            uiLayerBgColor.fieldName.uiText = "Background Color";
            UiManager.CreateColorInputField(panel, uiLayerBgColor, MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor), new RunnerLayerBGColor());

            UiTextInputField uiLayerFgColor = new UiTextInputField();
            uiLayerFgColor.uiPosition = new Vector2(100, -321.8f);
            uiLayerFgColor.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            uiLayerFgColor.uiSize = new Vector2(185, 98);
            uiLayerFgColor.uiButtonBackgroundObject.uiRayCast = true;
            uiLayerFgColor.uiButtonBackgroundObject.uiSize = uiLayerFgColor.uiSize;
            uiLayerFgColor.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiLayerFgColor.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiLayerFgColor.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiLayerFgColor.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiLayerFgColor.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            uiLayerFgColor.uiButtonIcon.uiSize = new Vector2(uiLayerFgColor.uiSize.x - 2, uiLayerFgColor.uiSize.y - 2);
            uiLayerFgColor.fieldName = new UiTextObject();
            uiLayerFgColor.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            uiLayerFgColor.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            uiLayerFgColor.fieldName.uiSize = uiLayerFgColor.uiSize;
            uiLayerFgColor.fieldName.uiPosition += new Vector2(3, 25);
            uiLayerFgColor.fieldName.uiText = "Forground Color";
            UiManager.CreateColorInputField(panel, uiLayerFgColor, MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor), new RunnerLayerFGColor());

            UiButtonObject uiLayerPlayerSpawn = new UiButtonObject();
            uiLayerPlayerSpawn.uiPosition = new Vector2(100, -398.27f);
            uiLayerPlayerSpawn.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            uiLayerPlayerSpawn.uiSize = new Vector2(185, 32);
            uiLayerPlayerSpawn.uiButtonBackgroundObject.uiRayCast = true;
            uiLayerPlayerSpawn.uiButtonBackgroundObject.uiSize = uiLayerPlayerSpawn.uiSize;
            uiLayerPlayerSpawn.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            uiLayerPlayerSpawn.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            uiLayerPlayerSpawn.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            uiLayerPlayerSpawn.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            uiLayerPlayerSpawn.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            uiLayerPlayerSpawn.uiButtonIcon.uiSize = new Vector2(uiLayerPlayerSpawn.uiSize.x - 2, uiLayerPlayerSpawn.uiSize.y - 2);
            uiLayerPlayerSpawn.uiButtonText = new UiTextObject();
            uiLayerPlayerSpawn.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            uiLayerPlayerSpawn.uiButtonText.uiText = "Set player spawn point";
            uiLayerPlayerSpawn.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);
            uiLayerPlayerSpawn.uiButtonText.uiSize = new Vector2(185, 32);
            EditorToolIndicator setSpawn = UiManager.CreateButton(panel, uiLayerPlayerSpawn).gameObject.AddComponent<EditorToolIndicator>();
            EditorToolPosition tool = new EditorToolPosition();
            tool.toolActiveSetterIcon = Resources.Load<Sprite>("BerrySystem/Textures/Topdown/alphaSpawn");
            setSpawn.targetTool = tool;

            uiLayerPlayerSpawn.uiPosition = new Vector2(100, -434.1f);
            uiLayerPlayerSpawn.uiButtonText.uiText = "Main Layer : " + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].mainLayer;
            if (XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].mainLayer)
            {
                UiManager.CreateButton(panel, uiLayerPlayerSpawn);
            }
            else
            {
                UiIntractable setToMain = UiManager.CreateButton(panel, uiLayerPlayerSpawn).GetComponent<UiIntractable>();
                setToMain.onMouseClickEvent = SetLayerMain;
            }

            windowRef.AddComponent<QuickUiAnimator>().PlayPosAnim(new Vector2(-230, 0), new Vector2(0, 0), false, false, 1800f);
        }

        public void SetLayerName(string newName)
        {
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerName = newName;
        }

        public void SetLayerMain()
        {
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataMainLayer].mainLayer = false;
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].mainLayer = true;
            MapDataManager.mapDataMainLayer = MapDataManager.mapDataCurrentLayer;
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

    public class editorLayersButton : ActionComponent
    {
        public override void Click()
        {
            UiDropDown dropDown = new UiDropDown();
            dropDown.uiPosition = new Vector2(132.5f, -104);
            dropDown.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            dropDown.uiSize = new Vector2(185, 185);
            dropDown.uiObjName = "DropDown";
            dropDown.dropDownOptions = new DropDownOption[XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length + 1];
            dropDown.dropDownOptions[0].Name = " + Create New Layer";
            dropDown.dropDownOptions[0].targetNewWindow = new NewLayer();
            string add = "";
            for (int i = 0; i < XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers.Length; i++)
            {
                add = "";
                if (XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[i].mainLayer)
                {
                    add = "[ Main ] ";
                }
                dropDown.dropDownOptions[i + 1].Name = add + XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[i].layerName;
                dropDown.dropDownOptions[i + 1].freeIndex = i;
                dropDown.dropDownOptions[i + 1].hack = true;
                dropDown.dropDownOptions[i + 1].targetNewWindow = new LayerSwitch();
            }
            UiManager.CreateDropDown(this.gameObject, dropDown);
        }
        public ListObj curDropDown;

        public override void AwakeActionComponent()
        {
            this.GetComponent<UiIntractable>().curAC = this;
        }
    }

    public class RunnerLayerBGColor : OptionRunner
    {
        public override void PassColor(Color32 passColor)
        {
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor = MapDataConverter.Color32ToCol(passColor);

            CameraManager.UpdateCurrentCamera();
        }
    }

    public class RunnerLayerFGColor : OptionRunner
    {
        public override void PassColor(Color32 passColor)
        {
            XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor = MapDataConverter.Color32ToCol(passColor);
            CameraManager.UpdateCurrentCamera();
        }
    }
}