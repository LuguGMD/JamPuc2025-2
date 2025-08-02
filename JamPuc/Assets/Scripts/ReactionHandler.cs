using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ReactionHandler : MonoBehaviour
{
    [SerializeField] private float m_goodReactionForce;
    [SerializeField] private float m_badReactionForce;
    [SerializeField] private float m_reactionDuration;
    private Vector3 m_originalPosition;

    private RectTransform m_rectTransform;


    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_originalPosition = m_rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onReactionTrigger += HandleReaction;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onReactionTrigger -= HandleReaction;
    }

    private void HandleReaction(ReactionType type)
    {
        float force = m_goodReactionForce;

        switch(type)
        {
            case ReactionType.Good:
                force = m_goodReactionForce;
                break;
            case ReactionType.Bad:
                force = m_badReactionForce;
                break;
            case ReactionType.Neutral:
                force = (m_goodReactionForce) / 2;
                break;
        }

        Debug.Log("Reaction" + type);

        m_rectTransform.DOShakeAnchorPos(m_reactionDuration, force).OnComplete(() =>
        {
            m_rectTransform.anchoredPosition = m_originalPosition;
        });
    }
}
