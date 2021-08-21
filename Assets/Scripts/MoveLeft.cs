using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float score = 0;
    public float dashScoreModifier = 3.0f;
    public float dashSpeedModifier = 2.0f;
    private float speed = 30;
    private float leftBound = -15;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isGameOver)
        {
            return;
        }

        if(!playerControllerScript.isReady)
        {
            return;
        }

        if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.LeftShift) && playerControllerScript.isGrounded)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed * dashSpeedModifier);
            score += Time.deltaTime * dashScoreModifier;
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            score += Time.deltaTime;
        }

        Debug.Log($"Score: {(int)score}");
    }
}
