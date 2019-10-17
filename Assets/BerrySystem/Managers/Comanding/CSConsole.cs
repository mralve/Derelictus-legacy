/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using System;

namespace ConstruiSystem
{
    public class CSConsole : MonoBehaviour
    {
        GameObject consoleCanvas;
        GameObject consoleBackgroundObj;
        public InputField consoleInput;
        public bool fullscreen;
        public void CreateConsole(bool fullscreenMode = true)
        {
            fullscreen = fullscreenMode;
            consoleCanvas = UiManager.CreateCanvas("console ui", this.gameObject);

            // Console Background.
            UiBackgroundObject consoleBackground = new UiBackgroundObject();
            consoleBackground.uiRayCast = false;
            consoleBackground.uiColor = new Color(0,0,0);
            if(fullscreen)
            {
                consoleBackground.uiAnchorMode = UiManager.UiAnchorsMode.FillStretch;
                consoleBackground.uiSize = new Vector2(0, 0);
            }else{
                consoleBackground.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
                consoleBackground.uiSize = new Vector2(0, 400);
                consoleBackground.uiPosition = new Vector2(0,0);
            }
            consoleBackgroundObj = UiManager.CreateBackgroundObj(consoleCanvas, consoleBackground);

            // Console input.  ca 16px txh
            
            UiTextInputField uiMapName = new UiTextInputField();
            uiMapName.uiPosition = new Vector2(0, 35);
            uiMapName.uiSize = new Vector2(0, 34);
            uiMapName.uiAnchorMode = UiManager.UiAnchorsMode.BottomStretchHorizontal;
            uiMapName.uiButtonBackgroundObject = null;
            uiMapName.uiButtonIcon.uiColor = new Color(1,1,1,1f);
            uiMapName.uiButtonIcon.uiAnchorMode = UiManager.UiAnchorsMode.BottomStretchHorizontal;
            uiMapName.uiButtonIcon.uiSize = new Vector2(uiMapName.uiSize.x, 1);
            uiMapName.uiButtonIcon.uiPosition = new Vector2(0, 20);
            uiMapName.fieldName = new UiTextObject();
            uiMapName.fieldName.uiText = "  >";
            uiMapName.fieldName.uiAnchorMode = UiManager.UiAnchorsMode.BottomStretchHorizontal;
            uiMapName.fieldName.uiSize = new Vector2(0,16);
            uiMapName.uiButtonText = new UiTextObject();
            uiMapName.uiButtonText.uiTextColor = new Color(1,1,1,1);
            uiMapName.uiButtonText.uiAnchorMode = UiManager.UiAnchorsMode.BottomStretchHorizontal;
            
            uiMapName.uiButtonText.uiText = "";
            uiMapName.uiButtonText.uiSize = new Vector2(uiMapName.uiSize.x, -10);
            
            consoleInput = UiManager.CreateTextInputField(consoleCanvas, uiMapName);
            consoleInput.caretWidth = 6;
            consoleInput.selectionColor = new Color(0,0,1,1);
            consoleInput.textComponent.color = new Color(1,1,1,1);
            consoleInput.textComponent.rectTransform.anchorMin = new Vector2(.01f,0);
            consoleInput.textComponent.fontSize = 12;
            //consoleInput.Select();
            consoleInput.ActivateInputField();
            consoleInput.onEndEdit.AddListener(delegate {submitToConsole();});

            // Console log.


        }

        public void submitToConsole()
        {
            ConsoleManager.Command(consoleInput.text);
            consoleInput.text = "";
        }

        void Update()
        {
            if (!consoleInput.isFocused)
            {
                consoleInput.ActivateInputField();
            }
        }

    }
}