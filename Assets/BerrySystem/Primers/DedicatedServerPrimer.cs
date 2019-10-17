/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class DedicatedServerPrimer : PrimerComponent
    {
        // On server start
        public override void PrimerInitialize()
        {
            Debug.Log("Server start request");
        }
    }
}