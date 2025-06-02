using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private Texture2D highlightTexture;
    [SerializeField] private Texture2D clickedTexture;

    public virtual void Interact()
    {

    }

    public bool PlayerIsClose()
    {
        return Physics2D.OverlapCircle(transform.position, interactionRadius, 1 << 6);
    }

    public void HighLight(bool enable)
    {
        Cursor.SetCursor(enable ? highlightTexture : null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!PlayerIsClose()) return;
        
        Interact();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighLight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighLight(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Cursor.SetCursor(clickedTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Cursor.SetCursor(highlightTexture, Vector2.zero, CursorMode.Auto);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}