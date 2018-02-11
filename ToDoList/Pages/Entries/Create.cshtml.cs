using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Models;

namespace ToDoList.Pages.Entries
{
    public class CreateModel : PageModel
    {
        private readonly ToDoList.Models.EntryContext _context;

        public CreateModel(ToDoList.Models.EntryContext context)
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

            if (String.IsNullOrEmpty(id) || !int.TryParse(id, out int parent)) parent = 0;

            var currentDateTime = System.DateTime.Now;
            
            Entry.Parent = parent;
            Entry.CreationDate = currentDateTime;

            _context.Entry.Add(Entry);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}