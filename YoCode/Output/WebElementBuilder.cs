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

        const string ESCAPE_AND = "&amp";
        private const string ESCAPE_LESS = "&lt";
        private const string ESCAPE_GREATER = "&gt";

        private static readonly Regex urlPattern = new Regex(@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?");
        private static readonly Regex urlTitlePattern = new Regex(@"(http|www|https)(:\/\/)?([\w+?\.\w+])+(\.)+([\w+?\.\w+])+([a-zA-Z0-9])?");

        public static string FormatAccordionElement(WebAccordionData data)
        {
            return messages.ListElementTemplate
                .Replace(TITLE_TAG, data.featureTitle).Replace(CONTENT_TAG, data.content).Replace(CONTENT_INFO_TAG, data.helperMessage);
        }

        public static string FormatFileDiff(List<string> file)
        {
            var sb = new StringBuilder();
            sb.Append("<span class=\"changedFileText\">");
            foreach(var line in file)
            {
                if(line.Length>1)
                sb.AppendLine(line[0] == '+' ? $"<span class=\"green-text\">{line}</span>" : $"<span class=\"red-text\">{line}</span>");
            }
            sb.Append("</span>");
            return sb.ToString();
        }

        public static string FormatParagraph(string text)
        {
            text = EscapeCharacters(text);
            text = FindAndEncapsulateLink(text);

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

        public static string FormatFeatureTitle(string title, bool? featurePassed, string score)
        {
            const string passIcon = "accordion-icon-pass";
            const string failIcon = "accordion-icon-fail";
            const string undefinedIcon = "accordion-icon-undefinded";
            const string passIconStyle = "fa-check-circle-o";
            const string failIconStyle = "fa-times-circle-o";
            const string undefinedStyle = "fa-question-circle-o";

            var chosenIcon = "";
            var chosenIconStyle = "";
            switch (featurePassed)
            {
                case true:
                    chosenIcon = passIcon;
                    chosenIconStyle = passIconStyle;
                    break;
                case false:
                    chosenIcon = failIcon;
                    chosenIconStyle = failIconStyle;
                    break;
                default:
                    chosenIcon = undefinedIcon;
                    chosenIconStyle = undefinedStyle;
                    break;
            }

            return String.Format(messages.HtmlTitleTemplate, chosenIcon, chosenIconStyle, score, title);
        }

        public static string FormatLink(string url, string title)
        {
            return $"<a href=\"{url}\">{title}</a>";
        }

        private static string EscapeCharacters(string text)
        {
            return text.Replace("<", ESCAPE_LESS).Replace(">", ESCAPE_GREATER);
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
