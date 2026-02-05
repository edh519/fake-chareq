using HtmlAgilityPack;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace BusinessLayer.Helpers;

public static class HtmlHelper
{
    public static string RemoveHtmlElementsFromString(string html, bool replaceBrWithNewLine = false, bool keepImageElements = false)
    {
        if (html is null) return "";

        HtmlDocument doc = new();

        if (replaceBrWithNewLine)
        {
            html = Regex.Replace(html, @"<\/? ?br ?\/?>", Environment.NewLine);
        }

        doc.LoadHtml(html);

        if (keepImageElements) return WebUtility.HtmlDecode(doc.DocumentNode.InnerText);

        if (doc.DocumentNode.SelectNodes("//img") is not { } imgNodes)
            return WebUtility.HtmlDecode(doc.DocumentNode.InnerText);

        foreach (HtmlNode imageNode in imgNodes)
        {
            HtmlNode nodeForReplace = HtmlNode.CreateNode("[Image removed, see ChaReq request]");
            imageNode.ParentNode.ReplaceChild(nodeForReplace, imageNode);
        }
        return WebUtility.HtmlDecode(doc.DocumentNode.InnerText);
    }

    public static string RemoveImageElementsFromString(string html, bool messageIsWorkRequest)
    {
        if (html is null) return "";

        HtmlDocument doc = new();

        doc.LoadHtml(html);

        if (doc.DocumentNode.SelectNodes("//img") is not { } imgNodes)
            return html;

        foreach (HtmlNode imageNode in imgNodes)
        {
            HtmlNode nodeForReplace = null;
            if (messageIsWorkRequest)
            {
                nodeForReplace = HtmlNode.CreateNode("[Image removed, see ChaReq request]");
            }
            else { nodeForReplace = HtmlNode.CreateNode("[Image removed, see Contact Us form]"); }

            imageNode.ParentNode.ReplaceChild(nodeForReplace, imageNode);
        }

        return doc.DocumentNode.InnerHtml;
    }
}