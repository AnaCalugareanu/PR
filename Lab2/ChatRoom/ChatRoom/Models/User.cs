namespace ChatRoom.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<ChatRoomEntity> ChatRooms { get; set; } = new();
    }
}