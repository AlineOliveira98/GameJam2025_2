using UnityEngine;

public class YggDrasil : MonoBehaviour
{
    [SerializeField] private GameObject[] stages;

    private int currentStage;

    void Start()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }

        stages[0].SetActive(true);

        currentStage = 0;
    }

    private void Grow()
    {
        if (currentStage >= stages.Length-1) return;

        stages[currentStage].SetActive(false);
        stages[currentStage+1].SetActive(true);
        currentStage++;
    }
}
