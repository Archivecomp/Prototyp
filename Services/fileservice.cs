using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Text.Json;

namespace neuesmodell.Services;
public static class FileService
{
    private static readonly string fileName = "userdata.json";

    public static async Task<UserCollection> LoadUsersAsync()
    {
        try
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            string json = await FileIO.ReadTextAsync(file);
            return JsonSerializer.Deserialize<UserCollection>(json) ?? new UserCollection();
        }
        catch
        {
            return new UserCollection();
        }
    }

    public static async Task SaveUsersAsync(UserCollection userCollection)
    {
        string json = JsonSerializer.Serialize(userCollection);
        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(file, json);
    }
}
