namespace SiteServer.Cli.Updater.Model41
{
    public static class TableErrorLog
    {
        public const string OldTableName = "ErrorLog";

        public static ConvertInfo Converter => new ConvertInfo
        {
            IsAbandon = true
        };
    }
}
