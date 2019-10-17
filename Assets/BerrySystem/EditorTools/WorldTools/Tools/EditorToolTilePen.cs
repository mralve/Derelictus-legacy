/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;
using UnityEngine.UI;

namespace ConstruiSystem
{
    public class EditorToolTilePen : EditorTool
    {
        Vector3 cursorTransform;
        float tileSize;
        bool spriteMode;
        SpriteRenderer Icon;
        Chunk targetChunk;
        MaterialPropertyBlock spriteProps = new MaterialPropertyBlock();

        public override void ToolActivation()
        {
            curTileSelector.gameObject.SetActive(!ScenePrimer.curEditorPrimer.curSpriteMode);
            curTileSelector.SetColor(new Color32(255, 255, 255, 255));
            tileSize = MapDataManager.mapDataTileSize;
            Icon = EditorPrimer.spriteIconRenderer;
            Icon.gameObject.SetActive(ScenePrimer.curEditorPrimer.curSpriteMode);
            spriteMode = ScenePrimer.curEditorPrimer.curSpriteMode;
        }

        public override void ToolUpdate()
        {
            if (ScenePrimer.curEditorPrimer.curSpriteMode)
            {
                if (ScenePrimer.curEditorPrimer.curSpriteId != -1)
                {
                    spriteMode = true;
                    curTileSelector.gameObject.SetActive(false);
                    Icon.gameObject.SetActive(true);
                    Icon = EditorPrimer.spriteIconRenderer;

                    Icon.GetPropertyBlock(spriteProps);
                    Icon.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[ScenePrimer.curEditorPrimer.curSpriteId]);
                    spriteProps.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
                    spriteProps.SetTexture("_tex", Icon.sprite.texture);
                    Icon.SetPropertyBlock(spriteProps);

                    Icon.sortingLayerName = EditorPrimer.GoodIdToBadUnityId(ScenePrimer.curEditorPrimer.curSortingLayer + 1);
                }
            }
        }
        public override void ToolDeActivation()
        {
            Icon.gameObject.SetActive(false);
        }

