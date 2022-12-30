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
            case ECharacterState.STAND_BY:
                break;
            case ECharacterState.ON_ACTION:
                break;
            case ECharacterState.IDLE:
                thisAI.Idle();
                break;
            case ECharacterState.MOVE:
                thisAI.MoveToTarget();
                if (thisAI.IsArriveAtTarget())
                {
                    state = ECharacterState.IDLE;
                }
                break;
            case ECharacterState.ATTACK:
                thisAI.Attack();
                break;
            case ECharacterState.KNOCK_BACK:
                thisAI.Knockback();
                break;
            case ECharacterState.STUN:
                break;
            case ECharacterState.DEAD:
                thisAI.Dead();
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

        hp = thisAI.hp;
        isInit = true;
    }

    public override void GiveKnockback(float pushed, int dir)
    {
        base.GiveKnockback(pushed, dir);
        thisAI.Cancel();
    }
}
