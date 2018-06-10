using System.Threading.Tasks;
using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("ButtonGroup")]
    [RestrictChildren("PrimaryButton", "SecondaryButton", "DangerButton", "SuccessButton", "WarningButton", "InfoButton", "LightButton", "DarkButton", "LinkButton")]
    public class ButtonGroupTagHelper : TagHelper
    {
        [HtmlAttributeName(HtmlConstants.Attributes.AriaLabelAttribute)]
        public string AriaLabel { get; set; }

        [HtmlAttributeName(HtmlConstants.Attributes.ClassAttribute)]
        public string CssClass { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Elements.Div;
            output.TagMode = TagMode.StartTagAndEndTag;

            if (ReferenceEquals(CssClass, null) || CssClass.Equals(BootstrapConstants.Buttons.ButtonGroup))
                CssClass = BootstrapConstants.Buttons.ButtonGroup;
            else
                CssClass = string.Concat(BootstrapConstants.Buttons.ButtonGroup, Constants.StringSpace, CssClass);

            output.Attributes.SetAttribute(HtmlConstants.Attributes.ClassAttribute, CssClass);
            output.Attributes.SetAttribute(HtmlConstants.Attributes.RoleAttribute, BootstrapConstants.Buttons.RoleAttibuteGroup);
            output.Attributes.SetAttribute(HtmlConstants.Attributes.AriaLabelAttribute, AriaLabel);

            output.Content.SetHtmlContent(await output.GetChildContentAsync());

            await base.ProcessAsync(context, output);
        }
    }
}
