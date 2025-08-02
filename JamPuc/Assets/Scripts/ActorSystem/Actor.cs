using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private List<ActionScriptable> m_actions = new List<ActionScriptable>();
    private List<ActionScriptable> m_availableActions = new List<ActionScriptable>();

    private bool m_doNeedLighting = false;

    #region Properties

    public List<ActionScriptable> availableActions
    {
        get => m_availableActions;
        private set => m_availableActions = value;
    }

    public List<ActionScriptable> actions
    {
        get => m_actions;
        private set => m_actions = value;
    }

    public bool hasAction
    {
        get => m_availableActions.Count > 0;
    }

    public bool doNeedLighting
    {
        get => m_doNeedLighting;
        set => m_doNeedLighting = value;
    }

    #endregion

    private void Start()
    {
        UpdateAvailableActions(null);
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onActionStateChange += UpdateAvailableActions;
        ActionsManager.Instance.onActorToggle?.Invoke(this, true);
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onActionStateChange -= UpdateAvailableActions;
        ActionsManager.Instance.onActorToggle?.Invoke(this, false);
    }

    private bool CheckActionDependencies(ActionScriptable action)
    {
        if (ActorManager.Instance.IsActionCompleted(action))
            return false;

        foreach (var condition in action.conditions)
        {
            if (condition.needToBeCompleted != ActorManager.Instance.IsActionCompleted(condition.action))
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateAvailableActions(ActionScriptable actionChanged)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            ActionScriptable action = actions[i];
            bool dependenciesMatch = CheckActionDependencies(action);
            bool contain = m_availableActions.Contains(action);
            if (dependenciesMatch && !contain)
            {
                m_availableActions.Add(action);
            }
            else if (!dependenciesMatch && contain)
            {
                m_availableActions.Remove(action);
            }
        }
    }

    [ContextMenu("Play Action Debug")]
    public void PlayAction()
    {
        if(m_availableActions.Count > 0)
        {
            ActorManager.Instance.PlayAction(m_availableActions[0]);
        }
        
    }

}