        public override void ToolPrimaryUse()
        {
            if (spriteMode)
            {

                usesDrag = false;
                targetChunk = SessionManager.GetChunk(Icon.transform.position, SessionManager.GetChunkCluster(Icon.transform.position), false);
                if (targetChunk != null)
                {
                    Icon.transform.position = SessionManager.ObjPosition(Icon.transform.position);
                    if (targetChunk.sprites == null)
                    {
                        targetChunk.sprites = new Spr[1];
                        targetChunk.sprites[0].posx = Icon.transform.position.x;
                        targetChunk.sprites[0].posy = Icon.transform.position.z;
                        targetChunk.sprites[0].sortLayer = (ushort)ScenePrimer.curEditorPrimer.curSortingLayer;
                        targetChunk.sprites[0].spriteId = (ushort)ScenePrimer.curEditorPrimer.curSpriteId;
                    }
                    else
                    {
                        Array.Resize(ref targetChunk.sprites, targetChunk.sprites.Length + 1);
                        targetChunk.sprites[targetChunk.sprites.Length - 1] = new Spr();
                        targetChunk.sprites[targetChunk.sprites.Length - 1].posx = Icon.transform.position.x;
                        targetChunk.sprites[targetChunk.sprites.Length - 1].posy = Icon.transform.position.z;
                        targetChunk.sprites[targetChunk.sprites.Length - 1].sortLayer = (ushort)ScenePrimer.curEditorPrimer.curSortingLayer;
                        targetChunk.sprites[targetChunk.sprites.Length - 1].spriteId = (ushort)ScenePrimer.curEditorPrimer.curSpriteId;
                        targetChunk.sprites[targetChunk.sprites.Length - 1].sortLayer = (ushort)ScenePrimer.curEditorPrimer.curSortingLayer;

                    }
                    if (targetChunk != null)
                    {
                        if (targetChunk.meshObjRef == null)
                        {
                            targetChunk.meshObjRef = targetChunk.chunkObjRef.GetComponent<TileMesh2D>();
                        }
                    }
                    targetChunk.meshObjRef.curCunk = targetChunk;
                    if (targetChunk.meshObjRef.spriteContainer != null)
                    {
                        GameObject spriteObj = new GameObject(ScenePrimer.curEditorPrimer.curSpriteId.ToString());
                        spriteObj.transform.SetParent(targetChunk.meshObjRef.spriteContainer.transform);
                        spriteObj.transform.localPosition = Icon.transform.position;
                        if (targetChunk.sprites.Length == 1)
                        {
                            spriteObj.AddComponent<SpriteInfo>().spriteIndex = 0;
                        }
                        else
                        {
                            spriteObj.AddComponent<SpriteInfo>().spriteIndex = targetChunk.sprites.Length - 1;
                        }
                        SpriteRenderer spriteRender = spriteObj.AddComponent<SpriteRenderer>();

                        spriteRender.material = ScenePrimer.curEditorPrimer.worldMaterial;
                        spriteRender.transform.eulerAngles = new Vector2(90, 0);
                        spriteRender.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[ScenePrimer.curEditorPrimer.curSpriteId]);
                        spriteRender.sortingOrder = SessionManager.SpriteSortByPos(spriteRender);
                        spriteRender.sortingLayerName = EditorPrimer.GoodIdToBadUnityId(ScenePrimer.curEditorPrimer.curSortingLayer + 1);
                        spriteObj.AddComponent<BoxCollider>().isTrigger = true;

                        spriteRender.GetPropertyBlock(spriteProps);
                        spriteRender.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[ScenePrimer.curEditorPrimer.curSpriteId]);
                        spriteProps.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
                        spriteProps.SetTexture("_tex", spriteRender.sprite.texture);
                        spriteRender.SetPropertyBlock(spriteProps);
                    }
                    else
                    {
                        targetChunk.meshObjRef.GenerateSprites();
                    }
                }
            }
            else
            {
                usesDrag = true;
                cursorTransform = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);

                cursorTransform.x = (float)System.Math.Round(cursorTransform.x / tileSize) * tileSize - tileSize * 0.5f;
                cursorTransform.z = (float)System.Math.Round(cursorTransform.z / tileSize) * tileSize + tileSize * 0.5f;

                SessionManager.PlaceTile(cursorTransform, ScenePrimer.curEditorPrimer.curLayer, ScenePrimer.curEditorPrimer.curTileId, true, false);
            }


        }

        public override void ToolSecondaryUse()
        {

        }
        public override void ToolModeSwap()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                spriteMode = ScenePrimer.curEditorPrimer.curSpriteMode = !ScenePrimer.curEditorPrimer.curSpriteMode;
            }

            curTileSelector.gameObject.SetActive(!spriteMode);
            if (ScenePrimer.curEditorPrimer.curSpriteMode)
            {
                if (ScenePrimer.curEditorPrimer.curSpriteId != -1)
                {
                    Icon = EditorPrimer.spriteIconRenderer;
                    Icon.sprite = XCPManager.PngToSprite(XCPManager.currentXCP.spriteTextures[ScenePrimer.curEditorPrimer.curSpriteId]);
                }
            }
            Icon.gameObject.SetActive(spriteMode);

            if (spriteMode)
            {
                ScenePrimer.curEditorPrimer.UpdateLayer(3);
            }
            else
            {
                ScenePrimer.curEditorPrimer.UpdateLayer(0);
            }

        }
    }

    public class MoveAlong : MonoBehaviour
    {
        Vector3 pos;
        public SpriteRenderer Icon;
        public void Update()
        {
            pos = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);
            //pos = SessionManager.objPosition(pos);
            /*
				pos.x = (float) System.Math.Round(pos.x, 3);
				pos.z = (float) System.Math.Round(pos.z, 3);
				pos.z += 0.3f;
			 */
            pos.y = 0.2f;
            Icon.sortingOrder = SessionManager.SpriteSortByPos(Icon);

            transform.position = pos;
        }
    }
}