/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class ConsolePrimer : PrimerComponent
    {
        public override void PrimerInitialize()
        {
            primerModeName = "Console";
            GlobalToolManager.DisabelTools();
            UiManager.CreateEventSystem(this.gameObject);
            ConsoleManager.CreateConsole();
            
            GL.Clear(true, true, new Color(0, 0, 0, 0));
        }

        public override string PrimerGrabName()
        {
            return primerModeName = "Console Primer";
        }

        public override void PrimerSwitchEvent()
        {
            GL.Clear(true, true, new Color(0, 0, 0, 1), 1);
        }

        public override void PrimerSaveSession()
        {

        }

        public override void PrimerTerminate()
        {

        }
    }
}
