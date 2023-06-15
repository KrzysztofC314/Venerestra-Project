using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
    public static ItemBehaviour ActivatedItem;
    private readonly LayerMask PlayerLayer = 3;
    public float radius;
    private bool Activated = false;
    private GameObject itemBubblePrefab => GameController.Instance.itemBubblePrefab;
    private static GameObject bubbleInstance;
    public string bubblePrompt = "Use item";
    private const int itemsLayerMask = 9;

    void Start()
    {
        gameObject.layer = itemsLayerMask;
    }

    /*void Update()
    {
        if (Activated)
        {
            Activated = IsActivated();
            if (!Activated)
            {
                SetActivated(false);
            }
        }
        else
        {
            Activated = IsActivated();
            if (Activated)
            {
                if (ActivatedItem == null)
                {
                    SetActivated(true);
                }
                else
                {
                    if (GetDistanceToPlayer() < ActivatedItem.GetDistanceToPlayer())
                    {
                        ActivatedItem.SetActivated(false);
                        SetActivated(true);
                    }
                    else
                    {
                        Activated = false;
                    }
                }
            }
        }
    }*/

    /*void SetActivated(bool activated)
    {
        ActivatedItem = activated ? this : null;
        if (activated)
        {
            bubbleInstance = Instantiate(itemBubblePrefab, transform);
            bubbleInstance.GetComponent<ItemBubble>().Initialize(bubblePrompt);
        }
        else
        {
            Destroy(bubbleInstance);
        }
    }*/

    /*Collider2D GetOverlappingPlayer()
    {
        return Physics2D.OverlapCircle(transform.position, radius, PlayerLayer);
    }*/

    /*float GetDistanceToPlayer()
    {
        var player = GetOverlappingPlayer();
        if (player == null)
        {
            return float.MaxValue;
        }
        return (player.transform.position - transform.position).magnitude;
    }*/

    /*bool IsActivated()
    {
        return GetOverlappingPlayer() != null;
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        if (!Activated)
        {
            Activated = true;
            ActivatedItem = this;
            bubbleInstance = Instantiate(itemBubblePrefab, transform);
            bubbleInstance.GetComponent<ItemBubbleWithoutBubbles>().Initialize(bubblePrompt);
        }
    }

    public static void Deactivate()
    {
        if (ActivatedItem != null)
        {
            ActivatedItem.Activated = false;
            ActivatedItem = null;
        }
        if (bubbleInstance != null)
            Destroy(bubbleInstance);
    }

    public static void Use(PlayableObject sender)
    {
        if (ActivatedItem != null)
            ActivatedItem.UseItem(sender);
    }

    protected abstract void UseItem(PlayableObject sender);
}
