/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFriction : MonoBehaviour
{

    Rigidbody target;
    GroundFrictionTracker GFT;

    void Awake()
    {
        if (gameObject.GetComponent<BoxCollider>() != null)
        {

        }
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "feet")
        {
            target = trigger.transform.parent.GetComponent<Rigidbody>();
            target.drag = 1.3f;
            target.mass = 2;
            GFT = target.gameObject.GetComponent<GroundFrictionTracker>();
            if (GFT == null)
            {
                GFT = target.gameObject.AddComponent<GroundFrictionTracker>();
            }
            GFT.parrentGF = this;
        }
    }

    void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "feet")
        {
            target = trigger.transform.parent.GetComponent<Rigidbody>();
            GFT = target.gameObject.GetComponent<GroundFrictionTracker>();
            if (GFT.parrentGF == this)
            {
                target.drag = 9;
                target.mass = 1;
            }
        }
    }
}

public class GroundFrictionTracker : MonoBehaviour
{
    public GroundFriction parrentGF;
}
