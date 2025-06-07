using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartEmpty;

    [Header("Skills")]
    [SerializeField] private SkillData[] skills;

    private Dictionary<SkillType, SkillUI> skillsDic = new();

    void OnEnable()
    {
        PlayerHealth.OnPlayerHealthChanged += UpdateHealth;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerHealthChanged -= UpdateHealth;
    }

    void Start()
    {
        foreach (var item in skills)
        {
            item.skillUI.SetVisual(false);
            skillsDic.Add(item.type, item.skillUI);
        }

        skillsDic[SkillType.Dash].SetVisual(true);
    }

    private void UpdateHealth(float currentHealth, float totalHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if ((i + 1) <= currentHealth)
                hearts[i].sprite = heartFull;
            else
                hearts[i].sprite = heartEmpty;
        }
    }
}

[Serializable]
public class SkillData
{
    public SkillType type;
    public SkillUI skillUI;
}
