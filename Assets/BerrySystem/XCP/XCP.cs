/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;

namespace ConstruiSystem
{
    // XCP or (Xnomoto Content Package) is the file where the game's actual content and resources is stored.
    [System.Serializable]
    public class XCP
    {
        // xcp Form Version is to check comparability.
        public string xcpGameName;
        public string xcpAuthor;
        public string xcpDescription;
        public float xcpFormVersion = 0;
        public int mainMapIndex;
        public MapData[] xpcMaps;
        public Png xcpIcon;
        public Png editorPlayerStart;
        public Png[] tileTextures;
        public Png[] spriteTextures;
        public Png[] itemTextures;
        public BEnt[] entities;
    }

    [System.Serializable]
    public struct Png
    {
        public Collider[] colliders;
        public float pixScale;
        public float sortPoint;
        public byte[] t;
    }

    [System.Serializable]
    public struct BEnt
    {
        public string entName;
        public short entID;
        public int entType;
        public float entHealth;
        public float entWalkingSpeed;
        public Png[] entSprites;
    }

    [System.Serializable]
    public struct CoEnt
    {
        public short entID;
        public V2 entPos;
    }

    [System.Serializable]
    public struct CoSpecial
    {
        public short entID;
        public V3 pos;
        public V3 scale;
        public V3 rot;
    }
    [System.Serializable]
    public struct CoCollider
    {
        public int colliderRot;
        public Vector2 colliderSize;
        public Vector2 colliderPos;
    }
}