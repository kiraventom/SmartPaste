using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPaste.Model;

namespace SmartPaste.Pages;

public class PasteModel(MainContext db) : PageModel
{
    public Model.Paste Paste { get; private set; }
    public bool IsOneShotShown { get; private set; }

    public async Task<IActionResult> OnGetAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        return Paste is null ? NotFound() : Page();
    }
    
    public async Task<IActionResult> OnPostShowAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        if (Paste is null)
            return NotFound();

        if (Paste.OneShot)
        {
            IsOneShotShown = true;

            db.Pastes.Remove(Paste);
            await db.SaveChangesAsync();
        }

        return Page();
    }
}
