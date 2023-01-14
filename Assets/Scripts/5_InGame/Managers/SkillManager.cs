using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] List<SkillBase> skillObjects = new List<SkillBase>();

    public SkillBase GetSkillObject(int index)
    {
        return skillObjects[index];
    }

    public SkillBase SpawnSkillObject(int index, Transform trans = null)
    {
        return Instantiate(skillObjects[index], trans);
    }

    public SkillBase SpawnSkillObjectByPosition(int index, Vector3 pos, Transform trans = null)
    {
        return Instantiate(skillObjects[index], pos, Quaternion.identity, trans);
    }
}
