using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly TaskContext _context;

        public DeleteModel(TaskContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            // delete the task and all children tasks
            Task.PerformActionOnChildrenTasks(await _context.Task.ToListAsync(), (int)id, DeleteTask);

            return RedirectToPage("./Index");
        }

        private void DeleteTask(TaskItem taskItem)
        {
            if (taskItem.Task != null)
            {
                _context.Task.Remove(taskItem.Task);
                _context.SaveChangesAsync();
            }
        }
    }
}
