using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using static ToDoList.Pages.Tasks.IndexModel;

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
            await MarkChildrenTasksDoneAsync(Task.ID);

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

        /// <summary>
        /// Marks all tasks who's ID or parent ID matches <c>parentID</c>, and all children of those tasks as completed.
        /// </summary>
        /// <param name="parentID">The ID marked as completed</param>
        private async System.Threading.Tasks.Task MarkChildrenTasksDoneAsync(int parentID)
        {
            var taskList = await _context.Task.ToListAsync();

            IList<TaskItem> Tasks = Models.Task.GetTaskItemList(taskList);

            foreach (var task in Tasks)
            {
                MarkTasksCompleted(parentID, task);
            }
        }

        /// <summary>
        /// Recursively marks tasks as completed if the parentID matches the task's ID or Parent ID field
        /// </summary>
        /// <param name="parentID">The ID marked as completed</param>
        /// <param name="taskItem">An task item being marked for completion</param>
        private void MarkTasksCompleted(int parentID, TaskItem taskItem)
        {
            if (taskItem.Task.Parent != parentID && taskItem.Task.ID != parentID)
            {
                foreach (TaskItem children in taskItem.Children)
                {
                    MarkTasksCompleted(parentID, children);
                }

                return;
            }

            // marks task as complete and needed to be updated
            taskItem.Task.Completed = true;
            _context.Attach(taskItem.Task).State = EntityState.Modified;

            foreach (TaskItem children in taskItem.Children)
            {
                MarkTasksCompleted(taskItem.Task.ID, children);
            }
        }
    }
}