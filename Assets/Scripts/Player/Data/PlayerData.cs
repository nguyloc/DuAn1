using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]

public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 5f;
    public int amountOfJumps = 1;

    [Header("check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
