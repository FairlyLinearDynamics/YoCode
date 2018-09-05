using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace YoCode
{
    internal struct WebAccordionData{
        public string featureTitle;
        public string content;
        public string helperMessage;
    }

    internal static class WebElementBuilder
    {
        private const string TITLE_TAG = "{TITLE}";
        private const string CONTENT_TAG = "{CONTENT}";
        private const string CONTENT_INFO_TAG = "{INFO-CONTENT}";

        const string PARAGRAPH_OPEN = "<p>";
        const string PARAGRAPH_CLOSE = "</p>";
        private const string HEADER_OPEN = "<h1>";
        private const string HEADER_CLOSE = "</h1>";
        private const string LIST_OPEN = "<ul>";
        private const string LIST_CLOSE = "</ul>";
        private const string LIST_ELEM_OPEN = "<li>";
        private const string LIST_ELEM_CLSOE = "</li>";
        private const string LINE_BREAK = "<br/>";
        private const string SPAN_OPEN = "<span>";
        private const string SPAN_CLOSE = "</span>";

        private const string ESCAPE_AND = "&amp";
        private const string ESCAPE_LESS = "&lt";
        private const string ESCAPE_GREATER = "&gt";
        private const string ESCAPE_QUOT = "&quot";
        private const string ESCAPE_APOS = "&apos";

        private static readonly Regex urlPattern = new Regex(@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?");
        private static readonly Regex urlTitlePattern = new Regex(@"(http|www|https)(:\/\/)?([\w+?\.\w+])+(\.)+([\w+?\.\w+])+([a-zA-Z0-9])?");

        public static string FormatAccordionElement(WebAccordionData data)
        {
            return messages.ListElementTemplate
                .Replace(TITLE_TAG, data.featureTitle).Replace(CONTENT_TAG, data.content).Replace(CONTENT_INFO_TAG, data.helperMessage);
        }

        public static string FormatImageElement(string src)
        {
            return $"<img src=\"{src}\" alr=\"UI Screenshot\" id=\"js-UIScreenShot\">";
        }

        public static string FormatFileDiff(List<string> file)
        {
            var sb = new StringBuilder();
            var escapedFile = EscapeCharacters(String.Join(Environment.NewLine,file)).Split(Environment.NewLine).ToList();

            sb.Append("<span class=\"changedFileText\">");
            foreach(var line in escapedFile)
            {
                if (line.Length > 0)
                {
                    if (line[0] == '+')
                    {
                        sb.AppendLine($"<span class=\"green-text\">{line}</span>");
                    }
                    else if (line[0] == '-') 
                    {
                        sb.AppendLine($"<span class=\"red-text\">{line}</span>");
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
            }
            sb.Append("</span>");
            return sb.ToString();
        }

        public static string FormatFileDiffButton(string buttonName)
        {
            return $"<span class=\"changedFile\">{buttonName}</span>";
        }

        public static string FormatAndEncapsulateParagraph(string text)
        {
            text = EscapeCharacters(text);
            text = FindAndEncapsulateLink(text);
            return FormatParagraph(text);
        }

        public static string FormatParagraph(string text)
        {
            var lines = text.Split(Environment.NewLine);
            var par = new StringBuilder();
            par.Append(SPAN_OPEN);
            foreach (var line in lines)
            {
                par.Append(line).Append(LINE_BREAK);
            }
            par.Append(SPAN_CLOSE);
            return messages.HtmlParagraphBlock
                .Replace(CONTENT_TAG, par.ToString());
        }

        public static string FormatHeader(string text)
        {
            return HEADER_OPEN + text + HEADER_CLOSE;
        }

        public static string FormatListOfStrings(IEnumerable<string> list)
        {
            var result = new StringBuilder();
            result.AppendLine(LIST_OPEN);
            foreach (var elem in list)
            {
                result.Append(LIST_ELEM_OPEN).Append(elem).AppendLine(LIST_ELEM_CLSOE);
            }
            result.AppendLine(LIST_CLOSE);
            return result.ToString();
        }

        public static string FormatPassedFeatureTitle(string title, string score)
        {
            const string passIcon = "accordion-icon-pass";
            const string passIconStyle = "fa-check-circle-o";

            return string.Format(messages.HtmlTitleTemplate, passIcon, passIconStyle, score, title);
        }

        public static string FormatFailedFeatureTitle(string title, string score)
        {
            const string failIcon = "accordion-icon-fail";
            const string failIconStyle = "fa-times-circle-o";
            
            return string.Format(messages.HtmlTitleTemplate, failIcon, failIconStyle, score, title);
        }

        public static string FormatInconclusiveFeatureTitle(string title, string score)
        {
            const string undefinedIcon = "accordion-icon-undefinded";
            const string undefinedStyle = "fa-question-circle-o";

            return string.Format(messages.HtmlTitleTemplate, undefinedIcon, undefinedStyle, score, title);
        }

        public static string FormatLink(string url, string title)
        {
            return $"<a href=\"{url}\">{title}</a>";
        }

        private static string EscapeCharacters(string text)
        {
            return text.Replace("&", ESCAPE_AND).Replace("<", ESCAPE_LESS).Replace(">", ESCAPE_GREATER).Replace("\"",ESCAPE_QUOT).Replace("\'",ESCAPE_APOS);
        }

        private static string FindAndEncapsulateLink(string text)
        {
            var linksInText = urlPattern.Matches(text);
            if (linksInText.Any())
            {
                foreach (var link in linksInText)
                {
                    text = text.Replace(link.ToString(), FormatLink(link.ToString(), urlTitlePattern.Match(link.ToString()).ToString()));
                }
                return text;
            }
            return text;
        }
    }
}
