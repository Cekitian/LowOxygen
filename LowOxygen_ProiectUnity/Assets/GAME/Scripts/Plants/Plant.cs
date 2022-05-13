using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    protected bool grounded = true;

    [SerializeField] private LayerMask groundLayer;  
    [SerializeField] private float gravityForce; 
    [SerializeField] private Collider2D plantCollider; 
    private float gravAccel = 0.5f;

    private bool playerInside;
    private Rigidbody2D plantRb;
    private bool grabbable;
    private bool onPlatform = false;
    private float defaultGravityForce;

    private Bounds initBounds;
    private int ungroundedFrames;
    protected void Awake()
    {
        plantRb = gameObject.GetComponent<Rigidbody2D>();
        initBounds = plantCollider.bounds;
        defaultGravityForce = gravityForce;
    }
    protected void FixedUpdate()
    {
        IsGrounded();
        if(!grounded)
        Gravity();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            if(!Player.Instance.playerController.IsGrabbing())
            collision.GetComponent<Player>().SetPlant(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            if (collision.GetComponent<Player>().nearbyPlant == this)
            {
                collision.GetComponent<Player>().SetPlant(null);
            }
        }
    }
    public bool IsGrabbable()
    {
        return grabbable;
    }
    public Rigidbody2D GetRigidBody()
    {
        return plantRb;
    }
    private void Gravity()
    {
        plantRb.velocity -= Vector2.up * Time.deltaTime * gravityForce;

        gravityForce += gravAccel;

    }

    private void IsGrounded()
    {
        RaycastHit2D rayLeft = Physics2D.Raycast(gameObject.transform.position - Vector3.right * initBounds.extents.x, Vector2.down,
            initBounds.extents.y+ 0.05f,groundLayer); 
        RaycastHit2D rayRight = Physics2D.Raycast(gameObject.transform.position + Vector3.right * initBounds.extents.x, Vector2.down,
            initBounds.extents.y+ 0.05f,groundLayer);

        //grounded = ray.collider != null;

        if(rayLeft.collider != null)
        {          
            if(Vector2.Distance(gameObject.transform.position,rayLeft.point) < initBounds.extents.y + 0.04f)
            {
                gameObject.transform.position += Vector3.up * (initBounds.extents.y + 0.04f - Vector2.Distance(gameObject.transform.position, rayLeft.point)); 
            }

            gravityForce = defaultGravityForce;
            plantRb.velocity -= Vector2.up * plantRb.velocity.y / 2;
            grabbable = true;
            
            grounded = true;
            ungroundedFrames = 0;
        }
        else 
        if(rayRight.collider != null)
        {          
            if(Vector2.Distance(gameObject.transform.position, rayRight.point) < initBounds.extents.y + 0.04f)
            {
                gameObject.transform.position += Vector3.up * (initBounds.extents.y + 0.04f - Vector2.Distance(gameObject.transform.position, rayRight.point)); 
            }

            gravityForce = defaultGravityForce;
            plantRb.velocity -= Vector2.up * plantRb.velocity.y / 2;
            grabbable = true;
            
            grounded = true;
            ungroundedFrames = 0;
        }
        else
        {      
            grounded = false;
            grabbable = false;

        }
    }

}
