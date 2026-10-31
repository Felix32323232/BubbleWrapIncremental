using UnityEngine;
using UnityEngine.UI;

/// Va en el prefab de la burbuja. Solo sabe reventarse y restaurarse.
[RequireComponent(typeof(Button), typeof(Image))]
public class Bubble : MonoBehaviour
{
    [SerializeField] Color activeColor = new Color(0.4f, 0.8f, 1f, 0.9f);
    [SerializeField] Color poppedColor = new Color(0.4f, 0.8f, 1f, 0.15f);

    ComboManager comboManager;
    Button button;
    Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(Pop);
        Restore();
    }

    // Lo llama BubbleArea justo después de crear la burbuja
    public void Init(ComboManager manager)
    {
        comboManager = manager;
    }

    void Pop()
    {
        button.interactable = false;
        image.color = poppedColor;
        comboManager.OnBubblePopped();
    }

    public void Restore()
    {
        button.interactable = true;
        image.color = activeColor;
    }
}