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

        public static void AddOrMergeAttribute(this TagHelperAttributeList tagHelperAttributes, string name, object content)
        {
            var tagHelperAttribute = default(TagHelperAttribute);

            var currentAttribute = tagHelperAttributes.GetTagHelperAttribute(name);

            if (ReferenceEquals(currentAttribute, null))
            {
                tagHelperAttribute = new TagHelperAttribute(name, content);
                tagHelperAttributes.Add(tagHelperAttribute);
            }
            else
            {
                tagHelperAttribute = new TagHelperAttribute(
                    name,
                    $"{currentAttribute.Value.ToString()} {content.ToString()}",
                    currentAttribute.ValueStyle
                    );

                tagHelperAttributes.Remove(currentAttribute);
                tagHelperAttributes.Add(tagHelperAttribute);

            }
        }
    }
}
