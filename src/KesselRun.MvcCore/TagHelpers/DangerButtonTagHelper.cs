using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("DangerButton")]
    public class DangerButtonTagHelper : Button
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.Attributes.AddOrMergeAttribute(HtmlConstants.Attributes.ClassAttribute,
            _isOutline
                    ? BootstrapConstants.Buttons.ButtonDangerOutlineFull
                    : BootstrapConstants.Buttons.ButtonDangerFull
                );
        }
    }
}
