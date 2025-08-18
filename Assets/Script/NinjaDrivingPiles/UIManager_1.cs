using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUIUI : MonoBehaviour
{
    public static UIUIUI instance;
    [SerializeField] TextMeshProUGUI scoreText;
    private int score;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
    public void AddScore(int _score)
    {
        scoreText.text = (score + _score).ToString();
    }
}
