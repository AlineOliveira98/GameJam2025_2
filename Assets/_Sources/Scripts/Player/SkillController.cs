using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public static SkillController Instance;

    [SerializeField] private SkillHandler[] skills;

    private Dictionary<SkillType, SkillHandler> skillsDic = new();

    public List<SkillHandler> CurrentSkills { get; private set; } = new();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < skills.Length; i++)
        {
            skillsDic.Add(skills[i].skillType, skills[i]);
        }

        ApplySkill(SkillType.Dash);
    }

    public void CollectSkill(SkillType skillType)
    {
        var skill = skillsDic[skillType];
        skill.currentAmount++;

        if (skill.currentAmount >= skill.amountToAcquire)
        {
            ApplySkill(skillType);
        }
    }

    private SkillHandler GetSkillHandler(SkillType skillType)
    {
        return skillsDic[skillType];
    }

    public void ApplySkill(SkillType skillType)
    {
        var skill = GetSkillHandler(skillType);

        if (CurrentSkills.Contains(skill)) return;

        CurrentSkills.Add(skill);
        skill.acquired = true;
    }
}

[Serializable]
public class SkillHandler
{
    public SkillType skillType;
    public int amountToAcquire;
    public int currentAmount;
    public bool acquired;
}

public enum SkillType
{
    Dash,
    Push,
    Invisible,
    Clone
}