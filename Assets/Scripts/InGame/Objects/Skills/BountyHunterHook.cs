using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterHook : SkillBase
{
    [SerializeField] float duration = 0.5f;
    [SerializeField] float originX = 1.14f;
    [SerializeField] float originY = 1.12f;

    [SerializeField] BoxCollider2D posCol; // 갈고리가 위치에 도착했을 때 타겟이 있는지 판별
    [SerializeField] Transform rotContainer; // 체인 각도 조절

    [SerializeField] SpriteRenderer chainSprite;

    [SerializeField] ContactFilter2D filter2D;

    PlayableObject targeted;
    bool success;

    CharacterGameObject subject;
    public Coroutine HookThrow(PlayableObject target, CharacterGameObject _subject)
    {
        targeted = target;
        subject = _subject;
        return StartCoroutine(ThrowCoroutine());
    }
    float distance;
    IEnumerator ThrowCoroutine()
    {
        posCol.transform.position = targeted.transform.position + new Vector3(0, 1, 0);

        Vector2 dir = rotContainer.position - targeted.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
        rotContainer.rotation = angleAxis;

        distance = Vector3.Distance(transform.position, targeted.transform.position) * 2f;
        float timer = 0f;
        while (timer <= duration)
        {
            float sizeX = Mathf.Lerp(originX, distance, easeOutSine(timer / duration));
            Vector2 size = new Vector2(sizeX, originY);

            chainSprite.size = size;

            timer += Time.deltaTime;
            yield return null;
        }

        List<Collider2D> result = new List<Collider2D>();
        Physics2D.OverlapCollider(posCol, filter2D, result);
        success = result.Count > 0;
        HostileGameObject target = null;
        if (success)
        {
            target = result[0].GetComponent<HostileGameObject>();
            target.SetKnockback();
            target.ai.SetRotation(target.transform.position, subject.transform.position);
        }
        targeted = target;

        float easeOutSine(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2);
        }
    }

    public Coroutine HookPull()
    {
        return StartCoroutine(PullCoroutine());
    }

    IEnumerator PullCoroutine()
    {
        float timer = duration;
        while (timer >= 0f)
        {
            float sizeX = Mathf.Lerp(originX, distance, easeInCubic(timer / duration));
            Vector2 size = new Vector2(sizeX, originY);

            chainSprite.size = size;

            if (success)
                targeted.transform.position = Vector3.Lerp(subject.transform.position + new Vector3(1, 0), posCol.transform.position - new Vector3(0, 1, 0), easeInCubic(timer / duration));

            timer -= Time.deltaTime;
            yield return null;
        }

        yield break;

        float easeInCubic(float x)
        {
            return Mathf.Pow(x, 3);
        }
    }

    public PlayableObject GetFinalTarget()
    {
        return targeted;
    }
}
