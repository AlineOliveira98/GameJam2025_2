using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PerCharacterGradientWithParabola : MonoBehaviour
{
    [Header("Cores")]
    public float hueSpeed = 0.5f;
    [Range(0f, 1f)] public float saturation = 1f;
    [Range(0f, 1f)] public float brightness = 1f;

    [Header("Curvatura")]
    [Tooltip("Altura máxima da curva no centro do texto")]
    public float curveStrength = 10f;

    private TMP_Text text;
    private TMP_TextInfo textInfo;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.ForceMeshUpdate();
        textInfo = text.textInfo;

        float time = Time.time * hueSpeed;
        float boundsWidth = text.bounds.size.x;
        float halfWidth = boundsWidth / 2f;

        float parabolaFactor = curveStrength / (halfWidth * halfWidth);

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            var charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            var vertices = textInfo.meshInfo[materialIndex].vertices;
            var colors = textInfo.meshInfo[materialIndex].colors32;

            Vector3 charMid = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2;

            float x = charMid.x;

            float curveY = -parabolaFactor * x * x + curveStrength;

            Vector3 offset = new Vector3(0, curveY, 0);

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] += offset;

            float hue = Mathf.Repeat(time + (i * 0.05f), 1f);
            Color32 c = Color.HSVToRGB(hue, saturation, brightness);

            for (int j = 0; j < 4; j++)
                colors[vertexIndex + j] = c;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
