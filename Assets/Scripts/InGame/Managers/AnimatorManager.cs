using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public List<RuntimeAnimatorController> charactersAnimators = new List<RuntimeAnimatorController>();
    public List<RuntimeAnimatorController> hostilesAnimators = new List<RuntimeAnimatorController>();

    public RuntimeAnimatorController GetCharacterAnimator(int index) => charactersAnimators[index];
    public RuntimeAnimatorController GetHostileAnimator(int index) => hostilesAnimators[index];
}
