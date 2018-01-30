﻿using SiteServer.Utils;

namespace SiteServer.CMS.Controllers
{
    public class ApiRoutePlugin
    {
        public const string Route = "{pluginId}";
        public const string RouteAction = "{pluginId}/{name}";
        public const string RouteActionAndId = "{pluginId}/{name}/{id}";

        public static string GetUrl(string apiUrl, string pluginId, string name = "", string id = "")
        {
            apiUrl = PageUtils.Combine(apiUrl, Route);
            apiUrl = apiUrl.Replace("{pluginId}", pluginId);
            if (!string.IsNullOrEmpty(name))
            {
                apiUrl = PageUtils.Combine(apiUrl, name);
                if (!string.IsNullOrEmpty(id))
                {
                    apiUrl = PageUtils.Combine(apiUrl, id);
                }
            }
            return apiUrl;
        }
    }
}