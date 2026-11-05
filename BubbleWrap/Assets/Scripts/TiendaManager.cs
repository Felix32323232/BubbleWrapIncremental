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

    // Valor de cada mejora al empezar, para poder reiniciarlas
    readonly List<float> valoresIniciales = new List<float>();

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

    // Al comprar: sube el valor de la mejora y lo aplica al juego
    void AplicarEfecto(Mejora mejora)
    {
        if (mejora.idMejora == "mas_burbujas")
        {
            if (bubbleArea != null) bubbleArea.GrowGrid();
            return;
        }

        mejora.valorActual += mejora.incrementoPorNivel;
        AplicarValor(mejora);
    }

    // Pasa el valorActual de la mejora al juego.
    // Se usa al comprar Y al reiniciar, así cada id solo aparece aquí una vez.
    void AplicarValor(Mejora mejora)
    {
        switch (mejora.idMejora)
        {
            case "combo_tiempo":
                if (comboManager != null) comboManager.comboDuration = mejora.valorActual;
                break;

            case "multiplicador":
                if (comboManager != null) comboManager.pointsMultiplier = mejora.valorActual;
                break;

            case "click":
                if (bubbleArea != null) bubbleArea.clickRadius = mejora.valorActual;
                break;
        }
    }

    public Mejora BuscarMejora(string idMejora)
    {
        return listaMejoras.Find(m => m != null && m.idMejora == idMejora);
    }

    // Devuelve todas las mejoras normales a nivel 0 y reaplica sus valores base
    public void ReiniciarMejoras()
    {
        for (int i = 0; i < listaMejoras.Count; i++)
        {
            Mejora m = listaMejoras[i];
            if (m == null) continue;

            m.nivelActual = 0;
            m.valorActual = valoresIniciales[i];
            AplicarValor(m);
        }
    }
}