using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumForce;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject frostBall;
    [SerializeField] Transform projectileShootPoint;
    Rigidbody rb;
    Animator animator;
    AudioSource audioSource;

    public bool isAlive = true;
    bool isGrounded = true;
    RaycastHit hit;
    float _vertical;
    float _horizontal;
    float turnSmoothVelocity;
    float onCooldown;
    private void Awake()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {

        PlayerMovement();
        PlayerJump();
        onCooldown += Time.deltaTime;
        FireBall();
        FrostBall();
    }


    void PlayerMovement()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(_horizontal, 0f, _vertical).normalized;

        if(direction.magnitude >= 0.1f && isAlive)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDirection * playerSpeed * Time.deltaTime);
        }



        if (_vertical != 0f || _horizontal !=0f)
        {
            animator.SetInteger("Run", 1);
        }
        else
        {
            animator.SetInteger("Run", 0);
        }
    }

    void PlayerJump()
    {
        if (isGrounded && Input.GetKey(KeyCode.Space) && isAlive)
        {
            animator.SetBool("jump", true);
            rb.AddForce(Vector3.up * jumForce, ForceMode.Impulse);
            isGrounded = false;
            
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.2f))
        {
            isGrounded = true;
            animator.SetBool("jump", false);
        }
    }

    void FireBall()
    {
        if (Input.GetButtonDown("Fire1") && isAlive)
        {
            if (onCooldown >= 1f)
            {
                Instantiate(fireBall, projectileShootPoint.position, this.transform.rotation);
            }
            onCooldown = 0f;
        }
    }

    void FrostBall()
    {
        if (Input.GetButtonDown("Fire2") && isAlive)
        {
            if (onCooldown >= 1f)
            {
                Instantiate(frostBall, projectileShootPoint.position, this.transform.rotation);
            }
            onCooldown = 0f;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectibles"))
        {
            gameManager.IncreaseScore();
            audioSource.Play();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("GameController"))
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        animator.SetTrigger("Die");
        isAlive = false;
        gameManager.ResurrectPlayer();
        Invoke("setpos", 3f);
        
    }

    void setpos()
    {
        transform.position = spawnPos.transform.position;
        animator.SetTrigger("Resurrect");
        isAlive = true;
    }
}
