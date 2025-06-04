using UnityEngine;

public class SkillDrop : MonoBehaviour, ICollectable
{
    [SerializeField] private SkillType skillType;

    public void Collect()
    {
        SkillController.Instance.CollectSkill(skillType);
        gameObject.SetActive(false);
    }
}