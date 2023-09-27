using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlockCoin : MonoBehaviour
{
    public Animator blockAnimator;
    public Animator coinAnimator;
    public GameObject brickBlock;
    public GameObject coin;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManagerScript.marioPosition.y < brickBlock.transform.position.y)
        {
            coinAnimator.SetBool("hitCoin", true);
            blockAnimator.SetTrigger("hitBlock");
        }
    }
}
