using DG.Tweening;
using UnityEngine;

public class EndingUI : MonoBehaviour
{
    [SerializeField] private AudioSource monologueAudio;
    [SerializeField] RectTransform textMonologue;
    [SerializeField] private float finalPosY = 0f;
    [SerializeField] private float duration = 8f;

    void OnEnable()
    {
        GameController.OnGameEnding += AnimEndGame;
    }

    void OnDisable()
    {
        GameController.OnGameEnding -= AnimEndGame;
    }

    private void AnimEndGame()
    {
        // monologueAudio.Play();
        textMonologue.DOAnchorPosY(finalPosY, duration).SetEase(Ease.Linear);
    }

    void Update()
    {
    }
}
