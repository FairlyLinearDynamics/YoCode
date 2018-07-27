using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace YoCode
{
    static class WebElementBuilder
    {
        const string TITLE_TAG = "{TITLE}";
        const string CONTENT_TAG = "{CONTENT}";

        const string PARAGRAPH_OPEN = "<p>";
        const string PARAGRAPH_CLOSE = "</p>";
        const string HEADER_OPEN = "<h1>";
        const string HEADER_CLOSE = "</h1>";
        const string LIST_OPEN = "<ul>";
        const string LIST_CLOSE = "</ul>";
        const string LIST_ELEM_OPEN = "<li>";
        const string LIST_ELEM_CLSOE = "</li>";
        const string LINE_BREAK = "<br/>";

        const string ESCAPE_AND = "&amp";
        const string ESCAPE_LESS = "&lt";
        const string ESCAPE_GREATER = "&gt";

        static Regex urlPattern = new Regex(@"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?");
        static Regex urlTitlePattern = new Regex(@"(http|www|https)(:\/\/)?([\w+?\.\w+])+(\.)+([\w+?\.\w+])+([a-zA-Z0-9])?");

        public static string FormatAccordionElement(string featureTitle, string content)
        {
            return messages.ListElementTemplate
                .Replace(TITLE_TAG,featureTitle).Replace(CONTENT_TAG,content);
        }

        public static string FormatParagraph(string text)
        {
            text = EscapeCharacters(text);
            text = FindAndEncapsulateLink(text);

            var lines = text.Split(Environment.NewLine);
            var par = new StringBuilder();

            foreach(var line in lines)
            {
                par.Append(line + LINE_BREAK);
            }
            return PARAGRAPH_OPEN + par.ToString() + PARAGRAPH_CLOSE;
        }

        public static string FormatHeader(string text)
        {
            return HEADER_OPEN + text + HEADER_CLOSE;
        }

        public static string FormatListOfStrings(IEnumerable<string> list)
        {
            var result = new StringBuilder();
            result.AppendLine(LIST_OPEN);
            foreach(var elem in list)
            {
                result.AppendLine(LIST_ELEM_OPEN + elem + LIST_ELEM_CLSOE);
            }
            result.AppendLine(LIST_CLOSE);
            return result.ToString();
        }

        public static string FormatCheckIcont(bool checkMark)
        {
            return checkMark ? "<i class=\"fa fa-check-circle-o\"></i>" : "<i class=\"fa fa-times-circle-o\"></i>";
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
                foreach(var link in linksInText)
                {
                    text = text.Replace(link.ToString(), FormatLink(link.ToString(), urlTitlePattern.Match(link.ToString()).ToString()));
                }
                return text;
            }
            return text;
        }
    }
}
