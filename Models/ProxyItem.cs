namespace FacilaIT.Models
{
    public class ProxyItem
    {

        public string URI { get; set; }

        public string Method { get; set; }

        public string Token { get; set; }

        public string PostPayload { get; set; }


        public List<CustomHeader> Header { get; set; }

    }

    public class CustomHeader
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}