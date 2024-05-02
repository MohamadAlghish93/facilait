using Microsoft.Extensions.Caching.Memory;
using FacilaIT.Helper.Shared;
using FacilaIT.Models;
using System.Web;

namespace FacilaIT.Business
{
    public class NotifyBusiness
    {

        private List<IntegrationItem> _listIntegration;

        public NotifyBusiness()
        {
            string cacheKeySettings = Constant.Cache_General_AppSettings;
            SettingItem? cachedDataSetting = new SettingItem();
            if (!CacheManager.Cache.TryGetValue(cacheKeySettings, out cachedDataSetting))
            {
                _listIntegration = new List<IntegrationItem>();
            }
            else
            {
                if (cachedDataSetting != null)
                {
                    _listIntegration = cachedDataSetting.Integration;
                }
            }
        }

        public void Process(string message)
        {
            try
            {
                foreach (var item in _listIntegration)
                {
                    if (item.Enabled && item.Name == "Slack")
                    {
                        // System.Console.WriteLine(item.UrlHooks);
                        SlackHooks(item.UrlHooks, message);
                    }
                    if (item.Enabled && item.Name == "Telegram")
                    {
                        // System.Console.WriteLine(item.UrlHooks);
                        TelegramHooks(item.UrlHooks, message);
                    }
                    if (item.Enabled && item.Name == "MSTeams")
                    {
                        // System.Console.WriteLine(item.UrlHooks);
                        TeamsHooks(item.UrlHooks, message);
                    }
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public async void SlackHooks(string url, string message)
        {
            ProxyBusiness pb = new ProxyBusiness();
            string response = null;

            try
            {
                List<CustomHeader> headers = new List<CustomHeader>();
                string payload = "{" + $"'text': '{message}'" + "}";
                // System.Console.WriteLine(payload);
                response = await pb.PostAsync(url, payload, "", headers);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async void TelegramHooks(string url, string message)
        {
            ProxyBusiness pb = new ProxyBusiness();
            string response = null;
            string encodedString = HttpUtility.UrlEncode(message);
            url = $"{url}&text={encodedString}";

            try
            {
                response = await pb.GetAsync(url, ""); // TODO Check response.IsSuccessStatusCode 
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async void TeamsHooks(string url, string message)
        {
            ProxyBusiness pb = new ProxyBusiness();
            string response = null;

            try
            {
                List<CustomHeader> headers = new List<CustomHeader>();
                string payload = "{" + $"'text': '{message}'" + "}";
                // System.Console.WriteLine(payload);
                response = await pb.PostAsync(url, payload, "", headers);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}