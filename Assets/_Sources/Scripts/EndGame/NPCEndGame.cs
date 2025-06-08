using UnityEngine;

public class NPCEndGame : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorOverrideController overrider;

    void Start()
    {
        if(overrider != null) anim.runtimeAnimatorController = overrider;
        anim.SetTrigger("Happy");
    }

    void OnEnable()
    {
        GameController.OnGameEnding += GameEnd;
    }

    void OnDisable()
    {
        GameController.OnGameEnding -= GameEnd;
    }

    private void GameEnd()
    {

    }
}
