public class ComputerItem : ItemBehaviour
{
    protected override void UseItem(PlayableObject sender)
    {
        sender.SwitchPlayable<Computer>();
    }
}
