
using System;
using System.IO;
using Newtonsoft.Json;
using FacilaIT.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using FacilaIT.Models.RABC;


namespace FacilaIT.Helper.Shared;

public class Startup
{
    private readonly IMemoryCache _cache;


    public Startup(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void ReadSettings()
    {

        ReadSettingsGeneral();
        ReadRoles();
        CacheManager.Cache = _cache;
    }



    public void ReadSettingsGeneral()
    {

        string fileName = "GeneralSettings.json";
        string cacheKey = Constant.Cache_General_AppSettings;
        Cachesetting<SettingItem>(fileName, cacheKey);
    }



    public void ReadRoles()
    {

        string fileName = "Roles.json";
        string cacheKey = Constant.Cache_Roles_AppSettings;
        Cachesetting<List<Role>>(fileName, cacheKey);
    }


    public void Cachesetting<T>(string fileName, string cacheKey)
    {

        string directorySetting = "Settings";
        string currentFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // AppDomain.CurrentDomain.BaseDirectory
        System.Console.WriteLine(currentFilePath);
        string path = Path.Combine(currentFilePath, directorySetting, fileName);
        T cachedData;

        try
        {
            if (!_cache.TryGetValue(cacheKey, out cachedData))
            {

                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    T data = JsonConvert.DeserializeObject<T>(json);

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(24));
                    _cache.Set(cacheKey, data, cacheOptions);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("The file '{0}' was not found.", ex.FileName);
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine("The directory for the file '{0}' was not found.", path);
        }
        catch (IOException ex)
        {
            Console.WriteLine("An I/O error occurred while reading the file '{0}': {1}", path, ex.Message);
        }
        catch (JsonException ex)
        {
            Console.WriteLine("An error occurred while deserializing the JSON file: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred: {0}", ex.Message);
        }
    }



    public bool SaveSetting(string fileName, object objSeriliaze)
    {
        try
        {
            string directorySetting = "Settings";
            string currentFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // AppDomain.CurrentDomain.BaseDirectory
            System.Console.WriteLine(currentFilePath);
            string filePath = Path.Combine(currentFilePath, directorySetting, fileName);

            // Convert the object to JSON
            string jsonString = JsonConvert.SerializeObject(objSeriliaze);

            // Save the JSON data to a file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(jsonString);
            }

            ReadSettingsGeneral();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occurred while saving JSON file: " + ex.Message);
            return false;
        }
    }
}