namespace ChatRoom.Models
{
    public class ChatRoomEntity
    {
        public int ChatRoomEntityId { get; set; }
        public string ChatName { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}