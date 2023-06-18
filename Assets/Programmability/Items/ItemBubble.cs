using TMPro;
using UnityEngine;

public class ItemBubble : MonoBehaviour
{
    public float offsetX = .423f;
    public float offsetY = .478f;
    private Transform textMeshTransform;

    public void Initialize(string prompt)
    {
        var bubble = GetComponentInChildren<ChatBubble>();
        bubble.Initialize(prompt);
        var textMesh = bubble.GetComponentInChildren<TMP_Text>();
        textMeshTransform = textMesh.transform;
        //var bubbles = GetComponentInChildren<ItemBubbleBubbles>();
        bubble.transform.localPosition = textMesh.textBounds.extents + new Vector3(offsetX, offsetY);
        //var callerItem = GetComponentInParent<ItemBehaviour>(); // zastanowić się czy można to zrobić bez tej linijki
        var itemCollider = /*callerItem.*/GetComponentInParent<Collider2D>();
        var corners = GetCorners(itemCollider);
        var extents = bubble.transform.localPosition + textMesh.textBounds.extents;
        if (IsOutOfBounds(corners[3] + extents.y, true))
            Flip(true);
        if (IsOutOfBounds(corners[1] + extents.x, false))
            Flip(false);
    }

    private void Flip(bool vertically)
    {
        transform.localScale = new Vector3(transform.localScale.x * (vertically ? 1 : -1), transform.localScale.y * (vertically ? -1 : 1));
        textMeshTransform.localScale = new Vector3(textMeshTransform.localScale.x * (vertically ? 1 : -1), textMeshTransform.localScale.y * (vertically ? -1 : 1));
    }

    private float[] GetCorners(Collider2D collider) // xmin, xmax, ymin, ymax
    {
        if (collider is BoxCollider2D)
        {
            var boxCollider = collider as BoxCollider2D;
            var center = boxCollider.transform.position;
            var extentX = boxCollider.size.x / 2f;
            var extentY = boxCollider.size.y / 2f;
            return new[]
            {
                center.x - extentX
                , center.x + extentX
                , center.y - extentY
                , center.y + extentY
            };
        }
        if (collider is CircleCollider2D)
        {
            var circleCollider = collider as CircleCollider2D;
            var center = circleCollider.transform.position;
            var extent = circleCollider.radius / Mathf.Sqrt(2);
            return new[]
            {
                center.x - extent
                , center.x + extent
                , center.y - extent
                , center.y + extent
            };
        }
        return new[] { collider.transform.position.x, collider.transform.position.x, collider.transform.position.y, collider.transform.position.y };
    }

    private bool IsOutOfBounds(float dimension, bool vertically)
    {
        var cameraExtent = vertically ? CameraFollow.Instance.height / 200f : CameraFollow.Instance.width / 200f;
        return dimension > cameraExtent || dimension < -cameraExtent;
    }
}
