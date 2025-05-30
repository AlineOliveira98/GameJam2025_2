using System;
using UnityEngine;

public class PlayerLayerController : MonoBehaviour
{
    private const int GroundLayers = 1 << 29 | 1 << 30 | 1 << 31;

    [SerializeField] private LayerMask mask;

    private int currentLayer;

    public LayerMask Mask => mask;

    public static Action<int> OnPlayerLayerChanged;

    void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1.0f, GroundLayers);

        if (hits.Length > 0)
        {
            RaycastHit2D highestLayerHit = hits[0];
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.layer > highestLayerHit.collider.gameObject.layer)
                {
                    highestLayerHit = hit;
                }
            }

            var newLayer = highestLayerHit.collider.gameObject.layer;

            if (newLayer != currentLayer)
            {
                OnPlayerLayerChanged?.Invoke(newLayer);

                currentLayer = newLayer;
                mask = 1 << currentLayer;
            }
        }
    }
}
