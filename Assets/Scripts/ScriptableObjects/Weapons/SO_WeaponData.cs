using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon")]
    public class SO_WeaponData : ScriptableObject
    {
        public float[] movementSpeed;
    }
}

