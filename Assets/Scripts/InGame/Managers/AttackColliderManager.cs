using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderManager : MonoBehaviour
{
    public Collider2D[] attackColliders;

    public Collider2D GetAttackCollider(int index, Transform target)
    {
        return Instantiate(attackColliders[index], target);
    }
}
