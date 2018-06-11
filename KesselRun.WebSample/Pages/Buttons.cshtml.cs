using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KesselRun.WebSample.Pages
{
    public class ButtonsModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "This page shows samples of the various Buttons TagHelpers.";
        }
    }
}
