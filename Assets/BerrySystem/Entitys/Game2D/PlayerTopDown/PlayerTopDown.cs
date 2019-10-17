/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */
 
using UnityEngine;

namespace ConstruiSystem
{
    public class PlayerTopDown : EntityAgent
    {
        public GameObject playerCameraObj;
        BPlayerController targetPlayerController;

        public override void CreateEntity(string entityName = "PlayerObj")
        {
            // Setup the player EntityAgent & rigidbody
            // transform.eulerAngles = new Vector2(90, 0);
            entityObj = gameObject;
            entityRigidbody = entityObj.AddComponent<Rigidbody>();
            entityRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            entityRigidbody.freezeRotation = true;
            entityRigidbody.transform.SetParent(transform);
            entityRigidbody.useGravity = false;
            entityRigidbody.drag = 9;

            // 
            //entityBoxCollider = entityObj.AddComponent<BoxCollider>();
            //entityBoxCollider.size = new Vector3(0.24f,2,0.48f);

            entityFeetCollider = new GameObject("Feet").AddComponent<CapsuleCollider>();
            entityFeetCollider.gameObject.transform.SetParent(entityObj.transform);
            entityFeetCollider.transform.localPosition = new Vector3(0, 0, 0);
            entityFeetCollider.radius = 0.06f;
            entityFeetCollider.height = 2;
            entityFeetCollider.tag = "feet";

            // Setup a new camera obj
            playerCameraObj = CameraManager.SetupNewCamera(ScenePrimer.curPrimerComponent.primerParrentObj, "PlayerCamera", true, true, 1, new Color(), new Vector3(entityObj.transform.position.x, 2, entityObj.transform.position.z), new Vector3(90, 0, 0));
            playerCameraObj.AddComponent<OldZoom>();
            playerCameraObj.AddComponent<ObjectFollow>().target = transform;

            // Add the BPlayerController to the camera.
            targetPlayerController = SessionManager.CreateBPlayerController(playerCameraObj);
            targetPlayerController.ControllerUpdateMode(true);
            targetPlayerController.targetEntityAgent = this;
            targetPlayerController.targetRigidbody = entityRigidbody;
            targetPlayerController.targetCameraObj = playerCameraObj;
            targetPlayerController.targetCamera = playerCameraObj.GetComponent<Camera>();
            targetPlayerController.targetCamera.backgroundColor = MapDataConverter.ColToColor32(XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex].mapLayers[MapDataManager.mapDataCurrentLayer].bgColor);
            PlayerSetupTextures();
        }


        public void FixedUpdate()
        {

        }

