using Blazored.SessionStorage;

using Microsoft.JSInterop;

namespace AttrectoTest.BlazorServer.Helpers;

public static class SessionStorageExtensions
{
    public static bool JSRuntimeAvailable(this ISessionStorageService storage)
    {
        try
        {
            // Ha a IJSRuntime null vagy prerender fázis → dobna exception
            var jsRuntime = storage.GetType()
                                   .GetProperty("JSRuntime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                   ?.GetValue(storage) as IJSRuntime;

            return jsRuntime is not null;
        }
        catch
        {
            return false;
        }
    }
}
