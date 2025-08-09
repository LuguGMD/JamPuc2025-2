using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialHandler : MonoBehaviour
{
    public enum Dialogue
    {
        Intro,
        MoveSpotlight,
        ActorSelection,
        ActorAction,
        End
    }

    [SerializeField] private List<Animator> m_curtains;
    [SerializeField] private LightController m_lightController;

    [SerializeField] private Actor m_knight;
    [SerializeField] private RawImage fade;

    private bool m_isDialogueFinished = false;

    [SerializeField] private List<TutorialDialogue> m_introDialogue;

    private void Start()
    {
        StartCoroutine(Tutorial());
        fade.DOFade(0,1.6f).SetUpdate(true);
    }

    private void OnEnable()
    {
        ActionsManager.Instance.onDialogueEnd += DialogueEnd;
    }

    private void OnDisable()
    {
        ActionsManager.Instance.onDialogueEnd -= DialogueEnd;
    }

    private void DialogueEnd()
    {
        m_isDialogueFinished = true;
    }

    private void OpenCurtains()
    {
        for (int i = 0; i < m_curtains.Count; i++)
        {
            m_curtains[i].enabled = true;
        }
    }

    private void CloseCurtains()
    {
        for (int i = 0; i < m_curtains.Count; i++)
        {
            m_curtains[i].SetTrigger("End");
        }
    }

    private IEnumerator Tutorial()
    {
        ActorManager.Instance.canPlayAction = false;

        yield return HandleDialogue(Dialogue.Intro);
        OpenCurtains();
        yield return new WaitForSeconds(2f);
        yield return HandleDialogue(Dialogue.MoveSpotlight);
        m_lightController.ChangeState(LightController.Control.Mouse);
        yield return new WaitForSeconds(5f);

        m_lightController.ChangeSelectedActor(m_knight, true);
        m_lightController.ChangeState(LightController.Control.Actor);
        m_lightController.canChangeState = false;
        yield return HandleDialogue(Dialogue.ActorSelection);

        m_lightController.canChangeState = true;
        m_lightController.ChangeState(LightController.Control.Mouse);

        yield return new WaitForSeconds(1f);

        yield return HandleDialogue(Dialogue.ActorAction);

        m_lightController.ChangeSelectedActor(m_knight, true);
        ActorManager.Instance.canPlayAction = true;

        yield return new WaitUntil(() => ActorManager.Instance.canPlayAction == false);
        yield return new WaitUntil(() => ActorManager.Instance.canPlayAction == true);

        yield return new WaitForSeconds(0.5f);
        CloseCurtains();
        yield return new WaitForSeconds(1.5f);

        yield return HandleDialogue(Dialogue.End);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene((int)Scenes.Game);
    }

    private IEnumerator HandleDialogue(Dialogue index)
    {
        ActionsManager.Instance.onDialogue?.Invoke(m_introDialogue[(int)index].text);
        yield return new WaitUntil(() => m_isDialogueFinished);
        m_isDialogueFinished = false;
    }


}

[System.Serializable]
public class TutorialDialogue
{
    public string m_tutorialDialogueName;
    [TextArea] public string[] text;
}