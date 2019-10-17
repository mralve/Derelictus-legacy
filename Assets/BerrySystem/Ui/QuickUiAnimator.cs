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
    public class QuickUiAnimator : MonoBehaviour
    {
        public enum AnimationType { fadeIn, posAnim, scale }
        public bool animating;
        public bool loop;
        public bool pingPong;
        public bool ping;

        public delegate void AnimCallBack();
        public AnimCallBack AnimationDoneCallback;

        private CanvasGroup curCanvasGroup;
        private AnimationType targetAnim;
        public bool animate { get; private set; }
        private float speed;
        private float time;

        float curAlphaFadeStart;
        float curAlphaFadeEnd;
        float targetAlphaFadeEnd;
        bool curDestroyParrent;

        public void PlayFadeAnim(float alphaFadeStart, float alphaFadeEnd, bool loopAnim, bool destroyParrent, float targetSpeed = 1, bool loopPingPong = true, AnimCallBack callBack = null)
        {
            animate = true;
            loop = loopAnim;
            pingPong = loopPingPong;
            curAlphaFadeStart = alphaFadeStart;
            curAlphaFadeEnd = alphaFadeEnd;
            targetAlphaFadeEnd = alphaFadeEnd;
            curDestroyParrent = destroyParrent;
            targetAnim = AnimationType.fadeIn;
            speed = targetSpeed;
            if (curCanvasGroup == null)
            {
                curCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                curCanvasGroup.alpha = curAlphaFadeStart;
            }
            if (callBack != null)
            {
                AnimationDoneCallback = callBack;
            }
        }

        private Vector2 startPosTarget;
        private Vector2 endPosTarget;

        public void PlayPosAnim(Vector2 startPos, Vector2 endPos, bool loopAnim, bool destroyParrent, float targetSpeed = 1, bool loopPingPong = true, AnimCallBack callBack = null)
        {
            loop = loopAnim;
            pingPong = loopPingPong;
            startPosTarget = startPos;
            endPosTarget = endPos;
            curDestroyParrent = destroyParrent;
            targetAnim = AnimationType.posAnim;
            speed = targetSpeed;
            animate = true;
            transform.localPosition = startPos;
            if (callBack != null)
            {
                AnimationDoneCallback = callBack;
            }
        }

        public void PlayScaleAnim(Vector2 startScale, Vector2 endScale, bool loopAnim, bool destroyParrent, float targetSpeed = 1, bool loopPingPong = true, AnimCallBack callBack = null)
        {
            loop = loopAnim;
            pingPong = loopPingPong;
            startPosTarget = startScale;
            endPosTarget = endScale;
            curDestroyParrent = destroyParrent;
            targetAnim = AnimationType.scale;
            speed = targetSpeed;
            animate = true;
            transform.localPosition = startScale;
            if (callBack != null)
            {
                AnimationDoneCallback = callBack;
            }
        }

        public void StopAnimations()
        {
            animate = false;
            animating = false;
        }

        public void Update()
        {
            if (animate)
            {
                animating = true;
                switch (targetAnim)
                {
                    case AnimationType.posAnim:
                        AnimPos();
                        break;
                    case AnimationType.fadeIn:
                        AnimFadeIn();
                        break;
                    case AnimationType.scale:
                        AnimScale();
                        break;
                }
            }
        }

        void AnimFadeIn()
        {
            if (curCanvasGroup == null)
            {
                curCanvasGroup = gameObject.AddComponent<CanvasGroup>();

                curCanvasGroup.alpha = curAlphaFadeStart;
            }
            curCanvasGroup.alpha = Mathf.MoveTowards(curCanvasGroup.alpha, curAlphaFadeEnd, Time.deltaTime * speed);

            if (curCanvasGroup.alpha == curAlphaFadeEnd)
            {
                if (AnimationDoneCallback != null)
                {
                    AnimationDoneCallback();
                }
                animating = false;
                if (!loop)
                {
                    animate = false;
                    Destroy(curCanvasGroup);
                }
                else
                {
                    animating = true;
                    if (pingPong)
                    {
                        if (targetAlphaFadeEnd == curAlphaFadeStart)
                        {
                            curAlphaFadeEnd = targetAlphaFadeEnd;
                            ping = false;
                        }
                        else
                        {
                            curAlphaFadeEnd = curAlphaFadeStart;
                            ping = true;
                        }
                    }
                }
                if (curDestroyParrent)
                {
                    Destroy(gameObject);
                }
            }
        }

        void AnimPos()
        {
            this.transform.localPosition = Vector2.MoveTowards(this.transform.localPosition, endPosTarget, Time.deltaTime * speed);

            if (transform.localPosition.x == endPosTarget.x && transform.localPosition.y == endPosTarget.y)
            {
                if (AnimationDoneCallback != null)
                {
                    AnimationDoneCallback();
                }
                animating = false;
                if (!loop)
                {
                    animate = false;
                }
                else
                {
                    transform.localPosition = startPosTarget;
                }
                if (curDestroyParrent)
                {
                    Destroy(gameObject);
                }
            }
        }
        void AnimScale()
        {
            this.transform.localScale = Vector2.MoveTowards(this.transform.localScale, endPosTarget, Time.deltaTime * speed);

            if (transform.localScale.x == endPosTarget.x && transform.localScale.y == endPosTarget.y)
            {
                if (AnimationDoneCallback != null)
                {
                    AnimationDoneCallback();
                }
                animating = false;
                if (!loop)
                {
                    animate = false;
                }
                else
                {
                    transform.localScale = startPosTarget;
                }
                if (curDestroyParrent)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}