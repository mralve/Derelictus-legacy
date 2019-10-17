/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public struct BUITexturePanel
    {
        // Core ui object settings.
        public BUIObject uiObj;

        // custom variables.
        public UiObject[] stackItems;
        public int width, hight, padding, stackHight;

        // Custom functions
        public void CreatePanelList(GameObject parrent)
        {
            GameObject panelObj = UiManager.sCreateUiObject(parrent, uiObj);

            // Set the defult width and hight if the parrent is filling the screen / area
            if (uiObj.uiAnchorMode == UiManager.UiAnchorsMode.FillStretch)
            {
                width = (int)parrent.GetComponent<RectTransform>().sizeDelta.x;
                hight = (int)parrent.GetComponent<RectTransform>().sizeDelta.y;
            }

            // Add the scrolling functions to the stack area
            ScrollRect stackScrollArea = panelObj.AddComponent<ScrollRect>();
            GameObject pStackView = new GameObject("listView");
            pStackView.transform.SetParent(panelObj.transform);
            GameObject pStackContnet = new GameObject("listContent");
            pStackContnet.transform.SetParent(pStackView.transform);
            stackScrollArea.content = pStackContnet.AddComponent<RectTransform>();
            pStackContnet.AddComponent<Image>();
            stackScrollArea.content.anchorMin = new Vector2();
            stackScrollArea.content.anchorMax = new Vector2(1, 1);
            stackScrollArea.content.sizeDelta = new Vector2(0, 0);
            stackScrollArea.content.offsetMax = new Vector2();
            stackScrollArea.content.offsetMin = new Vector2();
            stackScrollArea.vertical = true;
            stackScrollArea.horizontal = false;
            stackScrollArea.movementType = ScrollRect.MovementType.Elastic;
            stackScrollArea.elasticity = 0.05f;
            stackScrollArea.inertia = true;
            stackScrollArea.decelerationRate = 0.15f;
            stackScrollArea.scrollSensitivity = 2;
            stackScrollArea.viewport = pStackView.AddComponent<RectTransform>();
            pStackView.AddComponent<RectMask2D>();
            stackScrollArea.viewport.anchorMin = new Vector2();
            stackScrollArea.viewport.anchorMax = new Vector2(1, 1);
            stackScrollArea.viewport.sizeDelta = new Vector2(0, 0);
            stackScrollArea.viewport.offsetMax = new Vector2();
            stackScrollArea.viewport.offsetMin = new Vector2();
            stackScrollArea.verticalScrollbar = new GameObject("ScrollBar").AddComponent<Scrollbar>();
            stackScrollArea.verticalScrollbar.transform.SetParent(panelObj.transform);
            stackScrollArea.verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().sizeDelta = new Vector2(8000, 0);
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().rect.Set(20, 20, 8, 0);
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().offsetMax = new Vector2();
            stackScrollArea.verticalScrollbar.GetComponent<RectTransform>().offsetMin = new Vector2();
            UiBackgroundObject scrollBar = new UiBackgroundObject();
            scrollBar.uiRayCast = true;
            scrollBar.uiSize = new Vector2();
            scrollBar.uiAnchorMode = UiManager.UiAnchorsMode.FillStretch;
            stackScrollArea.verticalScrollbar.targetGraphic = UiManager.CreateBackgroundObj(stackScrollArea.verticalScrollbar.gameObject, scrollBar).GetComponent<Image>();
            stackScrollArea.verticalScrollbar.handleRect = stackScrollArea.verticalScrollbar.targetGraphic.GetComponent<RectTransform>();

            if (stackItems != null)
            {

                for (int i = 0; i < stackItems.Length; i++)
                {
                    stackHight += padding;

                    if (stackItems[i] != null)
                    {
                        stackItems[i].uiObjName = "KEK";
                        stackItems[i].uiSize = new Vector2(width, 32);
                        stackItems[i].uiAnchorMode = UiManager.UiAnchorsMode.TopCenter;
                        stackItems[i].uiPosition = new Vector2(stackHight, 0);
                        stackItems[i].CreateUiType(pStackContnet);
                    }
                }
            }
            stackHight = 700;

            if (stackHight >= hight)
            {
                stackHight = hight - stackHight;
                stackScrollArea.content.offsetMin = new Vector2(stackScrollArea.content.offsetMin.x, stackHight);
            }
        }
    }
}