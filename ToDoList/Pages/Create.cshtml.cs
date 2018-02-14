using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoList.Models;

namespace ToDoList.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly TaskContext _context;

        public CreateModel(TaskContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Task Task { get; set; }

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
                        
            Task.Parent = parent;
            Task.CreationDate = DateTime.Now;

            _context.Task.Add(Task);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}