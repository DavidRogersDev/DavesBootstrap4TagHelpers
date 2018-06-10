using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KesselRun.MvcCore.Infrastructure
{
    public static class TagBuilderExtensions
    {
        public static string ProcessTagBuilderAsString(this TagBuilder tagBuilder)
        {
            string result;

            using (var stringWriter = new StringWriter())
            {
                tagBuilder.WriteTo(stringWriter, HtmlEncoder.Default);
                result = stringWriter.ToString();
            }

            return result;
        }

        public static IHtmlContent ProcessTagBuilderAsHtml(this TagBuilder tagBuilder)
        {
            return new HtmlString(tagBuilder.ProcessTagBuilderAsString());
        }

        public static string ProcessIHtmlContentAsString(this IHtmlContent content)
        {
            string result;

            using (var writer = new StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                result = writer.ToString();
            }

            return result;
        }
    }
}
