using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private List<ActionScriptable> actions = new List<ActionScriptable>();
    private List<ActionScriptable> m_availableActions = new List<ActionScriptable>();

    private void Start()
    {
        UpdateAvailableActions(null);
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onActionStateChange += UpdateAvailableActions;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onActionStateChange -= UpdateAvailableActions;
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
    public void PlayActionDebug()
    {
        if(m_availableActions.Count > 0)
        {
            ActorManager.Instance.PlayAction(m_availableActions[0]);
        }
        
    }

}
