using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;

    public int score;

    private void Awake()
    {
        score = Scores.currentscore;
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = $"Score: {score.ToString()}";
    }

    public void AddPoint(int value)
    {
        score += value;
        scoreText.text = $"Score: {score.ToString()}";
    }

    public void RemovePoint(int value)
    {
        score -= value;
        scoreText.text = $"Score: {score.ToString()}";
    }
}
