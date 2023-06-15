using UnityEngine;

public class AquariumItem : ItemBehaviour
{
    public string promptIfNotFedYet = "Here you go, little one. Don’t betray me when the time comes.";
    public string promptIfAlreadyFed = "Nah, you already had enough. Don’t whine, you’ll get some later.";
    public bool hasBeenFed = false;

    protected override void UseItem(PlayableObject sender)
    {
        PlayerMovement.Instance.Prompt(hasBeenFed ? promptIfAlreadyFed : promptIfNotFedYet);
        PlayerMovement.Instance.TurnBack(5);
        hasBeenFed = true;
    }
}
