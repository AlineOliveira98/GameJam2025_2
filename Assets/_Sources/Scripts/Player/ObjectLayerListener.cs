using UltEvents;
using UnityEngine;

public class ObjectLayerListener : MonoBehaviour
{
    public UltEvent OnPlayerEnteredLayer;
    public UltEvent OnPlayerExitLayer;

    [SerializeField] private int objectLayer;
    void Awake()
    {
        // objectLayer = gameObject.layer;
    }

    void OnEnable()
    {
        PlayerLayerController.OnPlayerLayerChanged += Checklayer;
    }

    void OnDisable()
    {
        PlayerLayerController.OnPlayerLayerChanged -= Checklayer;
    }

    void Checklayer(int currentPlayerMask)
    {
        if (currentPlayerMask == objectLayer)
        {
            OnPlayerEnteredLayer?.Invoke();
        }
        else
        {
            OnPlayerExitLayer?.Invoke();
        }
    }
}
