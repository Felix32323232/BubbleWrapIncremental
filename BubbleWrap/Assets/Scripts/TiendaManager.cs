using UnityEngine;
using System.Collections.Generic;

public class TiendaManager : MonoBehaviour
{
    [Header("Lista de Mejoras")]
    public List<Mejora> listaMejoras = new List<Mejora>();

    [Header("Referencias de la Escena")]
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] BubbleArea bubbleArea;
    [SerializeField] ComboManager comboManager;

    // NUEVO: valor de cada mejora al empezar, para poder reiniciarlas
    readonly List<float> valoresIniciales = new List<float>();

    // NUEVO
    void Awake()
    {
        foreach (Mejora m in listaMejoras)
            valoresIniciales.Add(m != null ? m.valorActual : 0f);
    }

    // Método que llaman tus botones de la UI al hacer clic
    public void IntentarComprarMejora(string idMejora)
    {
        Mejora mejora = BuscarMejora(idMejora);

        if (mejora == null)
        {
            Debug.LogWarning("No existe ninguna mejora con el ID: " + idMejora);
            return;
        }

        if (mejora.nivelActual >= mejora.nivelMaximo)
        {
            Debug.Log("La mejora ya está al máximo nivel.");
            return;
        }

        int costo = Mathf.RoundToInt((float)mejora.CostoActual);

        if (scoreManager != null && scoreManager.TrySpend(costo))
        {
            mejora.nivelActual++;
            AplicarEfecto(mejora);
            Debug.Log("¡Comprado con éxito: " + mejora.nombreMostrado + "!");
        }
        else
        {
            Debug.Log("No hay suficientes puntos para comprar esto.");
        }
    }

    void AplicarEfecto(Mejora mejora)
    {
        switch (mejora.idMejora)
        {
            case "mas_burbujas":
                if (bubbleArea != null) bubbleArea.GrowGrid();
                break;

            case "combo_tiempo":
                mejora.valorActual += mejora.incrementoPorNivel;
                if (comboManager != null) comboManager.comboDuration = mejora.valorActual;
                break;

            case "multiplicador":
                mejora.valorActual += mejora.incrementoPorNivel;
                if (comboManager != null) comboManager.pointsMultiplier = mejora.valorActual;
                break;

            case "click":
                mejora.valorActual += mejora.incrementoPorNivel;
                if (bubbleArea != null) bubbleArea.clickRadius = mejora.valorActual;
                break;
        }
    }

    // NUEVO
    public Mejora BuscarMejora(string idMejora)
    {
        return listaMejoras.Find(m => m != null && m.idMejora == idMejora);
    }

    // NUEVO: devuelve todas las mejoras normales a nivel 0 y reaplica sus valores base
    public void ReiniciarMejoras()
    {
        for (int i = 0; i < listaMejoras.Count; i++)
        {
            Mejora m = listaMejoras[i];
            if (m == null) continue;
            m.nivelActual = 0;
            m.valorActual = valoresIniciales[i];
        }

        Mejora combo = BuscarMejora("combo_tiempo");
        if (combo != null && comboManager != null) comboManager.comboDuration = combo.valorActual;

        Mejora click = BuscarMejora("tamano_click");
        if (click != null && bubbleArea != null) bubbleArea.clickRadius = click.valorActual;

        Mejora mult = BuscarMejora("multiplicador");
        if (mult != null && comboManager != null) comboManager.pointsMultiplier = mult.valorActual;
    }
}