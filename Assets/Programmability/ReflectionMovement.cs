//using Mono.Cecil.Cil;
using UnityEngine;

public class ReflectionMovement : MovableBackground
{
    public float scale = .9f;
    public float relativePositionY;
    private PlayerMovement player => PlayerMovement.Instance;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public LayerMask mirrorLayer;

    void Start()
    {
        transform.localScale = new Vector3(-player.transform.localScale.x * scale, player.transform.localScale.y * scale);
        player.transform.position = new Vector3(0, player.transform.position.y + relativePositionY, 0);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        windowPosition = player.transform.position.x;
        base.Update();
        animator.SetBool("brigidIsMoving", player.isMoving);
        animator.SetBool("brigidIsHeadedLeft", player.isHeadedLeft);
        animator.SetBool("brigidIsTurnedBack", player.isTurnedBack);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var mirror = collision.GetComponent<MirrorMarker>();
        if (mirror != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            PlayerMovement.Instance.leftMirrorZone = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var mirror = collision.GetComponent<MirrorMarker>();
        if (mirror != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, .3f);
            PlayerMovement.Instance.leftMirrorZone = true;
        }
    }
}
