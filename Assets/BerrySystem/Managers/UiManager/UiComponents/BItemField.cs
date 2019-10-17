/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */
 
// Prototype. BerrySystem will get a remake in unity ECS with DOD.

using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ConstruiSystem
{
    public class BItemField : MonoBehaviour
    {
        public RectTransform fieldView;
        public RectTransform containerRect;
        public FieldItem[] items;

        public int hNr = 7;
        int curHNr = 7;
        public bool verticalList = false, useNumberShortCut;

        public GameObject container;
        ScrollRect scrollHandel;

        public void GenerateViewItems()
        {
            curHNr = hNr;

            if (fieldView == null)
            {
                fieldView = gameObject.GetComponent<RectTransform>();
            }

            if (fieldView.GetComponent<RectMask2D>() == null)
            {
                gameObject.AddComponent<RectMask2D>();
            }

            if (container != null)
            {
                GameObject.Destroy(container);
                container = null;
            }

            container = new GameObject("container");
            container.transform.parent = fieldView;
            containerRect = container.AddComponent<RectTransform>();
            containerRect.gameObject.AddComponent<Image>().color = new Color(0, 0, 0, 0);
            containerRect.localPosition = new Vector2(0, 0);
            containerRect.anchorMin = new Vector2(0.5f, 1);
            containerRect.anchorMax = new Vector2(0.5f, 1);
            containerRect.pivot = new Vector2(0.5f, 1);
            containerRect.sizeDelta = new Vector2(fieldView.sizeDelta.x, 100 * items.Length / curHNr);

            if (scrollHandel == null)
            {
                scrollHandel = gameObject.AddComponent<ScrollRect>();
                scrollHandel.horizontal = false;
                scrollHandel.viewport = fieldView;
                scrollHandel.content = containerRect;
                scrollHandel.scrollSensitivity = 10;
            }

            scrollHandel.content = containerRect;

            UiButtonObject itemBtn = new UiButtonObject();
            itemBtn.uiSize = new Vector2(64, 64);
            itemBtn.uiButtonText = new UiTextObject();
            itemBtn.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            itemBtn.uiButtonBackgroundObject = new UiInteractiveBackgroundObject();
            itemBtn.uiButtonBackgroundObject.uiAnchorMode = UiManager.UiAnchorsMode.FillStretch;
            itemBtn.uiButtonBackgroundObject.uiSize = new Vector2(0, 12);
            itemBtn.uiButtonBackgroundObject.uiPosition = new Vector2(0, -8);
            itemBtn.uiButtonBackgroundObject.uiRayCast = true;
            itemBtn.uiRayCast = true;
            itemBtn.uiButtonIcon = new UiBackgroundObject();
            itemBtn.uiButtonIcon.uiAnchorMode = UiManager.UiAnchorsMode.FillStretch;
            itemBtn.uiButtonIcon.uiSize = new Vector2(0, 0);
            itemBtn.uiPosition = new Vector2(5, -5);
            itemBtn.pivot = new Vector2(0, 1);
            itemBtn.uiButtonText.uiPosition = new Vector2(0, -30);
            itemBtn.uiButtonText.uiTextColor = new Color(0, 0, 0, 1);
            itemBtn.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            itemBtn.uiButtonText.uiTextSize = 10;

            int curV = 0;
            int curH = 0;

            if (items.Length != 0)
            {
                if (verticalList)
                {
                    curHNr = 1;
                }

                for (int i = 0; i < items.Length; i++)
                {
                    if (!items[i].hidden)
                    {
                        itemBtn.uiPosition = new Vector2(curH + 66 * curH + 2, curV * -1 - 77 * curV - 2);
                        itemBtn.uiButtonText.uiText = items[i].itemDisplayText;
                        itemBtn.uiButtonIcon.uiSize = new Vector2(-4, -4);
                        itemBtn.uiButtonIcon.uiTextureRef = items[i].itemIcon;
                        UiManager.CreateButton(container, itemBtn).gameObject.AddComponent<ItemPress>().itemTarget = items[i];
                        curH++;
                        if (curH >= curHNr)
                        {
                            curH = 0;
                            curV++;
                        }
                    }
                }
            }
        }

        public void Update()
        {
            if (useNumberShortCut)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SelectItem(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SelectItem(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    SelectItem(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    SelectItem(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    SelectItem(4);
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    SelectItem(5);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    SelectItem(6);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    SelectItem(7);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    SelectItem(8);
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    SelectItem(9);
                }
            }
        }

        public void SelectItem(int targetItem)
        {
            if(items == null){return;}
            if (targetItem <= items.Length)
            {
                if (items[targetItem].itemPressMethod != null)
                {
                    ItemPress target = new ItemPress();
                    items[targetItem].itemPressMethod("", true, null, true, items[targetItem].index);
                }
            }
        }
    }

    public class ItemPress : ActionComponent
    {
        public FieldItem itemTarget;
        public bool selected;
        public UiIntractable curLocAC;
        public override void AwakeActionComponent()
        {
            curLocAC = this.GetComponent<UiIntractable>();
            curLocAC.curAC = this;

            if (itemTarget.startSelect)
            {
                selected = true;
                curLocAC.selectIntractable(selected);
                if (itemTarget.itemPressMethod != null)
                {
                    itemTarget.itemPressMethod(itemTarget.filePath, selected, this, false, itemTarget.index);
                }
            }
        }
        public override void Click()
        {
            if (itemTarget.selectable)
            {
                selected = !selected;
                curLocAC.selectIntractable(selected);
            }

            if (itemTarget.itemPressMethod != null)
            {
                itemTarget.itemPressMethod(itemTarget.filePath, selected, this, false, itemTarget.index);
            }
        }

        public override void RightClick()
        {
            if (itemTarget.itemRightPressMethod != null)
            {
                itemTarget.itemRightPressMethod(itemTarget.filePath, selected, this, false, itemTarget.index);
            }
        }
    }

    public struct FieldItem
    {
        public bool useIcon, hidden, selectable, startSelect;
        public int index;
        public Sprite itemIcon;
        public string itemDisplayText;
        public string filePath;

        public delegate void ItemPressDel(string path, bool mode, ItemPress handler, bool headLess, int headLessId);
        public ItemPressDel itemPressMethod;
        public ItemPressDel itemRightPressMethod;
    }
}