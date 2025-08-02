using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "ActionScriptable", menuName = "Scriptable Objects/ActionScriptable")]
public class ActionScriptable : ScriptableObject
{
    [SerializeField] private string m_actionName;
    [SerializeField] private List<Condition> m_conditions = new List<Condition>();
    [SerializeField] private TimelineAsset m_timelineAsset;

    #region Properties

    public string actionName
    {
        get => m_actionName;
        private set => m_actionName = value;
    }
    public List<Condition> conditions
    {
        get => m_conditions;
        private set => m_conditions = value;
    }

    public TimelineAsset timelineAsset
    {
        get => m_timelineAsset;
        private set => m_timelineAsset = value;
    }

    #endregion
}

[System.Serializable]
public class Condition
{
    [SerializeField] private ActionScriptable m_action;
    [SerializeField] private bool m_needToBeCompleted = true;

    //NEEDS TIMELINE ANIMATION TO PLAY

    #region Properties

    public ActionScriptable action
    {
        get => m_action;
        private set => m_action = value;
    }

    public bool needToBeCompleted
    {
        get => m_needToBeCompleted;
        private set => m_needToBeCompleted = value;
    }

    #endregion
}