/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System;
using UnityEngine;


/*

    the Primer Component is an inherited class in with the sceneprimer
    uses to controll the application.

 */


namespace ConstruiSystem
{
    public class PrimerComponent : MonoBehaviour
    {

        public GameObject primerParrentObj;
        public string primerModeName;
        public bool primerPaused, disableInput, usePreviewMode, previewMode;
        public OldZoom curZoomComp;

        public Camera primerCurCamera;
        public GameObject primerCurCameraObj;

        public void PrimerPreInitialize()
        {
            PrimerGrabName();
            if (primerParrentObj == null)
            {
                primerParrentObj = new GameObject(primerModeName);
            }
            PrimerInitialize();
        }

        public virtual String PrimerGrabName()
        {
            return primerModeName = "NULL NAME!";
        }

        public virtual void PrimerInitialize()
        {

        }

        public virtual void PrimerSaveSession()
        {

        }

        public virtual void PrimerLoadSession()
        {

        }

        public virtual void PrimerCreateCamera()
        {

        }

        public virtual void PrimerPause(bool state)
        {
            primerParrentObj.SetActive(state);
        }

        public virtual void PrimerTerminate()
        {

        }

        public virtual void PrimerSwitchEvent()
        { }

        public virtual void PrimerMapUpdate()
        {

        }
    }
}
