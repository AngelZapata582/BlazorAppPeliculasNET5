using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BlazorAppPeliculasNET5.Client.Helpers
{
    public static class NavigationManagerxtensions
    {
        public static Dictionary<string, string> ObtenerQueryStrings(this NavigationManager navigation, string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !url.Contains("?") || url.Substring(url.Length - 1) == "?")
            {
                return null;
            }

            var queryString = url.Split(new string[] { "?" }, StringSplitOptions.None)[1];
            var dicQueryString = queryString.Split("&")
                .ToDictionary(c => c.Split("=")[0],
                              c => Uri.UnescapeDataString(c.Split("=")[1]));

            return dicQueryString;
        }
    }
}
