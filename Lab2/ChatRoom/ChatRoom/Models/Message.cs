using System;
using System.ComponentModel.DataAnnotations;

public class Message
{
    [Key]
    public int MessageId { get; set; }

    [Required]
    public string User { get; set; }

    [Required]
    public string MessageContent { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    public string ChatRoom { get; set; }
}