/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ConstruiSystem
{
    public static class UiManager
    {
        // CanvasHandeling
        //public static GameObject[] canvasContainers;
        public static CanvasObj[] canvasObjArray;

        public static GameObject[] focusSeparates;

        // EventSystem
        public static GameObject eventSystemRef;

        // Ui FocusInstance
        public static GameObject curFocusObj;
        public static GameObject focusInstanceObj;

        // Current cursor texture
        public static BerryCursor curCursor;
        public static BerryCursor holdCursor;
        public static bool isHold;

        // RectTransform Anchors
        public enum UiAnchorsMode
        {
            FillStretch,
            TopLeft,
            TopCenter,
            TopRight,
            TopStretchHorizontal,
            TopStretchVertical,
            MiddelLeft,
            MiddelLeftStretchVertical,
            MiddelRightStretchVertical,
            MiddelCenter,
            MiddelRight,
            MiddelStretchHorizontal,
            MiddelStretchVertical,
            BottomLeft,
            BottomCenter,
            BottomRight,
            BottomStretchHorizontal,
            BottomStretchVertical
        }
        //Standard Ui

        /*
		 * Canvas handel functions
		 */
        /*
		public static GameObject CreateCanvasContainerObj(string containerName, GameObject parrentObj, bool registerCanvasToContainerArray)
		{
			GameObject newContainer = new GameObject(containerName);
			if(parrentObj != null)
			{
				newContainer.transform.SetParent(parrentObj.transform);
			}
			if(registerCanvasToContainerArray)
			{

			}
			return newContainer;
		}
		 */

        public static GameObject CreateCanvas(String canvasName = "NewCanvas", GameObject parrentContainerObj = null, bool useRectMask = true, bool statOn = true, int sortOrder = 0, bool useRayCaster = false, bool isStatic = false)
        {
            GameObject newCanvas = new GameObject(canvasName);
            newCanvas.SetActive(statOn);
            CanvasObjComponentMananger curCOBJCM = newCanvas.AddComponent<CanvasObjComponentMananger>();
            if (useRectMask)
            {
                newCanvas.AddComponent<RectMask2D>();
            }
            if (parrentContainerObj != null)
            {
                if (parrentContainerObj.GetComponent<CanvasObjComponentMananger>() != null)
                {
                    curCOBJCM.isCanvasChild = true;
                    curCOBJCM.ParrentCanvasObj = parrentContainerObj.GetComponent<CanvasObjComponentMananger>().ParrentCanvasObj;
                }
                else
                {
                    curCOBJCM.isCanvasChild = false;
                    curCOBJCM.ParrentCanvasObj = newCanvas;
                }
                newCanvas.transform.SetParent(parrentContainerObj.transform);
            }
            else
            {
                if (newCanvas.GetComponent<CanvasObjComponentMananger>() != null)
                {
                    curCOBJCM.isCanvasChild = false;
                    curCOBJCM.ParrentCanvasObj = newCanvas;
                }
            }

            CanvasObj newCanvasObj = new CanvasObj();
            newCanvasObj.curCanvas = newCanvas.AddComponent<Canvas>();
            newCanvasObj.curCanvas.sortingOrder = sortOrder;
            newCanvasObj.curCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            AddCanvasObjReference(newCanvasObj);
            newCanvas.transform.position = Vector4.zero;
            newCanvas.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            newCanvas.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            newCanvas.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            newCanvas.isStatic = isStatic;

            if (useRayCaster)
            {
                if (eventSystemRef == null)
                {
                    CreateEventSystem(parrentContainerObj, "event");
                }
                newCanvas.AddComponent<GraphicRaycaster>();
            }
            return newCanvas;
        }

        public static GameObject CreateUiFocusObj(bool createSeparateFocusInstance = false, bool disabelBackBackground = false, bool fadeInAnimation = false, BerryWindow targetWindow = null, bool killInput = false, bool skipSystemRefrencerce = false)
        {
            if (createSeparateFocusInstance)
            {
                GameObject separateFocus = CreateCanvas("separateFocusCanvas", null, false, true, 10000, true, false);
                if (!disabelBackBackground)
                {
                    /*
					ScenePrimer.curPrimerComponent.disableInput = true;
					GlobalToolManager.globalToolManager.disableInput = true;
					 */
                    GlobalToolManager.DisabelTools();
                    UiInteractiveBackgroundObject focusBG = new UiInteractiveBackgroundObject();
                    focusBG.uiSize = new Vector2(0, 0);
                    focusBG.normalColor = new Color(0, 0, 0, 0.35f);
                    focusBG.hoverColor = new Color(0, 0, 0, 0.35f);
                    focusBG.pressedColor = new Color(0, 0, 0, 0.35f);
                    UiButtonObject focusBackgroundButton = new UiButtonObject();
                    focusBackgroundButton.uiSize = new Vector2(0, 0);
                    focusBackgroundButton.uiButtonBackgroundObject = focusBG;
                    focusBackgroundButton.uiButtonBackgroundObject.uiRayCast = true;
                    focusBackgroundButton.uiButtonBackgroundObject.uiColor = new Color(0, 0, 0, 0);
                    focusBackgroundButton.uiButtonIcon = null;
                    focusBackgroundButton.uiButtonBackgroundObject.uiAnchorMode = UiAnchorsMode.FillStretch;
                    focusBackgroundButton.uiAnchorMode = UiAnchorsMode.FillStretch;
                    GameObject defocusBg = CreateButton(separateFocus, focusBackgroundButton).gameObject;
                    defocusBg.AddComponent<DefocusBtn>().focusInstanceObjRef = focusInstanceObj;
                    defocusBg.GetComponent<DefocusBtn>().windowReference = targetWindow;
                    defocusBg.GetComponent<DefocusBtn>().killInput = killInput;
                }
                if (!skipSystemRefrencerce)
                {
                    addFocusSeparate(separateFocus);
                }
                return separateFocus;
            }
            else
            {
                if (curFocusObj != null)
                {
                    /*
					if(fadeInAnimation)
					{
						curFocusObj.transform.parent.gameObject.AddComponent<QuickUiAnimator>().PlayFadeAnim(0, 1, false, true, 0.01f);
					}else{
						curFocusObj.SetActive(true);
					}
					 */
                }
                else
                {
                    GameObject curFocus = CreateCanvas("FocusCanvas", null, false, true, 10000, true, false);
                    focusInstanceObj = curFocus;
                    if (!disabelBackBackground)
                    {
                        /*
						ScenePrimer.curPrimerComponent.disableInput = true;
						GlobalToolManager.globalToolManager.disableInput = true;
						 */
                        GlobalToolManager.DisabelTools();
                        UiInteractiveBackgroundObject focusBG = new UiInteractiveBackgroundObject();
                        focusBG.uiSize = new Vector2(0, 0);
                        focusBG.normalColor = new Color(0, 0, 0, 0.35f);
                        focusBG.hoverColor = new Color(0, 0, 0, 0.35f);
                        focusBG.pressedColor = new Color(0, 0, 0, 0.35f);
                        UiButtonObject focusBackgroundButton = new UiButtonObject();
                        focusBackgroundButton.uiSize = new Vector2(0, 0);
                        focusBackgroundButton.uiButtonBackgroundObject = focusBG;
                        focusBackgroundButton.uiButtonBackgroundObject.uiColor = new Color(0, 0, 0, 0);
                        focusBackgroundButton.uiButtonBackgroundObject.uiRayCast = true;
                        focusBackgroundButton.uiButtonIcon = null;
                        focusBackgroundButton.uiButtonBackgroundObject.uiAnchorMode = UiAnchorsMode.FillStretch;
                        focusBackgroundButton.uiAnchorMode = UiAnchorsMode.FillStretch;
                        GameObject defocusBg = CreateButton(curFocus, focusBackgroundButton).gameObject;
                        defocusBg.AddComponent<DefocusBtn>().focusInstanceObjRef = focusInstanceObj;
                        defocusBg.GetComponent<DefocusBtn>().windowReference = targetWindow;
                        defocusBg.GetComponent<DefocusBtn>().killInput = killInput;
                    }
                    curFocusObj = curFocus;
                    if (fadeInAnimation)
                    {

                    }
                    else
                    {
                        curFocusObj.SetActive(true);
                    }
                    return curFocus;
                }
                return focusInstanceObj;
            }
        }

        public static void addFocusSeparate(GameObject focusRef)
        {
            if (focusSeparates == null)
            {
                focusSeparates = new GameObject[1];
                focusSeparates[0] = focusRef;
            }
            else
            {
                Array.Resize(ref focusSeparates, focusSeparates.Length);
                focusSeparates[focusSeparates.Length - 1] = focusRef;
            }
        }

        public static void DestroyAllFocus()
        {
            // Enable scrolling. 
            if (ScenePrimer.curPrimerComponent.curZoomComp != null)
            {
                ScenePrimer.curPrimerComponent.curZoomComp.zoomingEnabled = true;
            }
            // Re-enable input.
            ScenePrimer.curPrimerComponent.disableInput = true;
            GlobalToolManager.DisableInput(true);
            if (focusSeparates != null)
            {
                for (int i = 0; i < focusSeparates.Length; i++)
                {
                    GameObject.Destroy(focusSeparates[i]);
                }
                focusSeparates = null;
            }
            GameObject.Destroy(curFocusObj);
            GameObject.Destroy(focusInstanceObj);
        }

        public static GameObject CreateEventSystem(GameObject parrentObj = null, string name = "EventSystem")
        {
            GameObject newEventSystemRef = new GameObject(name);
            if (parrentObj != null)
            {
                newEventSystemRef.transform.SetParent(parrentObj.transform);
            }
            newEventSystemRef.AddComponent<EventSystem>();
            newEventSystemRef.AddComponent<StandaloneInputModule>();
            eventSystemRef = newEventSystemRef;
            return newEventSystemRef;
        }

        public static CanvasObj[] AddCanvasObjReference(CanvasObj CanvasRef)
        {
            if (canvasObjArray == null) { canvasObjArray = new CanvasObj[0]; }
            Array.Resize(ref canvasObjArray, canvasObjArray.Length + 1);
            canvasObjArray[canvasObjArray.Length - 1] = CanvasRef;
            return canvasObjArray;
        }

        /*
		public static GameObject[] AddToCanvasContainers(GameObject CanvasRef)
		{
			Array.Resize(ref canvasContainers, canvasContainers.Length + 1);
			return canvasContainers;
		}
		 */

        /*
		 * RectTransform Utils
		 */

        public static Vector4 TargetAnchorVector(UiAnchorsMode targetAnchor)
        {
            switch (targetAnchor)
            {
                case UiAnchorsMode.TopLeft:
                    return new Vector4(0, 1, 0, 1);
                case UiAnchorsMode.TopCenter:
                    return new Vector4(0.5f, 1, 0.5f, 1);
                case UiAnchorsMode.TopRight:
                    return new Vector4(1, 1, 1, 1);
                case UiAnchorsMode.TopStretchHorizontal:
                    return new Vector4(0, 1, 1, 1);
                case UiAnchorsMode.TopStretchVertical:
                    return new Vector4(0, 0, 0, 1);
                case UiAnchorsMode.MiddelLeft:
                    return new Vector4(0, 0, 0.5f, 0.5f);
                case UiAnchorsMode.MiddelLeftStretchVertical:
                    return new Vector4(0, 0, 0, 1);
                case UiAnchorsMode.MiddelRightStretchVertical:
                    return new Vector4(1, 0, 1, 1);
                case UiAnchorsMode.MiddelCenter:
                    return new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
                case UiAnchorsMode.MiddelRight:
                    return new Vector4(1, 0.5f, 1, 0.5f);
                case UiAnchorsMode.MiddelStretchHorizontal:
                    return new Vector4(0, 0.5f, 1, 0.5f);
                case UiAnchorsMode.MiddelStretchVertical:
                    return new Vector4(0.5f, 0, 0.5f, 1);
                case UiAnchorsMode.BottomLeft:
                    return new Vector4(0, 0, 0, 0);
                case UiAnchorsMode.BottomCenter:
                    return new Vector4(0.5f, 0, 0.5f, 0);
                case UiAnchorsMode.BottomRight:
                    return new Vector4(1, 0, 1, 0);
                case UiAnchorsMode.BottomStretchHorizontal:
                    return new Vector4(0, 0, 1, 0);
                case UiAnchorsMode.BottomStretchVertical:
                    return new Vector4(1, 0, 1, 1);
                case UiAnchorsMode.FillStretch:
                    return new Vector4(0, 0, 1, 1);
                default:
                    return new Vector4(0, 0, 1, 1);
            }
        }

        public static void SetUiAnchors(GameObject targetObj, UiAnchorsMode targetAnchorSettings = UiAnchorsMode.FillStretch)
        {
            if (targetObj != null)
            {
                if (targetObj.GetComponent<RectTransform>() == null)
                {
                    targetObj.AddComponent<RectTransform>();
                }
                Vector4 anchorSetting = TargetAnchorVector(targetAnchorSettings);
                targetObj.GetComponent<RectTransform>().anchorMin = new Vector2(anchorSetting.x, anchorSetting.y);
                targetObj.GetComponent<RectTransform>().anchorMax = new Vector2(anchorSetting.z, anchorSetting.w);
            }
        }


        /*
		public static void SetWidthHight(GameObject UiTargetObj, Vector2 Scale)
		{
			if(UiTargetObj != null)
			{
				UiTargetObj.GetComponent<RectTransform>().sizeDelta = Scale;
			}
		}
		 */

        /*
		 * Standard UI Types and Utility's
		 */

        public static GameObject sCreateUiObject(GameObject parrent, BUIObject newUiObject)
        {
            GameObject newObj = new GameObject(newUiObject.displayName);
            if (parrent != null) { newObj.transform.SetParent(parrent.transform); }
            SetUiAnchors(newObj, newUiObject.uiAnchorMode);
            RectTransform curRect = newObj.GetComponent<RectTransform>();
            curRect.anchoredPosition = newUiObject.position;
            curRect.sizeDelta = newUiObject.size;
            curRect.pivot = newUiObject.pivot;
            newObj.isStatic = newUiObject.isStatic;
            return newObj;
        }

        public static GameObject CreateUiObject(GameObject parrent, UiObject newUiObject)
        {
            GameObject newObj = new GameObject(newUiObject.uiObjName);
            if (parrent != null)
            {
                newObj.transform.SetParent(parrent.transform);
            }
            SetUiAnchors(newObj, newUiObject.uiAnchorMode);
            RectTransform curRect = newObj.GetComponent<RectTransform>();
            curRect.anchoredPosition = newUiObject.uiPosition;
            curRect.sizeDelta = newUiObject.uiSize;
            curRect.pivot = newUiObject.pivot;
            newObj.isStatic = newUiObject.uiStaticObj;
            return newObj;
        }

        public static GameObject CreateTextObj(GameObject parrent, UiTextObject newUiTextObject)
        {
            GameObject textObj = CreateUiObject(parrent, newUiTextObject);
            Text curTextComp = textObj.AddComponent<Text>();
            curTextComp.color = newUiTextObject.uiTextColor;
            curTextComp.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
            curTextComp.text = newUiTextObject.uiText;
            curTextComp.fontSize = newUiTextObject.uiTextSize;
            curTextComp.alignment = newUiTextObject.uiTextAlign;
            curTextComp.supportRichText = false;
            if (newUiTextObject.uiTextSize > 15)
            {
                curTextComp.font = Resources.Load<Font>("BerrySystem/Fonts/Montserrat-TextBig");
            }
            else
            {
                curTextComp.font = Resources.Load<Font>("BerrySystem/Fonts/Montserrat-Medium");
            }
            curTextComp.raycastTarget = newUiTextObject.uiRayCast;
            return textObj;
        }

        public static GameObject CreateBackgroundObj(GameObject parrent, UiBackgroundObject newUiBackground)
        {
            GameObject backgroundObj = CreateUiObject(parrent, newUiBackground);
            Image curImageComp = backgroundObj.AddComponent<Image>();
            curImageComp.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
            curImageComp.color = newUiBackground.uiColor;
            curImageComp.raycastTarget = newUiBackground.uiRayCast;
            if (newUiBackground.uiTextureRef != null)
            {
                curImageComp.sprite = newUiBackground.uiTextureRef;
            }
            return backgroundObj;
        }


        // TODO : Replace all get component calls.
        public static UiIntractable CreateButton(GameObject parrent, UiButtonObject newUiButtonObject, bool disableText = false)
        {
            if (newUiButtonObject == null) { newUiButtonObject = new UiButtonObject(); }
            if (newUiButtonObject.uiButtonBackgroundObject == null)
            {
                newUiButtonObject.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            }
            GameObject buttonObj = CreateUiObject(parrent, newUiButtonObject);
            GameObject buttonRef = CreateBackgroundObj(buttonObj, newUiButtonObject.uiButtonBackgroundObject);
            UiIntractable button = buttonObj.AddComponent<UiIntractable>();
            button.targetImage = buttonRef.GetComponent<Image>();
            button.ButtonColors = newUiButtonObject.uiButtonBackgroundObject;
            button.switchColor(0, newUiButtonObject.uiButtonBackgroundObject.normalColor);
            if (newUiButtonObject.uiButtonIcon != null)
            {
                newUiButtonObject.uiButtonBackgroundObject.uiObjName = "Icon";
                buttonObj.GetComponent<UiIntractable>().targetIcon = CreateBackgroundObj(buttonObj, newUiButtonObject.uiButtonIcon);
                buttonObj.GetComponent<UiIntractable>().targetIcon.GetComponent<Image>().preserveAspect = true;
            }
            if (!disableText)
            {
                if (newUiButtonObject.uiButtonText != null)
                {
                    newUiButtonObject.uiButtonText.uiObjName = "Text";
                    CreateTextObj(buttonObj, newUiButtonObject.uiButtonText);
                }
            }
            return button;
        }

        public static Slider CreateSlider(GameObject parrent, UiSliderObject newUiSliderObject, bool disableText = false)
        {
            if (newUiSliderObject.uiButtonBackgroundObject == null)
            {
                newUiSliderObject.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            }
            newUiSliderObject.uiSize = newUiSliderObject.uiButtonBackgroundObject.uiSize;
            GameObject buttonObj = CreateUiObject(parrent, newUiSliderObject);
            Slider curSlider = buttonObj.AddComponent<Slider>();
            curSlider.wholeNumbers = true;
            curSlider.minValue = 0;
            curSlider.maxValue = 255;
            newUiSliderObject.uiButtonBackgroundObject.uiSize = new Vector3(0, 0, 0);
            GameObject buttonRef = CreateBackgroundObj(buttonObj, newUiSliderObject.uiButtonBackgroundObject);
            newUiSliderObject.uiButtonBackgroundObject.uiSize = new Vector3(10, 0, 0);
            newUiSliderObject.uiButtonBackgroundObject.uiColor = new Color(0.4f, 0.4f, 0.4f, 1);
            GameObject knobRef = CreateBackgroundObj(buttonObj, newUiSliderObject.uiButtonBackgroundObject);
            curSlider.fillRect = buttonRef.GetComponent<RectTransform>();
            curSlider.handleRect = knobRef.GetComponent<RectTransform>();
            buttonObj.AddComponent<UiIntractable>().targetImage = buttonRef.GetComponent<Image>();
            buttonObj.GetComponent<UiIntractable>().ButtonColors = newUiSliderObject.uiButtonBackgroundObject;
            buttonObj.GetComponent<UiIntractable>().switchColor(0, newUiSliderObject.uiButtonBackgroundObject.normalColor);
            if (newUiSliderObject.uiButtonIcon != null)
            {
                newUiSliderObject.uiButtonBackgroundObject.uiObjName = "Icon";
                buttonObj.GetComponent<UiIntractable>().targetIcon = CreateBackgroundObj(buttonObj, newUiSliderObject.uiButtonIcon);
            }
            if (!disableText)
            {
                if (newUiSliderObject.uiButtonText != null)
                {
                    newUiSliderObject.uiButtonText.uiObjName = "Text";
                    CreateTextObj(buttonObj, newUiSliderObject.uiButtonText);
                }
            }
            return curSlider;
        }

        public static GameObject CreateCheckBoxButton(GameObject parrent, UiCheckBoxButton newUiCheckBoxButton)
        {
            if (newUiCheckBoxButton.uiButtonBackgroundObject == null)
            {
                newUiCheckBoxButton.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            }
            GameObject buttonObj = CreateUiObject(parrent, newUiCheckBoxButton);
            GameObject buttonRef = CreateBackgroundObj(buttonObj, newUiCheckBoxButton.uiButtonBackgroundObject);
            UiIntractable curUiObj = buttonObj.AddComponent<UiIntractable>();
            curUiObj.targetImage = buttonRef.GetComponent<Image>();
            curUiObj.ButtonColors = newUiCheckBoxButton.uiButtonBackgroundObject;
            curUiObj.switchColor(0, newUiCheckBoxButton.uiButtonBackgroundObject.normalColor);
            curUiObj.isCheckBox = true;
            curUiObj.isCheckBoxChecked = true;
            curUiObj.checkBoxRef = CreateBackgroundObj(buttonObj, newUiCheckBoxButton.uiCheckMarkObject);
            if (newUiCheckBoxButton.uiButtonIcon != null)
            {
                buttonObj.GetComponent<UiIntractable>().targetIcon = CreateBackgroundObj(buttonObj, newUiCheckBoxButton.uiButtonIcon);
            }
            if (newUiCheckBoxButton.uiButtonText != null)
            {
                CreateTextObj(buttonObj, newUiCheckBoxButton.uiButtonText);
            }
            return buttonObj;
        }

        public static InputField CreateTextInputField(GameObject parrent, UiTextInputField textInputObj)
        {
            textInputObj.uiObjName = "textField";
            GameObject inputObj = CreateButton(parrent, textInputObj, true).gameObject;
            GameObject inputContainerObj = new GameObject("inputContainerObj");
            inputContainerObj.transform.SetParent(inputObj.transform);
            InputField textInput = inputObj.AddComponent<InputField>();
            GameObject description = CreateTextObj(inputObj, textInputObj.fieldName);

            if (textInputObj.uiButtonText == null)
            {
                textInputObj.uiButtonText = new UiTextObject();
                textInputObj.uiButtonText.uiText = "";
            }
            textInput.text = textInputObj.uiButtonText.uiText;

            textInputObj.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            textInputObj.uiButtonText.uiSize = new Vector2(textInputObj.uiSize.x - 15, textInputObj.uiSize.y);
            GameObject text = CreateTextObj(inputObj, textInputObj.uiButtonText);

            textInput.selectionColor = new Color32(0, 48, 255, 255);
            textInput.transition = Selectable.Transition.None;
            textInput.textComponent = text.GetComponent<Text>();

            return textInput;
        }

        public static InputField CreateVector2Input(GameObject parrent, UiVectorInputField textInputObj)
        {
            textInputObj.uiObjName = "positionField";
            GameObject inputObj = CreateButton(parrent, textInputObj, true).gameObject;
            GameObject inputContainerObj = new GameObject("positionFieldObj");
            GameObject input2Obj = CreateButton(parrent, textInputObj, true).gameObject;
            GameObject input2ContainerObj = new GameObject("positionFieldObj");
            inputContainerObj.transform.SetParent(inputObj.transform);
            input2ContainerObj.transform.SetParent(inputObj.transform);
            InputField textInput = inputObj.AddComponent<InputField>();
            GameObject description = CreateTextObj(inputObj, textInputObj.fieldName);

            if (textInputObj.uiButtonText == null)
            {
                textInputObj.uiButtonText = new UiTextObject();
                textInputObj.uiButtonText.uiText = "";
            }
            textInput.text = textInputObj.uiButtonText.uiText;

            textInputObj.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            textInputObj.uiButtonText.uiSize = new Vector2(textInputObj.uiSize.x - 15, textInputObj.uiSize.y);
            GameObject text = CreateTextObj(inputObj, textInputObj.uiButtonText);

            textInput.selectionColor = new Color32(0, 48, 255, 255);
            textInput.transition = Selectable.Transition.None;
            textInput.textComponent = text.GetComponent<Text>();

            return textInput;
        }

        public static GameObject CreateDropDown(GameObject parrent, UiDropDownField textInputObj)
        {
            textInputObj.uiObjName = "dropDownField";
            GameObject inputObj = CreateButton(parrent, textInputObj.fieldButton, true).gameObject;
            //GameObject inputContainerObj = new GameObject("inputContainerObj");
            //inputContainerObj.transform.SetParent(inputObj.transform);
            //InputField textInput = inputObj.AddComponent<InputField>();
            GameObject description = CreateTextObj(inputObj, textInputObj.fieldName);

            if (textInputObj.fieldButton.uiButtonText == null) { textInputObj.fieldButton.uiButtonText = new UiTextObject(); }
            textInputObj.fieldButton.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            textInputObj.fieldButton.uiButtonText.uiSize = new Vector2(textInputObj.uiSize.x - 15, textInputObj.uiSize.y);
            GameObject text = CreateTextObj(inputObj, textInputObj.fieldButton.uiButtonText);

            /*
			textInput.selectionColor = new Color32(0, 48, 255, 255);
			textInput.transition = Selectable.Transition.None;
			textInput.textComponent = text.GetComponent<Text>();
			 */

            return null;
        }

        public static GameObject CreateColorInputField(GameObject parrent, UiTextInputField textInputObj, Color32 PresetColor = new Color32(), OptionRunner PassType = null)
        {
            textInputObj.uiObjName = "colorField";
            GameObject inputObj = CreateButton(parrent, textInputObj, true).gameObject;
            GameObject inputContainerObj = new GameObject("colorObj");
            inputContainerObj.transform.SetParent(inputObj.transform);
            GameObject description = CreateTextObj(inputObj, textInputObj.fieldName);
            UiBackgroundObject colorPlane = new UiBackgroundObject();
            colorPlane.uiSize.y = textInputObj.uiSize.y;
            colorPlane.uiSize.x = 32;
            colorPlane.uiColor = new Color(0, 0, 0, 1);
            colorPlane.uiPosition.x = 76;
            GameObject ColorPlane = CreateBackgroundObj(inputObj, colorPlane);

            UiSliderObject slider0 = new UiSliderObject();
            slider0.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            slider0.uiButtonBackgroundObject.uiSize = new Vector2(textInputObj.uiSize.x - 32, 32);
            slider0.uiPosition = new Vector2(-16, 32);
            slider0.uiButtonBackgroundObject.normalColor = new Color(1, 1, 1, 0.3f);
            slider0.uiButtonBackgroundObject.uiRayCast = true;
            slider0.uiButtonIcon = null;
            slider0.uiButtonText = new UiTextObject();
            slider0.uiButtonText.uiText = "   R";
            slider0.uiButtonText.uiSize = new Vector2(textInputObj.uiSize.x - 32, 32);
            slider0.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);

            Slider Slider0 = CreateSlider(inputObj, slider0);
            slider0.uiButtonBackgroundObject.uiSize = new Vector2(textInputObj.uiSize.x - 32, 32);
            slider0.uiPosition = new Vector2(-16, 0);
            slider0.uiButtonText.uiText = "   G";

            Slider Slider1 = CreateSlider(inputObj, slider0);
            slider0.uiButtonBackgroundObject.uiSize = new Vector2(textInputObj.uiSize.x - 32, 32);
            slider0.uiPosition = new Vector2(-16, -32);
            slider0.uiButtonText.uiText = "   B";

            Slider Slider2 = CreateSlider(inputObj, slider0);

            if (textInputObj.uiButtonText == null)
            {
                textInputObj.uiButtonText = new UiTextObject();
                textInputObj.uiButtonText.uiText = "";
            }
            ColorPanel curColorPanel = inputObj.AddComponent<ColorPanel>();
            curColorPanel.sliderR = Slider0;
            curColorPanel.sliderG = Slider1;
            curColorPanel.sliderB = Slider2;
            curColorPanel.prewPlane = ColorPlane.GetComponent<Image>();
            curColorPanel.target = PassType;
            curColorPanel.SetColorNew(PresetColor);

            Slider0.onValueChanged.AddListener(delegate { curColorPanel.UpdateColor(); });
            Slider1.onValueChanged.AddListener(delegate { curColorPanel.UpdateColor(); });
            Slider2.onValueChanged.AddListener(delegate { curColorPanel.UpdateColor(); });

            textInputObj.uiButtonText.uiTextColor = new Color32(0, 0, 0, 255);
            textInputObj.uiButtonText.uiSize = new Vector2(textInputObj.uiSize.x - 15, textInputObj.uiSize.y);
            GameObject text = CreateTextObj(inputObj, textInputObj.uiButtonText);

            return null;
        }

        public static GameObject CreateFileBrowser(GameObject parrent, UiFileBrowser newUiBackground)
        {
            GameObject backgroundObj = CreateUiObject(parrent, newUiBackground);
            Image curImageComp = backgroundObj.AddComponent<Image>();
            curImageComp.material = Resources.Load("BerrySystem/UI/UI_Color", typeof(Material)) as Material;
            curImageComp.color = newUiBackground.uiColor;
            curImageComp.raycastTarget = newUiBackground.uiRayCast;

            UiTextInputField filePath = new UiTextInputField();
            filePath.uiPosition = new Vector2(100, -45);
            filePath.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            filePath.uiSize = new Vector2(185, 32);
            filePath.uiButtonBackgroundObject.uiRayCast = true;
            filePath.uiButtonBackgroundObject.uiSize = filePath.uiSize;
            filePath.uiButtonBackgroundObject.uiColor = new Color32(100, 100, 100, 100);
            filePath.uiButtonBackgroundObject.normalColor = new Color32(170, 170, 170, 100);
            filePath.uiButtonBackgroundObject.hoverColor = new Color32(134, 166, 255, 100);
            filePath.uiButtonBackgroundObject.pressedColor = new Color32(0, 150, 255, 100);
            filePath.uiButtonIcon.uiColor = new Color32(255, 255, 255, 110);
            filePath.uiButtonIcon.uiSize = new Vector2(filePath.uiSize.x - 2, filePath.uiSize.y - 2);
            filePath.uiButtonText = new UiTextObject();
            filePath.uiButtonText.uiText = "Target file path";
            filePath.fieldName = new UiTextObject();
            filePath.fieldName.uiTextColor = new Color32(0, 0, 0, 255);
            filePath.fieldName.uiTextAlign = TextAnchor.UpperLeft;
            filePath.fieldName.uiSize = filePath.uiSize;
            filePath.fieldName.uiPosition += new Vector2(3, 25);
            filePath.fieldName.uiText = newUiBackground.filePathDisplayText;
            CreateTextInputField(backgroundObj, filePath);

            if (newUiBackground.uiTextureRef != null)
            {
                curImageComp.sprite = newUiBackground.uiTextureRef;
            }
            return backgroundObj;
        }

        public static BItemField CreateItemsFeild(GameObject parrent, UiItemFeild newItemFeild)
        {
            GameObject fieldObject = CreateBackgroundObj(parrent, newItemFeild);
            BItemField newField = fieldObject.AddComponent<BItemField>();

            return newField;
        }

        /*
		 * UI Utility's
		 */
        public static GameObject CreateDropDown(GameObject parrent, UiDropDown dropDown)
        {
            if (dropDown.uiSize.x < 128)
            {
                dropDown.uiSize.x = 128;
            }
            if (dropDown.dropDownOptions != null)
            {
                dropDown.uiSize.y = 32 * dropDown.dropDownOptions.Length;
            }
            dropDown.uiObjName = "DropDownObject";
            dropDown.pivot = new Vector2(0.5f, 1);
            parrent = CreateBackgroundObj(CreateUiFocusObj(true, false, true), dropDown);

            dropDown.uiSize = new Vector2(dropDown.uiSize.x, 32);

            UiButtonObject listBtn = new UiButtonObject();
            listBtn.uiButtonIcon = null;
            listBtn.uiButtonBackgroundObject.speed = 1;
            listBtn.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            listBtn.uiButtonBackgroundObject.uiColor = new Color(0, 0, 0, 0);
            listBtn.uiButtonBackgroundObject.normalColor = new Color(0, 0, 0, 0);
            listBtn.uiButtonBackgroundObject.hoverColor = new Color(0, 0, 1, 0.2f);
            listBtn.uiButtonBackgroundObject.uiRayCast = true;
            listBtn.uiButtonBackgroundObject.uiSize = dropDown.uiSize;
            listBtn.uiSize = dropDown.uiSize;
            listBtn.uiButtonText = new UiTextObject();
            listBtn.uiButtonText.uiSize = dropDown.uiSize;
            listBtn.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);
            listBtn.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            listBtn.uiAnchorMode = UiManager.UiAnchorsMode.TopCenter;
            listBtn.pivot = new Vector2(0.5f, 1);

            if (dropDown.dropDownOptions != null)
            {
                for (int i = 0; i < dropDown.dropDownOptions.Length; i++)
                {
                    listBtn.uiButtonText.uiText = dropDown.dropDownOptions[i].Name;
                    if (dropDown.dropDownOptions[i].Name == null) { listBtn.uiButtonText.uiText = dropDown.dropDownOptions[i].targetNewWindow.WindowGrabName(); }
                    listBtn.uiPosition = new Vector2(0, 32 * i * -1);
                    CreateButton(parrent, listBtn).gameObject.AddComponent<DropDownButton>().optionData = dropDown.dropDownOptions[i];
                }
            }
            return parrent;
        }

        public static GameObject CreateDropDownOption(GameObject parrent, UiOptionDropDown newOptionDD)
        {
            UiTextObject curText = new UiTextObject();
            curText.uiText = newOptionDD.OptionDropDownName;
            curText.uiPosition = newOptionDD.textPos;
            curText.uiSize = newOptionDD.uiSize;
            curText.uiAnchorMode = newOptionDD.uiAnchorMode;
            curText.uiTextColor = new Color(0, 0, 0, 1);
            CreateTextObj(parrent, curText);

            UiButtonObject curBtn = new UiButtonObject();
            curBtn.uiPosition = newOptionDD.uiPosition;
            curBtn.uiAnchorMode = newOptionDD.uiAnchorMode;
            curBtn.uiButtonBackgroundObject.uiPosition = newOptionDD.uiPosition;
            curBtn.uiButtonBackgroundObject.uiAnchorMode = newOptionDD.uiAnchorMode;
            curBtn.uiButtonBackgroundObject.uiSize = newOptionDD.uiSize;
            curBtn.uiButtonBackgroundObject.normalColor = new Color(0.7f, 0.7f, 0.7f, 0.2f);
            curBtn.uiButtonIcon = null;
            curBtn.uiButtonBackgroundObject.uiRayCast = true;
            CreateButton(parrent, curBtn);

            return null;
        }

        public static GameObject CreateScrollView(GameObject parrent, UiScrollView newUiScrollView)
        {
            GameObject ScrollBox = CreateUiObject(parrent, newUiScrollView);
            ScrollBox.AddComponent<ScrollRect>();
            ScrollBox.AddComponent<RectMask2D>();
            return ScrollBox;
        }

        /*
		public static GameObject clamp2DObjInScreen(GameObject targetObj, bool queryChildrenCheck = false, bool clampScale = false)
		{
			Resolution curRes = new Resolution();
			curRes.width = Screen.width;
			curRes.height = Screen.height;
			Vector2 curPos = targetObj.GetComponent<RectTransform>().anchoredPosition;
			if(queryChildrenCheck)
			{

			}else
			{
				Debug.Log(curRes.height);
				Debug.Log(curPos.x);
				if(curPos.x < 0 || curPos.x > curRes.height)
				{
					Debug.Log("Height correction!");
				}

				if(curRes.width < curPos.y || curRes.width >= curPos.y)
				{
					Debug.Log("Width correction!");
				}
			}


			if(clampScale)
			{

			}
			return targetObj;
		}
		 */

        /*
		 * UI STANDARD
		 */

        public static void SetCursorTexture(BerryCursor cursorTarget, bool hold = false)
        {
            if (cursorTarget == null)
            {
                Cursor.SetCursor(curCursor.cursorTex, curCursor.pos, curCursor.cursorMode);
            }
            else
            {
                if (!isHold)
                {
                    curCursor = cursorTarget;
                    Cursor.SetCursor(cursorTarget.cursorTex, cursorTarget.pos, cursorTarget.cursorMode);
                }
                else
                {
                    holdCursor = cursorTarget;
                    Cursor.SetCursor(cursorTarget.cursorTex, cursorTarget.pos, cursorTarget.cursorMode);
                }
            }
        }

        public static int CreateMessageBox(string message = "Hello world!", bool canselBtn = true, bool saveAndOpen = false, bool openWithoutSave = false, bool okBtn = true, bool closeBtn = false)
        {
            GameObject curFocus = CreateUiFocusObj(true);
            curFocus.AddComponent<SaveMapPrompt>().Prompt(curFocus);
            return 1;
        }

        public static void ShowNotification(string message, int icon)
        {
            GameObject curFocus = CreateUiFocusObj(true, true);
            curFocus.AddComponent<Notification>().Prompt(curFocus, message);
        }
    } //CLASS END.

    public class ColorPanel : MonoBehaviour
    {
        public Color32 curColor;
        public Slider sliderR, sliderG, sliderB;
        public Image prewPlane;
        public OptionRunner target;

        public void SetColorNew(Color32 newColor)
        {
            sliderR.value = newColor.r;
            sliderG.value = newColor.g;
            sliderB.value = newColor.b;
            newColor.a = 255;
            prewPlane.color = curColor = newColor;
        }
        public Color32 UpdateColor()
        {
            curColor.r = (byte)sliderR.value;
            curColor.g = (byte)sliderG.value;
            curColor.b = (byte)sliderB.value;
            curColor.a = 255;
            prewPlane.color = curColor;
            if (target != null)
            {
                target.PassColor(curColor);
            }
            return curColor;
        }
    }

    public class OptionRunner
    {
        public virtual void Pass()
        {

        }
        public virtual void PassColor(Color32 passColor)
        {

        }
    }

    public class BerryCursor
    {
        public CursorMode cursorMode;
        public Texture2D cursorTex;
        public Vector2 pos;
    }

    public class ListObj
    {
        public GameObject self;
        public GameObject[] listContent;
    }

    public class CanvasObjComponentMananger : MonoBehaviour
    {
        public CanvasObj currentCanvasObj;
        public bool isCanvasChild;
        public GameObject ParrentCanvasObj;
    }
    /*
	public class UiInterface
	{

	}
	 */

    public class FocusManager : MonoBehaviour
    {

        public void DeFocusSeparates()
        {

        }

        public void DefocusAll()
        {

        }
    }

}
