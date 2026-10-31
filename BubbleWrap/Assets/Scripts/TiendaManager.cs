using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TiendaManager : MonoBehaviour
{
    [Header("Lista de Mejoras")]
    public List<Mejora> listaMejoras = new List<Mejora>();

    [Header("Referencias de la Escena")]
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] BubbleArea bubbleArea;
    [SerializeField] ComboManager comboManager;

    // Método que llaman tus botones de la UI al hacer clic
    public void IntentarComprarMejora(string idMejora)
    {
        Mejora mejora = listaMejoras.Find(m => m.idMejora == idMejora);

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

        // Usamos el método de pago del ScoreManager de vuestro compañero
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
                mejora.valorActual += mejora.incrementoPorNivel;
                if (bubbleArea != null)
                {
                    bubbleArea.AddBubbles(Mathf.RoundToInt(mejora.incrementoPorNivel));
                }
                break;

            case "combo_tiempo":
                mejora.valorActual += mejora.incrementoPorNivel;
                if (comboManager != null)
                {
                    comboManager.comboDuration = mejora.valorActual;
                }
                break;

            case "multiplicador":
                mejora.valorActual += mejora.incrementoPorNivel;
                break;
        }
    }
    void Update()
    {
        // Forma compatible con el New Input System para probar con la tecla 'C'
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            IntentarComprarMejora("multiplicador");
        }
    }
}