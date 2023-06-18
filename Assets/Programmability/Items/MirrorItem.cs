public class MirrorItem : ItemBehaviour
{
    public string onActivatePrompt;
    public float time;

    protected override void UseItem(PlayableObject sender)
    {
        PlayerMovement.Instance.Prompt(onActivatePrompt, time);
        PlayerMovement.Instance.TurnBack(time);
    }
}
