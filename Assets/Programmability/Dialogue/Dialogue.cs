public class Dialogue
{
    public string Title { get; set; }
    public Person Sender { get; set; }
    public string Message { get; set; }
    public Response[] Responses { get; set; }
    public Dialogue PreviousMessageInThisPassage { get; set; }

    public static implicit operator Dialogue(string message) => new() { Message = message };
}
