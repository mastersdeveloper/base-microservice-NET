namespace BASE.MICRONET.Cross.Token.Dir
{
    public class JwtOptions
    {
        public bool Enabled { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Expiration { get; set; }
    }
}
