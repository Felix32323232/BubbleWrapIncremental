using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField] Button button;
    ComboManager comboManager;
    bool popped;

    void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(Pop);
    }

    public void Init(ComboManager manager)
    {
        comboManager = manager;
    }

    public void Pop()
    {
        if (popped) return;
        popped = true;
        button.interactable = false;

        // Avisamos al ComboManager de que se ha roto una burbuja
        if (comboManager != null)
        {
            comboManager.OnBubblePopped();
        }

        // Destruimos o desactivamos la burbuja según vuestra lógica original
        gameObject.SetActive(false);
    }

    public void Restore()
    {
        popped = false;
        button.interactable = true;
        gameObject.SetActive(true);
    }
}