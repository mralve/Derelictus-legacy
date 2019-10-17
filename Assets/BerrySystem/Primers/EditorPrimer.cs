/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System;
using UnityEngine;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorPrimer : PrimerComponent
    {

        // Editor UI obj refs
        public GameObject editorUi, editorUiStatic, editorInteractive, editorTileSelector, editorMapTitle, editorGridObj, worldGrid2D, worfldGrid2D, saveMapMenu;
        public EditorGridBtn curGridBtn;

        // Currently loaded editor icons
        public Sprite[] editorIcons;
        public Material worldMaterial;
        public Material spriteMaterial;

        public GameObject layerPanel;
        public GameObject texturePanel;
        public GameObject spritePanel;
        public GameObject entityPanel;
        public GameObject itemPanel;
        public bool usingPanel;

        public static GameObject SpawnIcon;
        public static SpriteRenderer SpawnIconRenderer;

        public static GameObject spriteIcon;
        public static SpriteRenderer spriteIconRenderer;

        public Text tileInfo;

        int temp = 0;


        Image texturePrew;

        int curTile = -1;
        public bool takesInput = false;
        bool spriteMode;
        public bool curSpriteMode { get { return spriteMode; } set { spriteMode = value; UpdateTexturePrew(); return; } }
        public int curTileId { get { return curTile; } set { curTile = value; UpdateTexturePrew(); return; } }
        int spriteId = -1;
        public int curSpriteId { get { return spriteId; } set { spriteId = value; UpdateTexturePrew(); } }
        public int curLayer = 0;
        public int curSortingLayer = 3;

        // Editor tools.
        public EditorToolManager curEditorToolManager;

        // Button states.
        bool undoKeyDown, reDoKeyDown, saveKeyDown, resKeyDown, cameraReset;
        public bool editorCreated;

        // Editor version info.
        public static float editorVersionNumberInternal = 0.02f;
        public string editorVersionNumberDisplay = " V.0.02";

        // Move key cheking to a diffrent class.
        public void Update()
        {
            // This is a great way to get some extra performance!
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.N) && editorCreated)
                {
                    if (previewMode)
                    {
                        previewMode = false;
                        disableInput = false;
                        EditorPrimer.SpawnIcon.SetActive(true);
                        ScenePrimer.curSceneprimer.PrimerSwitchMode(ScenePrimer.PrimerStartModes.editor, false);
                    }
                    else
                    {
                        UiManager.DestroyAllFocus();
                        previewMode = true;
                        disableInput = true;
                        spriteIcon.gameObject.SetActive(false);
                        EditorPrimer.SpawnIcon.SetActive(false);
                        ScenePrimer.curSceneprimer.PrimerSwitchMode(ScenePrimer.PrimerStartModes.game, false);
                    }
                }

                if (!previewMode && editorCreated && !disableInput)
                {
                    if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
                    {
                        if (spriteMode)
                        {
                            UpdateLayer(curSortingLayer + 1);
                        }
                        else
                        {
                            UpdateLayer(curLayer + 1);
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
                    {
                        if (spriteMode)
                        {
                            UpdateLayer(curSortingLayer - 1);
                        }
                        else
                        {
                            if (curLayer != 0)
                            {
                                UpdateLayer(curLayer - 1);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (editorGridObj != null)
                        {
                            if (editorGridObj.activeInHierarchy)
                            {
                                editorGridObj.SetActive(false);
                            }
                            else
                            {
                                editorGridObj.SetActive(true);
                            }
                            if (worldGrid2D.activeInHierarchy)
                            {
                                worldGrid2D.SetActive(false);
                            }
                            else
                            {
                                worldGrid2D.SetActive(true);
                            }
                            curGridBtn.IconUpdate();
                        }
                    }


                    if (Input.GetKeyDown(KeyCode.F7))
                    {
                        Screen.fullScreen = !Screen.fullScreen;
                    }

                    // re Do change.
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Y))
                    {
                        if (!reDoKeyDown)
                        {
                            reDoKeyDown = true;
                            SystemHistory.HistoryReDo();
                        }
                    }
                    else { reDoKeyDown = false; }

                    // LayerPanel
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        if (layerPanel == null)
                        {
                            usingPanel = true;
                            UiManager.DestroyAllFocus();
                            layerPanel = WindowManager.CreateWindow(0, 0, new BLayerPanel(), true, false, false);
                        }
                        else
                        {
                            usingPanel = false;
                            UiManager.DestroyAllFocus();
                            layerPanel = null;
                        }
                    }

                    // re Do change.
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (spritePanel == null)
                        {
                            usingPanel = true;
                            UiManager.DestroyAllFocus();
                            spritePanel = WindowManager.CreateWindow(0, 0, new BSpritePanel(), true, false, false);
                        }
                        else
                        {
                            usingPanel = false;
                            UiManager.DestroyAllFocus();
                            layerPanel = null;
                        }
                    }

                    // re Do change.
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (texturePanel == null)
                        {
                            usingPanel = true;
                            UiManager.DestroyAllFocus();
                            texturePanel = WindowManager.CreateWindow(0, 0, new BTexturePanel(), true, false, false);
                        }
                        else
                        {
                            usingPanel = false;
                            UiManager.DestroyAllFocus();
                            layerPanel = null;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (entityPanel == null)
                        {
                            usingPanel = true;
                            UiManager.DestroyAllFocus();
                            entityPanel = WindowManager.CreateWindow(0, 0, new BEntityPanel(), true, false, false);
                        }
                        else
                        {
                            usingPanel = false;
                            UiManager.DestroyAllFocus();
                            layerPanel = null;
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        if (itemPanel == null)
                        {
                            usingPanel = true;
                            UiManager.DestroyAllFocus();
                            itemPanel = WindowManager.CreateWindow(0, 0, new BItemPanel(), true, false, false);
                        }
                        else
                        {
                            usingPanel = false;
                            UiManager.DestroyAllFocus();
                            layerPanel = null;
                        }
                    }

                    // Undo change.
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z))
                    {
                        if (!undoKeyDown)
                        {
                            undoKeyDown = true;
                            SystemHistory.HistoryUndo();
                        }
                    }
                    else { undoKeyDown = false; }

                    // Save map key
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.S))
                    {
                        if (!saveKeyDown)
                        {
                            if (saveMapMenu == null)
                            {
                                if (MapDataManager.mapDataFilePath != "")
                                {
                                    saveKeyDown = true;
                                    BerryWindow targetWindow = new BMapSave();
                                    saveMapMenu = WindowManager.CreateWindow(300, 300, targetWindow);
                                }
                            }
                        }
                    }
                    else { saveKeyDown = false; }

                    // Toggel Resource View
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.T))
                    {
                        if (!resKeyDown)
                        {
                            resKeyDown = true;
                            //curResourceView.Click();
                        }
                    }
                    else { resKeyDown = false; }

                    // Camera pos reset 
                    // TODO : Fix so that we disable the rigidbody before the teleport to origin, and update the grids pos (Unity's physics system apparently don't like sudden teleports)
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
                    {
                        if (!cameraReset)
                        {
                            CameraManager.CurrentRenderCamera.transform.position = new Vector3(0, 2, 0);
                        }
                    }
                    else { cameraReset = false; }
                }
            }
        }

        public override void PrimerMapUpdate()
        {
            if (editorMapTitle != null)
            {
                editorMapTitle.GetComponent<Text>().text = MapDataManager.mapDataFileName;
            }
        }

        public override void PrimerCreateCamera()
        {
            primerCurCameraObj = CameraManager.SetupNewCamera(primerParrentObj, "EditorCamera", true, true, 5, new Color(0, 0.023f, 0.05f, 1), new Vector3(0, 0, 0), new Vector3(90, 0, 0), true);
            primerCurCamera = primerCurCameraObj.GetComponent<Camera>();
            CameraManager.UpdateCurrentCamera();
            primerCurCamera.clearFlags = CameraClearFlags.Color;
            primerCurCameraObj.transform.position = new Vector3(0, 2, 0);
            curZoomComp = primerCurCameraObj.AddComponent<OldZoom>();
            primerCurCameraObj.AddComponent<EditorMove>();
        }

        public override void PrimerInitialize()
        {
            primerModeName = "Editor Mode";
            worldMaterial = ScenePrimer.StandardWorld;
            spriteMaterial = ScenePrimer.StandardSprite;
            // Create a empty map
            SessionManager.SessionManagerClearRefs();
            editorIcons = Resources.LoadAll<Sprite>("BerrySystem/Icons/editorIcons");

            if (XCPManager.currentXCP == null)
            {
                takesInput = false;
                BXCPManager xcpWin = new BXCPManager();
                xcpWin.windowTitleName = "editorXCPWindow";
                WindowManager.CreateWindow(200, 300, xcpWin, false, true);
                disableInput = true;


                /*
                // Display logo.
                UiBackgroundObject logo = new UiBackgroundObject();
                logo.uiObjName = "Xonomoto logo";
                logo.uiTextureRef = Resources.Load<Sprite>("Xonomoto_studios");
                logo.uiAnchorMode = UiManager.UiAnchorsMode.TopRight;
                UiManager.CreateBackgroundObj(null, logo);
                 */


                return;
            }
            else
            {

            }
            /*
			if(MapDataManager.mapDataXCPIndex == -1){
				MapDataManager.mapData = new MapData().createEmptyMapData("New Map", EditorPrimer.editorVersionNumberInternal,"Derelictus");
				MapDataManager.mapDataFileName = "New Map";
				MapDataManager.mapDataTileSize = 0.32f;
				SessionManager.CreateMapLayer("Layer 0", true, true);
				MapDataManager.MapDataCreateMapObj();
			}
			 */


            // Create the editor camera.
            // = MapDataConverter.Vector3ToV3( new Vector3(0, 5.8f, 12.75f) );
            // = MapDataConverter.Color32ToV3( new Color32(109, 166, 255, 255) );
            if (primerCurCameraObj == null)
            {
                PrimerCreateCamera();
            }
            disableInput = false;

            editorUi = UiManager.CreateCanvas("editorUi", primerParrentObj);

            SpawnIcon = new GameObject("editorSetterIcon");
            SpawnIconRenderer = SpawnIcon.AddComponent<SpriteRenderer>();
            SpawnIconRenderer.sprite = Resources.Load<Sprite>("BerrySystem/Textures/Topdown/alphaSpawn1");
            SpawnIconRenderer.transform.eulerAngles = new Vector2(90, 0);
            SpawnIconRenderer.transform.position = MapDataConverter.V3ToVector3(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].layerSpawn);
            SpawnIconRenderer.transform.position += new Vector3(0, 0.3f, 0);
            SpawnIconRenderer.sortingOrder = SessionManager.SpriteSortByPos(SpawnIconRenderer);

            spriteIcon = new GameObject("editorSpritePrew");
            spriteIconRenderer = spriteIcon.AddComponent<SpriteRenderer>();
            spriteIconRenderer.transform.eulerAngles = new Vector2(90, 0);
            spriteIconRenderer.transform.position += new Vector3(0, 0.4f, 0);
            spriteIconRenderer.material = spriteMaterial;
            spriteIcon.AddComponent<MoveAlong>().Icon = spriteIconRenderer;
            spriteIcon.SetActive(false);


            /*
			 */
            editorGridObj = new GameObject("editorGrid");
            editorGridObj.transform.SetParent(primerParrentObj.transform);
            editorGridObj.AddComponent<CameraGrid2D>();

            worldGrid2D = new GameObject("World2DGrid");
            worldGrid2D.transform.SetParent(primerParrentObj.transform);
            worldGrid2D.AddComponent<WorldGrid2D>();
            editorUiStatic = UiManager.CreateCanvas("EditorStatic", editorUi, false, true, 0, false);
            UiManager.SetUiAnchors(editorUiStatic, UiManager.UiAnchorsMode.FillStretch);

            // Editor side bar
            UiBackgroundObject editorSideBar = new UiBackgroundObject();
            editorSideBar.uiObjName = "editorSideBar";
            editorSideBar.uiPosition = new Vector2(16, 0);
            editorSideBar.uiSize = new Vector2(32, 0);
            editorSideBar.uiColor = new Color32(0, 115, 180, 255);
            editorSideBar.uiAnchorMode = UiManager.UiAnchorsMode.MiddelLeftStretchVertical;
            UiManager.CreateBackgroundObj(editorUiStatic, editorSideBar);

            // Editor side bar
            UiBackgroundObject editorTextureWindow = new UiBackgroundObject();
            editorTextureWindow.uiObjName = "editorTexture";
            editorTextureWindow.uiPosition = new Vector2(74, 40);
            editorTextureWindow.uiSize = new Vector2(70, 70);
            editorTextureWindow.uiRayCast = true;
            editorTextureWindow.uiColor = new Color32(255, 255, 255, 250);
            editorTextureWindow.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            texturePrew = UiManager.CreateBackgroundObj(editorUiStatic, editorTextureWindow).GetComponent<Image>();
            texturePrew.preserveAspect = true;
            if (ScenePrimer.curEditorPrimer.curTileId == -1)
            {
                ScenePrimer.curEditorPrimer.curTileId = 0;
            }

            // Editor Window Title Bar
            UiBackgroundObject editorWindowTitleBar = new UiBackgroundObject();
            editorWindowTitleBar.uiObjName = "editorWindowTitleBar";
            editorWindowTitleBar.uiPosition = new Vector2(0, -16);
            editorWindowTitleBar.uiSize = new Vector2(0, 32);
            editorWindowTitleBar.uiColor = new Color32(40, 170, 224, 255);
            editorWindowTitleBar.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            UiManager.CreateBackgroundObj(editorUiStatic, editorWindowTitleBar);

            // Editor Tool Bar
            UiBackgroundObject editorToolBar = new UiBackgroundObject();
            editorToolBar.uiObjName = "editorToolBar";
            editorToolBar.uiPosition = new Vector2(0, -48);
            editorToolBar.uiSize = new Vector2(0, 32);
            editorToolBar.uiColor = new Color(0, 0.552f, 0.807f, 1f);
            editorToolBar.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            UiManager.CreateBackgroundObj(editorUiStatic, editorToolBar);

            editorInteractive = UiManager.CreateCanvas("EditorInteractive", editorUi, false, true, 0, true);
            UiManager.SetUiAnchors(editorInteractive, UiManager.UiAnchorsMode.FillStretch);

            UiTextObject editorTileLayerInfo = new UiTextObject();
            editorTileLayerInfo.uiObjName = "editorTileLayerInfo";
            editorTileLayerInfo.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            editorTileLayerInfo.uiTextColor = new Color(1, 1, 1, 1);
            editorTileLayerInfo.uiText = "Layer : " + curLayer;
            editorTileLayerInfo.uiTextAlign = TextAnchor.MiddleCenter;
            editorTileLayerInfo.uiPosition = new Vector2(74, 90);
            editorTileLayerInfo.uiSize = new Vector2(70, 70);
            tileInfo = UiManager.CreateTextObj(editorInteractive, editorTileLayerInfo).GetComponent<Text>();

            UiButtonObject editorGridButton = new UiButtonObject();
            editorGridButton.uiObjName = "editorGridButton";
            editorGridButton.uiPosition = new Vector2(-16, -48);
            editorGridButton.uiSize = new Vector2(32, 33);
            editorGridButton.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorGridButton.uiButtonIcon.uiTextureRef = editorIcons[5];
            editorGridButton.uiAnchorMode = UiManager.UiAnchorsMode.TopRight;
            editorGridButton.uiButtonBackgroundObject.uiRayCast = true;
            editorGridButton.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorGridButton.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            UiManager.CreateButton(editorInteractive, editorGridButton).gameObject.AddComponent<EditorGridBtn>();

            UiTextObject mapTitle = new UiTextObject();
            mapTitle.uiObjName = "editorTitle";
            mapTitle.uiTextAlign = TextAnchor.MiddleCenter;
            mapTitle.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            mapTitle.uiSize = new Vector2(32, 32);
            mapTitle.uiPosition = new Vector2(0, -16);
            mapTitle.uiText = MapDataManager.mapDataFileName;
            editorMapTitle = UiManager.CreateTextObj(editorInteractive, mapTitle);

            // Editor File Menu
            UiButtonObject editorFileMenu = new UiButtonObject();
            editorFileMenu.uiObjName = "editorFileMenu";
            editorFileMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorFileMenu.uiPosition = new Vector2(25, -48);
            editorFileMenu.uiSize = new Vector2(50, 32);
            editorFileMenu.uiStaticObj = true;
            editorFileMenu.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            editorFileMenu.uiButtonText = new UiTextObject();
            editorFileMenu.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorFileMenu.uiButtonText.uiText = "File";

            editorFileMenu.uiButtonIcon = null;

            editorFileMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorFileMenu.uiButtonBackgroundObject.uiSize = new Vector2(50, 32);
            editorFileMenu.uiButtonBackgroundObject.normalColor = new Color32(255, 255, 255, 0);
            editorFileMenu.uiButtonBackgroundObject.hoverColor = new Color32(255, 255, 255, 40);
            editorFileMenu.uiButtonBackgroundObject.pressedColor = new Color32(255, 255, 255, 70);
            UiManager.CreateButton(editorInteractive, editorFileMenu).onMouseClickEvent = ClickFileBtn;

            // Editor Editor Menu
            UiButtonObject editorEditorMenu = new UiButtonObject();
            editorEditorMenu.uiObjName = "editorEditorMenu";
            editorEditorMenu.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorEditorMenu.uiPosition = new Vector2(80, -48);
            editorEditorMenu.uiSize = new Vector2(60, 32);
            editorEditorMenu.uiStaticObj = true;
            editorEditorMenu.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            editorEditorMenu.uiButtonText = new UiTextObject();
            editorEditorMenu.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorEditorMenu.uiButtonText.uiText = "Editor";

            editorEditorMenu.uiButtonIcon = null;

            editorEditorMenu.uiButtonBackgroundObject.uiRayCast = true;
            editorEditorMenu.uiButtonBackgroundObject.uiSize = new Vector2(60, 32);
            editorEditorMenu.uiButtonBackgroundObject.normalColor = new Color32(255, 255, 255, 0);
            editorEditorMenu.uiButtonBackgroundObject.hoverColor = new Color32(255, 255, 255, 40);
            editorEditorMenu.uiButtonBackgroundObject.pressedColor = new Color32(255, 255, 255, 70);
            UiManager.CreateButton(editorInteractive, editorEditorMenu).onMouseClickEvent = ClickEditBtn;

            // Editor Editor Menu
            UiButtonObject editorLevelMode = new UiButtonObject();
            editorLevelMode.uiObjName = "editorLevelMode";
            editorLevelMode.uiAnchorMode = UiManager.UiAnchorsMode.TopCenter;
            editorLevelMode.uiPosition = new Vector2(-30, -48);
            editorLevelMode.uiSize = new Vector2(60, 32);
            editorLevelMode.uiStaticObj = true;
            editorLevelMode.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            editorLevelMode.uiButtonText = new UiTextObject();
            editorLevelMode.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorLevelMode.uiButtonText.uiText = "Level";

            editorLevelMode.uiButtonIcon = null;

            editorLevelMode.uiButtonBackgroundObject.uiRayCast = true;
            editorLevelMode.uiButtonBackgroundObject.uiSize = new Vector2(60, 32);
            editorLevelMode.uiButtonBackgroundObject.normalColor = new Color32(255, 255, 255, 30);
            editorLevelMode.uiButtonBackgroundObject.hoverColor = new Color32(255, 255, 255, 40);
            editorLevelMode.uiButtonBackgroundObject.pressedColor = new Color32(255, 255, 255, 70);
            UiManager.CreateButton(editorInteractive, editorLevelMode);

            // Editor Editor Menu
            UiButtonObject editorUiMode = new UiButtonObject();
            editorUiMode.uiObjName = "editorUiMode";
            editorUiMode.uiAnchorMode = UiManager.UiAnchorsMode.TopCenter;
            editorUiMode.uiPosition = new Vector2(30, -48);
            editorUiMode.uiSize = new Vector2(60, 32);
            editorUiMode.uiStaticObj = true;
            editorUiMode.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            editorUiMode.uiButtonText = new UiTextObject();
            editorUiMode.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            editorUiMode.uiButtonText.uiText = "Ui";

            editorUiMode.uiButtonIcon = null;

            editorUiMode.uiButtonBackgroundObject.uiRayCast = true;
            editorUiMode.uiButtonBackgroundObject.uiSize = new Vector2(60, 32);
            editorUiMode.uiButtonBackgroundObject.normalColor = new Color32(255, 255, 255, 0);
            editorUiMode.uiButtonBackgroundObject.hoverColor = new Color32(255, 255, 255, 40);
            editorUiMode.uiButtonBackgroundObject.pressedColor = new Color32(255, 255, 255, 70);
            UiManager.CreateButton(editorInteractive, editorUiMode);

            // Editor Preview Menu
            UiButtonObject preview = new UiButtonObject();
            preview.uiObjName = "editorPreview";
            preview.uiAnchorMode = UiManager.UiAnchorsMode.TopRight;
            preview.uiPosition = new Vector2(-64, -48);
            preview.uiSize = new Vector2(60, 32);
            preview.uiStaticObj = true;
            preview.uiButtonBackgroundObject.uiColor = new Color32(255, 255, 255, 0);

            preview.uiButtonText = new UiTextObject();
            preview.uiButtonText.uiTextAlign = TextAnchor.MiddleCenter;
            preview.uiButtonText.uiText = "Preview";

            preview.uiButtonIcon = null;

            preview.uiButtonBackgroundObject.uiRayCast = true;
            preview.uiButtonBackgroundObject.uiSize = new Vector2(60, 32);
            preview.uiButtonBackgroundObject.normalColor = new Color32(255, 255, 255, 0);
            preview.uiButtonBackgroundObject.hoverColor = new Color32(255, 255, 255, 40);
            preview.uiButtonBackgroundObject.pressedColor = new Color32(255, 255, 255, 70);
            UiManager.CreateButton(editorInteractive, preview).onMouseClickEvent = ClickPreviewBtn;

            // Create fps and millisecond display.
            UiTextObject editorWindowTitle = new UiTextObject();
            editorWindowTitle.uiObjName = "editorWindowTitle";
            editorWindowTitle.uiText = "Construi";
            editorWindowTitle.uiPosition = new Vector2(40, -15);
            editorWindowTitle.uiSize = new Vector2(60, 32);
            editorWindowTitle.uiTextColor = new Color(1, 1, 1, 1f);
            editorWindowTitle.uiTextSize = 12;
            editorWindowTitle.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            UiManager.CreateTextObj(editorUiStatic, editorWindowTitle);

            // Create fps and millisecond display.
            UiTextObject editorFps = new UiTextObject();
            editorFps.uiObjName = "editorFps";
            editorFps.uiPosition = new Vector2(120, -15);
            editorFps.uiSize = new Vector2(90, 32);
            editorFps.uiTextColor = new Color(1, 1, 1, 1f);
            editorFps.uiTextSize = 12;
            editorFps.uiAnchorMode = UiManager.UiAnchorsMode.TopStretchHorizontal;
            UiManager.CreateTextObj(editorUiStatic, editorFps).AddComponent<FPSDisplay>();

            // Create pentool.
            UiButtonObject editorLayerSettings = new UiButtonObject();
            editorLayerSettings.uiObjName = "editorLayerSettings";
            editorLayerSettings.uiPosition = new Vector2(16, -80);
            editorLayerSettings.uiSize = new Vector2(32, 32);
            editorLayerSettings.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorLayerSettings.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorLayerSettings.uiButtonBackgroundObject.uiRayCast = true;
            editorLayerSettings.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorLayerSettings.uiButtonIcon.uiTextureRef = editorIcons[16];
            UiManager.CreateButton(editorInteractive, editorLayerSettings).onMouseClickEvent = ClickLayerBtn;

            UiButtonObject editorTexturePanel = new UiButtonObject();
            editorTexturePanel.uiObjName = "editorTexturePanel";
            editorTexturePanel.uiPosition = new Vector2(16, -112);
            editorTexturePanel.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonBackgroundObject.uiRayCast = true;
            editorTexturePanel.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorTexturePanel.uiButtonIcon.uiTextureRef = editorIcons[7];
            UiManager.CreateButton(editorInteractive, editorTexturePanel).onMouseClickEvent = ClickTexturesBtn;

            UiButtonObject editorSpritePanel = new UiButtonObject();
            editorTexturePanel.uiObjName = "editorTexturePanel";
            editorTexturePanel.uiPosition = new Vector2(16, -112 - 32);
            editorTexturePanel.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorTexturePanel.uiButtonBackgroundObject.uiRayCast = true;
            editorTexturePanel.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorTexturePanel.uiButtonIcon.uiTextureRef = editorIcons[28];
            UiManager.CreateButton(editorInteractive, editorTexturePanel).onMouseClickEvent = ClickSpritePanelBtn;

            UiButtonObject editorEntityPanel = new UiButtonObject();
            editorEntityPanel.uiObjName = "editorEntityPanel";
            editorEntityPanel.uiPosition = new Vector2(16, -112 - 64);
            editorEntityPanel.uiSize = new Vector2(32, 32);
            editorEntityPanel.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorEntityPanel.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorEntityPanel.uiButtonBackgroundObject.uiRayCast = true;
            editorEntityPanel.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorEntityPanel.uiButtonIcon.uiTextureRef = editorIcons[37];
            UiManager.CreateButton(editorInteractive, editorEntityPanel).onMouseClickEvent = ClickEntityPanelBtn;

            UiButtonObject editorItemPanel = new UiButtonObject();
            editorItemPanel.uiObjName = "editorItemPanel";
            editorItemPanel.uiPosition = new Vector2(16, -112 - 64 - 32);
            editorItemPanel.uiSize = new Vector2(32, 32);
            editorItemPanel.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorItemPanel.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorItemPanel.uiButtonBackgroundObject.uiRayCast = true;
            editorItemPanel.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            editorItemPanel.uiButtonIcon.uiTextureRef = editorIcons[36];
            UiManager.CreateButton(editorInteractive, editorItemPanel).onMouseClickEvent = ClickItemPanelBtn;

            // Create editor tool manager
            curEditorToolManager = GlobalToolManager.CreateGlobalToolManager(primerParrentObj);

            // Create pentool.
            UiButtonObject editorPenTool = new UiButtonObject();
            editorPenTool.uiObjName = "editorPenTool";
            editorPenTool.uiPosition = new Vector2(16, 32);
            editorPenTool.uiSize = new Vector2(32, 32);
            editorPenTool.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorPenTool.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorPenTool.uiButtonBackgroundObject.uiRayCast = true;
            editorPenTool.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            editorPenTool.uiButtonIcon.uiTextureRef = editorIcons[1];

            curEditorToolManager.EditorTools[0].curToolIndicator = UiManager.CreateButton(editorInteractive, editorPenTool).gameObject.AddComponent<EditorToolIndicator>();
            curEditorToolManager.EditorTools[0].curToolIndicator.targetTool = curEditorToolManager.EditorTools[0];
            curEditorToolManager.EditorTools[0].curToolIndicator.toolActivatedIcon = editorIcons[12];
            curEditorToolManager.EditorTools[0].curToolIndicator.toolDeActivatedIcon = editorIcons[1];

            UiButtonObject EditorToolEraserPen = new UiButtonObject();
            EditorToolEraserPen.uiObjName = "editorToolEraserPen";
            EditorToolEraserPen.uiPosition = new Vector2(16, 64);
            EditorToolEraserPen.uiSize = new Vector2(32, 32);
            EditorToolEraserPen.uiButtonIcon.uiSize = new Vector2(32, 32);
            EditorToolEraserPen.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            EditorToolEraserPen.uiButtonBackgroundObject.uiRayCast = true;
            EditorToolEraserPen.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            EditorToolEraserPen.uiButtonIcon.uiTextureRef = editorIcons[17];

            curEditorToolManager.EditorTools[1].curToolIndicator = UiManager.CreateButton(editorInteractive, EditorToolEraserPen).gameObject.AddComponent<EditorToolIndicator>();
            curEditorToolManager.EditorTools[1].curToolIndicator.targetTool = curEditorToolManager.EditorTools[1];
            curEditorToolManager.EditorTools[1].curToolIndicator.toolActivatedIcon = editorIcons[18];
            curEditorToolManager.EditorTools[1].curToolIndicator.toolDeActivatedIcon = editorIcons[17];

            UiButtonObject editorFillTool = new UiButtonObject();
            editorFillTool.uiObjName = "editorFillTool";
            editorFillTool.uiPosition = new Vector2(16, 96);
            editorFillTool.uiSize = new Vector2(32, 32);
            editorFillTool.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorFillTool.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorFillTool.uiButtonBackgroundObject.uiRayCast = true;
            editorFillTool.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            editorFillTool.uiButtonIcon.uiTextureRef = editorIcons[0];

            curEditorToolManager.EditorTools[2].curToolIndicator = UiManager.CreateButton(editorInteractive, editorFillTool).gameObject.AddComponent<EditorToolIndicator>();
            curEditorToolManager.EditorTools[2].curToolIndicator.targetTool = curEditorToolManager.EditorTools[2];
            curEditorToolManager.EditorTools[2].curToolIndicator.toolActivatedIcon = editorIcons[11];
            curEditorToolManager.EditorTools[2].curToolIndicator.toolDeActivatedIcon = editorIcons[0];

            UiButtonObject editorSelect = new UiButtonObject();
            editorSelect.uiObjName = "editorSelect";
            editorSelect.uiPosition = new Vector2(16, 128);
            editorSelect.uiSize = new Vector2(32, 32);
            editorSelect.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorSelect.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorSelect.uiButtonBackgroundObject.uiRayCast = true;
            editorSelect.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            editorSelect.uiButtonIcon.uiTextureRef = editorIcons[26];

            curEditorToolManager.EditorTools[3].curToolIndicator = UiManager.CreateButton(editorInteractive, editorSelect).gameObject.AddComponent<EditorToolIndicator>();
            curEditorToolManager.EditorTools[3].curToolIndicator.targetTool = curEditorToolManager.EditorTools[2];
            curEditorToolManager.EditorTools[3].curToolIndicator.toolActivatedIcon = editorIcons[25];
            curEditorToolManager.EditorTools[3].curToolIndicator.toolDeActivatedIcon = editorIcons[26];

            UiButtonObject editorLineMesh = new UiButtonObject();
            editorLineMesh.uiObjName = "editorLineMesh";
            editorLineMesh.uiPosition = new Vector2(16, 128 + 32);
            editorLineMesh.uiSize = new Vector2(32, 32);
            editorLineMesh.uiButtonIcon.uiSize = new Vector2(32, 32);
            editorLineMesh.uiButtonBackgroundObject.uiSize = new Vector2(32, 32);
            editorLineMesh.uiButtonBackgroundObject.uiRayCast = true;
            editorLineMesh.uiAnchorMode = UiManager.UiAnchorsMode.BottomLeft;
            editorLineMesh.uiButtonIcon.uiTextureRef = editorIcons[30];

            curEditorToolManager.EditorTools[5].curToolIndicator = UiManager.CreateButton(editorInteractive, editorLineMesh).gameObject.AddComponent<EditorToolIndicator>();
            curEditorToolManager.EditorTools[5].curToolIndicator.targetTool = curEditorToolManager.EditorTools[5];
            curEditorToolManager.EditorTools[5].curToolIndicator.toolActivatedIcon = editorIcons[32];
            curEditorToolManager.EditorTools[5].curToolIndicator.toolDeActivatedIcon = editorIcons[30];

            editorCreated = true;
            takesInput = true;

            UpdateTexturePrew();

        }

        // Thanks unity for this, please get your shit together;
        public static string GoodIdToBadUnityId(int layerId)
        {
            switch (layerId)
            {
                case 0:
                    return "tilelayer";
                case 1:
                    return "spriteground1";
                case 2:
                    return "spriteground2";
                case 3:
                    return "spriteground3";
                case 4:
                    return "Deafult";
                case 5:
                    return "spritesky";
                default:
                    return "Deafult";
            }
        }

        public void UpdateTexturePrew()
        {
            if (texturePrew && editorCreated)
            {
                UpdateLayer(-1);
                if (curSpriteMode)
                {
                    if (spriteId != -1)
                    {
                        texturePrew.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[spriteId]);
                    }
                }
                else
                {
                    if (curTileId != -1)
                    {
                        if (XCPManager.currentXCP.tileTextures != null)
                        {
                            texturePrew.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.tileTextures[curTile]);
                        }
                    }
                }
            }
            GlobalToolManager.SendToolUpdate();
        }

        public void UpdateLayer(int targetLayer)
        {
            if (spriteMode)
            {
                curSortingLayer = targetLayer;
                if (curSortingLayer > 4) { curSortingLayer = 4; }
                if (curSortingLayer <= -1) { curSortingLayer = 0; }
                tileInfo.text = "Sorting Layer : " + curSortingLayer;
            }
            else
            {
                if (tileInfo != null)
                {
                    if (targetLayer != -1)
                    { curLayer = targetLayer; }
                    tileInfo.text = "Layer : " + curLayer;
                }
            }
            GlobalToolManager.SendToolUpdate();
        }

        /*------------------------------------------------ */
        // Editor UI interaction functions.
        /*------------------------------------------------ */
        public void ClickFileBtn()
        {
            UiDropDown dropDown = new UiDropDown();
            dropDown.uiPosition = new Vector2(64, -64);
            dropDown.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            dropDown.uiObjName = "DropDown";
            dropDown.dropDownOptions = new DropDownOption[7];
            dropDown.dropDownOptions[0].targetNewWindow = new BMapNew();
            dropDown.dropDownOptions[1].targetNewWindow = new BMapSave();
            dropDown.dropDownOptions[2].targetNewWindow = new BMapSaveAs();
            dropDown.dropDownOptions[3].targetNewWindow = new BMapImport();
            dropDown.dropDownOptions[4].targetNewWindow = new BXCPManager();
            dropDown.dropDownOptions[5].targetNewWindow = new BEditorInfoWindow();
            dropDown.dropDownOptions[6].targetNewWindow = new EditorXDP();
            UiManager.CreateDropDown(this.gameObject, dropDown);
        }

        public void ClickEditBtn()
        {
            UiDropDown dropDown = new UiDropDown();
            dropDown.uiPosition = new Vector2(108, -64);
            dropDown.uiAnchorMode = UiManager.UiAnchorsMode.TopLeft;
            dropDown.uiObjName = "Editor-DropDown";
            dropDown.dropDownOptions = new DropDownOption[2];
            dropDown.dropDownOptions[0].Name = "Console";
            dropDown.dropDownOptions[0].destroy = true;
            dropDown.dropDownOptions[0].itemPressMethod = createEditorConsole;
            dropDown.dropDownOptions[1].targetNewWindow = new TerminatePreview();
            UiManager.CreateDropDown(this.gameObject, dropDown);
        }

        public void createEditorConsole()
        {
            ConsoleManager.CreateConsole();
        }

        public void ClickPreviewBtn()
        {
            EditorPrimer.SpawnIcon.SetActive(false);
            ScenePrimer.curPrimerComponent.previewMode = true;
            ScenePrimer.curSceneprimer.PrimerSwitchMode(ScenePrimer.PrimerStartModes.game, false);
        }

        /* LEVEL EDITOR SIDE BAR BUTTONS ... */

        public void ClickLayerBtn()
        {
            ScenePrimer.curEditorPrimer.usingPanel = true;
            ScenePrimer.curEditorPrimer.layerPanel = WindowManager.CreateWindow(0, 0, new BLayerPanel(), true, false, false);
        }

        public void ClickTexturesBtn()
        {
            ScenePrimer.curEditorPrimer.usingPanel = true;
            ScenePrimer.curEditorPrimer.texturePanel = WindowManager.CreateWindow(0, 0, new BTexturePanel(), true, false, false);
        }

        public void ClickSpritePanelBtn()
        {
            ScenePrimer.curEditorPrimer.usingPanel = true;
            ScenePrimer.curEditorPrimer.spritePanel = WindowManager.CreateWindow(0, 0, new BSpritePanel(), true, false, false);
        }

        public void ClickEntityPanelBtn()
        {
            ScenePrimer.curEditorPrimer.usingPanel = true;
            ScenePrimer.curEditorPrimer.spritePanel = WindowManager.CreateWindow(0, 0, new BEntityPanel(), true, false, false);
        }

        public void ClickItemPanelBtn()
        {
            ScenePrimer.curEditorPrimer.usingPanel = true;
            ScenePrimer.curEditorPrimer.spritePanel = WindowManager.CreateWindow(0, 0, new BItemPanel(), true, false, false);
        }

        /*------------------------------------------------ */
        // The final primer functions.
        /*------------------------------------------------ */
        public override string PrimerGrabName()
        {
            return primerModeName = "Editor Primer";
        }

        public override void PrimerSwitchEvent()
        {
            SpawnIcon.SetActive(true);
            CameraManager.curCamera = primerCurCamera;
            CameraManager.CurrentRenderCamera = primerCurCameraObj;
            ScenePrimer.curPrimerComponent.curZoomComp.zoomingEnabled = true;
        }
    }
    
    // TODO : Deprecate the use of a seprate class (ActionComponent) for this button.
    public class EditorGridBtn : ActionComponent
    {
        public GameObject curGridObjRef;
        EditorPrimer curEditorObj;
        Image icon;
        UiIntractable btnRef;

        public override void AwakeActionComponent()
        {
            btnRef = this.GetComponent<UiIntractable>();
            btnRef.curAC = this;
            curEditorObj = ScenePrimer.curPrimerComponent.GetComponent<EditorPrimer>();
            curEditorObj.curGridBtn = this;
            curGridObjRef = curEditorObj.editorGridObj;
            icon = btnRef.targetIcon.GetComponent<Image>();
        }

        public override void Click()
        {
            if (curGridObjRef.activeInHierarchy)
            {
                curGridObjRef.SetActive(false);
            }
            else
            {
                curGridObjRef.SetActive(true);
            }
            IconUpdate();
        }

        public void IconUpdate()
        {
            if (curGridObjRef.activeInHierarchy)
            {
                icon.sprite = curEditorObj.editorIcons[5];
            }
            else
            {
                icon.sprite = curEditorObj.editorIcons[4];
            }
        }
    }
}
