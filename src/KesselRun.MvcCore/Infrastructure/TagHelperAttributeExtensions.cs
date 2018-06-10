using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KesselRun.MvcCore.Infrastructure
{
    public static class TagHelperAttributeExtensions
    {
        public static TagHelperAttribute GetTagHelperAttribute(this ReadOnlyTagHelperAttributeList tagHelperAttribute, string name)
        {
            if(tagHelperAttribute.TryGetAttribute(name, out var attribute))
                return attribute;

            return null;
        }
    }
}
