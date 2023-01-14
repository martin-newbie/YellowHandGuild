using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public List<RuntimeAnimatorController> charactersAnimators = new List<RuntimeAnimatorController>();
    public List<RuntimeAnimatorController> hostilesAnimators = new List<RuntimeAnimatorController>();

    public RuntimeAnimatorController GetCharacterAnimator(int index) => charactersAnimators[index];
    public RuntimeAnimatorController GetHostileAnimator(int index) => hostilesAnimators[index];

    private void Awake()
    {
        var char_temp = Resources.LoadAll<RuntimeAnimatorController>("Animators/Character");
        var char_order = char_temp.OrderBy((item) => int.Parse(item.name.Split('_')[0])).ToList();

        var host_temp = Resources.LoadAll<RuntimeAnimatorController>("Animators/Hostiles");
        var host_order = host_temp.OrderBy((item) => int.Parse(item.name.Split('_')[0])).ToList();

        charactersAnimators = new List<RuntimeAnimatorController>(char_order);
        hostilesAnimators = new List<RuntimeAnimatorController>(host_order);
    }
}
