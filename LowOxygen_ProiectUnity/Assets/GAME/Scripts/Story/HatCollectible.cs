using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HatCollectible : MonoBehaviour
{
    [SerializeField] private int hatId;
    [TextArea(2, 4)] [SerializeField] private string pickupText;
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
        if (GameDataManager.Instance.DATA.hasHat[hatId])
        {
            Destroy(gameObject);
        }

        animator = gameObject.GetComponent<Animator>();
        lightEmitter = gameObject.GetComponentInChildren<Light2D>();
    }
    private void Start()
    {
        playerRef = Player.Instance;
        spriteHolder.GetComponentInChildren<SpriteRenderer>().sprite = playerRef.playerHats.hatSprites[hatId];
    }
    private void FixedUpdate()
    {
        if (!chasePlayer)
            return;

        Vector3 dirToPlayer = playerRef.transform.position - gameObject.transform.position;

        gameObject.transform.position += new Vector3(dirToPlayer.normalized.x, dirToPlayer.normalized.y, 0f) * Time.fixedDeltaTime * speed;
        gameObject.transform.localEulerAngles += Vector3.forward * 15f;
        spriteHolder.transform.localScale = Vector3.one * Mathf.Clamp(dirToPlayer.magnitude / 5f, 0f, 1f);

        if (dirToPlayer.magnitude < 1)
        {
            chasePlayer = false;
            PickedUp();
            Destroy(gameObject, 0.05f);
        }

        speed += Time.fixedDeltaTime * 2f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doingCinematic)
        {
            StartCoroutine(CollectCinematic());
            doingCinematic = true;
        }
    }
    private IEnumerator CollectCinematic()
    {
        CameraManager.Instance.ChangeTarget(cameraWatchPoint, cameraWatchPoint);
        yield return new WaitForSeconds(1f);

        collectParticles.Play();
        animator.Play("PickUp");
       // CameraManager.Instance.ScreenShake(1f, 2.5f, CameraManager.FadeType.IN);

        yield return new WaitForSeconds(4f);
        CameraManager.Instance.ChangeTarget(Player.Instance.gameObject.transform, Player.Instance.gameObject.transform);
        chasePlayer = true;

        yield break;
    }
    private void PickedUp()
    {
        Player.Instance.playerDialogue.DisplayDialogue(pickupText, 10f);
        Player.Instance.frogHat.SetActive(true);

        GameDataManager.Instance.DATA.hasHat[hatId] = true;
        Player.Instance.playerHats.AddHat(hatId);
    }
}
