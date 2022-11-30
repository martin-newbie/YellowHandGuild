using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    List<ParticleSystem> effects = new List<ParticleSystem>();

    private void Start()
    {
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            effects.Add(item);
        }
    }

    public ParticleSystem PlayEffect(int index, Vector3 pos)
    {
        ParticleSystem result = effects[index];
        
        result.transform.position = pos;
        result.Play();

        return result;
    }
}
