using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartEmpty;

    [Header("Skills")]
    [SerializeField] private Image[] skills;

    void OnEnable()
    {
        PlayerHealth.OnPlayerHealthChanged += UpdateHealth;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerHealthChanged -= UpdateHealth;
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
