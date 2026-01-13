using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmartPaste.Model;

namespace SmartPaste.Pages;

public class IndexModel(MainContext db) : PageModel
{
    private const int MAX_ATTEMPTS = 10;
    private const int SQLITE_CONSTRAINT_ERROR_CODE = 19;
    private const int SQLITE_CONSTRAINT_UNIQUE_ERROR_CODE = 2067;

    private const string SYMBOLS = "abcdefghkmnprstuvwxyzABCDEFGHKLMNPRSTUVWXYZ";
    private const int LINK_LENGTH = 6;

    public static IReadOnlyDictionary<Expiration, string> ExpirationCaptions { get; } = 
    new Dictionary<Expiration, string>() {
        {Expiration.OnceRead, "Once read" },
        {Expiration.TenMinutes, "In 10 minutes" },
        {Expiration.Hour, "In 1 hour" },
        {Expiration.SixHours, "In 6 hours" },
        {Expiration.Day, "In a day" },
        {Expiration.Week, "In a week" },
        {Expiration.Never, "Never" },
    };

    [BindProperty]
    [Required]
    public string Text { get; set; }

    [BindProperty]
    public Expiration Expiration { get; set; } = Expiration.TenMinutes;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (string.IsNullOrEmpty(Text))
            return Page();

        var paste = await SavePaste();
        if (paste is not null)
        {
            Text = null;
            return RedirectToPage("/Paste", new { link = paste.Link });
        }
        else
        {
            ModelState.AddModelError("UNIQUE", "Failed to generate unique link");
            return Page();
        }
    }

    private async Task<Model.Paste> SavePaste()
    {
        for (int i = 0; i < MAX_ATTEMPTS; ++i)
        {
            var paste = new Model.Paste()
            {
                Text = Text, 
                Created = DateTime.UtcNow,
                ExpiresMin = (int)Expiration,
                Link = BuildLink()
            };

            db.Pastes.Add(paste);

            try
            {
                await db.SaveChangesAsync();
                return paste;
            }
            catch (DbUpdateException ex) when 
                (ex.InnerException is SqliteException sqliteEx 
                 && sqliteEx.SqliteErrorCode == SQLITE_CONSTRAINT_ERROR_CODE 
                 && sqliteEx.SqliteExtendedErrorCode == SQLITE_CONSTRAINT_UNIQUE_ERROR_CODE)
            {
                db.Entry(paste).State = EntityState.Detached;
            }
        }

        return null;
    }

    private static string BuildLink()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < LINK_LENGTH; ++i)
        {
            var index = Random.Shared.Next(SYMBOLS.Length);
            sb.Append(SYMBOLS[index]);
        }

        return sb.ToString();
    }
}
