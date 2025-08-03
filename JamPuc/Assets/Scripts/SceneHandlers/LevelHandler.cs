using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine.Timeline;

public class LevelHandler : MonoBehaviour
{

    [SerializeField] private CinemachineCamera m_camera;
    [SerializeField] private List<Animator> m_curtainAnimators;

    [SerializeField] private TimelineAsset m_timelineAsset;

    [Header("UI")]
    [SerializeField] private List<Image> m_stageImages;
    [SerializeField] private Slider m_slider;


    private float m_score;
    private float m_actionScore;

    [Header("Score")]
    [SerializeField] private float m_scoreNotPrecisionMultiplier;
    [SerializeField] private float m_scorePrecisionThreshold;

    private float m_maxScore;

    [Range(0f,1f)] [SerializeField] private float m_goodScorePercentage;
    [Range(0f,1f)] [SerializeField] private float m_badScorePercentage;

    
    

    private void OnEnable()
    {
        ActionsManager.Instance.onActionStart += ActionStart;
        ActionsManager.Instance.onActionEnd += ActionEnd;
        ActionsManager.Instance.onLightActor += ActorLighted;
        ActionsManager.Instance.onLevelEnd += EndLevel;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onActionStart -= ActionStart;
        ActionsManager.Instance.onActionEnd -= ActionEnd;
        ActionsManager.Instance.onLightActor -= ActorLighted;
        ActionsManager.Instance.onLevelEnd -= EndLevel;
    }

    #region Score

    private void ActorLighted(Actor actor, float distancePercentage)
    {
        CalculatePoints(distancePercentage);
    }

    private void ActionStart()
    {
        HideImages();

        m_maxScore = (float)ActorManager.Instance.playableDirector.duration;

        m_slider.value = 0;
        m_slider.gameObject.SetActive(true);
    }

    private void ActionEnd()
    {
        ShowImages();
        CalculateReaction();

        m_slider.gameObject.SetActive(false);
    }

    private void CalculateReaction()
    {
        m_score += m_actionScore;
        float percentage = m_actionScore / m_maxScore;

        if (percentage <= m_badScorePercentage)
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Bad);
        }
        else if (percentage >= m_goodScorePercentage)
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Good);
        }
        else
        {
            ActionsManager.Instance.onReactionTrigger?.Invoke(ReactionType.Neutral);
        }

        m_actionScore = 0f;
    }

    private void CalculatePoints(float distancePercentage)
    {
        float multiplier = 1f;
        if (distancePercentage > m_scorePrecisionThreshold) multiplier = m_scoreNotPrecisionMultiplier;

        m_actionScore += Time.deltaTime * multiplier;

        float percentage = m_actionScore / m_maxScore;
        m_slider.value = percentage;
    }

    #endregion

    public void ShakeCamera(float intensity)
    {
        Vector3 originalPos = m_camera.transform.position;
        m_camera.transform.DOShakePosition(0.5f, intensity).OnComplete(()=>
        {
            m_camera.transform.position = originalPos;
        });
    }

    private void ShowImages()
    {
        foreach (Image image in m_stageImages)
        {
            if(image != null)
                image.DOColor(Color.white, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    private void HideImages()
    {
        foreach (Image image in m_stageImages)
        {
            if (image != null)
                image.DOColor(Color.clear, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    #region Scene

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

    #endregion

    public void EndLevel()
    {
        ShowImages();

        foreach (Animator animator in m_curtainAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger("End");
            }
        }
    }
}

public enum ReactionType
{
    Good,
    Bad,
    Neutral
}