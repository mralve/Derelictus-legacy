/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    // This is the class that manages custom classes in BerrySystem
    public static class BerryClassManager
    {

    }

    // This class holds a reference to the custom class & gives the BerrySystem a description for the custom class for the end user.
    // 
    public class IBerryCustomScript : MonoBehaviour
    {
        public IBerry customClass;
        public string className = "Custom Class Name";
        public string classDescription = "Custom class description";

    }

    // This is the class that allows external scripts to hook in to the editor & BerrySystem.
    // IBerry inherits from MonoBehavior so that normal unity calls is possible.
    public class IBerry : MonoBehaviour
    {
        // 
        // public virtual void b

        // bTrigger
        // This is the function that recives trigger events from the BerrySystem.
        public virtual void BTrigger() { }
    }
}