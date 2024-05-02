namespace FacilaIT.Models.RABC
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Supervisor = "Supervisor";

        public const string SettingsPage = "SettingsPage";
        public const string DesignPage = "DesignPage";
        public const string LogPage = "LogPage";
        public const string QueryPage = "QueryPage";
        public const string ServicePage = "ServicePage";
        public const string SysMonitorPage = "SysMonitorPage";
        public const string UserPage = "UserPage";
        public const string ClientPage = "ClientPage";


        public static List<string> GetListRoles()
        {
            List<string> ListRoles = new List<string>();
            ListRoles.Add(Admin);
            ListRoles.Add(User);
            ListRoles.Add(Supervisor);
            ListRoles.Add(SettingsPage);
            ListRoles.Add(DesignPage);
            ListRoles.Add(LogPage);
            ListRoles.Add(QueryPage);
            ListRoles.Add(ServicePage);
            ListRoles.Add(SysMonitorPage);
            ListRoles.Add(UserPage);
            ListRoles.Add(ClientPage);

            return ListRoles;
        }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<User>? Users { get; set; }
    }

    public class RoleUser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string>? Roles { get; set; }
    }
}