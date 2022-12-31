using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileGameObject : PlayableObject
{
    public HostileAI ai;
    public int hostileIdx;
    bool isInit = false;

    public override AI_Base thisAI => ai;

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
                ai.Idle();
                break;
            case ECharacterState.MOVE:
                ai.MoveToTarget();
                if (ai.IsArriveAtTarget())
                {
                    state = ECharacterState.IDLE;
                }
                break;
            case ECharacterState.ATTACK:
                ai.Attack();
                break;
            case ECharacterState.KNOCK_BACK:
                ai.Knockback();
                break;
            case ECharacterState.STUN:
                break;
            case ECharacterState.DEAD:
                ai.Dead();
                break;
        }
    }

    public void HostileInit(int index)
    {
        hostileIdx = index;

        switch (index)
        {
            case 0:
                ai = new Skeleton(this);
                break;
            case 1:
                break;
            case 2:
                ai = new SkeletonCrossbow(this);
                break;
            default:
                break;
        }

        isInit = true;
    }

    public override void GiveKnockback(float pushed, int dir)
    {
        base.GiveKnockback(pushed, dir);
        ai.Cancel();
    }
}
