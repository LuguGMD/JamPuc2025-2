using System;
using UnityEngine;

public class ReactionVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem happyMask;
    [SerializeField] ParticleSystem sadMask;
    private void OnEnable()
    {
        ActionsManager.Instance.onReactionTrigger += HappyMask;
    }
    private void OnDisable()
    {
        ActionsManager.Instance.onReactionTrigger -= HappyMask;        
    }

    private void HappyMask(ReactionType type)
    {
        switch (type)
        {
            case ReactionType.Good:
                happyMask.Play();
                break;
            case ReactionType.Bad:
                sadMask.Play();
                break;
            case ReactionType.Neutral:
                happyMask.Play();
                sadMask.Play();
                break;
        }
    }
}
