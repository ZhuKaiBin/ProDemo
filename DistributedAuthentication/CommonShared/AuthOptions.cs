namespace CommonShared
{
    public static class AuthOptions
    {
        public const string Issuer = "MyAuthServer";
        public const string SecretKey = "super_secret_key_123!qwqedwewqerfwerwerq3q3"; // 至少16字符
        public static readonly string[] Audiences = { "WebA", "WebB" };
    }
}
