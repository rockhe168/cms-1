﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SiteServer.CMS.Core;
using SiteServer.Plugin;

namespace SiteServer.Cli.Updater.Model40
{
    public partial class TableConfig
    {
        [JsonProperty("isInitialized")]
        public string IsInitialized { get; set; }

        [JsonProperty("databaseVersion")]
        public string DatabaseVersion { get; set; }

        [JsonProperty("restrictionBlackList")]
        public string RestrictionBlackList { get; set; }

        [JsonProperty("restrictionWhiteList")]
        public string RestrictionWhiteList { get; set; }

        [JsonProperty("isRelatedUrl")]
        public string IsRelatedUrl { get; set; }

        [JsonProperty("rootUrl")]
        public string RootUrl { get; set; }

        [JsonProperty("updateDate")]
        public DateTimeOffset UpdateDate { get; set; }

        [JsonProperty("settingsXML")]
        public string SettingsXml { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class TableConfig
    {
        public const string OldTableName = "Config";

        public static ConvertInfo Converter => new ConvertInfo
        {
            NewTableName = NewTableName,
            NewColumns = NewColumns,
            ConvertKeyDict = ConvertKeyDict,
            ConvertValueDict = ConvertValueDict
        };

        private static readonly string NewTableName = DataProvider.ConfigDao.TableName;

        private static readonly List<TableColumn> NewColumns = DataProvider.ConfigDao.TableColumns;

        private static readonly Dictionary<string, string> ConvertKeyDict = null;

        private static readonly Dictionary<string, string> ConvertValueDict = null;
    }
}
