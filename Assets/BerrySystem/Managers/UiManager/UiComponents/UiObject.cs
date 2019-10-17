/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    // TODO: Migrate from class types to structs for more efficient memory layout and performance.
    public class UiObject
    {
        public string uiObjName = "obj";
        public bool uiRayCast = false;
        public bool uiStaticObj = false;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
        public Vector2 uiSize = new Vector2(64, 64);
        //public Vector2 uiOffset = new Vector2();
        public Vector2 uiPosition = new Vector2();
        public UiManager.UiAnchorsMode uiAnchorMode = UiManager.UiAnchorsMode.MiddelCenter;
        public Vector2 uiAnchorOffsetLR = new Vector2(0, 0);
        public Vector2 uiAnchorOffsetTB = new Vector2(0, 0);
        public virtual void CreateUiType(GameObject parrent)
        {
            UiManager.CreateUiObject(parrent, this);
        }
    }

    public class UiTextObject : UiObject
    {
        public int uiTextSize = 12;
        public Color uiTextColor = new Color(255, 255, 255, 255);
        public string uiText = "Text Obj";
        public TextAnchor uiTextAlign = TextAnchor.MiddleLeft;
    }

    public class UiBackgroundObject : UiObject
    {
        public Sprite uiTextureRef = null;
        public Color uiColor = new Color32(255, 255, 255, 255);
    }

    public class UiPanelStack : UiObject
    {
        public Sprite uiTextureRef = null;
        public Color uiColor = new Color32(255, 255, 255, 255);
    }

    public class UiInteractiveBackgroundObject : UiBackgroundObject
    {
        public Color normalColor = new Color32(0, 140, 205, 0);
        public Color hoverColor = new Color32(0, 140, 205, 120);
        public Color pressedColor = new Color32(0, 160, 225, 255);
        public Color draggingColor = new Color(1, 0.4f, 1, 0.3f);
        public float speed = 5;
        public bool dragCursor;
        public Texture2D cursorImg = Resources.Load<Texture2D>("BerrySystem/UI/cursors/drag_up");
    }

    public class UiButtonObject : UiObject
    {
        public UiTextObject uiButtonText;
        public UiBackgroundObject uiButtonIcon = new UiBackgroundObject();
        public UiInteractiveBackgroundObject uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
    }

    public class UiSliderObject : UiObject
    {
        public UiTextObject uiButtonText;
        public UiBackgroundObject uiButtonIcon = new UiBackgroundObject();
        public UiInteractiveBackgroundObject uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
    }

    public class UiCheckBoxButton : UiButtonObject
    {
        public UiInteractiveBackgroundObject uiCheckMarkObject = new UiInteractiveBackgroundObject();
    }

    public class UiItemFeild : UiBackgroundObject
    {

    }

    public class UiTextInputField : UiButtonObject
    {
        public string hintText = "Text here";
        public UiTextObject fieldName = new UiTextObject();
    }

    public class UiDropDownField : UiObject
    {
        public string hintText = "Text here";
        public UiButtonObject fieldButton = new UiButtonObject();
        public UiTextObject fieldName = new UiTextObject();
        public override void CreateUiType(GameObject parrent)
        {
            UiManager.CreateDropDown(parrent, this);
        }
    }

    public class UiColorField : UiButtonObject
    {
        public string hintText = "Text here";
        public UiTextObject fieldName = new UiTextObject();
    }
    public class UiVectorInputField : UiButtonObject
    {
        public V2 targetPosition;
        public string hintText = "Text here";
        public UiTextObject fieldName = new UiTextObject();
    }

    public class UiScrollView : UiObject
    {
        public UiBackgroundObject uiScrollViewBackgroundObject;
    }

    public class UiDropDown : UiBackgroundObject
    {
        public DropDownOption[] dropDownOptions;
    }

    public struct DropDownOption
    {
        public string Name;
        public bool destroy, hack, skipInstanceRef;
        public int freeIndex;
        public BerryWindow targetNewWindow;
        public ActionComponent targetActionComponent;

        public delegate void ItemPressDel();
        public ItemPressDel itemPressMethod;
    }

    public class UiOptionDropDown : UiBackgroundObject
    {
        public string OptionDropDownName;
        public Vector2 textPos;
        public string currentOptionName;
        public int currentOption;
        public UiTextInputField UiOptionField;
        public UiDropDown UiDropDown;

    }

    public class UiFileBrowser : UiBackgroundObject
    {
        public string filePathDisplayText = "File Path";
    }

}