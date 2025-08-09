using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_menuPanels;
    [SerializeField] private RawImage fade;

    public enum Panels
    {
        MainMenu,
        Credits,
        Settings,
    }

    private void Awake()
    {
        ChangePanel(Panels.MainMenu);
        fade.DOFade(0,0.3f);
    }

    public void ChangePanel(int panel)
    {
        ChangePanel((Panels)panel);
    }

    public void ChangePanel(Panels panel)
    {
        for (int i = 0; i < m_menuPanels.Count; i++)
        {
            m_menuPanels[i].SetActive(i == (int)panel);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public async void Play()
    {
        await fade.DOFade(1, 0.3f).SetUpdate(true).AsyncWaitForCompletion();
        SceneManager.LoadScene((int)Scenes.Tutorial);
    }
}
