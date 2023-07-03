namespace OpenGate.Models
{
    public class User
    {
        public readonly string Domain;
        public readonly int Port;

        public readonly string[] SubDomains;

        public string Name { get; set; } = string.Empty;
        
        public string DomainName => this.ToDomainName();

        public string FullId => $"{Name}@{Domain}";

        public bool IsValid { get; set; }

        public User(string domain, int port = 389)
        {
            SubDomains = domain.Split('.');
            Domain = SubDomains[^2];
            Port = port;
        }

        public override string ToString() => $"{Name}@{Domain}";
    }
}
