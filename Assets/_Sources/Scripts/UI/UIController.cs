using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Panel[] panels;

    private Dictionary<PanelType, Canvas> panelsDic = new();

    private PanelType currentPanel;

    public GameObject settingsPanelObject;

    public GameObject exitGamePanelObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanelObject != null)
                settingsPanelObject.SetActive(!settingsPanelObject.activeSelf);
        }
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

    public void ToggleExitGamePanel()
    {
        if (exitGamePanelObject != null)
            exitGamePanelObject.SetActive(!exitGamePanelObject.activeSelf);
    }

    public void CloseExitGamePanel()
    {
        if (exitGamePanelObject != null)
            exitGamePanelObject.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();             
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
    Dialogue,
    Defeat,
    Victory
}