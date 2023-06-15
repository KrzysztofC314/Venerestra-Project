using UnityEngine;

public class ChatBubbleBody : MonoBehaviour, IChatTile
{
    public void SetScale(Vector2 scale)
    {
        transform.localScale = scale;
    }
}
