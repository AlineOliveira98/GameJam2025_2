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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!PlayerIsClose()) return;
        
        Interact();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCursor(highlightTexture);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetCursor(null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(clickedTexture != null) SetCursor(clickedTexture);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetCursor(highlightTexture);
    }

    private void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}