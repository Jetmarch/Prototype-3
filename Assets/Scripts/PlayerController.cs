using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10;
    public float gravityModifier = 1;
    public bool isGameOver = false;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public bool isGrounded = true;
    public bool isReady = false;

    private bool isDobleJumpActive = true;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;


        playerAnim.SetFloat("Speed_f", 0.3f);
        dirtParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            return;
        }

        //Intro animation
        if(!isReady)
        {
            IntroAnimation();
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isDobleJumpActive)
        {
            DoubleJump();
        }
    }

    private void IntroAnimation()
    {
        transform.Translate(Vector3.forward * 2 * Time.deltaTime);
        if (transform.position.x >= 3.0f)
        {
            isReady = true;
            playerAnim.SetFloat("Speed_f", 1.0f);
            dirtParticle.Play();
        }
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        playerAnim.SetTrigger("Jump_trig");
        dirtParticle.Stop();
        playerAudio.PlayOneShot(jumpSound, 1.0f);
    }

    private void DoubleJump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isDobleJumpActive = false;
        playerAnim.SetTrigger("Jump_trig");
        dirtParticle.Stop();
        playerAudio.PlayOneShot(jumpSound, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isGameOver)
        {
            return;
        }

        if(!isReady)
        {
            return;
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isDobleJumpActive = true;
            dirtParticle.Play();
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
}