        public virtual void PlayerSetupTextures()
        {
            entityBodyAnimator = new GameObject("PlayerBody").AddComponent<SpriteAnimator>();
            entityBodyAnimator.transform.SetParent(entityObj.transform);
            entityBodyAnimator.transform.localPosition = new Vector3(0, 0, 0);
            TextureManager.playerSprites = Resources.LoadAll<Sprite>("BerrySystem/Textures/Topdown/player");
            entityBodyAnimator.AnimationSprite.sprite = TextureManager.playerSprites[0];

            entityBodyAnimator.UpdateSpriteRendering(TextureManager.playerSprites[0]);

            entityBodyAnimator.gameObject.AddComponent<SpriteUpdater>().img = entityBodyAnimator.AnimationSprite;

            // Player Idle animation
            SpriteAnimation playerIdleAnim = new SpriteAnimation();
            playerIdleAnim.animationLoop = true;
            playerIdleAnim.animationName = "Idle";
            AnimationFrame[] idleAnim = new AnimationFrame[4];
            idleAnim[0].frameSprite = TextureManager.playerSprites[0];
            idleAnim[1].frameSprite = TextureManager.playerSprites[1];
            idleAnim[2].frameSprite = TextureManager.playerSprites[2];
            idleAnim[3].frameSprite = TextureManager.playerSprites[3];
            playerIdleAnim.animationFrames = idleAnim;
            EntityAddAnimation(playerIdleAnim);


            // Player Walking animation
            SpriteAnimation playerWalkingAnim = new SpriteAnimation();
            playerWalkingAnim.animationLoop = true;
            playerWalkingAnim.animationName = "Walking";
            playerWalkingAnim.animationSpeed = 12f;
            AnimationFrame[] walkingAnim = new AnimationFrame[8];
            walkingAnim[0].frameSprite = TextureManager.playerSprites[4];
            walkingAnim[1].frameSprite = TextureManager.playerSprites[5];
            walkingAnim[2].frameSprite = TextureManager.playerSprites[6];
            walkingAnim[3].frameSprite = TextureManager.playerSprites[7];
            walkingAnim[4].frameSprite = TextureManager.playerSprites[8];
            walkingAnim[5].frameSprite = TextureManager.playerSprites[9];
            walkingAnim[6].frameSprite = TextureManager.playerSprites[10];
            walkingAnim[7].frameSprite = TextureManager.playerSprites[11];
            playerWalkingAnim.animationFrames = walkingAnim;
            EntityAddAnimation(playerWalkingAnim);

            // Player back Idle animation
            SpriteAnimation playerBackIdleAnim = new SpriteAnimation();
            playerBackIdleAnim.animationLoop = true;
            playerBackIdleAnim.animationName = "Idle";
            playerBackIdleAnim.animationSpeed = 3f;
            AnimationFrame[] backIdleAnim = new AnimationFrame[4];
            backIdleAnim[0].frameSprite = TextureManager.playerSprites[12];
            backIdleAnim[1].frameSprite = TextureManager.playerSprites[13];
            backIdleAnim[2].frameSprite = TextureManager.playerSprites[14];
            backIdleAnim[3].frameSprite = TextureManager.playerSprites[15];
            playerBackIdleAnim.animationFrames = backIdleAnim;
            EntityAddAnimation(playerBackIdleAnim);

            // Player Back Walking animation
            SpriteAnimation backPlayerWalkingAnim = new SpriteAnimation();
            backPlayerWalkingAnim.animationLoop = true;
            backPlayerWalkingAnim.animationName = "backWalkingAnim";
            backPlayerWalkingAnim.animationSpeed = 12f;
            AnimationFrame[] backWalkingAnim = new AnimationFrame[8];
            backWalkingAnim[0].frameSprite = TextureManager.playerSprites[16];
            backWalkingAnim[1].frameSprite = TextureManager.playerSprites[17];
            backWalkingAnim[2].frameSprite = TextureManager.playerSprites[18];
            backWalkingAnim[3].frameSprite = TextureManager.playerSprites[19];
            backWalkingAnim[4].frameSprite = TextureManager.playerSprites[20];
            backWalkingAnim[5].frameSprite = TextureManager.playerSprites[21];
            backWalkingAnim[6].frameSprite = TextureManager.playerSprites[22];
            backWalkingAnim[7].frameSprite = TextureManager.playerSprites[23];
            backPlayerWalkingAnim.animationFrames = backWalkingAnim;
            EntityAddAnimation(backPlayerWalkingAnim);

            // Player Death animation
            entityShadow = new GameObject("PlayerShadow").AddComponent<SpriteRenderer>();


            entityShadow.transform.SetParent(entityRigidbody.transform);
            entityShadow.gameObject.AddComponent<SpriteUpdater>().img = entityShadow;
            entityShadow.transform.eulerAngles = new Vector2(90, 0);
            entityShadow.transform.localScale = new Vector2(0.87f, 1);
            entityShadow.transform.localPosition = new Vector3(0, 0, 0);
            entityShadow.sprite = Resources.Load<Sprite>("BerrySystem/Textures/Topdown/DropShadow");
            entityShadow.sortingOrder = 1;
        }

        public override void EntityDeath(bool ignoreHealth)
        {

        }

    }
}