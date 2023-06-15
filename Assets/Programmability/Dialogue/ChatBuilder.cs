using System.Collections.Generic;

public class ChatBuilder
{
    public static IEnumerable<Chat> Build(IEnumerable<ChatDto> chatDtos)
    {
        var list = new List<Chat>();
        foreach (var chatDto in chatDtos)
        {
            var chat = new Chat(chatDto.filePath, chatDto.chatName);
            chat.Initialize();
            list.Add(chat);
        }
        return list;
    }
}
