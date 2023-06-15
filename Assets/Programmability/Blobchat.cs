using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Blobchat : PlayableObject
{
    //public GameObject chatListElementPrefab;
    private Vector2 Place;
    public List<GameObject> chatList = new List<GameObject>(); 
    //public GameObject selectionPrefab;
    private GameObject selection;
    private ChatEntry ActiveElement;
    public float radius = 0.01f;
    public LayerMask elementLayerMask;
    public LayerMask responseLayerMask;
    public GameObject darkBubble;
    public GameObject lightBubble;
    private GameObject openedChat;
    private Dialogue lastMessage;
    private GameObject lastBubble;
    public GameObject responsePrefab;
    private DateTime timeToSwitch;
    private Action delayedSwitch;
    private float lowerScrollConstraint;
    private float upperScrollConstraint;
    private Vector3 shift;
    private Vector3 openedChatLocalScale;

    /*private GameObject Selection()
    {
        if (selection == null || selection.IsDestroyed())
            selection = Instantiate(selectionPrefab, transform);
        return selection;
    }*/

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Place = transform.position + new Vector3(0, (GameController.Instance.ResolutionY - Chat.heightInList) / 200f);
        /*foreach (var chat in dayContext.Chats)
        {
            InstantiateAndInitialize(chatListElementPrefab, chat);
        }*/
        CameraFollows = false;
    }

    private void Initialize(IEnumerable<GameObject> chatList, IEnumerable<Chat> chats)
    {
        var chatEntries = chatList.Select(c => c.GetComponent<ChatEntry>()).ToArray();
        foreach (var chat in chats)
        {
            chatEntries.Single(e => AreEqual(e.chatName, chat.chatName)).Initialize(chat);
        }
    }

    private bool AreEqual(string a, string b)
    {
        bool result = a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
        return result;
    }

    public override void Play()
    {
        base.Play();

        /*var vertices = GetComponent<SpriteRenderer>().sprite.vertices;
        var x = Math.Abs(vertices.Select(v => v.x).Sum() / 4.0f - vertices[0].x) * 2;
        var y = Math.Abs(vertices.Select(v => v.y).Sum() / 4.0f - vertices[0].y) * 2;
        Size = new Vector2(x, y);*/

        Run = MenuUpdate;
    }

    // Update is called once per frame
    private void MenuUpdate()
    {
        var pos = Input.mousePosition;
        var worldPos = (Vector2)(Camera.main.ScreenToWorldPoint(pos));
        var collider = Physics2D.OverlapCircle(worldPos, radius, elementLayerMask);
        Debug.Log($"{worldPos}, active: {ActiveElement != null}, collider: {collider != null}");

        if (collider != null)
        {
            var element = collider.GetComponent<ChatEntry>();
            if (element != null) 
                Activate(element);
        }
        else
        {
            Deactivate();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ActiveElement != null)
            {
                ActiveElement.Clicked();
            }
            else
            {
                collider = Physics2D.OverlapPoint(worldPos, elementLayerMask);
                if (collider != null)
                {
                    if (collider.GetComponent<ChatExitButton>() != null)
                    {
                        SwitchPlayable<Computer>();
                    }
                    else if (collider.GetComponent<ComputerExitButton>() != null)
                    {
                        SwitchPlayable<PlayerMovement>();
                    }
                }
            }
        }
    }

    public Action OpenChat(/*Chat chat*/)
    {
        return () =>
        {
            var chat = ActiveElement.chat;
            var siblingsCount = transform.childCount;
            for (int i = 0; i < siblingsCount; i++)
            {
                var sibling = transform.GetChild(i);
                if (sibling.GetComponent<ChatButton>() == null)
                    continue;
                sibling.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            ActiveElement.GetComponentInChildren<SpriteRenderer>().enabled = true;
            if (openedChat != null)
                Destroy(openedChat);
            shift = Vector3.zero;
            if (chat == null)
                return;
            shift = Vector3.zero;
            openedChat = new GameObject("openedChat");
            openedChat.transform.parent = GetComponentInChildren<ChatSpace>().transform;
            openedChat.transform.localPosition = Vector3.zero;
            openedChatLocalScale = openedChat.transform.localScale;
            openedChat.transform.localScale = Vector3.one;
            lowerScrollConstraint = openedChat.transform.position.y;
            CreateNewBubble(chat.firstMessage, true);

            Run = MenuUpdate;
            Run += ChatUpdate;
            Run += WriteUpdate;
        };
    }

    private void CreateNewBubble(Dialogue dialogue, bool isIncoming)
    {
        if (dialogue.PreviousMessageInThisPassage != null)
        {
            CreateNewBubble(dialogue.PreviousMessageInThisPassage, dialogue.PreviousMessageInThisPassage.Sender != Person.player);
        }
        if (string.IsNullOrWhiteSpace(dialogue.Message))
            return;
        lastBubble = Instantiate(isIncoming ? darkBubble : lightBubble, openedChat.transform);
        var bubbleInstance = lastBubble.GetComponent<ChatBubble>();
        bubbleInstance.Initialize(dialogue);//, openedChat);
        if (isIncoming) 
            lastMessage = dialogue;
        var addedShift = new Vector3(0, bubbleInstance.height, 0);
        shift += addedShift;// / openedChatLocalScale.y;
        openedChat.transform.localPosition = shift;// / openedChatLocalScale.y;
        lastBubble.transform.localPosition = NewBubblePosition(lastBubble, isIncoming) - shift;
        upperScrollConstraint = openedChat.transform.position.y;
    }

    private Vector3 NewBubblePosition(GameObject bubble, bool isIncoming)
    {
        var vector = 
             new Vector3(
                -GameController.Instance.resolutionX / 2 + IChatTile.baseSize
                , -GameController.Instance.resolutionY / 2 + IChatTile.baseSize
                , 0) 
            + bubble.GetComponentInChildren<Message>().GetComponent<TMP_Text>().textBounds.extents;
        if (!isIncoming)
        {
            vector = new Vector3(2 * bubble.transform.parent.position.x - vector.x, vector.y, vector.z);
        }
        return vector;
    }

    public void CloseChat()
    {
        Destroy(openedChat);
        Run = MenuUpdate;
    }

    private void Activate(ChatEntry element)
    {
        if (element != null)
        {
            ActiveElement = element;
            /*var position = element.transform.position;
            Selection().transform.position = position;*/
        }
    }

    private void Deactivate()
    {
        ActiveElement = null;
        //Destroy(Selection());
    }

    void ChatUpdate()
    {
        var worldPos = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var responseCollider = Physics2D.OverlapCircle(worldPos, radius, responseLayerMask);
        if (responseCollider != null)
        {
            var responseObject = responseCollider.GetComponentInParent<ResponseObject>();
            responseObject.Activate();
        }
        else
        {
            ResponseObject.Deactivate();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (ResponseObject.writing)
                EndWriting();
            else if (ResponseObject.ActiveResponse != null)
            {
                var message = ResponseObject.ActiveResponse.response.Message;
                var nextDialogue = ResponseObject.ActiveResponse.response.Action;
                var responseObjects = GetComponentsInChildren<ResponseObject>();
                foreach (var responseObject in responseObjects)
                {
                    openedChat.transform.position += new Vector3(0, -responseObject.GetComponent<TMP_Text>().textBounds.size.y);
                    Destroy(responseObject.gameObject);
                    ResponseObject.ActiveResponse = null;
                }
                CreateNewBubble(message, false);
                CreateNewBubble(nextDialogue, true);
                Run += DelayedSwitch(WriteUpdate, 500);
            }
        }
        var scroll = (Vector3)Input.mouseScrollDelta;
        if (scroll.y != 0)
        {
            openedChat.transform.position -= scroll;
            ApplyScrollConstraint();
        }
    }

    private void ApplyScrollConstraint()
    {
        if (openedChat.transform.position.y > upperScrollConstraint)
            openedChat.transform.position = new Vector3(openedChat.transform.position.x, upperScrollConstraint, openedChat.transform.position.z);
        else if (openedChat.transform.position.y < lowerScrollConstraint)
            openedChat.transform.position = new Vector3(openedChat.transform.position.x, lowerScrollConstraint, openedChat.transform.position.z);
    }

    private void EndWriting()
    {
        var responseObjects = GetComponentsInChildren<ResponseObject>();
        for (int i = responseObjects.Length; i < lastMessage.Responses.Length; i++)
        {
            ShowResponse(lastMessage.Responses[i]);
        }
        foreach (var responseObject in responseObjects)
        {
            responseObject.EndWriting();
        }
    }

    private Action DelayedSwitch(Action switchTo, float timeInMilliseconds)
    {
        timeToSwitch = DateTime.Now.AddMilliseconds(timeInMilliseconds);
        delayedSwitch = () =>
        {
            if (DateTime.Now > timeToSwitch)
            {
                Run -= delayedSwitch;
                Run += switchTo;
            }
        };
        return delayedSwitch;
    }

    void WriteUpdate()
    {
        var responseObjects = openedChat.GetComponentsInChildren<ResponseObject>();
        if (responseObjects.Length >= lastMessage.Responses.Length)
        {
            Run -= WriteUpdate;
            return;
        }
        if (lastMessage.Responses != null && !ResponseObject.writing) 
        {
            ShowResponse(lastMessage.Responses[responseObjects.Length]);
        }
    }

    private void ShowResponse(Response response)
    {
        var responseObject = Instantiate(responsePrefab, lastBubble.transform);
        responseObject.GetComponent<ResponseObject>().response = response;
        var text = responseObject.GetComponent<TMP_Text>();
        text.text = response.Message;
        text.ForceMeshUpdate();
        float height = text.textBounds.size.y;
        openedChat.transform.position += new Vector3(0, height, 0);
        text.text = string.Empty;
        responseObject.transform.position = GetComponentInChildren<ChatSpace>().transform.position + new Vector3(0, height - GameController.Instance.resolutionY / (2f * openedChatLocalScale.y), 0);
        text.ForceMeshUpdate();
        upperScrollConstraint = openedChat.transform.position.y;
    }

    private void InstantiateAndInitialize(GameObject obj, Chat chat)
    {
        var element = Instantiate(obj, Place, Quaternion.identity, transform);
        chatList.Add(element);
        var chatEntry = element.GetComponent<ChatEntry>();
        chatEntry.Initialize(chat);
        Place += new Vector2(0, -Chat.heightInList / 100f);
    }

    public override void Stop()
    {
        Destroy(gameObject);
    }
}
