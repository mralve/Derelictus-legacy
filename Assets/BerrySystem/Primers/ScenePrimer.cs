/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;


/*
 *   ConstruiSystem 101.
 *
 *   The Sceneprimer is the most important class in the intire ConstruiSystem,
 *   as it is the so called "the entry point" or the "main()" for the system.
 *
 *   The ScenePrimer uses "primer classes", a primer class is an class that inherits the PrimerComponent class,
 *   the goal of a primer is to controll the applications current core behavior,
 *   for example, an editor, game, or a dedicated server.
 *
 *   The Sceneprimer is meant to handle the switching diffrent primers with the PrimerSwitchMode() function.
 *
 *   PrimerStartModes enum controlls the target primer class.
 *   
 */


namespace ConstruiSystem
{
    public class ScenePrimer : MonoBehaviour
    {
        // This is the objects that we can add the primer mode or managers to.
        public static GameObject curPrimerParrentObj;
        public static ScenePrimer curSceneprimer;
        
        // curPrimerComponent is the reference to this script.
        public static PrimerComponent curPrimerComponent;
        public static PrimerComponent[] curPrimerSessions;

        public bool fullscreen;

        // Some static references
        public static PrimerComponent curGamePrimer;
        public static EditorPrimer curEditorPrimer;
        public static PrimerComponent curModusPrimer;

        // This is the diffrent start modes for the game.
        public enum PrimerStartModes { game, gameLauncher, editor, console, dedicatedServer }
        public PrimerComponent primerMode;

        // This is the defult start mode if nothing is selected by defult.
        public PrimerStartModes defaultPrimerMode = PrimerStartModes.game;

        public static Material StandardUiMaterial, StandardWorld, StandardSprite;

        public IBerryCustomScript[] customScripts;

        // The standard awake call from unity, this effectively automatically kick starts the entire system.
        public void Awake()
        {
            // These settings will be loaded and configurable later on, maybe let the primers to do the job?
            Screen.fullScreen = fullscreen;
            /*
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
             */

            curSceneprimer = this;
            // Fill with a reverence of self.
            if (curPrimerParrentObj == null)
            {
                curPrimerParrentObj = this.gameObject;
            }

            // Load the standard berrySystem Material for the ui.
            if (StandardUiMaterial == null)
            {
                StandardUiMaterial = Resources.Load<Material>("BerrySystem/UI/UI_Color");
            }

            // Load the standard BerrySystem Material for the world.
            if (StandardWorld == null)
            {
                StandardWorld = Resources.Load<Material>("BerrySystem/Shaders/mat_world");
                //Debug.Log(StandardWorld);
            }

            if (StandardSprite == null)
            {
                StandardSprite = Resources.Load<Material>("BerrySystem/Shaders/spriteDeafult");
            }

            // Kick start BerrySystem.
            PrimerTargetStartMode(defaultPrimerMode);
        }

        // The PrimerTargetStartMode() must preferably only ever be called once by the scene primer itself.
        public void PrimerTargetStartMode(PrimerStartModes targetPrimerMode)
        {
            switch (targetPrimerMode)
            {
                case PrimerStartModes.game:
                    PrimerStartGame();
                    break;
                case PrimerStartModes.gameLauncher:
                    PrimerStartGameLauncher();
                    break;
                case PrimerStartModes.editor:
                    PrimerStartEditor();
                    break;
                case PrimerStartModes.dedicatedServer:
                    PrimerStartDedicatedServer();
                    break;
                case PrimerStartModes.console:
                    PrimerStartConsole();
                    break;
            }
        }

        public void PrimerSwitchMode(PrimerStartModes targetMode, bool state)
        {
            curPrimerComponent.PrimerPause(state);
            PrimerTargetStartMode(targetMode);
        }

        public void PrimerTerminateCurrent()
        {
            curPrimerComponent.PrimerTerminate();
        }

        public void PrimerStartEditor()
        {
            if (curEditorPrimer == null)
            {
                curEditorPrimer = curPrimerParrentObj.AddComponent<EditorPrimer>();
                curPrimerComponent = curEditorPrimer;
                curPrimerComponent.PrimerPreInitialize();
            }
            else
            {
                curEditorPrimer.primerParrentObj.SetActive(true);
                curPrimerComponent = curEditorPrimer;
                CameraManager.UpdateCurrentCamera();
                curPrimerComponent.PrimerSwitchEvent();
            }
        }

        public void PrimerStartGame()
        {
            if (curGamePrimer == null)
            {
                curPrimerComponent = PrimerRegisterSession(curPrimerParrentObj.AddComponent<GamePrimer>());
                curGamePrimer = curPrimerComponent;
                curPrimerComponent.PrimerPreInitialize();
            }
            else
            {
                curGamePrimer.primerParrentObj.SetActive(true);
                curPrimerComponent = curGamePrimer;
                CameraManager.UpdateCurrentCamera();
                curPrimerComponent.PrimerSwitchEvent();
                GL.Clear(true, true, new Color(0, 0, 0, 0), 1);
            }
        }

        public void PrimerStartModus()
        {
            if (curModusPrimer == null)
            {
                curPrimerComponent = PrimerRegisterSession(curPrimerParrentObj.AddComponent<Modus>());
                curModusPrimer = curPrimerComponent;
                curPrimerComponent.PrimerPreInitialize();
            }
            else
            {
                curModusPrimer.primerParrentObj.SetActive(true);
                curPrimerComponent = curModusPrimer;
                CameraManager.UpdateCurrentCamera();
                curPrimerComponent.PrimerSwitchEvent();
            }
        }

        public void PrimerStartGameLauncher()
        {
            curPrimerComponent = PrimerRegisterSession(curPrimerParrentObj.AddComponent<LauncherPrimer>());
            curPrimerComponent.PrimerPreInitialize();
        }

        public void PrimerStartDedicatedServer()
        {
            curPrimerComponent = PrimerRegisterSession(curPrimerParrentObj.AddComponent<DedicatedServerPrimer>());
            curPrimerComponent.PrimerPreInitialize();
        }

        public void PrimerStartConsole()
        {
            curPrimerComponent = PrimerRegisterSession(curPrimerParrentObj.AddComponent<ConsolePrimer>());
            curPrimerComponent.PrimerPreInitialize();
        }
        public PrimerComponent PrimerRegisterSession(PrimerComponent targetSession)
        {
            if (curPrimerSessions == null)
            {
                curPrimerSessions = new PrimerComponent[1];
                return curPrimerSessions[0] = targetSession;
            }
            else
            {
                Array.Resize(ref curPrimerSessions, curPrimerSessions.Length + 1);
                return curPrimerSessions[curPrimerSessions.Length - 1] = targetSession;
            }
        }
    }

}
