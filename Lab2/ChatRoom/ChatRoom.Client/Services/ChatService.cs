using Microsoft.AspNetCore.SignalR.Client;

namespace ChatRoom.Services
{
    public class ChatService
    {
        private HubConnection _connection;

        public event Action<string> OnMessageReceived;

        public async Task StartConnection(string roomName, string username)
        {
            if (_connection == null)
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5001/chathub") // Update the URL if necessary
                    .Build();

                // Handle incoming messages from the server
                _connection.On<string>("ReceiveMessage", (message) =>
                {
                    OnMessageReceived?.Invoke(message); // Raise the event to notify the Blazor component
                });
            }

            // Start the connection if it's disconnected
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }

            // After starting, join the specified room
            await JoinRoom(roomName, username);
        }

        public async Task JoinRoom(string roomName, string username)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("JoinRoom", roomName, username);
            }
        }

        public async Task SendMessage(string roomName, string username, string message)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendMessage", roomName, username, message);
            }
        }

        public async Task LeaveRoom(string roomName, string username)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("LeaveRoom", roomName, username);
            }
        }
    }
}