using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image cooldown;
    [SerializeField] private GameObject lockedState;

    private bool isCooldown;
    private float cooldownDuration;
    private float currentCooldown;

    public void SetVisual(bool enable)
    {
        lockedState.SetActive(!enable);
    }

    void Update()
    {
        if (!isCooldown) return;

        currentCooldown -= Time.deltaTime;
        cooldown.fillAmount = currentCooldown / cooldownDuration;

        if (currentCooldown <= 0)
        {
            isCooldown = false;
        }
    }

    public void UseSkill(float cooldownDuration)
    {
        this.cooldownDuration = cooldownDuration;
        currentCooldown = cooldownDuration;
        isCooldown = true;
    }
}
