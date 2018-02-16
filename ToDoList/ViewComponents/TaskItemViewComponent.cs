using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.ViewComponents
{
    public class TaskItemViewComponent : ViewComponent
    {
        public TaskItemViewComponent()
        {

        }

        public IViewComponentResult Invoke(TaskItem taskitem)
        {
            return View(taskitem.Task);
        }
    }
}
