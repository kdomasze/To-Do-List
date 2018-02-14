using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Validation;

namespace ToDoList.Models
{
    public class Task
    {
        public int ID { get; set; }
        [Display(Name = "Task")]
        public string Title { get; set; }
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DateGreaterOrEqualToToday()]
        public DateTime DueDate { get; set; }
        [Display(Name = "Completed")]
        public bool Completed { get; set; }
        public int Parent { get; set; }
        
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
                        if (parentTaskItem == null) continue;

                        ((TaskItem)parentTaskItem).Children.Add(new TaskItem(task));
                        break;
                    }
                }
            }

            return TaskItemList;
        }

        /// <summary>
        /// Recursively finds the parent for each <c>task</c>. If no parent is found in the specified <c>taskItem</c>, a nullified version of <c>TaskItem</c> will be returned.
        /// </summary>
        /// <param name="task">The child <c>Task</c>.</param>
        /// <param name="taskItem">The <c>TaskItem</c> to start the search from.</param>
        /// <returns>The <c>TaskItem</c> that represents the parent of task or a new <c>TaskItem(null)</c>.</returns>
        private static TaskItem? FindParentForTask(Task task, TaskItem taskItem)
        {
            TaskItem? output = null;

            if (taskItem.Task.ID != task.Parent)
            {
                foreach (TaskItem children in taskItem.Children)
                {
                    output = FindParentForTask(task, children);
                    if (output != null) break;
                }
            }
            else
            {
                output = taskItem;
            }

            return output;
        }

        /// <summary>
        /// Performs the specified action on all tasks who's ID or parent ID matches <c>parentID</c>, and all children of those tasks.
        /// </summary>
        /// <param name="taskList">List of all <c>Task</c>s</param>
        /// <param name="parentID">The ID of the root <c>Task</c></param>
        /// <param name="action">The action to be applied to all <c>Task</c>s who are children of the <c>parentID</c></param>
        public static void PerformActionOnChildrenTasks(List<Task> taskList, int parentID, Action<TaskItem> action)
        {
            IList<TaskItem> Tasks = GetTaskItemList(taskList);

            foreach (var task in Tasks)
            {
                PerformAction(parentID, task, action);
            }
        }

        /// <summary>
        /// Recursively performs the specified action on tasks if the parentID matches the task's ID or Parent ID field
        /// </summary>
        /// <param name="parentID">The ID of the root <c>Task</c></param>
        /// <param name="taskItem">The task item being checked to have the <c>action</c> applied to it</param>
        /// <param name="action">The action to be applied to all <c>Task</c>s who are children of the <c>parentID</c></param>
        private static void PerformAction(int parentID, TaskItem taskItem, Action<TaskItem> action)
        {
            if (taskItem.Task.Parent != parentID && taskItem.Task.ID != parentID)
            {
                foreach (TaskItem children in taskItem.Children)
                {
                    PerformAction(parentID, children, action);
                }

                return;
            }

            action(taskItem);

            foreach (TaskItem children in taskItem.Children)
            {
                PerformAction(taskItem.Task.ID, children, action);
            }
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
