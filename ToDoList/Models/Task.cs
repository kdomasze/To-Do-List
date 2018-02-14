using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [Display(Name = "Completed")]
        public bool Completed { get; set; }
        public int Parent { get; set; }

        /// <summary>
        /// Recursively finds the parent for each <c>task</c>. If no parent is found in the specified <c>taskItem</c>, a nullified version of <c>TaskItem</c> will be returned.
        /// </summary>
        /// <param name="task">The child <c>Task</c>.</param>
        /// <param name="taskItem">The <c>TaskItem</c> to start the search from.</param>
        /// <returns>The <c>TaskItem</c> that represents the parent of task or a new <c>TaskItem(null)</c>.</returns>
        public static TaskItem FindParentForTask(Task task, TaskItem taskItem)
        {
            TaskItem output = new TaskItem(null);

            if (taskItem.Task.ID != task.Parent)
            {
                foreach (TaskItem children in taskItem.Children)
                {
                    output = FindParentForTask(task, children);
                    if (output.Task != null) break;
                }
            }
            else
            {
                output = taskItem;
            }

            return output;
        }

        /// <summary>
        /// Returns a list of <c>TaskList</c>s based on a list of <c>Task</c>s.
        /// </summary>
        /// <param name="taskList">List of <c>Task</c>s to base list on.</param>
        /// <returns>List of <c>TaskList</c>s</returns>
        public static IList<TaskItem> GetTaskItemList(List<Task> taskList)
        {
            IList<TaskItem> TaskItemList = new List<TaskItem>();

            foreach (var task in taskList)
            {
                if (task.Parent == 0)
                {
                    TaskItemList.Add(new TaskItem(task));
                }
                else
                {
                    foreach (var taskItem in TaskItemList)
                    {
                        var parentTaskItem = FindParentForTask(task, taskItem);
                        if (parentTaskItem.Task == null) continue;

                        parentTaskItem.Children.Add(new TaskItem(task));
                        break;
                    }
                }
            }

            return TaskItemList;
        }
    }

    public struct TaskItem
    {
        public Task Task;
        public IList<TaskItem> Children;

        public TaskItem(Task task)
        {
            Task = task;
            Children = new List<TaskItem>();
        }
    }
}
