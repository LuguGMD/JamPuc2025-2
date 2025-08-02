using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_menuPanels;

    public enum Panels
    {
        MainMenu,
        Settings,
        Credits
    }

    public void ChangePanel(int panel)
    {
        for (int i = 0; i < m_menuPanels.Count; i++)
        {
            m_menuPanels[i].SetActive(i == panel);
        }
    }
}
