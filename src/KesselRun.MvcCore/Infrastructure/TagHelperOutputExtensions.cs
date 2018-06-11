using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.Infrastructure
{
    public static class TagHelperOutputExtensions
    {
        public static void ReplaceAttribute(this TagHelperOutput tagHelperOutput, TagHelperAttribute tagHelperAttribute)
        {
            if(ReferenceEquals(tagHelperOutput.Attributes.GetTagHelperAttribute(tagHelperAttribute.Name), null))
            {
                tagHelperOutput.Attributes.Add(tagHelperAttribute);
            }
            else
            {
                tagHelperOutput.Attributes.Remove(tagHelperAttribute);
                tagHelperOutput.Attributes.Add(tagHelperAttribute);
            }
        }
    }
}
