/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class FPSDisplay : MonoBehaviour
{
    Text textComRef;
    float delta;
    int updateFrequency;

    void Awake()
    {
        if (this.GetComponent<Text>())
        {
            textComRef = this.GetComponent<Text>();
            textComRef.supportRichText = false;
        }
    }

    void Update()
    {
        delta = Time.deltaTime;
    }

    void FixedUpdate()
    {
        updateFrequency++;
        if (updateFrequency == 10)
        {
            textComRef.text = 1 / delta + " " + delta * 1000;
            updateFrequency = 0;
        }
    }
}
