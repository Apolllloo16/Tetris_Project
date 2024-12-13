using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI lineTillNextLevel;

    int highScore = 0;

       void Start()
    {
        highScore = PlayerPrefs.GetInt("Highscore",0);
        scoreText.text = Board.instance.score.ToString() + " Score";
        highScoreText.text = "Highscore: " + highScore.ToString();
        levelText.text = "Level " + Board.instance.level.ToString();
        lineTillNextLevel.text = "Lines till the next level " + Board.instance.linesTillNextLevel.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Board.instance.score.ToString() + " Score";
        levelText.text = "Level " + Board.instance.level.ToString();
        lineTillNextLevel.text = "Lines till the next level " + Board.instance.linesTillNextLevel.ToString();
        if(highScore<Board.instance.score)
        {
            PlayerPrefs.SetInt("Highscore", Board.instance.score);
        }
    }
}
