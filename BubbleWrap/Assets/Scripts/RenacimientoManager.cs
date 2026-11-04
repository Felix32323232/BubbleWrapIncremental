using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// Renacimiento (prestigio): reinicia el progreso a cambio de Perlas,
/// una moneda especial para comprar mejoras permanentes que nunca se reinician.
public class RenacimientoManager : MonoBehaviour
{
    [System.Serializable]
    public class MejoraEspecial
    {
        public string idMejora;            // "puntos_permanentes", "inicio_burbujas", "combo_base"
        public string nombreMostrado;
        public int costoBase = 1;          // coste en perlas
        public float multiplicadorCosto = 1.5f;
        public int nivelActual = 0;
        public int nivelMaximo = 10;
        public float incrementoPorNivel = 0.25f;
        public TMP_Text texto;             // texto del botón: muestra nombre, nivel y coste

        public int CostoActual => Mathf.RoundToInt(costoBase * Mathf.Pow(multiplicadorCosto, nivelActual));
        public float Bonus => nivelActual * incrementoPorNivel;
    }

    [Header("Renacimiento")]
    [SerializeField] double costoBase = 5000;        // puntos necesarios para el primer renacimiento
    [SerializeField] double multiplicadorCosto = 3;  // cada renacimiento cuesta x3 más
    [SerializeField] float divisorPerlas = 1000f;    // perlas = raíz cuadrada de (puntos ganados / divisor)
    [SerializeField] int ladoInicial = 3;            // 3 = cuadrícula de 9 (debe coincidir con BubbleArea)

    [Header("Referencias")]
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] TiendaManager tienda;
    [SerializeField] BubbleArea bubbleArea;
    [SerializeField] ComboManager comboManager;

    [Header("UI")]
    [SerializeField] TMP_Text perlasText;
    [SerializeField] TMP_Text renacerText;           // texto del botón de renacer

    [Header("Mejoras especiales (se pagan con perlas y nunca se reinician)")]
    [SerializeField] List<MejoraEspecial> especiales = new List<MejoraEspecial>();

    public int Perlas { get; private set; }
    public int Renacimientos { get; private set; }

    int CostoRenacimiento =>
        Mathf.RoundToInt((float)(costoBase * System.Math.Pow(multiplicadorCosto, Renacimientos)));

    int PerlasAlRenacer =>
        Mathf.Max(1, Mathf.FloorToInt(Mathf.Sqrt(scoreManager.TotalEarned / divisorPerlas)));

    void Start()
    {
        InvokeRepeating(nameof(RefrescarUI), 0f, 0.25f);
    }

    // --- Botón de renacer ---

    public void IntentarRenacer()
    {
        int ganadas = PerlasAlRenacer; // se calcula ANTES de reiniciar

        if (!scoreManager.TrySpend(CostoRenacimiento))
        {
            Debug.Log("No tienes puntos suficientes para renacer.");
            return;
        }

        Perlas += ganadas;
        Renacimientos++;
        ReiniciarPartida();
        RefrescarUI();
    }

    void ReiniciarPartida()
    {
        scoreManager.ResetScore();
        tienda.ReiniciarMejoras();

        int lado = ladoInicial + Mathf.RoundToInt(Bonus("inicio_burbujas"));
        bubbleArea.SetBubbleCount(lado * lado);
        bubbleArea.RestoreAll();

        AplicarEspeciales();
    }

    // Reaplica los bonus permanentes tras un reinicio
    void AplicarEspeciales()
    {
        scoreManager.globalMultiplier = 1f + Bonus("puntos_permanentes");

        Mejora combo = tienda.BuscarMejora("combo_tiempo");
        if (combo != null)
        {
            combo.valorActual += Bonus("combo_base");
            comboManager.comboDuration = combo.valorActual;
        }
    }

    // --- Botones de mejoras especiales ---

    public void ComprarEspecial(string id)
    {
        MejoraEspecial m = especiales.Find(e => e.idMejora == id);

        if (m == null)
        {
            Debug.LogWarning("No existe ninguna mejora especial con el ID: " + id);
            return;
        }

        if (m.nivelActual >= m.nivelMaximo) return;

        int costo = m.CostoActual;
        if (Perlas < costo)
        {
            Debug.Log("No tienes suficientes perlas.");
            return;
        }

        Perlas -= costo;
        m.nivelActual++;

        // Efecto inmediato (inicio_burbujas se aplica en el próximo renacimiento)
        switch (id)
        {
            case "puntos_permanentes":
                scoreManager.globalMultiplier = 1f + m.Bonus;
                break;

            case "combo_base":
                {
                    Mejora combo = tienda.BuscarMejora("combo_tiempo");
                    if (combo != null)
                    {
                        combo.valorActual += m.incrementoPorNivel;
                        comboManager.comboDuration = combo.valorActual;
                    }
                    break;
                }
        }

        RefrescarUI();
    }

    float Bonus(string id)
    {
        MejoraEspecial m = especiales.Find(e => e.idMejora == id);
        return m != null ? m.Bonus : 0f;
    }

    // --- UI ---

    void RefrescarUI()
    {
        if (perlasText != null) perlasText.text = "Perlas: " + Perlas;

        if (renacerText != null)
            renacerText.text = "RENACER\nCoste: " + CostoRenacimiento + " puntos\nGanas: " + PerlasAlRenacer + " perlas";

        foreach (MejoraEspecial m in especiales)
        {
            if (m.texto == null) continue;
            m.texto.text = m.nivelActual >= m.nivelMaximo
                ? m.nombreMostrado + " (MAX)"
                : m.nombreMostrado + "  Nv." + m.nivelActual + "\n" + m.CostoActual + " perlas";
        }
    }
}