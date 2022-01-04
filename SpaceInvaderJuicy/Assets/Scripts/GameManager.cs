using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private int maxLives = 3;

    [SerializeField]
    private Text livesLabel;

    private int lives;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lives = maxLives;
        livesLabel.text = $"Lives: {lives}";
    }

    public void UpdateLives()
    {
        lives = Mathf.Clamp(lives - 1, 0, maxLives);
        livesLabel.text = $"Lives: {lives}";
    }

}
