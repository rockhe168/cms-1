﻿using System;
using System.Collections.Generic;
using SiteServer.Cli.Core;
using SiteServer.Utils;
using SiteServer.Cli.Updater.Model36;
using SiteServer.Cli.Updater.Plugins.GovInteract;
using SiteServer.Cli.Updater.Plugins.GovPublic;

namespace SiteServer.Cli.Updater
{
    public class Updater36 : UpdaterBase
    {
        public const string Version = "3.6";

        public Updater36(TreeInfo oldTreeInfo, TreeInfo newTreeInfo) : base(oldTreeInfo, newTreeInfo)
        {
        }

        public override bool UpdateTableInfo(string oldTableName, TableInfo oldTableInfo, List<string> contentTableNameList, out string newTableName, out TableInfo newTableInfo)
        {
            ConvertInfo converter = null;

            var oldTableNameWithoutPrefix = oldTableName.Substring(oldTableName.IndexOf("_", StringComparison.Ordinal) + 1);

            if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableAdministrator.OldTableName))
            {
                converter = TableAdministrator.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableAdministratorsInRoles.OldTableName))
            {
                converter = TableAdministratorsInRoles.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableArea.OldTableName))
            {
                converter = TableArea.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableChannel.OldTableName))
            {
                converter = TableChannel.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableChannelGroup.OldTableName))
            {
                converter = TableChannelGroup.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableConfig.OldTableName))
            {
                converter = TableConfig.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableContentCheck.OldTableName))
            {
                converter = TableContentCheck.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableContentGroup.OldTableName))
            {
                converter = TableContentGroup.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableCount.OldTableName))
            {
                converter = TableCount.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableDbCache.OldTableName))
            {
                converter = TableDbCache.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableDepartment.OldTableName))
            {
                converter = TableDepartment.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableErrorLog.OldTableName))
            {
                converter = TableErrorLog.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableKeyword.OldTableName))
            {
                converter = TableKeyword.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableLog.OldTableName))
            {
                converter = TableLog.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TablePermissionsInRoles.OldTableName))
            {
                converter = TablePermissionsInRoles.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableRelatedField.OldTableName))
            {
                converter = TableRelatedField.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableRelatedFieldItem.OldTableName))
            {
                converter = TableRelatedFieldItem.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableRole.OldTableName))
            {
                converter = TableRole.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableSite.OldTableName))
            {
                converter = TableSite.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableSiteLog.OldTableName))
            {
                converter = TableSiteLog.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableSitePermissions.OldTableName))
            {
                converter = TableSitePermissions.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTable.OldTableName))
            {
                converter = TableTable.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTableMetadata.OldTableName))
            {
                converter = TableTableMetadata.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTableStyle.OldTableName))
            {
                converter = TableTableStyle.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTableStyleItem.OldTableName))
            {
                converter = TableTableStyleItem.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTag.OldTableName))
            {
                converter = TableTag.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTemplate.OldTableName))
            {
                converter = TableTemplate.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableTemplateMatch.OldTableName))
            {
                converter = TableTemplateMatch.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableUser.OldTableName))
            {
                converter = TableUser.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractChannel.OldTableName))
            {
                converter = TableGovInteractChannel.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractLog.OldTableName))
            {
                converter = TableGovInteractLog.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractPermissions.OldTableName))
            {
                converter = TableGovInteractPermissions.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractRemark.OldTableName))
            {
                converter = TableGovInteractRemark.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractReply.OldTableName))
            {
                converter = TableGovInteractReply.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovInteractType.OldTableName))
            {
                converter = TableGovInteractType.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovPublicCategory.OldTableName))
            {
                converter = TableGovPublicCategory.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovPublicCategoryClass.OldTableName))
            {
                converter = TableGovPublicCategoryClass.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovPublicIdentifierRule.OldTableName))
            {
                converter = TableGovPublicIdentifierRule.Converter;
            }
            else if (StringUtils.EqualsIgnoreCase(oldTableNameWithoutPrefix, TableGovPublicIdentifierSeq.OldTableName))
            {
                converter = TableGovPublicIdentifierSeq.Converter;
            }
            else if (StringUtils.ContainsIgnoreCase(contentTableNameList, oldTableName))
            {
                converter = TableContent.GetConverter(oldTableName, oldTableInfo.Columns);
            }

            return GetNewTableInfo(oldTableName, oldTableInfo, converter, out newTableName, out newTableInfo);
        }
    }
}
