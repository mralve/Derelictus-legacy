/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace ConstruiSystem
{
    public class UiIntractable : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        public Image targetImage;
        public GameObject targetIcon;

        public bool canDrag = false, selectorLock = false;
        bool selMode;
        public UiInteractiveBackgroundObject ButtonColors;
        public bool updateColor;

        public bool isCheckBox;
        public bool isCheckBoxChecked;
        public GameObject checkBoxRef;

        bool isDragging = false;
        Vector3 dragOffset = new Vector3(1, 0, 1);
        Color targetColor;

        public float speed = 5.5f;

        public ActionComponent curAC;

        public delegate void InterationHandler();
        public InterationHandler onMouseClickEvent;

        void Awake()
        {
            if (ButtonColors == null)
            {
                ButtonColors = new UiInteractiveBackgroundObject();
            }
            speed = ButtonColors.speed;
            targetColor = new Color();
            switchColor(100, ButtonColors.normalColor);
        }

        void Update()
        {
            if (!selectorLock)
            {
                if (updateColor)
                {
                    if (targetImage)
                    {
                        targetImage.color = Vector4.MoveTowards(targetImage.color, targetColor, Time.deltaTime * speed);
                        if (targetImage.color == targetColor)
                        {
                            updateColor = false;
                        }
                    }
                    else
                    {
                        updateColor = false;
                    }
                }
            }
            if (isDragging)
            {
                this.transform.position = Input.mousePosition + dragOffset;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isCheckBox) { if (isCheckBoxChecked) { isCheckBoxChecked = false; checkBoxRef.SetActive(false); } else { isCheckBoxChecked = true; checkBoxRef.SetActive(true); } }
            if (onMouseClickEvent != null) { onMouseClickEvent.Invoke(); }
            if (eventData.button == 0)
            {
                signalState(0);
            }
            else { signalState(6); }
            switchColor(2, ButtonColors.hoverColor);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            signalState(1);
            if (isDragging)
            {
                isDragging = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            signalState(2);
            dragOffset = this.transform.position - Input.mousePosition;
            switchColor(2, ButtonColors.pressedColor);
        }

        public void OnDrag(PointerEventData eventData)
        {
            signalState(3);
            if (canDrag)
            {
                isDragging = true;
                switchColor(2, ButtonColors.draggingColor);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ButtonColors.dragCursor)
            {
                Cursor.SetCursor(ButtonColors.cursorImg, new Vector2(0, 7), CursorMode.Auto);
            }
            signalState(4);
            switchColor(2, ButtonColors.hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ButtonColors.dragCursor)
            {
                BerryCursor cursor = new BerryCursor();
                cursor.cursorTex = Resources.Load<Texture2D>("BerrySystem/UI/cursors/drag_up");
                cursor.pos = new Vector2(0, 0);
                cursor.cursorMode = CursorMode.Auto;
                UiManager.SetCursorTexture(cursor, true);
            }
            signalState(5);
            switchColor(2, ButtonColors.normalColor);
        }

        public void switchColor(float animSpeed, Color newTargetColor)
        {
            targetColor = newTargetColor;
            updateColor = true;
        }

        public void selectIntractable(bool mode)
        {
            if (targetImage)
            {
                if (mode)
                {
                    targetImage.color = new Color(1, 0.6f, 0, 0.8f);
                    selectorLock = true;
                }
                else
                {
                    selectorLock = false;
                }
            }
        }

        public void signalState(int state)
        {
            if (curAC != null)
            {
                switch (state)
                {
                    case 0:
                        curAC.Click();
                        break;
                    case 1:
                        curAC.Up();
                        break;
                    case 2:
                        curAC.Down();
                        break;
                    case 3:
                        curAC.Drag();
                        break;
                    case 4:
                        curAC.Enter();
                        break;
                    case 5:
                        curAC.Exit();
                        break;
                    case 6:
                        curAC.RightClick();
                        break;
                }
            }
        }
    }
}
