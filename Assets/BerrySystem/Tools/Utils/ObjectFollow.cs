/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{

    public bool targetPositionCorrection = true;
    public float YPosCorrection = 2;
    public float ZPosCorrection = 0.4f;
    public float LerpSpeed = 6f; // 7
    public Quaternion targetRot;
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            if (targetPositionCorrection)
            {
                targetPos.y = YPosCorrection;
                targetPos.z = targetPos.z + ZPosCorrection;
            }

            if (target)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, LerpSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, LerpSpeed * Time.deltaTime);
            }
        }
    }
}