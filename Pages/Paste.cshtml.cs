using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPaste.Model;

namespace SmartPaste.Pages;

public class PasteModel(MainContext db) : PageModel
{
    public Model.Paste Paste { get; private set; }

    public async Task<IActionResult> OnGetAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        return Paste is null ? NotFound() : Page();
    }
}
