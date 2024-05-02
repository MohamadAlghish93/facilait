using FacilaIT.Helper.Shared;
using FacilaIT.Models;
using FacilaIT.Models.RABC;
using Microsoft.Extensions.Caching.Memory;

namespace FacilaIT.Business
{
    public class SettingBusiness
    {

        public SettingBusiness() { }

        public static bool SettingInternalDB() {

            //
            string cacheKeySettings = Constant.Cache_General_AppSettings;
            bool InternalDB = false;
           
            //
            SettingItem? cachedDataSetting = new SettingItem();
            if (!CacheManager.Cache.TryGetValue(cacheKeySettings, out cachedDataSetting))
            {
                InternalDB = false;
            }
            else
            {
                if (cachedDataSetting != null)
                {
                    InternalDB = cachedDataSetting.InternalDB;
                }
            }
            //
            return InternalDB;
        }
    }
}
