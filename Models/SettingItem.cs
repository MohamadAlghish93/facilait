namespace FacilaIT.Models
{
    public class SettingItem
    {

        public string ConnectionStringOra { get; set; }

        public string ConnectionStringSql { get; set; }

        public string OraPassword { get; set; }

        public string SQLPassword { get; set; }

        public bool InternalDB { get; set; } = false;

        public List<IntegrationItem> Integration { get; set; }
    }

    public class IntegrationItem
    {
        public string Name { get; set; }

        public string UrlHooks { get; set; }

        public string Token { get; set; }

        public bool Enabled { get; set; } = false;
    }
}