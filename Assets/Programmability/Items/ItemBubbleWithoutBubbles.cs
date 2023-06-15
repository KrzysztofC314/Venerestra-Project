using TMPro;
using UnityEngine;

public class ItemBubbleWithoutBubbles : MonoBehaviour
{
    public float offsetX = 0;
    public float offsetY = 0;
    public const float basePositionY = .5f;

    public void Initialize(string prompt)
    {
        var bubble = GetComponentInChildren<ChatBubble>();
        bubble.Initialize(prompt);
        var textMesh = bubble.GetComponentInChildren<TMP_Text>();
        bubble.transform.position = new Vector3(offsetX + GetComponentInParent<Transform>().position.x, offsetY + basePositionY);
    }
}
