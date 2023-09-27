using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBlock : MonoBehaviour
{
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
        // if mario is above the block, do not allow the block to bounce
        if (GameManagerScript.marioPosition.y > this.transform.position.y)
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            // Debug.Log("test1");
        }
        else
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            // Debug.Log("test2");
        }
    }
}
