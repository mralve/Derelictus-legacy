/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class SpriteAnimator : MonoBehaviour
    {
        // The target sprite that is going to be updated.
        public SpriteRenderer AnimationSprite;
        public GameObject AnimationObject;
        public SpriteAnimation idleAnimation;
        // The current animation
        public SpriteAnimation curAnimation;
        private MaterialPropertyBlock curPropBlock;

        // Anim play track
        private int animeStep;
        private int animLength;
        private float animTime;
        private float animSpeed;
        //private bool animLast;

        private string curAnimationName;

        void Awake()
        {
            transform.eulerAngles = new Vector2(90, 0);
            AnimationObject = gameObject;
            if (GetComponent<SpriteRenderer>() != null)
            {
                AnimationSprite = GetComponent<SpriteRenderer>();
            }
            else
            {
                AnimationSprite = transform.gameObject.AddComponent<SpriteRenderer>();
                AnimationSprite.material = Resources.Load<Material>("BerrySystem/shaders/spriteDeafult");
            }
            AnimationSprite.sortingOrder = 2;
            curPropBlock = new MaterialPropertyBlock();
        }
        // Play the requsested animation
        public void AnimationPlay(SpriteAnimation targetAnimation, bool revesePlay = false, bool resetAnim = false)
        {
            if (targetAnimation.animationName != curAnimationName || resetAnim)
            {
                curAnimation = targetAnimation;
                curAnimationName = curAnimation.animationName;
                animSpeed = curAnimation.animationSpeed;
                animLength = curAnimation.animationFrames.Length - 1;
                animTime = 0;
                animeStep = 0;
            }
        }

        // Pause the current animation
        public void AnimationPause()
        { }
        // Stops the current animation and sets curAnimation to null!
        public void AnimationStop()
        { }
        public void UpdateSpriteRendering(Sprite overrideSprite = null)
        {
            AnimationSprite.GetPropertyBlock(curPropBlock);
            if (overrideSprite != null)
            {
                AnimationSprite.sprite = overrideSprite;
            }
            else
            {
                AnimationSprite.sprite = curAnimation.animationFrames[animeStep].frameSprite;
            }
            curPropBlock.SetTexture("_tex", AnimationSprite.sprite.texture);
            curPropBlock.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
            AnimationSprite.SetPropertyBlock(curPropBlock);
        }

        // Draws from standard unity update
        public void Update()
        {
            if (curAnimation != null)
            {
                AnimationSprite.GetPropertyBlock(curPropBlock);
                AnimationSprite.sprite = curAnimation.animationFrames[animeStep].frameSprite;
                curPropBlock.SetTexture("_tex", AnimationSprite.sprite.texture);
                curPropBlock.SetColor("_col", MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].forColor));
                AnimationSprite.SetPropertyBlock(curPropBlock);

                if (animTime >= 1)
                {
                    if (animeStep == animLength)
                    {
                        if (curAnimation.animationLoop)
                        {
                            animeStep = 0;
                        }
                        else
                        {
                            curAnimation = null;
                        }
                    }
                    else
                    {
                        animeStep++;
                    }
                    animTime = 0;
                }
                else
                {
                    animTime = animTime + animSpeed * Time.deltaTime;
                }
            }
        }
    }
}