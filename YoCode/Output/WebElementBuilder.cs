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
        const string SPAN_OPEN = "<span>";
        const string SPAN_CLOSE = "</span>";

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
            par.Append(SPAN_OPEN);
            foreach(var line in lines)
            {
                par.Append(line+LINE_BREAK);
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
            foreach(var elem in list)
            {
                result.AppendLine(LIST_ELEM_OPEN + elem + LIST_ELEM_CLSOE);
            }
            result.AppendLine(LIST_CLOSE);
            return result.ToString();
        }

        public static string FormaFeatureTitle(string title, bool? featurePassed, double score = 0.0)
        {
            var passIcon = "accordion-icon-pass";
            var failIcon = "accordion-icon-fail";
            var undefinedIcon = "accordion-icon-undefinded";
            var passIconStyle = "fa-check-circle-o";
            var failIconStyle = "fa-times-circle-o";
            var undefinedStyle = "fa-question-circle-o";

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

            return String.Format(messages.HtmlTitleTemplate, chosenIcon, chosenIconStyle, score+"%",title);
        }

        private static string FormatCheckIcont(bool checkMark)
        {
            return checkMark ? "<span class=\"accordion-icon accordion-icon-pass\"><span class=\"fa fa-check-circle-o\"></span></span>" 
                : "<span class=\"accordion-icon accordion-icon-fail\"><span class=\"fa fa-times-circle-o\"></span></span>";
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
