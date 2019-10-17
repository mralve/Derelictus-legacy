/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class Console : BerryWindow
    {

        public override void WindowCreate(int sizeX, int sizeY, GameObject windowRef)
        {
            ConsoleManager.CreateConsole();
            WindowTerminate();
        }

        public override string WindowGrabName()
        {
            // Set the window ui name here.
            return windowShortName = "Dev Console";
        }
    }
}