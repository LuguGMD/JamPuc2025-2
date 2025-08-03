using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_dialogueText;
    [SerializeField] private GameObject m_dialoguePanel;

    private bool m_isFinalDialogue;

    private int m_dialogueIndex = 0;
    private List<string> m_dialogues = new List<string>();

    private void OnEnable()
    {
        ActionsManager.Instance.onDialogue += AddDialogue;
        ActionsManager.Instance.onLevelEnd += FinalDialogue;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onDialogue -= AddDialogue;
        ActionsManager.Instance.onLevelEnd -= FinalDialogue;
    }

    private void FinalDialogue()
    {
        m_isFinalDialogue = true;
    }

    public void AddDialogue(params string[] dialogueText)
    {
        m_dialogues = dialogueText.ToList();
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
        UpdateDialogue();
    }

    private void DisableDialogue()
    {
        m_dialoguePanel.SetActive(false);
        m_dialogueIndex = 0;

        if(m_isFinalDialogue)
        {
            SceneManager.LoadScene((int)Scenes.Game);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            PassDialogue();
        }
    }

}
