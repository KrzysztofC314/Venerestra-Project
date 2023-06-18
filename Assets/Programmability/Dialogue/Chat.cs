
public class Chat
{
    public string filePath;
    public Dialogue firstMessage;
    public Person sender;
    public string chatName => ExtractChatName();
    public const int heightInList = 60;

    public Chat(string filePath, Person sender)
    {
        this.filePath = filePath;
        this.sender = sender;
    }

    public void Initialize()
    {
        var parser = new DialogueParser(filePath);
        firstMessage = parser.Parse();
    }

    private string ExtractChatName()
    {
        var lastIndex = filePath.LastIndexOf("/");
        var startIndex = filePath.LastIndexOf('.', lastIndex) + 1;
        return filePath.Substring(startIndex, lastIndex - startIndex);
    }
}
