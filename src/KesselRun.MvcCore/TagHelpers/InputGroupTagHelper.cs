using System;
using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

/*  NOTE: Multiple inputs is not supported at this time. It's not a common scenario  
 *  and probably warrants its own separate TagHelper.
 */

namespace KesselRun.MvcCore.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("InputGroup")]
    public class InputGroupTagHelper : TagHelper
    {
        private const string InvalidCharReplacement = "z";
        private const string AppendContentAttribute = "kr-appendcontent";
        private const string PrependContentAttribute = "kr-prependcontent";
        private const string AppendIdAttribute = "kr-appendid";
        private const string PrependIdAttribute = "kr-prependid";
        private const string InnerLabelAttribute = "kr-innerlabel";
        private const string PlaceholderAttribute = "kr-placeholder";
        private const string GrouptypeAttribute = "kr-grouptype";


        [HtmlAttributeName(AppendContentAttribute)]
        public string AppendContent { get; set; }

        [HtmlAttributeName(PrependContentAttribute)]
        public string PreContent { get; set; }

        [HtmlAttributeName(HtmlConstants.Attributes.ClassAttribute)]
        public string CssClass { get; set; }

        [HtmlAttributeName(AppendIdAttribute)]
        public string AppendId { get; set; }

        [HtmlAttributeName(PrependIdAttribute)]
        public string PrependId { get; set; }

        [HtmlAttributeName(InnerLabelAttribute)]
        public string InnerLabel { get; set; }

        [HtmlAttributeName(PlaceholderAttribute)]
        public string InputPlaceholder { get; set; }

        [HtmlAttributeName(GrouptypeAttribute)]
        public InputMeta.InputGroupInputType InputGroupInputType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Elements.Div;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute(HtmlConstants.Attributes.ClassAttribute,
                string.IsNullOrWhiteSpace(CssClass)
                    ? BootstrapConstants.Groups.InputGroup
                    : string.Concat(BootstrapConstants.Groups.InputGroup, Constants.StringSpace, CssClass));

            // These two Tagbuilders will be used regardles of whether the InputGroupInputType is both or not.
            // Where "both", a 2nd TagHelper for each is created in the switch block below.
            var spanBuilder = new TagBuilder(HtmlConstants.Elements.Span);
            var pendDivBuilder = new TagBuilder(HtmlConstants.Elements.Div); // named "pend" b/c may be append or prepend.

            spanBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupText);
            spanBuilder.GenerateId(
                InputGroupInputType == InputMeta.InputGroupInputType.prepend || InputGroupInputType == InputMeta.InputGroupInputType.both
                ? PrependId : AppendId, InvalidCharReplacement);

            // A TagBuilder for the input element inside the input group. Only single inputs are supported by this TagHelper.
            var inputBuilder = new TagBuilder(HtmlConstants.Elements.Input);
            inputBuilder.AddCssClass(BootstrapConstants.Forms.FormControl);
            inputBuilder.MergeAttribute(HtmlConstants.Attributes.TypeAttribute, HtmlConstants.AttributeValues.Text);

            if (!string.IsNullOrWhiteSpace(PrependId))
                inputBuilder.MergeAttribute(HtmlConstants.Attributes.AriaDescribedByAttribute, PrependId);
            if (!string.IsNullOrWhiteSpace(InnerLabel))
                inputBuilder.MergeAttribute(HtmlConstants.Attributes.AriaLabelAttribute, InnerLabel);
            if (!string.IsNullOrWhiteSpace(InputPlaceholder))
                inputBuilder.MergeAttribute(HtmlConstants.Attributes.PlaceholderAttribute, InputPlaceholder);

            switch (InputGroupInputType)
            {
                case InputMeta.InputGroupInputType.append:
                    spanBuilder.InnerHtml.Append(AppendContent);
                    pendDivBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupAppend);
                    pendDivBuilder.InnerHtml.AppendHtml(spanBuilder);
                    output.Content.AppendHtml(inputBuilder); // input
                    output.Content.AppendHtml(pendDivBuilder); // followed by append
                    break;
                case InputMeta.InputGroupInputType.both:
                    spanBuilder.InnerHtml.Append(PreContent);
                    pendDivBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupPrepend);
                    pendDivBuilder.InnerHtml.AppendHtml(spanBuilder);
                    var appendSpanBuilder = new TagBuilder(HtmlConstants.Elements.Span);
                    appendSpanBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupText);
                    appendSpanBuilder.GenerateId(AppendId, InvalidCharReplacement);
                    appendSpanBuilder.InnerHtml.Append(AppendContent);
                    var appendDivBuilder = new TagBuilder(HtmlConstants.Elements.Div);
                    appendDivBuilder.InnerHtml.AppendHtml(appendSpanBuilder);
                    appendDivBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupAppend);
                    output.Content.AppendHtml(pendDivBuilder); // prepend
                    output.Content.AppendHtml(inputBuilder); // then input
                    output.Content.AppendHtml(appendDivBuilder); // append
                    break;
                case InputMeta.InputGroupInputType.prepend:
                    spanBuilder.InnerHtml.Append(PreContent);
                    pendDivBuilder.AddCssClass(BootstrapConstants.Groups.InputGroupPrepend);
                    pendDivBuilder.InnerHtml.AppendHtml(spanBuilder);
                    output.Content.AppendHtml(pendDivBuilder); // prepend
                    output.Content.AppendHtml(inputBuilder); // followd by input
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Process(context, output);
        }
    }
}
