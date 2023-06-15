using UnityEngine;

public class ChatBubbleCorner : MonoBehaviour, IChatTile
{
    public void SetScale(Vector2 scale)
    {
        int angle = (int)transform.rotation.eulerAngles.z % 360;
        float distanceX = IChatTile.baseSize * (scale.x + 1) * .5f;
        float distanceY = IChatTile.baseSize * (scale.y + 1) * .5f;
        switch (angle)
        {
            case 0:
                transform.localPosition = new Vector3(distanceX, distanceY, 0);
                break;
            case 90:
                transform.localPosition = new Vector3(-distanceX, distanceY, 0);
                break;
            case 180:
                transform.localPosition = new Vector3(-distanceX, -distanceY, 0);
                break;
            default:
                transform.localPosition = new Vector3(distanceX, -distanceY, 0);
                break;
        }
    }
}
