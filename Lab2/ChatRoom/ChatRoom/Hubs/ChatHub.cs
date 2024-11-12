using ChatRoom.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string roomName, string username, string messageContent)
    {
        // Save the message to the database
        var message = new Message
        {
            User = username,
            MessageContent = messageContent,
            Timestamp = DateTime.UtcNow,
            ChatRoom = roomName
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // Broadcast the message to the chat room
        await Clients.Group(roomName).SendAsync("ReceiveMessage", $"{username}: {messageContent}");
    }

    public async Task JoinRoom(string roomName, string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        await LoadMessages(roomName);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", $"{username} has joined the room.");
    }

    public async Task LeaveRoom(string roomName, string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", $"{username} has left the room.");
    }

    public async Task LoadMessages(string roomName)
    {
        var messages = await _context.Messages
            .Where(m => m.ChatRoom == roomName)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        foreach (var message in messages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", $"{message.Timestamp.ToShortTimeString()} {message.User}: {message.MessageContent}");
        }
    }
}