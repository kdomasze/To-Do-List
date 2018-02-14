using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Tasks
{
    public class DoneModel : PageModel
    {
        private readonly TaskContext _context;

        public DoneModel(TaskContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Task Task { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Task = await _context.Task.SingleOrDefaultAsync(m => m.ID == id);

            if (Task == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // marks the task and all children tasks as completed
            Models.Task.PerformActionOnChildrenTasks(await _context.Task.ToListAsync(), Task.ID, MarkTaskComplete);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(Task.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.ID == id);
        }

        private void MarkTaskComplete(TaskItem taskItem)
        {
            taskItem.Task.Completed = true;
            _context.Attach(taskItem.Task).State = EntityState.Modified;
        }
    }
}