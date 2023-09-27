using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // UI Elements
    public GameObject GameOverPanel;
    public TMPro.TextMeshProUGUI ScoreText;
    public GameObject restartButton;

    public static Vector3 marioPosition;

    // game over screen
    public void GameOver()
    {
        // show GameOverPanel
        GameOverPanel.SetActive(true);

        // move UI Elements
        ScoreText.transform.localPosition = new Vector3(0, -100, 0);
        restartButton.transform.localPosition = new Vector3(0, -200, 0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
