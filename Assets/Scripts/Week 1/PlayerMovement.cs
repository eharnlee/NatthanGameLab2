using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public GameObject enemies;
    private Vector3 startPosition;
    public JumpOverGoomba jumpOverGoomba;

    // UI Elements
    public GameObject GameOverPanel;
    public TMPro.TextMeshProUGUI ScoreText;
    private Vector3 scoreTextOriginalPosition;
    public GameObject restartButton;
    private Vector3 restartButtonOriginalPosition;

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        marioBody = GetComponent<Rigidbody2D>();
        GameOverPanel.SetActive(false);

        // get starting position of Mario and UI Elements
        startPosition = transform.localPosition;
        scoreTextOriginalPosition = ScoreText.transform.localPosition;
        restartButtonOriginalPosition = restartButton.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
            Time.timeScale = 0.0f;

            // show GameOverPanel
            GameOverPanel.SetActive(true);

            // move UI Elements
            ScoreText.transform.localPosition = new Vector3(0, -100, 0);
            restartButton.transform.localPosition = new Vector3(0, -200, 0);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // hide GameOverPanel
        GameOverPanel.SetActive(false);
        // reset position
        marioBody.transform.position = startPosition;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        ScoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        // reset UI elements
        ScoreText.transform.localPosition = scoreTextOriginalPosition;
        restartButton.transform.localPosition = restartButtonOriginalPosition;

        // reset score
        jumpOverGoomba.score = 0;
    }
}
