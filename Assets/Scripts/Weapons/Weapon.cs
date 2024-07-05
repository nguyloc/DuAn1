using Player.PlayerStates.SubStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons
{
    public class Weapon : MonoBehaviour
    {
        protected Animator baseAnimator;
        protected Animator weaponAnimator;

        protected PlayerAttackState state;

        protected virtual void Start()
        {
            baseAnimator = transform.Find("Base").GetComponent<Animator>();
            weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

            gameObject.SetActive(false);
        }
        public virtual void EnterWeapon()
        {

            gameObject.SetActive(true);
            baseAnimator.SetBool("attack", true);
            weaponAnimator.SetBool("attack", true);
        }

        public virtual void ExitWeapon()
        {
            gameObject.SetActive(false);
            baseAnimator.SetBool("attack", false);
            weaponAnimator.SetBool("attack", false);
        }

        #region Animation Triggers

        public virtual void AnimationFinishTrigger()
        {
            state.AnimationFinishTrigger();
        }


        #endregion

        public void InitializeWeapon(PlayerAttackState state)
        {
            this.state = state;

        }
    }
}

   
