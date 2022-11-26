using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance = null;
    public static InGameManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public AttackColliderManager attackColManager;

    public Collider2D GetAttackCollider(int index, Transform target) => attackColManager.GetAttackCollider(index, target);
}
