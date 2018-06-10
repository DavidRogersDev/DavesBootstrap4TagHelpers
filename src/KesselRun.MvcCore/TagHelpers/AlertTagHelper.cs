using System;
using System.Text;
using KesselRun.MvcCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

/* This TH was originally written by Rick Strahl */
/* Here, as modified by David Rogers */

namespace KesselRun.MvcCore.TagHelpers
{
    [HtmlTargetElement("alert")]
    public class AlertTagHelper : TagHelper
    {
        const string AlertClassAttrbute = "kr-alert-class";
        const string DismissableAttribute = "kr-dismissible";
        const string HeaderAttribute = "kr-header";
        const string HeaderAsHtmlAttribute = "kr-header-as-html";
        const string IncludeHtmlAsChildAttribute = "kr-include-html-child";
        const string IconAttribute = "kr-icon";
        const string MessageAttribute = "kr-message";
        const string MessageAsHtmlAttribute = "kr-message-as-html";
        const string AlertPrimary = "primary";

        const string DangerIcon = "danger";
        const string InfoIcon = "info";
        const string SuccessIcon = "success";
        const string WarningIcon = "warning";

        // Font-Awesome classes
        const string FaInfoIcon = "info-circle";
        const string FaSuccessIcon = "check-circle";
        const string FaWarningIcon = "exclamation-triangle";

        // Boostrap-specific classes
        const string BsAlertClasses = "alert alert-";
        const string BsAlertDismissibleClass = "alert-dismissible";
        const string BsAlertHeadingClass = "alert-heading";
        const string BsTextDangerClass = "text-danger";

        const string StringSpace = " ";

        /// <summary>
        /// The main Message that gets displayed
        /// </summary>
        [HtmlAttributeName(MessageAttribute)]
        public string Message { get; set; }

        /// <summary>
        /// Optional Header that is displayed in big text. Use for 
        /// 'noisy' warnings and stop errors only please :-)
        /// The Message is displayed below the Header.
        /// </summary>
        [HtmlAttributeName(HeaderAttribute)]
        public string Header { get; set; }

        /// <summary>
        /// Font-awesome icon name without the fa- prefix.
        /// Example: info, warning, lightbulb-o, 
        /// If none is specified - "warning" is used
        /// To force no icon use "none"
        /// </summary>
        [HtmlAttributeName(IconAttribute)]
        public string Icon { get; set; }

        /// <summary>
        /// CSS class. Handled here so we can capture the existing
        /// class value and append the BootStrap alert class.
        /// </summary>
        [HtmlAttributeName(HtmlConstants.Attributes.ClassAttribute)]
        public string CssClass { get; set; }

        /// <summary>
        /// Optional - specifies the alert class used on the top level
        /// window. If not specified uses the same as the icon. 
        /// Override this if the icon and alert classes are different
        /// (often they are not).
        /// </summary>
        [HtmlAttributeName(AlertClassAttrbute)]
        public string AlertClass { get; set; }

        /// <summary>
        /// If true embeds the Message text as HTML. Use this 
        /// flag if you need to display HTML text. If false
        /// the text is HtmlEncoded.
        /// </summary>
        [HtmlAttributeName(MessageAsHtmlAttribute)]
        public bool MessageAsHtml { get; set; }


        /// <summary>
        /// If true embeds the Header text as HTML. Use this 
        /// flag if you need to display raw HTML text. If false
        /// the text is HtmlEncoded.
        /// </summary>
        [HtmlAttributeName(HeaderAsHtmlAttribute)]
        public bool HeaderAsHtml { get; set; }

        /// <summary>
        /// If true displays a close icon to close the alert.
        /// </summary>
        [HtmlAttributeName(DismissableAttribute)]
        public bool Dismissible { get; set; }

        /// <summary>
        /// If true, then the Header and Message properties are ignored and the content for the alert is taken 
        /// from whatever content (html) is included between the alert tags i.e. children of the alert element.
        /// </summary>
        [HtmlAttributeName(IncludeHtmlAsChildAttribute)]
        public bool HtmlChild { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(Header) && !HtmlChild)
                return;

