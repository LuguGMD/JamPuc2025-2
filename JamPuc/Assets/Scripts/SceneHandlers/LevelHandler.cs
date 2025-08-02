using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private List<Image> m_stageImages;

    private float m_score;
    private float m_actionScore;

    [SerializeField] private float m_scorePerSecond;
    [SerializeField] private float m_scorePrecisionMultiplier;
    [SerializeField] private float m_scorePrecisionThreshold;

    private float m_maxScore;

    [Range(0f,1f)] [SerializeField] private float m_goodScorePercentage;
    [Range(0f,1f)] [SerializeField] private float m_badScorePercentage;

    private float m_actionDuration;

    private void OnEnable()
    {
        ActionsManager.Instance.onActionStart += ActionStart;
        ActionsManager.Instance.onActionEnd += ActionEnd;
        ActionsManager.Instance.onLightActor += ActorLighted;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onActionStart -= ActionStart;
        ActionsManager.Instance.onActionEnd -= ActionEnd;
        ActionsManager.Instance.onLightActor -= ActorLighted;
    }

    private void ActorLighted(Actor actor, float distance)
    {
        CalculatePoints(distance);
    }

    private void ActionStart()
    {
        HideImages();

        m_actionDuration = (float)ActorManager.Instance.playableDirector.duration;
        m_maxScore = m_actionDuration * m_scorePerSecond;
    }

    private void ActionEnd()
    {
        ShowImages();
        CalculateReaction();
    }

    private void CalculateReaction()
    {
        m_score += m_actionScore;
        float percentage = m_actionScore / m_maxScore;

        if (percentage <= m_badScorePercentage)
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Bad);
        }
        else if (percentage >= m_badScorePercentage)
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Good);
        }
        else
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Neutral);
        }

        m_actionScore = 0f;
    }

    private void CalculatePoints(float Distance)
    {
        float multiplier = 1f;
        if (Distance < m_scorePrecisionThreshold) multiplier = m_scorePrecisionMultiplier;

        m_actionScore += Time.deltaTime * m_scorePerSecond * multiplier;
    }

    private void ShowImages()
    {
        foreach (Image image in m_stageImages)
        {
            image.DOColor(Color.white, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    private void HideImages()
    {
        foreach (Image image in m_stageImages)
        {
            image.DOColor(Color.clear, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    public void ChangeScene(Scenes scene)
    {
        ChangeScene((int)scene);
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum ReactionType
{
    Good,
    Bad,
    Neutral
}