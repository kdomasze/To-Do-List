using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskContext _context;

        public IndexModel(TaskContext context)
        {
            _context = context;
        }

        public IList<TaskItem> Tasks { get; set; }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            var taskList = await _context.Task.ToListAsync();

            Tasks = Models.Task.GetTaskItemList(taskList);
        }
    }
}