            var htmlContent = HtmlChild
                ? ConstructWithHtmlChild(output)
                : ConstructElementAndBody(output);

            output.Content.SetHtmlContent(htmlContent);
        }

        public string ConstructWithHtmlChild(TagHelperOutput output)
        {
            var childContent = output.GetChildContentAsync().Result.GetContent();

            BuildAlertCss();

            output.TagName = HtmlConstants.Elements.Div;

            output.Attributes.Add(HtmlConstants.Attributes.ClassAttribute, CssClass);
            output.Attributes.Add(HtmlConstants.Attributes.RoleAttribute, "alert");

            return childContent;
        }

        public string ConstructElementAndBody(TagHelperOutput output)
        {
            var icon = ResolveIcon(Icon);
            
            if (Icon.StartsWith(DangerIcon))
            {
                icon = string.Concat(FaWarningIcon, StringSpace, BsTextDangerClass); // force to error color
            }

            if (Dismissible && !AlertClass.Contains(BsAlertDismissibleClass))
                AlertClass += string.Concat(StringSpace, BsAlertDismissibleClass);

            BuildAlertCss();

            output.TagName = HtmlConstants.Elements.Div;
            output.Attributes.Add(HtmlConstants.Attributes.ClassAttribute, CssClass);
            output.Attributes.Add(HtmlConstants.Attributes.RoleAttribute, "alert");

            var htmlContentStringBuilder = new StringBuilder();

            if (Dismissible)
            {
                var dismissButtonBuilder = new TagBuilder(HtmlConstants.Elements.Button) { TagRenderMode = TagRenderMode.Normal };
                dismissButtonBuilder.MergeAttribute("data-dismiss", "alert");
                dismissButtonBuilder.MergeAttribute("aria-label", "close");
                dismissButtonBuilder.MergeAttribute(HtmlConstants.Attributes.TypeAttribute, "button");
                dismissButtonBuilder.InnerHtml.AppendHtml("<span aria-hidden='true'>&times;</span>");
                dismissButtonBuilder.AddCssClass("close");

                htmlContentStringBuilder.Append(dismissButtonBuilder.ProcessTagBuilderAsString());
            }

            string messageText = !MessageAsHtml ? System.Net.WebUtility.HtmlEncode(Message) : Message;
            string headerText = !HeaderAsHtml ? System.Net.WebUtility.HtmlEncode(Header) : Header;

            if (string.IsNullOrEmpty(Header))
            {
                if (!string.IsNullOrEmpty(icon))
                {
                    htmlContentStringBuilder.Append($"<i class='fa fa-{icon}'></i>&nbsp;");
                }

                htmlContentStringBuilder.Append($"{messageText}");
            }
            else
            {
                htmlContentStringBuilder.Append("<h4 class='alert-heading'>");

                htmlContentStringBuilder.Append(
                    string.IsNullOrEmpty(icon)
                        ? $"{headerText}</h4><hr/>{messageText}"
                        : $"{headerText}</h4><hr/><i class='fa fa-{icon}'></i>&nbsp;{messageText}"
                    );
            }

            return htmlContentStringBuilder.ToString();

        }

        private void BuildAlertCss()
        {
            if (ReferenceEquals(CssClass, null))
                CssClass = BsAlertClasses + AlertClass;
            else
                CssClass = string.Concat(CssClass, StringSpace, BsAlertClasses, AlertClass);
        }

        private string ResolveIcon(string icon)
        {
            if (string.IsNullOrEmpty(icon))
            {
                Icon = ""; // prevent nullref exceptions further on in code.
                return "";
            }

            icon = icon.Trim();

            string faIconClass;

            switch (icon)
            {
                case InfoIcon: faIconClass = FaInfoIcon; break;
                case SuccessIcon: faIconClass = FaSuccessIcon; break;
                case DangerIcon:
                case WarningIcon: faIconClass = FaWarningIcon; break;
                default: faIconClass = ""; break;
            }

            return faIconClass;
        }
    }
}
