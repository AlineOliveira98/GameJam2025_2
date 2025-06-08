using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextArcEffect : MonoBehaviour
{
    public float curveStrength = 10f;

    private TMP_Text text;
    private TMP_TextInfo textInfo;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        text.ForceMeshUpdate();
        textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            var charInfo = textInfo.characterInfo[i];
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;


            Vector3 offsetToMidBaseline = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2;
            float x0 = offsetToMidBaseline.x;

            float curveY = Mathf.Sin(x0 / text.bounds.size.x * Mathf.PI) * curveStrength;

            Vector3 offset = new Vector3(0, curveY, 0);

            for (int j = 0; j < 4; j++)
                vertices[vertexIndex + j] += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
