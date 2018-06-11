using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KesselRun.WebSample.Pages
{
    public class AlertsModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "This page shows samples of the Alert TagHelpers.";
        }
    }
}
