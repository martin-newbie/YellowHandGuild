using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private static SpriteManager instance = null;
    public static SpriteManager Instance => instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] List<Sprite> SkillSprites = new List<Sprite>();
    public static Sprite GetSkillSprites(int index)
    {
        return instance.SkillSprites[index];
    }

    [SerializeField] List<Sprite> mapSprites = new List<Sprite>();
    public static Sprite GetMapSprite(int index)
    {
        return instance.mapSprites[index];
    }

    [SerializeField] List<Sprite> jobSprites = new List<Sprite>();
    public static Sprite GetJopSprite(EPosType type)
    {
        if (type == EPosType.NONE) return null;

        return instance.jobSprites[(int)type];
    }

    [SerializeField] List<Sprite> characterUnitSprite = new List<Sprite>();
    public static Sprite GetCharacterUnitSprite(int index)
    {
        return instance.characterUnitSprite[index];
    }
}
