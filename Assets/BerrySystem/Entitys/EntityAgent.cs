/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using System;
using UnityEngine;

namespace ConstruiSystem
{
    public class EntityAgent : MonoBehaviour
    {
        // Entity stats
        public bool entityWalking = false;
        public bool entityWasWalking = false;
        public bool entityJumping = false;
        public bool entityIsUpDir = false;
        public bool entityDirUpdate = false;

        public bool entityIsDead = false;
        public bool entityOnFire = false;
        public float entityStamina = 100;
        public float entityAir = 100;
        public float entityHealth = 100;
        public float entityMana = 100;
        public float entityMass = 10;

        public float range = 0.3f;


        // Object refs
        public GameObject entityObj;

        // Comp refs
        public Rigidbody entityRigidbody;
        public SpriteRenderer entityShadow;
        public BoxCollider entityBoxCollider;
        public CapsuleCollider entityFeetCollider;
        public EntityInventory entityInventory;

        // Entity sprite animators
        public SpriteAnimator entityBodyAnimator;
        public SpriteAnimator[] entitySpriteAnimators;
        public SpriteAnimation[] entityAnimations;

        // Create Entity.
        public virtual void CreateEntity(string entityName = "New Entity")
        {
            // Setup the player EntityAgent & rigidbody
            entityRigidbody = gameObject.AddComponent<Rigidbody>();
            entityRigidbody.transform.SetParent(transform);
            entityRigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            entityRigidbody.useGravity = false;
        }

        public void Start()
        {
            if (entityBodyAnimator != null)
            {
                if (entityAnimations != null)
                {
                    // on spawn play the very first anim in the list, deafult the idle animation to be on 0.
                    entityBodyAnimator.AnimationPlay(entityAnimations[0]);
                }
            }
        }

        // Unity update
        public void Update()
        {
            if (entityRigidbody.velocity.x != 0)
            {
                if (entityRigidbody.velocity.x <= 0)
                {
                    entityBodyAnimator.AnimationSprite.flipX = true;
                }
                else
                {
                    entityBodyAnimator.AnimationSprite.flipX = false;
                }
            }
        }

        // This takes controll of the Entity
        public void EntityAttachPlayerController()
        {
            
        }

        // Entity attack system.
        public virtual void EntityPrimaryAttack()
        {

        }

        public virtual void EntitySecondaryAttack()
        {
            
        }

        // Movement
        public void EntityMove(Vector3 direction, int power = 700)
        {
            if (direction == Vector3.zero)
            {
                entityWalking = false;
                EntityUpdateAnimState();
                entityDirUpdate = true;
            }
            else
            {
                entityWalking = true;
                entityDirUpdate = true;

                if (direction.z < 0)
                {
                    entityIsUpDir = false;
                }
                if (direction.z > 0)
                {
                    entityIsUpDir = true;
                }
                EntityUpdateAnimState();
                direction.Normalize();
                entityRigidbody.AddForce(direction * power * Time.deltaTime, ForceMode.Force);
            }
        }

        public virtual void EntityJump()
        {
            if (!entityJumping)
            {
                entityJumping = true;
            }
        }

        public virtual void EntityAttack(Vector3 direction, int damageAmount, float movementCharge)
        {
            Ray ray = CameraManager.curCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.GetComponent<EntityAgent>() != null)
                {
                    //Debug.Log(hit.transform.gameObject.name);
                    EntityAgent target = hit.transform.GetComponent<EntityAgent>();
                    Debug.DrawRay(target.transform.position, CameraManager.curCamera.ScreenToWorldPoint(Input.mousePosition), Color.green, 30);
                    target.entityRigidbody.AddForce(new Vector3(transform.localPosition.x * -6, 0, transform.localPosition.y * -6), ForceMode.Impulse);
                    target.EntityReceiveDamage(damageAmount);
                    target.EntityWobble(32);
                }
            }
        }

        public virtual void EntityCharge(Vector3 direction, float chargeAmount)
        {
            entityRigidbody.AddForce(direction * chargeAmount, ForceMode.Impulse);
            EntityUpdateAnimState();

        }

        public virtual void EntityWobble(int wobbleAmount)
        {

        }

        public void EntityAddAnimation(SpriteAnimation newAnim)
        {
            if (entityAnimations == null)
            {
                entityAnimations = new SpriteAnimation[1];
                entityAnimations[0] = newAnim;
            }
            else
            {
                Array.Resize(ref entityAnimations, entityAnimations.Length + 1);
                entityAnimations[entityAnimations.Length - 1] = newAnim;
            }
        }

        // Animation state
        public void EntityUpdateAnimState()
        {
            entityWasWalking = entityWalking;
            if (entityWalking)
            {
                if (entityDirUpdate)
                {
                    entityDirUpdate = false;
                    if (!entityIsUpDir)
                    {
                        entityBodyAnimator.AnimationPlay(entityAnimations[1]);
                    }
                    else
                    {
                        entityBodyAnimator.AnimationPlay(entityAnimations[3]);
                    }
                }
            }
            else
            {
                if (entityDirUpdate)
                {
                    if (!entityIsUpDir)
                    {
                        entityBodyAnimator.AnimationPlay(entityAnimations[0]);
                    }
                    else
                    {
                        entityBodyAnimator.AnimationPlay(entityAnimations[2]);
                    }
                }
            }
        }

        // Entity health system.
        public virtual void EntityReceiveDamage(float damageAmount)
        {
            if (damageAmount > 0) { EntityReceiveHealth(damageAmount); }
            else { }
        }

        public virtual void EntityReceiveHealth(float healthAmount)
        {
            if (healthAmount < 0) { EntityReceiveDamage(healthAmount); }
            else { }
        }

        public virtual void EntityDeath(bool ignoreHealth = false)
        {
            if (!entityIsDead) { if (ignoreHealth) { } else { } }
        }

        public virtual void EntityCatchOnFire()
        { 

        }

        public virtual void EntityPutOutFire() { }

        public virtual void EntityPanic() { }
        public virtual void EntityGrabObject() { }
    }
}