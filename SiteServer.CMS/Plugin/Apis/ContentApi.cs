﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.Attributes;
using SiteServer.Plugin;
using TableColumn = SiteServer.Plugin.TableColumn;

namespace SiteServer.CMS.Plugin.Apis
{
    public class ContentApi : IContentApi
    {
        private ContentApi() { }

        private static ContentApi _instance;
        public static ContentApi Instance => _instance ?? (_instance = new ContentApi());

        public IContentInfo GetContentInfo(int siteId, int channelId, int contentId)
        {
            if (siteId <= 0 || channelId <= 0 || contentId <= 0) return null;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var tableName = ChannelManager.GetTableName(siteInfo, channelId);

            return DataProvider.ContentDao.GetContentInfo(tableName, contentId);
        }

        public List<IContentInfo> GetContentInfoList(int siteId, int channelId, string whereString, string orderString, int limit, int offset)
        {
            if (siteId <= 0 || channelId <= 0) return null;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var tableName = ChannelManager.GetTableName(siteInfo, channelId);

            return DataProvider.ContentDao.GetListByLimitAndOffset(tableName, channelId, whereString, orderString, limit, offset);
        }

        public int GetCount(int siteId, int channelId, string whereString)
        {
            if (siteId <= 0 || channelId <= 0) return 0;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var tableName = ChannelManager.GetTableName(siteInfo, channelId);

            return DataProvider.ContentDao.GetCount(tableName, channelId, whereString);
        }

        public string GetTableName(int siteId, int channelId)
        {
            if (siteId <= 0 || channelId <= 0) return string.Empty;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var nodeInfo = ChannelManager.GetChannelInfo(siteId, channelId);
            return ChannelManager.GetTableName(siteInfo, nodeInfo);
        }

        public List<TableColumn> GetTableColumns(int siteId, int channelId)
        {
            if (siteId <= 0 || channelId <= 0) return null;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var nodeInfo = ChannelManager.GetChannelInfo(siteId, channelId);
            var tableName = ChannelManager.GetTableName(siteInfo, nodeInfo);
            var relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(siteId, channelId);

            var tableStyleInfoList = TableStyleManager.GetTableStyleInfoList(tableName, relatedIdentities);
            var tableColumnList = new List<TableColumn>
            {
                new TableColumn
                {
                    AttributeName = ContentAttribute.Title,
                    DataType = DataType.VarChar,
                    DataLength = 255,
                    InputStyle = new InputStyle
                    {
                        InputType = InputType.Text,
                        DisplayName = "标题",
                        IsRequired = true,
                        ValidateType = ValidateType.None
                    }
                }
            };

            foreach (var styleInfo in tableStyleInfoList)
            {
                tableColumnList.Add(new TableColumn
                {
                    AttributeName = styleInfo.AttributeName,
                    DataType = DataType.VarChar,
                    DataLength = 50,
                    InputStyle = new InputStyle
                    {
                        InputType = styleInfo.InputType,
                        DisplayName = styleInfo.DisplayName,
                        DefaultValue = styleInfo.DefaultValue,
                        IsRequired = styleInfo.Additional.IsRequired,
                        ValidateType = styleInfo.Additional.ValidateType,
                        MinNum = styleInfo.Additional.MinNum,
                        MaxNum = styleInfo.Additional.MaxNum,
                        RegExp = styleInfo.Additional.RegExp,
                        Width = styleInfo.Additional.Width,
                    }
                });
            }

            tableColumnList.Add(new TableColumn
            {
                AttributeName = ContentAttribute.IsTop,
                DataType = DataType.VarChar,
                DataLength = 18,
                InputStyle = new InputStyle
                {
                    InputType = InputType.CheckBox,
                    DisplayName = "置顶"
                }
            });
            tableColumnList.Add(new TableColumn
            {
                AttributeName = ContentAttribute.IsRecommend,
                DataType = DataType.VarChar,
                DataLength = 18,
                InputStyle = new InputStyle
                {
                    InputType = InputType.CheckBox,
                    DisplayName = "推荐"
                }
            });
            tableColumnList.Add(new TableColumn
            {
                AttributeName = ContentAttribute.IsHot,
                DataType = DataType.VarChar,
                DataLength = 18,
                InputStyle = new InputStyle
                {
                    InputType = InputType.CheckBox,
                    DisplayName = "热点"
                }
            });
            tableColumnList.Add(new TableColumn
            {
                AttributeName = ContentAttribute.IsColor,
                DataType = DataType.VarChar,
                DataLength = 18,
                InputStyle = new InputStyle
                {
                    InputType = InputType.CheckBox,
                    DisplayName = "醒目"
                }
            });
            tableColumnList.Add(new TableColumn
            {
                AttributeName = ContentAttribute.AddDate,
                DataType = DataType.DateTime,
                InputStyle = new InputStyle
                {
                    InputType = InputType.DateTime,
                    DisplayName = "添加时间"
                }
            });

            return tableColumnList;
        }

