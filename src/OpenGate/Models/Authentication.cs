namespace OpenGate.Models
{
    public class AuthenticationPayload
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }

    public class AuthenticationResponse
    {
        public bool success { get; set; }
    }
}
