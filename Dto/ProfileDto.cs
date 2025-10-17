namespace ClawSome.Dto;

public class ProfileDto
{
    public string Status { get; set; } = "success";
    public UserDto User { get; set; }
    public string Timestamp { get; set; } = DateTimeOffset.UtcNow.ToString("o");
    public string Fact { get; set; }
}
