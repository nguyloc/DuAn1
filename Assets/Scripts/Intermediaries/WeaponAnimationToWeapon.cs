using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Player.Weapons;

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

}
