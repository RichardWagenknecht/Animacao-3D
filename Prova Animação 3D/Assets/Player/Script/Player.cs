using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    void Awake()
    {
        instance = this;
    }

    public CharacterController controller;
    public Transform cameraTransform;
    public Animator animator;
    public float speed = 6f;
    public float runSpeedMultiplier = 1.5f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public float gravity = -9.81f;
    private Vector3 velocity;
    private bool isGrounded;
    public float strafeSpeedMultiplier = 0.8f;
    public AudioSource playerAudio;
    public AudioClip andarAudio;
    public bool andando;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal") * strafeSpeedMultiplier;
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? speed * runSpeedMultiplier : speed;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);


            if (!playerAudio.isPlaying)
            {
                playerAudio.clip = andarAudio;
                playerAudio.loop = true;
                playerAudio.Play();
            }

            animator.SetBool("Walk", !isRunning && direction.magnitude > 0.1f);
            animator.SetBool("Run", isRunning);
            andando = true;
        }
        else
        {
            playerAudio.Stop();
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            andando = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chegada"))
        {
            GameManager.instance.Vitoria();
        }
    }
    public void Morrer()
    {
        animator.SetTrigger("Death");
        this.GetComponent<CharacterController>().enabled = false;
    }

}
