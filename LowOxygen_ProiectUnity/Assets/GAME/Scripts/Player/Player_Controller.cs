using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip jumpSound;
    [Space]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField]private LayerMask ladderLayer;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float jumpSpeed;
    [Space]
    [SerializeField]private ParticleSystem moveParticles;
    [SerializeField]private ParticleSystem jumpParticles;

    private Player player;
    private Animator animator;
    private SpriteRenderer charRenderer;

    private float moveDir;
    private float ladderDir;
    private float gravForce = 30;
    private float baseGravForce;

    private float gravAccel = 0.5f;

    private Rigidbody2D rb;
    private bool grounded;

    private bool canJump = true;
    private bool willJump = false;
    private bool grabbing;
    private float jumpInputDelay;

    private float colliderWidth;
    private float colliderHeight;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = gameObject.GetComponent<Player>();
        animator = gameObject.GetComponent<Animator>();
        charRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        colliderWidth = player.playerCollision.playerCollider.size.x / 2;
        colliderHeight = player.playerCollision.playerCollider.size.y / 2;

        baseGravForce = gravForce;

        player.oxygenState.ChangedOxygenState += OnOxygenChange;
    }
    private void Update()
    {
        GroundedCheck();
    }
    private void FixedUpdate()
    {
        if (player.playerCollision.onLadder && !grabbing)
            ClimbInput();
        else
            MoveInput();


        if(Mathf.Pow(2,GroundedCheck()) == groundLayer.value)
        if (grounded && canJump && !player.playerCollision.onLadder)
            JumpInput();

        if (player.nearbyPlant != null && grounded)
            GrabInput();
        else
        {
            canJump = true;
            grabbing = false;
        }

        if (!grounded)
            Gravity();
    }
    public int GroundedCheck()
    {
        RaycastHit2D rayCenter = Physics2D.Raycast(gameObject.transform.position , Vector2.down, colliderHeight + 0.05f, groundLayer + ladderLayer);
        RaycastHit2D rayRight = Physics2D.Raycast(gameObject.transform.position + Vector3.right * colliderWidth / 2, Vector2.down, colliderHeight + 0.05f, groundLayer + ladderLayer);
        RaycastHit2D rayLeft = Physics2D.Raycast(gameObject.transform.position - Vector3.right * colliderWidth / 2, Vector2.down, colliderHeight + 0.05f, groundLayer + ladderLayer);
        if (rayCenter.collider != null || rayLeft.collider != null || rayRight.collider != null)
        {
            Vector3 point;
            if (rayCenter.collider != null)
                point = rayCenter.point;
            else if (rayLeft.collider != null)
                point = rayLeft.point;
            else point = rayRight.point;
           
            if(Vector2.Distance(gameObject.transform.position, point) > colliderHeight +0.01f)
            {
                gameObject.transform.position -=Vector3.up * (Vector2.Distance(gameObject.transform.position, point) - colliderHeight);
                rb.velocity -= Vector2.up * rb.velocity.y;
            }

            if(grounded == false)
            {
                HitGround();
            }

            grounded = true;
            gravForce = baseGravForce;
            if (rayCenter.collider != null)
                return rayCenter.collider.gameObject.layer;
            else if (rayLeft.collider != null)
                return rayLeft.collider.gameObject.layer;
            else
                return rayRight.collider.gameObject.layer;


        }
        else
            grounded = false;

        return -1;
    }
    public bool IsGrabbing()
    {
        return grabbing;
    }

    private void GrabInput()
    {
        if (Input.GetKey(PlayerInputKeys.interact) && player.nearbyPlant.IsGrabbable())
        {
            grabbing = true;
            canJump = false;
            player.nearbyPlant.GetRigidBody().velocity = Vector2.right * rb.velocity + (rb.velocity- rb.velocity * Vector2.right);
        }
        else
        {
            grabbing = false;
            canJump = true;
        }           
    }
    private void MoveInput()
    {
        float dir = 0;
        if (Input.GetKey(PlayerInputKeys.right))
        {
            dir += 1;
        }
        if (Input.GetKey(PlayerInputKeys.left))
        {
            dir += -1;
        }

        RaycastHit2D horizRayTop = Physics2D.Raycast(gameObject.transform.position + Vector3.up * colliderHeight, Vector2.right * dir, colliderWidth + 0.2f, groundLayer);
        RaycastHit2D horizRayBot = Physics2D.Raycast(gameObject.transform.position - Vector3.up * (colliderHeight - 0.05f), Vector2.right * dir, colliderWidth + 0.2f, groundLayer);
        RaycastHit2D horizRayCent = Physics2D.Raycast(gameObject.transform.position, Vector2.right * dir, colliderWidth + 0.2f, groundLayer);
        if (horizRayTop.collider != null || (horizRayBot.collider != null && !grounded) || horizRayCent.collider != null)
            moveDir = 0;
        else
        { moveDir = dir; }

        animator.SetBool("Moving", moveDir != 0);
        if(dir != 0)
        {
            charRenderer.flipX = dir < 0;
            foreach(SpriteRenderer x in player.playerHats.GetRenderers())
            {
                x.flipX = dir < 0;
            }
        }

        rb.velocity = Vector2.up * rb.velocity.y + Vector2.right * moveDir * moveSpeed;

        if(moveDir != 0 && grounded)
        {
            if(moveParticles.isPlaying == false)
            {
                moveParticles.Play();
            }
        }
        else
        {
                moveParticles.Stop();
        }
    }
    private void ClimbInput()
    {
        float horizDir = 0;
        if (Input.GetKey(PlayerInputKeys.right))
        {
            horizDir += 1;
        }
        if (Input.GetKey(PlayerInputKeys.left))
        {
            horizDir += -1;
        }

        float vertiDir = 0;
        if (Input.GetKey(PlayerInputKeys.up))
        {
            vertiDir += 1;
        }
        if (Input.GetKey(PlayerInputKeys.down))
        {
            vertiDir += -1;
        }

        ladderDir = vertiDir;

        animator.SetBool("Moving", ladderDir != 0 || horizDir != 0);

        rb.velocity = Vector2.right * rb.velocity.x + Vector2.up * ladderDir * moveSpeed;
        rb.velocity = Vector2.up * rb.velocity.y + Vector2.right * horizDir * moveSpeed;
    }
    private void JumpInput()
    {
        if (Input.GetKey(PlayerInputKeys.up))
            willJump = true;
        else
            willJump = false;
        

        if(willJump && Time.time >= jumpInputDelay)
        {
            AudioManager.Instance.PlaySound(jumpSound, 0.1f, Random.Range(1.25f, 1.5f), false);
            AudioManager.Instance.PlaySound(walkSound, 0.25f, Random.Range(1.25f, 1.5f), false);

            animator.SetTrigger("Jumping");

            jumpInputDelay = Time.time + 0.1f;
            willJump = false;

            rb.velocity += Vector2.up * jumpSpeed;

            jumpParticles.Play();
        }
    }
    private void Gravity()
    {
        rb.velocity -= Vector2.up * Time.deltaTime * gravForce;

        gravForce += gravAccel;
    }
    private void OnOxygenChange()
    {
        Debug.Log("CHANGED OXYGEN PL");

        if (player.oxygenState.oxygenState == Oxygen_State.OXYGEN.NO_OXYGEN)
        {
            baseGravForce = 15;
        }
        else
        {
            baseGravForce = 30;
        }
    }
    private void PlayWalkSound()
    {
        if(grounded)
        AudioManager.Instance.PlaySound(walkSound, 0.05f, Random.Range(1.75f,2f), false);
    }
    private void HitGround()
    {
        animator.SetTrigger("HitGround");
    }
}
