using System;
using System.Collections.Generic;
#nullable enable
#pragma warning disable CS8618
[Serializable]
public class DayContext
{
    public ChatDto[] chatDtos;
    public IEnumerable<Chat> Chats => chats ??= ChatBuilder.Build(chatDtos);
    private IEnumerable<Chat>? chats;
}
#pragma warning restore CS8618