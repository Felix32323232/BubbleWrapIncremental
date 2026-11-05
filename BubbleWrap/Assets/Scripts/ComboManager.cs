using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Controla la barra de combo. Cuando se acaba, restaura todas las burbujas.
public class ComboManager : MonoBehaviour
{
    [Header("Ajustes (las mejoras modificarán esto)")]
    public float comboDuration = 2f;
    public float pointsMultiplier = 1f;
    public bool resetTimerOnEachClick = false;

    [Header("Referencias")]
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] BubbleArea bubbleArea; // la zona ZonaBurbujas
    [SerializeField] GameObject comboBar;   // el objeto BarraCombo
    [SerializeField] Image comboFill;       // el hijo Relleno
    [SerializeField] TMP_Text comboText;

    int comboCount;
    float timer;
    bool active;

    void Start()
    {
        comboBar.SetActive(false);
        comboText.text = "";
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        comboFill.rectTransform.localScale = new Vector3(Mathf.Clamp01(timer / comboDuration), 1f, 1f);

        if (timer <= 0f) EndCombo();
    }

    // Lo llama cada burbuja al reventarse
    public void OnBubblePopped()
    {
        if (!active)
        {
            active = true;
            comboCount = 0;
            timer = comboDuration;
            comboBar.SetActive(true);
        }
        else if (resetTimerOnEachClick)
        {
            timer = comboDuration;
        }

        comboCount++;
        // Multiplicamos los puntos del combo por el multiplicador de tu TiendaManager
        float mult = 1f;
        TiendaManager tienda = Object.FindFirstObjectByType<TiendaManager>();

        if (tienda != null && tienda.listaMejoras != null)
        {
            Mejora mejoraMult = tienda.listaMejoras.Find(m => m != null && m.idMejora == "multiplicador");
            if (mejoraMult != null)
            {
                mult = mejoraMult.valorActual;
            }
        }

        int puntosFinales = Mathf.RoundToInt(comboCount * mult);
        scoreManager.AddPoints(puntosFinales);
        comboText.text = "Combo X" + comboCount; 
    }
    // Cancela el combo actual sin restaurar burbujas (lo usa el renacimiento)
    public void CancelCombo()
    {
        active = false;
        comboCount = 0;
        comboBar.SetActive(false);
        comboText.text = "";
    }
    // Lo llama BubbleArea al terminar cada click
    public void CheckAllPopped()
    {
        if (active && bubbleArea.RemainingBubbles == 0) EndCombo();
    }
    void EndCombo()
    {
        active = false;
        comboCount = 0;
        comboBar.SetActive(false);
        comboText.text = "";

        bubbleArea.RestoreAll();
    }
}