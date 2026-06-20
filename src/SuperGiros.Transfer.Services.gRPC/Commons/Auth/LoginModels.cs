namespace SuperGiros.Transfer.Services.gRPC.Commons.Auth
{
    public class LoginModels
    {
        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public bool IsSuccess { get; set; }
            public string Token { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
        }
    }
}
