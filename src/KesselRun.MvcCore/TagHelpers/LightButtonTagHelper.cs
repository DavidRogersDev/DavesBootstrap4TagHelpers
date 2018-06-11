using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("LightButton")]
    public class LightButtonTagHelper : Button
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.Attributes.AddOrMergeAttribute(HtmlConstants.Attributes.ClassAttribute,
                    _isOutline
                        ? BootstrapConstants.Buttons.ButtonLightOutlineFull
                        : BootstrapConstants.Buttons.ButtonLightFull
            );
        }
    }
}
