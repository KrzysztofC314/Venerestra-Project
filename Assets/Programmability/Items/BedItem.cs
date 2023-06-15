using System;

public class BedItem : ItemBehaviour
{
    public bool canGoToSleep = false;
    public string prompt = "I slept too much already, and what came out of it? I missed my lectures. Again.";

    protected override void UseItem(PlayableObject sender)
    {
        if (canGoToSleep)
        {
            throw new NotImplementedException();
        }
        else
        {
            PlayerMovement.Instance.Prompt(prompt);
        }
    }
}
