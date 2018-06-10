using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("InfoButton")]
    public class InfoButtonTagHelper : Button
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.Attributes.Add(
                HtmlConstants.Attributes.ClassAttribute,
                _isOutline
                    ? BootstrapConstants.Buttons.ButtonInfoOutlineFull
                    : BootstrapConstants.Buttons.ButtonInfoFull
            );
        }
    }
}
