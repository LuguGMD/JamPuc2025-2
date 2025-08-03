using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_dialogueText;
    [SerializeField] private GameObject m_dialoguePanel;

    private int m_dialogueIndex = 0;
    private List<string> m_dialogues = new List<string>();

    private void OnEnable()
    {
        ActionsManager.Instance.onDialogue += AddDialogue;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onDialogue -= AddDialogue;
    }

    public void AddDialogue(params string[] dialogueText)
    {
        EnableDialogue();
    }

    public void AddDialogue(string dialogue)
    {
        m_dialogues.Add(dialogue);
        EnableDialogue();
    }

    private void PassDialogue()
    {
        m_dialogueIndex++;

        if(m_dialogueIndex >= m_dialogues.Count)
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
    }

    private void DisableDialogue()
    {
        m_dialoguePanel.SetActive(false);
        m_dialogueIndex = 0;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            PassDialogue();
        }
    }

}