        public string GetContentValue(int siteId, int channelId, int contentId, string attributeName)
        {
            if (siteId <= 0 || channelId <= 0 || contentId <= 0) return null;

            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var tableName = ChannelManager.GetTableName(siteInfo, channelId);

            return DataProvider.ContentDao.GetValue(tableName, contentId, attributeName);
        }

        public IContentInfo NewInstance()
        {
            return new ContentInfo();
        }

        public IContentInfo Parse(object dataItem)
        {
            var contentInfo = NewInstance();
            contentInfo.Load(dataItem);
            return contentInfo;
        }

        public IContentInfo Parse(IDataReader rdr)
        {
            var contentInfo = NewInstance();
            contentInfo.Load(rdr);
            return contentInfo;
        }

        public IContentInfo Parse(NameValueCollection attributes)
        {
            var contentInfo = NewInstance();
            contentInfo.Load(attributes);
            return contentInfo;
        }

        public IContentInfo Parse(string str)
        {
            var contentInfo = NewInstance();
            contentInfo.Load(str);
            return contentInfo;
        }

        //public void SetValuesToContentInfo(int siteId, int channelId, NameValueCollection form, IContentInfo contentInfo)
        //{
        //    var siteInfo = SiteManager.GetSiteInfo(siteId);
        //    var nodeInfo = NodeManager.GetChannelInfo(siteId, channelId);
        //    var tableName = NodeManager.GetTableName(siteInfo, nodeInfo);
        //    var tableStyle = NodeManager.GetTableStyle(siteInfo, nodeInfo);
        //    var relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(siteId, channelId);

        //    var extendImageUrl = ContentAttribute.GetExtendAttributeName(BackgroundContentAttribute.ImageUrl);
        //    if (form.AllKeys.Contains(StringUtils.LowerFirst(extendImageUrl)))
        //    {
        //        form[extendImageUrl] = form[StringUtils.LowerFirst(extendImageUrl)];
        //    }

        //    InputTypeParser.AddValuesToAttributes(tableStyle, tableName, siteInfo, relatedIdentities, form, contentInfo.ToNameValueCollection(), ContentAttribute.HiddenAttributes);
        //}

        public int Insert(int siteId, int channelId, IContentInfo contentInfo)
        {
            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var nodeInfo = ChannelManager.GetChannelInfo(siteId, channelId);
            var tableName = ChannelManager.GetTableName(siteInfo, nodeInfo);

            return DataProvider.ContentDao.Insert(tableName, siteInfo, contentInfo);
        }

        public void Update(int siteId, int channelId, IContentInfo contentInfo)
        {
            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var nodeInfo = ChannelManager.GetChannelInfo(siteId, channelId);
            var tableName = ChannelManager.GetTableName(siteInfo, nodeInfo);

            DataProvider.ContentDao.Update(tableName, siteInfo, contentInfo);
        }

        public void Delete(int siteId, int channelId, int contentId)
        {
            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var nodeInfo = ChannelManager.GetChannelInfo(siteId, channelId);
            var tableName = ChannelManager.GetTableName(siteInfo, nodeInfo);
            var contentIdList = new List<int> { contentId };
            DataProvider.ContentDao.TrashContents(siteId, tableName, contentIdList);
        }

        public List<int> GetContentIdList(int siteId, int channelId)
        {
            var siteInfo = SiteManager.GetSiteInfo(siteId);
            var tableName = ChannelManager.GetTableName(siteInfo, channelId);
            return DataProvider.ContentDao.GetContentIdListCheckedByChannelId(tableName, siteId, channelId);
        }
    }
}
