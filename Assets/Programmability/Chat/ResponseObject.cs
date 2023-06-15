using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseObject : MonoBehaviour
{
    private const int maxTypeTime = 200;
    private int updateTime => randomTime();
    private DateTime lastUpdate = new(2001, 1, 1);
    public Response response;
    private TMP_Text textBox;
    private Action Run;
    public static bool writing = false;
    public static ResponseObject ActiveResponse;

    // Start is called before the first frame update
    void Start()
    {
        textBox = GetComponent<TMP_Text>();
        Run = WriteUpdate;
        writing = true;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    private int randomTime()
    {
        var random = new System.Random();
        return random.Next(maxTypeTime);
    }

    private void WriteUpdate()
    {
        if (textBox.text == response.Message)
        {
            Run = () => { };
            writing = false;
            return;
        }
        if (DateTime.Now.Subtract(lastUpdate).TotalMilliseconds > updateTime)
        {
            textBox.text += response.Message[textBox.text.Length];
            lastUpdate = DateTime.Now;
            GetComponentInChildren<Collider2D>().transform.localScale = new Vector3(1, textBox.bounds.size.y, 1);
        }
    }

    public void Activate()
    {
        if (ActiveResponse != null && !ReferenceEquals(this, ActiveResponse))
            Deactivate();
        textBox.color = Color.white;
        ActiveResponse = this;
    }

    public static void Deactivate()
    {
        if (ActiveResponse == null)
            return;
        ActiveResponse.textBox.color = new Color(.2031f, .0353f, .1537f);
        ActiveResponse = null;
    }

    public void EndWriting()
    {
        textBox.text = response.Message;
        writing = false;
    }
}
