using UnityEngine;

[System.Serializable]
public class Mejora
{
    [Header("Información Básica")]
    public string idMejora; // Identificador único (ej: "radio", "velocidad", "multiplicador")
    public string nombreMostrado;
    [TextArea] public string descripcion;

    [Header("Economía y Niveles")]
    public double costoBase = 10;
    public double multiplicadorCosto = 1.15; // Sube un 15% cada nivel
    public int nivelActual = 0;
    public int nivelMaximo = 10;

    [Header("Valores de Efecto")]
    public float valorActual;
    public float incrementoPorNivel = 1f;

    // Calcula el costo actual basado en el nivel
    public double CostoActual
    {
        get { return costoBase * System.Math.Pow(multiplicadorCosto, nivelActual); }
    }
}