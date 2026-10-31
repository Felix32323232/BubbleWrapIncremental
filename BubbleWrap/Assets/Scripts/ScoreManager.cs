using UnityEngine;
using TMPro;

/// Guarda los puntos del jugador. La futura tienda de mejoras usará TrySpend().
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public int Score { get; private set; }

    void Start()
    {
        Refresh();
    }

    public void AddPoints(int amount)
    {
        Score += amount;
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

    void Refresh()
    {
        scoreText.text = "Puntos: " + Score;
    }
}