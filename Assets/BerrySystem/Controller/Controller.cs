/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    // This is the standard controller class, This is the class that handles the intput from the player,
    // and allows the controll of EntityAgent
    public class BEntityController : MonoBehaviour
    {
        public EntityAgent targetEntityAgent;
        public Rigidbody targetRigidbody;

        public virtual void ContollerUpdate()
        { }
        public virtual void ContollerUpdateInput()
        { }
    }
}