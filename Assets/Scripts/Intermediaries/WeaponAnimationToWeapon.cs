using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Player.Weapons;


namespace Intermediaries.WeaponAnimationToWeapon
{
    public class WeaponAnimationToWeapon : MonoBehaviour
    {
        private Weapon weapon;

        private void Start()
        {
            weapon = GetComponentInParent<Weapon>();
        }

        private void AnimationFinishTrigger()
        {
            weapon.AnimationFinishTrigger();
        }

        private void AnimationStartMovementTrigger()
        {
            weapon.AnimationStartMovementTrigger();
        }

        private void AnimationStopMovementTrigger()
        {
            weapon.AnimationStopMovementTrigger();
        }

        private void AnimationTurnOffFlipTrigger()
        {
            weapon.AnimationTurnOffFlipTrigger();
        }

        private void AnimationTurnOnFlipTrigger()
        {
            weapon.AnimationTurnOnFlipTigger();
        }

        private void AnimationActionTrigger()
        {
            weapon.AnimationActionTrigger();
        }
    }
}