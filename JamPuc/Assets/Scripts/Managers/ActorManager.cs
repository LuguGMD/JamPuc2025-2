using System.Collections.Generic;
using UnityEngine;

public class ActorManager : SingletonMono<ActorManager>
{
    private List<ActionScriptable> m_completedActions = new List<ActionScriptable>();

    #region Properties

    public List<ActionScriptable> completedActions
    {
        get => m_completedActions;
        private set => m_completedActions = value;
    }

    #endregion

    public bool IsActionCompleted(ActionScriptable action)
    {
        return m_completedActions.Contains(action);
    }

    public void PlayAction(ActionScriptable actionToPlay)
    {
        Debug.Log("Action Played: " + actionToPlay.actionName);

        m_completedActions.Add(actionToPlay);

        ActionsManager.Instance.onActionStateChange?.Invoke(actionToPlay);
    }
}
