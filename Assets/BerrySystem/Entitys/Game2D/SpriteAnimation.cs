/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    public class SpriteAnimation
    {
        // Current animation name.
        public string animationName;
        // The current array of animations or "Frames"
        public AnimationFrame[] animationFrames;

        // The speed of the animation.
        public float animationSpeed = 1;

        // Auto loop the animation?
        public bool animationLoop = false;
    }

    public struct AnimationFrame
    {
        // the frame time is how long the frame will take in the animation. if it's zero, then we will assume it's same as animationSpeed.
        public float frameTime;

        // the sprite for this animation step.
        public Sprite frameSprite;
    }
}