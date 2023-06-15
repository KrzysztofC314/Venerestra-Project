using System;
using TMPro;
using UnityEngine;

public class Thought : MonoBehaviour
{
    //public Vector3 bubblesOffset;
    //public Vector3 mainBubbleOffset;
    public static float secondsShown;
    //private bool flipped = false;
    public float positionY;
    private float secondsToPass;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(GameController.useItemKey) || secondsToPass <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            secondsToPass -= Time.deltaTime;
        }
        /*var relativePositionX = transform.position.x - Camera.main.transform.position.x;
        if ((flipped && relativePositionX < -.5f) || (!flipped && relativePositionX > .5f))
            Flip();*/
    }

    public void Initialize(string prompt)
    {
        var textMesh = GetComponentInChildren<TMP_Text>();
        textMesh.text = prompt;
        textMesh.ForceMeshUpdate();
        transform.position = new(Camera.main.transform.position.x, positionY);
        secondsToPass = secondsShown;

        /*var bubble = GetComponentInChildren<ChatBubble>();
        bubble.Initialize(prompt);
        mainBubbleOffset += new Vector3(0, bubble.GetComponentInChildren<TMP_Text>().textBounds.extents.y);
        bubble.transform.localPosition += mainBubbleOffset;
        transform.localPosition = new Vector3(2 * IChatTile.baseSize, 2 * IChatTile.baseSize) + bubblesOffset + bubble.GetComponentInChildren<TMP_Text>().textBounds.extents;
        startTime = DateTime.Now;*/
    }

    /*public void Flip()
    {
        transform.localScale = new(-transform.localScale.x, transform.localScale.y);
        var textMeshTransform = GetComponentInChildren<TMP_Text>().transform;
        textMeshTransform.localScale = new(-textMeshTransform.localScale.x, textMeshTransform.localScale.y);
        transform.localPosition = new(-transform.localPosition.x, transform.localPosition.y);
        flipped = !flipped;
    }*/
}
