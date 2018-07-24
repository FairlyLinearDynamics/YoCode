using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    static class WebTemplateBuilder
    {
        const string HTML_TEMPLATE = @"C:\Users\ukekar\source\repos\YoCode\webReport\HTMLTemplate.html";
        const string LIST_ELEMENT_TEMPLATE = @"C:\Users\ukekar\source\repos\YoCode\webReport\ListElementTemplate.html";

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

        public static StringBuilder FormatAccordionElement(string featureTitle, string content)
        {
            return new StringBuilder().Append(File.ReadAllText(LIST_ELEMENT_TEMPLATE)
                .Replace(TITLE_TAG,featureTitle).Replace(CONTENT_TAG,content));
        }

        public static StringBuilder FormatParagraph(string text)
        {
            return new StringBuilder().Append(PARAGRAPH_OPEN + text + PARAGRAPH_CLOSE);
        }

        public static StringBuilder FormatHeader(string text)
        {
            return new StringBuilder().Append(HEADER_OPEN + text + HEADER_CLOSE);
        }

        public static StringBuilder FormatListOfStrings(IEnumerable<string> list)
        {
            var result = new StringBuilder();
            result.AppendLine(LIST_OPEN);
            foreach(var elem in list)
            {
                result.AppendLine(LIST_ELEM_OPEN + elem + LIST_ELEM_CLSOE);
            }
            result.AppendLine(LIST_CLOSE);
            return result;
        }
    }
}
