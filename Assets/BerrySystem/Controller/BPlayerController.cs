/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    // This player controller targets a EntityAgent with a rigidbody to controll.
    public class BPlayerController : BEntityController
    {
        // This is the reference to the EntityAgent that we want to controll.

        public GameObject targetCameraObj;
        public Camera targetCamera;
        public bool isCurrentController;
        public Vector3 controllerVel;

        public virtual void ControllerUpdateMode(bool targetMode, bool toggleCamera = true)
        {
            if (targetMode)
            {
                isCurrentController = true;
            }
            else
            {
                isCurrentController = false;
            }
        }

        void Update()
        {
            if (isCurrentController)
            {
                controllerVel.x = Input.GetAxisRaw("Horizontal");
                controllerVel.z = Input.GetAxisRaw("Vertical");

                targetEntityAgent.EntityMove(controllerVel);

                if (Input.GetAxisRaw("Jump") != 0)
                {
                    targetEntityAgent.EntityJump();
                }

                    Vector3 pos = Input.mousePosition;
                    targetEntityAgent.EntityAttack(pos, 32, 6f);
                if (Input.GetAxisRaw("Fire1") != 0)
                {
                }
            }
        }
    }
}