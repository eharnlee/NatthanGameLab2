using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    public float deathImpulse = 10;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    public GameManagerScript gameManager;

    // UI Elements
    public GameObject GameOverPanel;
    public TMPro.TextMeshProUGUI ScoreText;

    public GameObject restartButton;


    // for animation
    public Animator marioAnimator;

    // for audio
    public AudioSource marioAudio;
    public AudioClip marioDeath;

    // state
    [System.NonSerialized]
    public bool alive = true;

    public Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        gameManager.GameOver(); // replace this with whichever way you triggered the game over screen for Checkoff 1
    }

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        marioBody = GetComponent<Rigidbody2D>();
        GameOverPanel.SetActive(false);

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Collided with goomba!");

            // play death animation
            marioAnimator.Play("Mario Die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        GameManagerScript.marioPosition = marioBody.position;
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (alive)
        {
            if (Mathf.Abs(moveHorizontal) > 0)
            {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                // check if it doesn't go beyond maxSpeed
                if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
            }

            // // stop when lifting up horizontal movement key
            // if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            // {
            //     // stop
            //     marioBody.velocity = Vector2.zero;
            // }

            if (Input.GetKeyDown("space") && onGroundState)
            {
                marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                onGroundState = false;
                // update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    public void RestartButtonCallback(int input)
    {
        // Debug.Log("Restart!");

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
        marioBody.transform.position = new Vector3(-6.5f, 2, 0);
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
        ScoreText.transform.localPosition = new Vector3(-937, 604, 0);
        restartButton.transform.localPosition = new Vector3(1114, 592, 0);

        // reset score
        jumpOverGoomba.score = 0;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 6.5f, -10);
    }
}
