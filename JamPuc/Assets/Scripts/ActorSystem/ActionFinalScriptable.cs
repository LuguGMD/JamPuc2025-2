using UnityEngine;

[CreateAssetMenu(fileName = "ActionFinalScriptable", menuName = "Scriptable Objects/ActionFinalScriptable")]
public class ActionFinalScriptable : ActionScriptable
{
    [TextArea] [SerializeField] private string[] m_finalTexts;

    #region Properties

    public string[] finalTexts
    {
        get => m_finalTexts;
        private set => m_finalTexts = value;
    }

    #endregion
}
