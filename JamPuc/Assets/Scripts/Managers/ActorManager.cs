using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class ActorManager : SingletonMono<ActorManager>
{
    private List<ActionScriptable> m_completedActions = new List<ActionScriptable>();
    private PlayableDirector m_playableDirector;

    private bool m_isTimelinePlaying = false;
    private bool m_canPlayAction = true;

    #region Properties

    public List<ActionScriptable> completedActions
    {
        get => m_completedActions;
        private set => m_completedActions = value;
    }

    public PlayableDirector playableDirector
    {
        get => m_playableDirector;
        private set => m_playableDirector = value;
    }

    public bool isTimelinePlaying
    {
        get => m_isTimelinePlaying;
        private set => m_isTimelinePlaying = value;
    }

    public bool canPlayAction
    {
        get => m_canPlayAction;
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        m_playableDirector.stopped += TimelineEnd;
    }

    private void OnDisable()
    {
        m_playableDirector.stopped -= TimelineEnd;
    }

    public bool IsActionCompleted(ActionScriptable action)
    {
        return m_completedActions.Contains(action);
    }

    public void PlayAction(ActionScriptable actionToPlay)
    {
        StartCoroutine(HandleAction(actionToPlay));
    }

    private IEnumerator HandleAction(ActionScriptable actionToPlay)
    {
        if (actionToPlay.timelineAsset == null)
        {
            Debug.LogWarning("Action " + actionToPlay.actionName + " has no timeline asset assigned.");
            yield break;
        }
        else
        {
            m_playableDirector.playableAsset = actionToPlay.timelineAsset;
            m_playableDirector.Play();
            m_isTimelinePlaying = true;
            m_canPlayAction = false;

            ActionsManager.Instance.onActionStart?.Invoke();
            yield return new WaitUntil(() => !m_isTimelinePlaying);
        }

        m_completedActions.Add(actionToPlay);
        ActionsManager.Instance.onActionStateChange?.Invoke(actionToPlay);

        yield return new WaitForSeconds(0.2f);
        m_canPlayAction = true;

        ActionsManager.Instance.onActionEnd?.Invoke();
        
        if(actionToPlay is ActionFinalScriptable)
        {
            ActionsManager.Instance.onLevelEnd?.Invoke();
            yield return new WaitForSeconds(2f);
            ActionsManager.Instance.onDialogue?.Invoke(((ActionFinalScriptable)actionToPlay).finalTexts);
        }
    }

    private void TimelineEnd(PlayableDirector director)
    {
        m_isTimelinePlaying = false;
    }
}
