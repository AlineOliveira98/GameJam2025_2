using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PerCharacterGradient : MonoBehaviour
{
    public float hueSpeed = 0.5f;
    public float saturation = 1f;
    public float brightness = 1f;

    private TMP_Text text;
    private TMP_TextInfo textInfo;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.ForceMeshUpdate();
    }

    void Update()
    {
        text.ForceMeshUpdate();
        textInfo = text.textInfo;

        float time = Time.time * hueSpeed;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            var charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            var vertices = textInfo.meshInfo[materialIndex].colors32;

            float hue = Mathf.Repeat(time + (i * 0.05f), 1f);
            Color32 c = Color.HSVToRGB(hue, saturation, brightness);

            vertices[vertexIndex + 0] = c;
            vertices[vertexIndex + 1] = c;
            vertices[vertexIndex + 2] = c;
            vertices[vertexIndex + 3] = c;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
