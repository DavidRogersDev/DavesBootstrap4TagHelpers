using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    public class Button : TagHelper
    {
        const string IconAttribute = "kr-icon";
        protected const string OutlineAttribute = "kr-outline";
        protected bool _isOutline = false;

        [HtmlAttributeName(IconAttribute)]
        public virtual string Icon { get; set; }

        [HtmlAttributeName(HtmlConstants.Attributes.TypeAttribute)]
        public virtual string Type { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Elements.Button;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute(HtmlConstants.Attributes.TypeAttribute, Type ?? HtmlConstants.Attributes.SubmitAttribute);

            var outlineAttribute = context.AllAttributes.GetTagHelperAttribute(OutlineAttribute);

            if (!ReferenceEquals(null, outlineAttribute))
            {
                // set _isOutline for child classes
                _isOutline = true;
                // remove the custom attribute from the markup. Keep it clean.
                output.Attributes.Remove(outlineAttribute);
            }

            if (!string.IsNullOrEmpty(Icon))
            {
                var iconBuilder = new TagBuilder(HtmlConstants.Elements.I) { TagRenderMode = TagRenderMode.Normal };
                iconBuilder.AddCssClass(string.Concat(FontAwesomeConstants.FontClassSuffix, Icon));

                output.PreContent.SetHtmlContent(iconBuilder);
                output.PreContent.AppendHtml(HtmlConstants.NonBreakingSpace);
            }
        }
    }
}
