using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusketeerReposte : SkillBase
{

    ParticleSystem particle;

    public void InitReposteEffect(Vector3 start, Vector3 end, Action endAction = null)
    {
        particle = GetComponent<ParticleSystem>();

        float duration = Vector3.Distance(start, end) * 0.01f;
        StartCoroutine(ReposteMovement(start, end, duration, endAction));
    }

    IEnumerator ReposteMovement(Vector3 start, Vector3 end, float duration, Action endAction = null)
    {
        float timer = 0f;
        particle.Play();

        while (timer <= duration)
        {
            transform.position = Vector3.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        particle.Stop();
        endAction?.Invoke();

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield break;
    }
}
