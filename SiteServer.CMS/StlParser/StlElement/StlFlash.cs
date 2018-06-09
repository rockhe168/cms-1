﻿using System.Text;
using SiteServer.Utils;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model.Attributes;
using SiteServer.CMS.StlParser.Cache;
using SiteServer.CMS.StlParser.Model;
using SiteServer.CMS.StlParser.Parsers;
using SiteServer.CMS.StlParser.Utility;
using SiteServer.Utils.Enumerations;

namespace SiteServer.CMS.StlParser.StlElement
{
    [StlClass(Usage = "显示Flash", Description = "通过 stl:flash 标签在模板中获取并显示栏目或内容的Flash")]
    public class StlFlash
    {
        private StlFlash() { }
        public const string ElementName = "stl:flash";

        private static readonly Attr ChannelIndex = new Attr("channelIndex", "栏目索引");
        private static readonly Attr ChannelName = new Attr("channelName", "栏目名称");
        private static readonly Attr Parent = new Attr("parent", "显示父栏目");
        private static readonly Attr UpLevel = new Attr("upLevel", "上级栏目的级别");
        private static readonly Attr TopLevel = new Attr("topLevel", "从首页向下的栏目级别");
        private static readonly Attr Type = new Attr("type", "指定存储flash的字段");
        private static readonly Attr Src = new Attr("src", "显示的flash地址");
        private static readonly Attr AltSrc = new Attr("altSrc", "当指定的flash不存在时显示的flash地址");
        private static readonly Attr Width = new Attr("width", "宽度");
        private static readonly Attr Height = new Attr("height", "高度");

        public static string Parse(PageInfo pageInfo, ContextInfo contextInfo)
        {
            var isGetPicUrlFromAttribute = false;
            var channelIndex = string.Empty;
            var channelName = string.Empty;
            var upLevel = 0;
            var topLevel = -1;
            var type = BackgroundContentAttribute.ImageUrl;
            var src = string.Empty;
            var altSrc = string.Empty;
            var width = "100%";
            var height = "180";

            foreach (var name in contextInfo.Attributes.AllKeys)
            {
                var value = contextInfo.Attributes[name];

                if (StringUtils.EqualsIgnoreCase(name, ChannelIndex.Name))
                {
                    channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(value, pageInfo, contextInfo);
                    if (!string.IsNullOrEmpty(channelIndex))
                    {
                        isGetPicUrlFromAttribute = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, ChannelName.Name))
                {
                    channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(value, pageInfo, contextInfo);
                    if (!string.IsNullOrEmpty(channelName))
                    {
                        isGetPicUrlFromAttribute = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, Parent.Name))
                {
                    if (TranslateUtils.ToBool(value))
                    {
                        upLevel = 1;
                        isGetPicUrlFromAttribute = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, UpLevel.Name))
                {
                    upLevel = TranslateUtils.ToInt(value);
                    if (upLevel > 0)
                    {
                        isGetPicUrlFromAttribute = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, TopLevel.Name))
                {
                    topLevel = TranslateUtils.ToInt(value);
                    if (topLevel >= 0)
                    {
                        isGetPicUrlFromAttribute = true;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, Type.Name))
                {
                    type = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Src.Name))
                {
                    src = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, AltSrc.Name))
                {
                    altSrc = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Width.Name))
                {
                    width = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Height.Name))
                {
                    height = value;
                }
            }

            return ParseImpl(pageInfo, contextInfo, isGetPicUrlFromAttribute, channelIndex, channelName, upLevel, topLevel, type, src, altSrc, width, height);
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, bool isGetPicUrlFromAttribute, string channelIndex, string channelName, int upLevel, int topLevel, string type, string src, string altSrc, string width, string height)
        {
            var parsedContent = string.Empty;

            var contentId = 0;
            //判断是否图片地址由标签属性获得
            if (!isGetPicUrlFromAttribute)
            {
                contentId = contextInfo.ContentId;
            }
            var contentInfo = contextInfo.ContentInfo;

            string picUrl;
            if (!string.IsNullOrEmpty(src))
            {
                picUrl = src;
            }
            else
            {
                if (contentId != 0)//获取内容Flash
                {
                    if (contentInfo == null)
                    {
                        var nodeInfo = ChannelManager.GetChannelInfo(contextInfo.SiteInfo.Id, contextInfo.ChannelId);
                        var tableName = ChannelManager.GetTableName(contextInfo.SiteInfo, nodeInfo);

                        //picUrl = DataProvider.ContentDao.GetValue(tableName, contentId, type);
                        picUrl = Content.GetValue(tableName, contentId, type);
                    }
                    else
                    {
                        picUrl = contextInfo.ContentInfo.GetString(type);
                    }
                }
                else//获取栏目Flash
                {
                    var channelId = StlDataUtility.GetChannelIdByLevel(pageInfo.SiteId, contextInfo.ChannelId, upLevel, topLevel);

                    channelId = StlDataUtility.GetChannelIdByChannelIdOrChannelIndexOrChannelName(pageInfo.SiteId, channelId, channelIndex, channelName);
                    var channel = ChannelManager.GetChannelInfo(pageInfo.SiteId, channelId);

                    picUrl = channel.ImageUrl;
                }
            }

            if (string.IsNullOrEmpty(picUrl))
            {
                picUrl = altSrc;
            }

            // 如果是实体标签则返回空
            if (contextInfo.IsStlEntity)
            {
                return picUrl;
            }

            if (!string.IsNullOrEmpty(picUrl))
            {
                var extension = PathUtils.GetExtension(picUrl);
                if (EFileSystemTypeUtils.IsImage(extension))
                {
                    parsedContent = StlImage.Parse(pageInfo, contextInfo);
                }
                else if (EFileSystemTypeUtils.IsPlayer(extension))
                {
                    parsedContent = StlPlayer.Parse(pageInfo, contextInfo);
                }
                else
                {
                    pageInfo.AddPageBodyCodeIfNotExists(PageInfo.Const.JsAcSwfObject);

                    picUrl = PageUtility.ParseNavigationUrl(pageInfo.SiteInfo, picUrl, pageInfo.IsLocal);

                    if (string.IsNullOrEmpty(contextInfo.Attributes["quality"]))
                    {
                        contextInfo.Attributes["quality"] = "high";
                    }
                    if (string.IsNullOrEmpty(contextInfo.Attributes["wmode"]))
                    {
                        contextInfo.Attributes["wmode"] = "transparent";
                    }
                    var paramBuilder = new StringBuilder();
                    var uniqueId = pageInfo.UniqueId;
                    foreach (var key in contextInfo.Attributes.AllKeys)
                    {
                        paramBuilder.Append($@"    so_{uniqueId}.addParam(""{key}"", ""{contextInfo.Attributes[key]}"");").Append(StringUtils.Constants.ReturnAndNewline);
                    }

                    parsedContent = $@"
<div id=""flashcontent_{uniqueId}""></div>
<script type=""text/javascript"">
    // <![CDATA[
    var so_{uniqueId} = new SWFObject(""{picUrl}"", ""flash_{uniqueId}"", ""{width}"", ""{height}"", ""7"", """");
{paramBuilder}
    so_{uniqueId}.write(""flashcontent_{uniqueId}"");
    // ]]>
</script>
";
                }
            }

            return parsedContent;
        }
    }
}
