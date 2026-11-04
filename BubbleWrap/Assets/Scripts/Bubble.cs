using UnityEngine;

/// Va en el prefab de la burbuja. Ya NO es un botón: los clicks los gestiona BubbleArea.
public class Bubble : MonoBehaviour
{
    ComboManager comboManager;
    bool popped;

    public bool IsPopped => popped;

    // Lo llama BubbleArea justo después de crear la burbuja
    public void Init(ComboManager manager)
    {
        comboManager = manager;
    }

    public void Pop()
    {
        if (popped) return;
        popped = true;

        // Avisamos al ComboManager de que se ha roto una burbuja
        if (comboManager != null) comboManager.OnBubblePopped();

        gameObject.SetActive(false);
    }

    public void Restore()
    {
        popped = false;
        gameObject.SetActive(true);
    }
}