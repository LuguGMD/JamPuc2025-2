using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_menuPanels;

    public enum Panels
    {
        MainMenu,
        Credits,
        Settings,
    }

    private void Awake()
    {
        ChangePanel(Panels.MainMenu);
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

    public void Play()
    {
        SceneManager.LoadScene((int)Scenes.Tutorial);
    }
}
