namespace ChromiumDownloader.Classes
{
    class Download
    {
        static public string sourceCode(string url)
        {
            return new System.Net.WebClient().DownloadString(url);
        }
    }
}
