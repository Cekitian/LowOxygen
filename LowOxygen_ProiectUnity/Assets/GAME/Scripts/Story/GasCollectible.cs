using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GasCollectible : MonoBehaviour
{
    public Transform followTarget;
    public bool notFollowPlayer;

    [SerializeField] private int gasId;
    [TextArea(2,4)] [SerializeField] private string pickupText;
    [SerializeField] private ParticleSystem collectParticles;
    [SerializeField] private Transform cameraWatchPoint;
    [SerializeField] private GameObject spriteHolder;

    private Animator animator;
    private Light2D lightEmitter;
    private bool doingCinematic = false;
    private float speed = 10f;

    private Player playerRef;
    private bool chasePlayer = false;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        lightEmitter = gameObject.GetComponentInChildren<Light2D>();  
    }
    private void Start()
    {
        if (gasId < GameDataManager.Instance.DATA.canistersCollected && !notFollowPlayer)
        {
            Destroy(gameObject);
        }

        if (notFollowPlayer)
        {
            StartCoroutine(CollectCinematic());
            doingCinematic = true;
        }

        playerRef = Player.Instance;
        if(followTarget == null)
        {
            followTarget = playerRef.gameObject.transform;
        }
    }
    private void FixedUpdate()
    {
        if (!chasePlayer)
            return;

        Vector3 dirToTarget = followTarget.transform.position - gameObject.transform.position;

        gameObject.transform.position += new Vector3(dirToTarget.normalized.x, dirToTarget.normalized.y, dirToTarget.normalized.z) * Time.fixedDeltaTime * speed;
        gameObject.transform.localEulerAngles += Vector3.forward * 15f;
        spriteHolder.transform.localScale = Vector3.one * Mathf.Clamp(dirToTarget.magnitude / 5f, 0f, 1f);
        Debug.Log(dirToTarget + " THIS IS THE DISTANCE TO THE PLAYER");
        if (dirToTarget.magnitude < 1)
        {
            chasePlayer = false;

            if (followTarget == playerRef.gameObject.transform)
                PickedUp();

            Destroy(gameObject, 0.05f);
        }

        speed += Time.fixedDeltaTime * 2f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !doingCinematic)
        {
            StartCoroutine(CollectCinematic());
            doingCinematic = true;
        }
    }
    private IEnumerator CollectCinematic()
    {
        if(!notFollowPlayer)
        CameraManager.Instance.ChangeTarget(cameraWatchPoint, cameraWatchPoint);
        yield return new WaitForSeconds(1f);

        collectParticles.Play();
        animator.Play("PickUp");

        yield return new WaitForSeconds(4f);
        CameraManager.Instance.ChangeTarget(Player.Instance.gameObject.transform, Player.Instance.gameObject.transform);
        chasePlayer = true;

        yield break;
    }
    private void PickedUp()
    {
        Player.Instance.playerDialogue.DisplayDialogue(pickupText, 10f);
        GameDataManager.Instance.DATA.canistersCollected++;
    }
}
