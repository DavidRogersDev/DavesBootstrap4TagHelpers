using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("SuccessButton")]
    public class SuccessButtonTagHelper : Button
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.Attributes.AddOrMergeAttribute(HtmlConstants.Attributes.ClassAttribute,
                            _isOutline
                    ? BootstrapConstants.Buttons.ButtonSuccessOutlineFull
                    : BootstrapConstants.Buttons.ButtonSuccessFull
            );
        }
    }
}
