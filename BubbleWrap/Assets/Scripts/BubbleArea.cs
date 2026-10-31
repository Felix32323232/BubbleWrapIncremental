using System.Collections.Generic;
using UnityEngine;

/// Zona donde viven las burbujas. Las genera y las reparte en una cuadrícula
/// que se adapta al tamaño de la zona: cuantas más burbujas, más pequeñas.
/// Va en el objeto ZonaBurbujas.
public class BubbleArea : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Bubble bubblePrefab;
    [SerializeField] ComboManager comboManager;

    [Header("Ajustes")]
    [SerializeField] int initialBubbles = 9;
    [Range(0.3f, 1f)]
    [SerializeField] float bubbleScale = 0.85f; // tamaño de la burbuja respecto a su celda

    RectTransform area;
    readonly List<Bubble> bubbles = new List<Bubble>();

    public int Count => bubbles.Count;

    void Awake()
    {
        area = (RectTransform)transform;
    }

    void Start()
    {
        SetBubbleCount(initialBubbles);
    }

    // --- Para la tienda ---

    /// Añade burbujas (por ejemplo, AddBubbles(1) al comprar una mejora).
    public void AddBubbles(int amount)
    {
        SetBubbleCount(bubbles.Count + amount);
    }

    /// Fija el número total de burbujas.
    public void SetBubbleCount(int count)
    {
        count = Mathf.Max(1, count);

        while (bubbles.Count < count)
        {
            Bubble b = Instantiate(bubblePrefab, area);
            b.Init(comboManager);
            bubbles.Add(b);
        }

        while (bubbles.Count > count)
        {
            int last = bubbles.Count - 1;
            Destroy(bubbles[last].gameObject);
            bubbles.RemoveAt(last);
        }

        Layout();
    }

    // --- Lo usa el ComboManager ---

    public void RestoreAll()
    {
        foreach (Bubble b in bubbles) b.Restore();
    }

    // --- Colocación ---

    void Layout()
    {
        int n = bubbles.Count;
        int cols = Mathf.CeilToInt(Mathf.Sqrt(n));
        int rows = Mathf.CeilToInt(n / (float)cols);

        Rect r = area.rect;
        float cell = Mathf.Min(r.width / cols, r.height / rows);
        float size = cell * bubbleScale;

        for (int i = 0; i < n; i++)
        {
            int row = i / cols;
            int col = i % cols;
            int inThisRow = (row == rows - 1) ? n - row * cols : cols; // centra la última fila

            float x = (col - (inThisRow - 1) / 2f) * cell;
            float y = ((rows - 1) / 2f - row) * cell;

            RectTransform rt = (RectTransform)bubbles[i].transform;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(size, size);
            rt.anchoredPosition = new Vector2(x, y);
        }
    }
}