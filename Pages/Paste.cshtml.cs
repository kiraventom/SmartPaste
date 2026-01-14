using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPaste.Model;

namespace SmartPaste.Pages;

public class PasteModel(MainContext db) : PageModel
{
    public Model.Paste Paste { get; private set; }

    [BindProperty]
    public string Password { get; set; }

    public bool GetUnlocked(string link) => HttpContext.Session.GetInt32(UnlockKey(link)) == 1;
    private void SetUnlocked(string link, bool val) => HttpContext.Session.SetInt32(UnlockKey(link), val ? 1 : 0);

    public bool IsOneShotShown 
    {
        get => TempData.TryGetValue("IsOneShotShown", out var isOneShotShown) && (bool)isOneShotShown;
        set => TempData["IsOneShotShown"] = value;
    }

    private string UnlockKey(string link) => $"Unlocked:{link}";

    public async Task<IActionResult> OnGetAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        return Paste is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostUnlockAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        if (Paste is null)
            return NotFound();

        if (Paste.Protected && !GetUnlocked(link))
        {
            var passwordHash = Utils.Md5Hex(Password);
            if (string.Equals(Paste.PasswordHash, passwordHash, StringComparison.Ordinal))
                SetUnlocked(link, true);
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostShowAsync(string link)
    {
        Paste = db.Pastes.SingleOrDefault(p => p.Link == link);
        if (Paste is null)
            return NotFound();

        if (Paste.Protected && !GetUnlocked(link))
            return Page(); 

        if (Paste.OneShot)
        {
            IsOneShotShown = true;

            db.Pastes.Remove(Paste);
            await db.SaveChangesAsync();
        }

        return Page();
    }
}
