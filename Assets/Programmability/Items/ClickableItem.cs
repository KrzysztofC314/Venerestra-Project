using UnityEngine;

public class ClickableItem : MonoBehaviour
{
    public string afterClickPrompt = "Item has been clicked";
    public static ClickableItem ActivatedItem;

    public void Activate()
    {
        ActivatedItem = this;
        CursorSettings.Instance.SetCursorActive(true);
    }

    public static void Deactivate()
    {
        ActivatedItem = null;
        CursorSettings.Instance.SetCursorActive(false);
    }
}
