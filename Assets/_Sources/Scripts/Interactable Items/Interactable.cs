using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Texture2D highlightTexture;
    [SerializeField] private Texture2D clickedTexture;
    public virtual void Interact()
    {

    }

    public void HighLight(bool enable)
    {
        Cursor.SetCursor(enable ? highlightTexture : null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
}