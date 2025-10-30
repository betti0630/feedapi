namespace AttrectoTest.BlazorWeb.Services.Notification
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int UserId { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
