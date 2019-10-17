/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace ConstruiSystem
{
    public class EditorToolEraserPen : EditorTool
    {
        Vector3 cursorTransform;
        SpriteHighlighter curHigh;
        float tileSize;
        bool spriteMode = false;

        public override void ToolActivation()
        {
            ScenePrimer.curEditorPrimer.curSpriteMode = false;
            curTileSelector.SetColor(new Color32(255, 0, 0, 255));
            tileSize = MapDataManager.mapDataTileSize;
            usesDrag = true;
        }

        public override void ToolModeSwap()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                spriteMode = ScenePrimer.curEditorPrimer.curSpriteMode = !ScenePrimer.curEditorPrimer.curSpriteMode;
            }

            if (spriteMode)
            {
                usesDrag = false;
                curTileSelector.gameObject.SetActive(false);
                if (curHigh == null)
                {
                    curHigh = new GameObject().AddComponent<SpriteHighlighter>();
                }
                else
                {
                    curHigh.gameObject.SetActive(false);
                }
            }
            else
            {
                usesDrag = true;
                curTileSelector.gameObject.SetActive(true);
                if (curHigh != null)
                {
                    curHigh.gameObject.SetActive(false);
                }
            }

            if (ScenePrimer.curEditorPrimer.curSpriteMode)
            {
                if (ScenePrimer.curEditorPrimer.curSpriteId != -1)
                {

                }
            }

        }

        public override void ToolDeActivation()
        {
            curTileSelector.gameObject.SetActive(true);
            if (curHigh != null) { GameObject.Destroy(curHigh); }
        }

        public override void ToolPrimaryUse()
        {
            if (!spriteMode)
            {
                ScenePrimer.curEditorPrimer.curSpriteMode = false;
                cursorTransform = CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition);
                cursorTransform.x = (float)System.Math.Round(cursorTransform.x / tileSize) * tileSize - tileSize * 0.5f;
                cursorTransform.z = (float)System.Math.Round(cursorTransform.z / tileSize) * tileSize + tileSize * 0.5f;
                SessionManager.PlaceTile(cursorTransform, ScenePrimer.curEditorPrimer.curLayer, 0, false, true);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = CameraManager.curCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        //hit.transform.gameObject.m
                        //Debug.Log( hit.transform.gameObject.name );
                        int index = hit.transform.GetComponent<SpriteInfo>().spriteIndex;
                        Chunk curChunk = SessionManager.GetChunk(hit.transform.position, SessionManager.GetChunkCluster(hit.transform.transform.position), false);
                        List<Spr> sprites = curChunk.sprites.ToList();
                        sprites.RemoveAt(index);
                        curChunk.sprites = sprites.ToArray();
                        if (curChunk.meshObjRef == null)
                        {
                            curChunk.meshObjRef = curChunk.chunkObjRef.GetComponent<TileMesh2D>();
                        }
                        curChunk.meshObjRef.GenerateSprites();
                        GameObject.Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    public class SpriteHighlighter : MonoBehaviour
    {
        public Ray ray;
        public Material passedMat;
        public SpriteRenderer passedSprite;
        public SpriteRenderer current;
        public RaycastHit hit;

        public void Awake()
        {

        }
        /*
		public void FixedUpdate()
		{
			ray = CameraManager.curCamera.ScreenPointToRay( Input.mousePosition );
			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				current = hit.transform.gameObject.GetComponent<SpriteRenderer>();
				if(current != null)
				{
					if(passedSprite == null){
						passedSprite = current;
						passedMat = passedSprite.material;

					}
					if(current != passedSprite)
					{
						passedMat = current.material;
						current.material = Resources.Load("BerrySystem/Shaders/select", typeof(Material)) as Material;
					}
				}else{
					passedSprite.material = passedMat; 
					passedMat = null;
					passedSprite = null;
				}
			}
		}
		 */

    }
}