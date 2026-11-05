using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // NUEVO

/// Zona donde viven las burbujas. Las genera, las reparte en una cuadrícula
/// y gestiona los clicks (un click puede reventar varias burbujas).
/// Va en el objeto ZonaBurbujas.
public class BubbleArea : MonoBehaviour, IPointerDownHandler // NUEVO: IPointerDownHandler
{
    [Header("Referencias")]
    [SerializeField] Bubble bubblePrefab;
    [SerializeField] ComboManager comboManager;

    [Header("Ajustes")]
    [SerializeField] int initialBubbles = 9;
    [Range(0.3f, 1f)]
    [SerializeField] float bubbleScale = 0.85f; // tamaño de la burbuja respecto a su celda

    [Header("Click")] // NUEVO
    // Radio EXTRA del click, en unidades de canvas. 0 = solo revienta la burbuja pulsada.
    // La mejora "tamano_click" modifica este valor.
    public float clickRadius = 0f;

    RectTransform area;
    readonly List<Bubble> bubbles = new List<Bubble>();

    public int Count => bubbles.Count;
    public int RemainingBubbles
    {
        get
        {
            int n = 0;
            foreach (Bubble b in bubbles) if (!b.IsPopped) n++;
            return n;
        }
    }

    void Awake()
    {
        area = (RectTransform)transform;
    }

    void Start()
    {
        SetBubbleCount(initialBubbles);
    }

    // --- Click (NUEVO) ---

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // Posición del click en el espacio local de la zona
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                area, eventData.position, eventData.pressEventCamera, out Vector2 click)) return;

        foreach (Bubble b in bubbles)
        {
            if (b.IsPopped) continue;

            RectTransform rt = (RectTransform)b.transform;
            Vector2 center = area.InverseTransformPoint(rt.position);
            float reach = clickRadius + rt.rect.width / 2f; // radio del click + radio de la burbuja

            if (Vector2.Distance(click, center) <= reach) b.Pop();
        }
        comboManager.CheckAllPopped();
    }

    // --- Para la tienda ---

    /// Añade burbujas sueltas.
    public void AddBubbles(int amount)
    {
        SetBubbleCount(bubbles.Count + amount);
    }

    /// Crece la cuadrícula un lado más: 9 -> 16 -> 25 -> 36...
    public void GrowGrid()
    {
        int side = Mathf.RoundToInt(Mathf.Sqrt(bubbles.Count)) + 1;
        SetBubbleCount(side * side);
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