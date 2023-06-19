using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject healthBar;
    public string[] lines;
    public float textSpeed;
    [SerializeField] private enum Character
    {
        Elli,
        MissionControl,
        None,
    }

    [SerializeField] private Character[] character;
    [SerializeField] GameObject elliDialogueBox;
    [SerializeField] GameObject missionControlDialogueBox;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetActive(false);
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        ShowBox();
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            ShowBox();
        }
        else
        {
            dialogueBox.SetActive(false);
            healthBar.SetActive(true);
            elliDialogueBox.SetActive(false);
            missionControlDialogueBox.SetActive(false);
        }
    }

    void ShowBox()
    {
        switch (character[index])
        {
            default:
            case Character.None:
                elliDialogueBox.SetActive(false);
                missionControlDialogueBox.SetActive(false);
                break;
            case Character.Elli:
                missionControlDialogueBox.SetActive(false);
                elliDialogueBox.SetActive(true);
                break;
            case Character.MissionControl:
                elliDialogueBox.SetActive(false);
                missionControlDialogueBox.SetActive(true);
                break;
        }
    }
}
