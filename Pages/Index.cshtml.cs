using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartPaste.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Text { get; set; }

    public string Result { get; set; }

    public void OnGet()
    {

    }

    public void OnPost()
    {
        Result = $"Text: {Text}";
        Text = null;
    }
}
