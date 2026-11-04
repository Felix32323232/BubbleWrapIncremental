using UnityEngine;
using TMPro;

/// Guarda los puntos del jugador. La tienda usa TrySpend() para pagar.
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public int Score { get; private set; }

    // NUEVO: puntos ganados en la partida actual (sirve para calcular las perlas al renacer)
    public int TotalEarned { get; private set; }

    // NUEVO: multiplicador global de puntos (lo modifican las mejoras especiales)
    public float globalMultiplier = 1f;

    void Start()
    {
        Refresh();
    }

    public void AddPoints(int amount)
    {
        int gained = Mathf.RoundToInt(amount * globalMultiplier);
        Score += gained;
        TotalEarned += gained;
        Refresh();
    }

    // Devuelve true si había puntos suficientes y los gasta
    public bool TrySpend(int cost)
    {
        if (Score < cost) return false;
        Score -= cost;
        Refresh();
        return true;
    }

    // NUEVO: lo usa el renacimiento
    public void ResetScore()
    {
        Score = 0;
        TotalEarned = 0;
        Refresh();
    }

    void Refresh()
    {
        scoreText.text = "Puntos: " + Score;
    }
}