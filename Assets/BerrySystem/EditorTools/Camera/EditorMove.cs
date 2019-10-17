/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMove : MonoBehaviour
{

    public float sensitivity = 11f, curCamZoom, speedAfterZoom;
    public bool invertControls = false;
    public Rigidbody curRigidbody;
    public Camera curCam;

    void Awake()
    {
        curCam = GetComponent<Camera>();
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            curRigidbody = gameObject.AddComponent<Rigidbody>();
            curRigidbody.useGravity = false;
            curRigidbody.drag = 6.5f;
            curRigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1) == true)
        {

            curCamZoom = curCam.orthographicSize;
            speedAfterZoom = sensitivity * curCamZoom;

            //			if(invertControls){

            //    			curRigidbody.AddForce(Input.GetAxis("Mouse X")*speedAfterZoom,0,Input.GetAxis("Mouse Y")*speedAfterZoom);

            //			} else {

            curRigidbody.AddForce(Input.GetAxis("Mouse X") * speedAfterZoom * -1, 0, Input.GetAxis("Mouse Y") * speedAfterZoom * -1);

            //			}
        }
    }
}
