using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEvents : MonoBehaviour
{
    public UnityEvent PlayStep;
    public void PlayStepSound()
    {
        PlayStep.Invoke();
    }
}
