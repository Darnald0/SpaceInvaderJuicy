using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private int maxLives = 3;

    [SerializeField]
    private Text livesLabel;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private GameObject gameOver;

    [SerializeField]
    private GameObject allClear;

    [SerializeField]
    private Button restartButton;

    private int lives;
    private int score;

    public Light2D AmbientLight;
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

        score = 0;
        scoreLabel.text = $"Score: {score}";
        gameOver.gameObject.SetActive(false);
        allClear.gameObject.SetActive(false);

        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        });
        restartButton.gameObject.SetActive(false);
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

        if (lives > 0)
        {
            return;
        }

        TriggerGameOver();
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreLabel.text = $"Score: {score}";
    }

    public void TriggerGameOver(bool failure = true)
    {
        if (Camera.main.GetComponent<CameraShake>())
        {
            Camera.main.GetComponent<CameraShake>().StopShake();
        }
        gameOver.SetActive(failure);
        allClear.SetActive(!failure);
        restartButton.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

}
