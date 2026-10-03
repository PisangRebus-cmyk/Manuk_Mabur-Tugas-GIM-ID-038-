using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Logic : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    public SpriteRenderer mySprite;
    
    public void addScore(int X)
    {
        playerScore += X;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }


}
