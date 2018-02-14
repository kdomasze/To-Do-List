using System.Collections.Generic;
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
            if (id == null)
            {
                return NotFound();
            }
            
            await DeleteChildrenTasksAsync(id);

            return RedirectToPage("./Index");
        }

        private async System.Threading.Tasks.Task DeleteChildrenTasksAsync(int? parentID)
        {
            var taskList = await _context.Task.ToListAsync();

            IList<TaskItem> Tasks = Models.Task.GetTaskItemList(taskList);

            foreach (var task in Tasks)
            {
                await DeleteTasksAsync(parentID, task);
            }
        }

        private async System.Threading.Tasks.Task DeleteTasksAsync(int? parentID, TaskItem taskItem)
        {
            if (taskItem.Task.Parent != parentID && taskItem.Task.ID != parentID)
            {
                foreach (TaskItem children in taskItem.Children)
                {
                    await DeleteTasksAsync(parentID, children);
                }

                return;
            }

            // delete task
            Task = await _context.Task.FindAsync(taskItem.Task.ID);

            if (Task != null)
            {
                _context.Task.Remove(Task);
                await _context.SaveChangesAsync();
            }

            foreach (TaskItem children in taskItem.Children)
            {
                await DeleteTasksAsync(taskItem.Task.ID, children);
            }
        }
    }
}
