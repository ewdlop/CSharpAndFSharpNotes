using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Crawler
{
    public static class WebCrawler
    {
        public static async IAsyncEnumerable<string> Crawl(string url)
        {
            string content = await GetWebContent(url);
            yield return content;
            foreach (string item in AnalyzeHtmlContent(content))
            {
                yield return await GetWebContent(item);
            }
        }

        public static Task<string> GetWebContent(string url)
        {
            using var wc = new WebClient();
            return wc.DownloadStringTaskAsync(new Uri(url));
        }
        public static readonly Regex regexLink = new Regex(@"(?<=href=('|""))https?://.*?(?=\1)");
        public static IEnumerable<string> AnalyzeHtmlContent(string text)
        {
            foreach (var url in regexLink.Matches(text))
                yield return url.ToString();
        }
        public static readonly Regex regexTitle = new Regex("<title>(?<title>.*?)<\\/title>", RegexOptions.Compiled);
        public static string ExtractWebPageTitle(string textPage)
        {
            if (regexTitle.IsMatch(textPage))
                return regexTitle.Match(textPage).Groups["title"].Value;
            return "No Page Title Found!";
        }
    }
}
