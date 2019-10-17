/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class Modus : PrimerComponent
    {
        public GameObject targetPlayerObj, modusUI;
        public EntityAgent targetPlayEntityAgent;

        public GameObject curCam, camParrent, editorGridObj, worldGrid2D;

        public override void PrimerInitialize()
        {
            primerModeName = "Modus";

            // UI
            modusUI = UiManager.CreateCanvas("ModusUI", primerParrentObj);

            // CORE
            camParrent = new GameObject("Camera");
            camParrent.transform.SetParent(primerParrentObj.transform);
            curCam = CameraManager.SetupNewCamera(camParrent, "ModusCam", true, true, 1, new Color32(195, 195, 195, 255), new Vector3(0, 1, -4));
            curCam.GetComponent<Camera>().orthographic = false;
            curCam.AddComponent<Zoom3D>();

            editorGridObj = new GameObject("EditorGrid");
            editorGridObj.transform.SetParent(primerParrentObj.transform);
            editorGridObj.AddComponent<CameraGrid2D>().normalLineColor = new Color(0, 0, 0, 0.5f);
            editorGridObj.GetComponent<CameraGrid2D>().DrawGrid();

            /*
			worldGrid2D = new GameObject("World2DGrid");
			worldGrid2D.transform.SetParent(primerParrentObj.transform);
			worldGrid2D.AddComponent<WorldGrid2D>();
             */

        }

        public override string PrimerGrabName()
        {
            return primerModeName = "Modus Primer";
        }

        public override void PrimerSaveSession()
        {

        }

        public override void PrimerTerminate()
        {

        }
    }
}
