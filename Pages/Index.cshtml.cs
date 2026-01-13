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

    [BindProperty]
    public string Text { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
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
                ExpiresHours = 1, // TODO
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
