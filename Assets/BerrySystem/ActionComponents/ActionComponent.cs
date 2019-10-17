/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstruiSystem
{

    // Deprecate and replace with c# delegates. 

    public class ActionComponent : MonoBehaviour
    {

        public string actionName = "";
        public bool isInitialized = false;
        public UiIntractable curUiIntractable;

        public delegate void ACCallBack();
        public ACCallBack ACClick;

        public virtual void InitializeActionComponent(string actionNameTarget)
        {
            if (this.gameObject.GetComponent<UiIntractable>() != null)
            {
                //this.gameObject.GetComponent<UiIntractable>().curAC = CurActionComponent;
            }
            else
            {
                Debug.Log("Error : UiIntractable component not found on " + this.gameObject.name);
            }
            actionName = actionNameTarget;
            isInitialized = true;
        }

        public void Awake()
        {
            if (this.GetComponent<UiIntractable>() != null)
            {
                // Connect this components logic to the button.
                curUiIntractable = this.GetComponent<UiIntractable>();
                curUiIntractable.curAC = this;
                AwakeActionComponent();
            }
        }

        public virtual void AwakeActionComponent()
        {
        }

        public virtual void Click()
        {
        }
        public virtual void RightClick()
        {
        }

        public virtual void Up()
        {
        }

        public virtual void Down()
        {
        }

        public virtual void Drag()
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

    }
}
