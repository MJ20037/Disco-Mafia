using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private int _currentScore;
    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        Health.OnDeath += EnemyDestroyed;
    }
    private void OnDisable()
    {
        Health.OnDeath -= EnemyDestroyed;
    }

    private void EnemyDestroyed(Health sender)
    {
        _currentScore++;
        _scoreText.text = _currentScore.ToString("D3");
    }
}
