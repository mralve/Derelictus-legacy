/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace ConstruiSystem
{
    // Don't hard wire the tool system in to the editor primer,
    // This enable the tool system in other primer modes.
    public static class GlobalToolManager
    {
        public static EditorToolManager globalToolManager;

        public static void SendToolUpdate()
        {
            if (globalToolManager != null)
            {
                globalToolManager.ToolUpdate();
            }
        }

        public static void DisabelTools()
        {
            if (globalToolManager != null)
            {
                globalToolManager.DeActivateTool();
                globalToolManager.ToolDestroySelector();
            }
        }

        public static void DisableInput(bool inputState)
        {
            if (globalToolManager != null)
            {
                globalToolManager.disableInput = !inputState;
            }
        }

        public static EditorToolManager CreateGlobalToolManager(GameObject parent = null)
        {
            if (globalToolManager == null)
            {
                globalToolManager = new GameObject("ToolManager").AddComponent<EditorToolManager>();
                globalToolManager.transform.parent = parent.transform;
                return globalToolManager;
            }
            else
            {
                return globalToolManager;
            }
        }
    }

    public class EditorToolManager : MonoBehaviour
    {
        public EditorTool curEditorTool;
        public EditorTool[] EditorTools;
        public TileSelector tileSelector;

        public bool disableInput;

        public void Awake()
        {
            EditorTools = new EditorTool[6];
            EditorTools[0] = new EditorToolTilePen();
            EditorTools[1] = new EditorToolEraserPen();
            EditorTools[2] = new EditorToolTileFill();
            EditorTools[3] = new EditorToolSelect();
            EditorTools[4] = new EditorToolPosition();
            EditorTools[5] = new EditorToolLineMesh();
        }

        public void Update()
        {
            if (!disableInput)
            {
                if (Input.anyKey)
                {
                    // exit tool
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        DeActivateTool();
                    }
                    // Toggel pen tool key
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        SetTool(EditorTools[0]);
                    }

                    // Toggel EditorToolEraserPen tool key
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        SetTool(EditorTools[1]);
                    }

                    // Toggel fill tool key
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        SetTool(EditorTools[2]);
                    }
                    // Toggel Select tool key
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        SetTool(EditorTools[3]);
                    }
                    if (curEditorTool != null)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject() && curEditorTool.careAbutUi)
                        {
                            // Hock in to the tool
                            if (curEditorTool.usesDrag)
                            {
                                if (Input.GetKey(KeyCode.Mouse0))
                                {
                                    curEditorTool.ToolPrimaryUse();
                                }
                                if (Input.GetKey(KeyCode.Mouse1))
                                {
                                    curEditorTool.ToolSecondaryUse();
                                }

                            }
                            else
                            {
                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    curEditorTool.ToolPrimaryUse();
                                }
                                if (Input.GetKeyDown(KeyCode.Mouse1))
                                {
                                    curEditorTool.ToolSecondaryUse();
                                }
                            }

                            if (Input.GetKeyDown(KeyCode.Tab))
                            {
                                curEditorTool.ToolModeSwap();
                            }

                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                curEditorTool.ToolLock(true);
                            }
                            else
                            {
                                curEditorTool.ToolLock(false);
                            }
                        }
                    }
                }
            }
        }

        public void SetTool(EditorTool targetTool)
        {
            if (tileSelector == null) { tileSelector = new GameObject("TileSelector").AddComponent<TileSelector>(); tileSelector.transform.SetParent(ScenePrimer.curPrimerComponent.primerParrentObj.transform); }
            if (targetTool == curEditorTool)
            {
                curEditorTool.PreToolDeActivation();
                tileSelector = curEditorTool.curTileSelector;
                tileSelector.gameObject.SetActive(false);
                curEditorTool = null;
            }
            else
            {
                if (curEditorTool != null)
                {
                    curEditorTool.PreToolDeActivation();
                    tileSelector = curEditorTool.curTileSelector;
                    tileSelector.gameObject.SetActive(false);
                }
                curEditorTool = targetTool;
                tileSelector.gameObject.SetActive(true);
                curEditorTool.curTileSelector = tileSelector;
                curEditorTool.PreToolActivation();
            }
        }

        public void ToolUpdate()
        {
            if (curEditorTool != null)
            {
                curEditorTool.ToolUpdate();
            }
        }

        public void ToolDestroySelector()
        {
            GameObject.Destroy(tileSelector);
        }

        public void ToolSelectorDeactivate()
        {
            tileSelector.gameObject.SetActive(false);
        }

        public void DeActivateTool()
        {
            if (curEditorTool != null)
            {
                curEditorTool.PreToolDeActivation();
                tileSelector = curEditorTool.curTileSelector;
                tileSelector.gameObject.SetActive(false);
                curEditorTool = null;
            }
        }
    }
}