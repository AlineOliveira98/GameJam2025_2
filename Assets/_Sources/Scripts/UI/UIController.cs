using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Panel[] panels;

    private Dictionary<PanelType, Canvas> panelsDic = new();

    private PanelType currentPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        panelsDic.Clear();

        for (int i = 0; i < panels.Length; i++)
        {
            panelsDic.Add(panels[i].type, panels[i].canvas);
        }

        OpenPanel(PanelType.Menu);
    }

    public void OpenPanel(PanelType type)
    {
        if (currentPanel == type) return;

        panelsDic[currentPanel].enabled = false;

        currentPanel = type;
        panelsDic[currentPanel].enabled = true;
    }
}

[Serializable]
public class Panel
{
    public PanelType type;
    public Canvas canvas;
}

public enum PanelType
{
    Menu,
    Settings,
    Gameplay,
    Dialogue
}