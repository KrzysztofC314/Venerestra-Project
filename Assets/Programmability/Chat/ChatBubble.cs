using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubble : MonoBehaviour
{
    public Dialogue Dialogue;
    public float height;

    public void Initialize(Dialogue dialogue)//, GameObject chat)
    {
        if (dialogue == null)
            return;
        Dialogue = dialogue;
        var messageTextMesh = GetComponentInChildren<Message>().GetComponent<TMP_Text>();
        messageTextMesh.text = Dialogue.Message;
        messageTextMesh.ForceMeshUpdate();
        if (!string.IsNullOrWhiteSpace(dialogue.Sender) && dialogue.Sender != Person.player)
        {
            var senderTextMesh = GetComponentInChildren<Sender>().GetComponent<TMP_Text>();
            senderTextMesh.text = Dialogue.Sender;
            senderTextMesh.ForceMeshUpdate();
            /*var x1 = messageTextMesh.textBounds.extents.x;
            var x2 = -transform.parent.position.x;
            var y1 = transform.position.y;
            var y2 = messageTextMesh.textBounds.extents.y;*/
            senderTextMesh.transform.localPosition = new Vector3((senderTextMesh.rectTransform.sizeDelta.x - messageTextMesh.textBounds.size.x) / 2f, messageTextMesh.textBounds.extents.y + IChatTile.baseSize, 0);
        }
        else
        {
            var sender = GetComponentInChildren<Sender>();
            if (sender != null)
            {
                Destroy(sender.gameObject);
            }
        }
        var scale = messageTextMesh.textBounds.size / IChatTile.baseSize;
        SetScale(scale);
        messageTextMesh.transform.position = transform.position;
        height = messageTextMesh.textBounds.size.y + 2 * IChatTile.baseSize;
    }

    public void SetScale(Vector2 scale)
    {
        var tiles = GetComponentsInChildren<IChatTile>();
        foreach (var tile in tiles)
        {
            tile.SetScale(scale);
        }
    }
}
