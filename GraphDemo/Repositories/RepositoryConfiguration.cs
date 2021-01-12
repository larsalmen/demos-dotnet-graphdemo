namespace GraphDemo.Repositories
{
    public class RepositoryConfiguration
    {
        public string Hostname { get; set; }
        public string Port { get; set; }
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string Container { get; set; }
        public string Username => $"/dbs/{Database}/colls/{Container}";
    }
}
