using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{

    public void Damage(float amount)
    {
        Debug.Log(amount + " Damage taken");
    }
}
