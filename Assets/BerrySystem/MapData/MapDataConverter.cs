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
    public static class MapDataConverter
    {

        // Vectors
        public static Vector2 V2ToVector2(V2 toVector)
        {
            Vector2 newVector = new Vector2();
            newVector.x = toVector.x;
            newVector.y = toVector.y;
            return newVector;
        }
        public static V2 Vector2ToV2(Vector2 toV2)
        {
            V2 newV2 = new V2();
            newV2.x = toV2.x;
            newV2.y = toV2.y;
            return newV2;
        }
        public static Vector3 V3ToVector3(V3 toVector)
        {
            Vector3 newVector = new Vector3();
            newVector.x = toVector.x;
            newVector.y = toVector.y;
            newVector.z = toVector.z;
            return newVector;
        }

        public static Color32 ColToColor32(Col toVector)
        {
            Color32 newVector = new Color32();
            newVector.r = (byte)toVector.x;
            newVector.g = (byte)toVector.y;
            newVector.b = (byte)toVector.z;
            return newVector;
        }

        public static Col Color32ToCol(Color32 toVector)
        {
            Col newVector = new Col();
            newVector.x = (byte)toVector.r;
            newVector.y = (byte)toVector.g;
            newVector.z = (byte)toVector.b;
            return newVector;
        }

        public static V3 Vector3ToV3(Vector3 toV3)
        {
            V3 newV3 = new V3();
            newV3.x = toV3.x;
            newV3.y = toV3.y;
            newV3.z = toV3.z;
            return newV3;
        }
        // Quaternions
        public static V3 QuaternionToV3(Quaternion toV3)
        {
            V3 newV3 = new V3();
            newV3.x = toV3.x;
            newV3.y = toV3.y;
            newV3.x = toV3.z;
            return newV3;
        }
        public static Quaternion V3ToQuaternion(V3 toQuat)
        {
            Quaternion newV3 = new Quaternion();
            newV3.x = toQuat.x;
            newV3.y = toQuat.y;
            newV3.x = toQuat.z;
            return newV3;
        }
    }
}
