using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_dialogueText;
    [SerializeField] private GameObject m_dialoguePanel;

    private int m_dialogueIndex = 0;
    private string[] m_dialogues = new string[0];

    private void OnEnable()
    {
        ActionsManager.Instance.onDialogue += HandleDialogue;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onDialogue -= HandleDialogue;
    }

    public void HandleDialogue(params string[] dialogueText)
    {
        EnableDialogue();
    }

    private void PassDialogue()
    {
        m_dialogueIndex++;

        if(m_dialogueIndex >= m_dialogues.Length)
        {
            DisableDialogue();
            return;
        }

        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        m_dialogueText.text = m_dialogues[m_dialogueIndex];
    }

    private void EnableDialogue()
    {
        m_dialoguePanel.SetActive(true);
        m_dialogueIndex = 0;
    }

    private void DisableDialogue()
    {
        m_dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            PassDialogue();
        }
    }

}
