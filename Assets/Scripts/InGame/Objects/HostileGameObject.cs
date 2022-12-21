using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileGameObject : PlayableObject
{
    public HostileAI thisAI;
    public int hostileIdx;
    bool isInit = false;

    protected override void Start()
    {
        base.Start();
        // debug
        HostileInit(hostileIdx);
    }

    private void Update()
    {
        if (!isInit) return;

        switch (state)
        {
            case CharacterState.STAND_BY:
                break;
            case CharacterState.ON_ACTION:
                break;
            case CharacterState.IDLE:
                thisAI.Idle();
                break;
            case CharacterState.MOVE:
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    state = CharacterState.ATTACK;
                }
                break;
            case CharacterState.ATTACK:
                thisAI.Attack();
                break;
            case CharacterState.KNOCK_BACK:
                thisAI.Knockback();
                break;
            case CharacterState.STUN:
                break;
            case CharacterState.DEAD:
                break;
        }
    }

    public void HostileInit(int index)
    {
        hostileIdx = index;

        switch (index)
        {
            case 0:
                thisAI = new Skeleton(this);
                break;
            case 1:
                break;
            case 2:
                thisAI = new SkeletonCrossbow(this);
                break;
            default:
                break;
        }

        isInit = true;
    }

}
