using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoList.Models;

namespace ToDoList.Pages.Entries
{
    public class CreateModel : PageModel
    {
        private readonly EntryContext _context;

        public CreateModel(EntryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Entry Entry { get; set; }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }            

            /*
             * Sets default details for internal book keeping.
             * CreationDate is set to the current DateTime.
             * Parent is set to the id in the POST data if there is one. Otherwise, it is set to 0.
             */
            if (String.IsNullOrEmpty(id) || !int.TryParse(id, out int parent)) parent = 0;
                        
            Entry.Parent = parent;
            Entry.CreationDate = DateTime.Now;

            _context.Entry.Add(Entry);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}