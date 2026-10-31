using UnityEngine;
using System.Collections.Generic;

public class TiendaManager : MonoBehaviour
{
    [Header("Lista de Mejoras de la Tienda")]
    public List<Mejora> listaMejoras = new List<Mejora>();

    [Header("Referencias Externas (Conéctalo cuando juntéis escenas)")]
    // Aquí tu compañero pondrá la referencia a su script principal de puntos/partida
    // public GameManager gameManager; 

    // Referencia a tu propio script de gestión de clics en la mesa 2D
    public GestorClics2D gestorClics;

    // Método para comprar una mejora
    public void IntentarComprarMejora(string idMejora)
    {
        Mejora mejoraEncontrada = listaMejoras.Find(m => m.idMejora == idMejora);

        if (mejoraEncontrada == null || mejoraEncontrada.nivelActual >= mejoraEncontrada.nivelMaximo)
            return;

        double costo = mejoraEncontrada.CostoActual;

        // --- AQUÍ SE CONECTARÁ CON LOS PUNTOS DE TU COMPAÑERO ---
        // if (gameManager.PuntosTotales >= costo)
        // {
        //     gameManager.PuntosTotales -= costo;
        //     EjecutarCompra(mejoraEncontrada);
        // }
    }

    void EjecutarCompra(Mejora mejora)
    {
        mejora.nivelActual++;
        AplicarEfectoMejora(mejora);
    }

    void AplicarEfectoMejora(Mejora mejora)
    {
        switch (mejora.idMejora)
        {
            case "radio":
                mejora.valorActual += mejora.incrementoPorNivel;
                if (gestorClics != null) gestorClics.radioClick = mejora.valorActual;
                break;
            case "multiplicador":
                mejora.valorActual += mejora.incrementoPorNivel;
                break;
            case "autopop":
                // Lógica de burbujas automáticas por segundo
                break;
        }
    }
}