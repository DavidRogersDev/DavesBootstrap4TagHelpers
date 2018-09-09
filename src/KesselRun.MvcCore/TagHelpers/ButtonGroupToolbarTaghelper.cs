using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("BtnGroupToolbar")]
    public class ButtonGroupToolbarTagHelper : TagHelper
    {
        [HtmlAttributeName(HtmlConstants.Attributes.AriaLabelAttribute)]
        public string AriaLabel { get; set; }

        [HtmlAttributeName(HtmlConstants.Attributes.ClassAttribute)]
        public string CssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Elements.Div;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrWhiteSpace(CssClass) || CssClass.Equals(BootstrapConstants.Buttons.ButtonToolbar))
                CssClass = BootstrapConstants.Buttons.ButtonToolbar;
            else
                CssClass = string.Concat(BootstrapConstants.Buttons.ButtonToolbar, Constants.StringSpace, CssClass);

            output.Attributes.SetAttribute(HtmlConstants.Attributes.ClassAttribute, CssClass);
            output.Attributes.SetAttribute(HtmlConstants.Attributes.RoleAttribute, BootstrapConstants.Buttons.ToolbarRole);
            output.Attributes.SetAttribute(HtmlConstants.Attributes.AriaLabelAttribute, AriaLabel);

            // Look like this is redundant in this version of razor! Nesting just works!
            //output.Content.SetHtmlContent(output.GetChildContentAsync().Result.GetContent());

            base.Process(context, output);

        }

    }
}
