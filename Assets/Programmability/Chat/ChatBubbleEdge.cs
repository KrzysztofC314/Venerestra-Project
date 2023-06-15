
using UnityEngine;

public class ChatBubbleEdge : MonoBehaviour, IChatTile
{
    public void SetScale(Vector2 scale)
    {
        int angle = (int)transform.rotation.eulerAngles.z % 360;
        var scaleAndDistance = angle % 180 == 0 ? (scale.x, scale.y) : (scale.y, scale.x);
        float distance = IChatTile.baseSize * (scaleAndDistance.Item2 + 1) * .5f;
        transform.localScale = new Vector3(scaleAndDistance.Item1, 1, 1);
        switch (angle)
        {
            case 0:
                transform.localPosition = new Vector3(0, distance, 0);
                break;
            case 90:
                transform.localPosition = new Vector3(-distance, 0, 0);
                break;
            case 180:
                transform.localPosition = new Vector3(0, -distance, 0);
                break;
            default:
                transform.localPosition = new Vector3(distance, 0, 0);
                break;
        }
    }
}
