using UnityEngine;

public class TargetTexture : MonoBehaviour
{
    [Tooltip("Must match the Rings value on TargetScore.")]
    [SerializeField] private int rings = 10;

    [SerializeField] private int textureSize = 512;
    [SerializeField] private Color bullseyeColor = new Color(0.85f, 0.1f, 0.1f);
    [SerializeField] private Color ringColorA = Color.white;
    [SerializeField] private Color ringColorB = new Color(0.1f, 0.1f, 0.1f);
    [SerializeField] private Color lineColor = Color.black;

    [Tooltip("Thickness of the lines between rings (0 to 0.05).")]
    [SerializeField] private float lineThickness = 0.01f;

    private void Awake()
    {
        Renderer targetRenderer = GetComponent<Renderer>();
        if (targetRenderer == null)
        {
            Debug.LogWarning("TargetTexture needs a Renderer on the same object.");
            return;
        }

        targetRenderer.material.mainTexture = BuildTexture();
    }

    private Texture2D BuildTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, true);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        float center = (textureSize - 1) * 0.5f;
        Color[] pixels = new Color[textureSize * textureSize];

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float dx = x - center;
                float dy = y - center;
                float ratio = Mathf.Sqrt(dx * dx + dy * dy) / center;

                Color pixel;
                if (ratio >= 1f)
                {
                    pixel = ringColorB;
                }
                else
                {
                    float scaled = ratio * rings;
                    int ringIndex = Mathf.FloorToInt(scaled);
                    float distanceToEdge = (scaled - ringIndex) / rings;

                    if (distanceToEdge < lineThickness || (1f / rings) - distanceToEdge < lineThickness)
                    {
                        pixel = lineColor;
                    }
                    else if (ringIndex == 0)
                    {
                        pixel = bullseyeColor;
                    }
                    else if (ringIndex % 2 == 0)
                    {
                        pixel = ringColorA;
                    }
                    else
                    {
                        pixel = ringColorB;
                    }
                }

                pixels[y * textureSize + x] = pixel;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}