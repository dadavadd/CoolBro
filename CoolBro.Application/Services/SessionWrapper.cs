using CoolBro.Domain.Entities;
using CoolBro.Infrastructure.Data.Interfaces;
using System.Text.Json;

namespace CoolBro.Application.Services;

public class SessionWrapper(
    ISessionRepository sessionRepository,
    State userState)
{
    private static Dictionary<string, string> StateData = new();

    public T Get<T>(string key)
    {
        var data = StateData;
        if (!data.ContainsKey(key))
            throw new KeyNotFoundException($"Key {key} not found in state data");

        return JsonSerializer.Deserialize<T>(data[key])!;
    }

    public T? GetOrDefault<T>(string key)
    {
        var data = StateData;
        return data.ContainsKey(key)
            ? JsonSerializer.Deserialize<T>(data[key])
            : default;
    }

    public List<T> GetList<T>(string key)
    {
        var value = GetOrDefault<List<T>>(key);
        return value ?? new List<T>();
    }

    public void Set<T>(string key, T value)
    {
        var data = StateData;
        data[key] = JsonSerializer.Serialize(value);
        SaveStateData(data);
    }

    public void SetList<T>(string key, IEnumerable<T> list)
    {
        Set(key, list.ToList());
    }

    public void Remove(params string[] keys)
    {
        var data = StateData;
        foreach (var key in keys)
            data.Remove(key);
        SaveStateData(data);
    }

    public void Clear()
    {
        SaveStateData(new Dictionary<string, string>());
    }

    public async Task SaveAsync()
    {
        await sessionRepository.SetUserSessionAsync(userState);
    }

    public bool ContainsKey(string key) => StateData.ContainsKey(key);

    private void SaveStateData(Dictionary<string, string> data)
    {
        userState.StateData = JsonSerializer.Serialize(data);
    }
}
