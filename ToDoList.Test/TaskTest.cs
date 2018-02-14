using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Test
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void GetTaskItemListTest()
        {
            var taskList = new List<Task>
            {
                new Task
                    {
                        ID = 1,
                        Title = "Task 1",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 2,
                        Title = "Task 2",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-4"),
                        DueDate = DateTime.Parse("2018-2-18"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 3,
                        Title = "Task 3",
                        Parent = 1,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 4,
                        Title = "Task 4",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 5,
                        Title = "Task 5",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-6"),
                        DueDate = DateTime.Parse("2018-2-16"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 6,
                        Title = "Task 6",
                        Parent = 5,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 7,
                        Title = "Task 7",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 8,
                        Title = "Task 8",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 9,
                        Title = "Task 9",
                        Parent = 8,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    }
            };

            var list = Task.GetTaskItemList(taskList);

            Assert.AreEqual(list[0].Task.Title, "Task 1");
            Assert.AreEqual(list[1].Task.Title, "Task 2");
            Assert.AreEqual(list[0].Children[0].Task.Title, "Task 3");
            Assert.AreEqual(list[1].Children[0].Task.Title, "Task 4");
            Assert.AreEqual(list[1].Children[1].Task.Title, "Task 5");
            Assert.AreEqual(list[1].Children[1].Children[0].Task.Title, "Task 6");
            Assert.AreEqual(list[1].Children[2].Task.Title, "Task 7");
            Assert.AreEqual(list[1].Children[3].Task.Title, "Task 8");
            Assert.AreEqual(list[1].Children[3].Children[0].Task.Title, "Task 9");
        }

        [TestMethod]
        public void PerformActionOnChildrenTasksTest()
        {
            var taskList = new List<Task>
            {
                new Task
                    {
                        ID = 1,
                        Title = "Task 1",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 2,
                        Title = "Task 2",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-4"),
                        DueDate = DateTime.Parse("2018-2-18"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 3,
                        Title = "Task 3",
                        Parent = 1,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 4,
                        Title = "Task 4",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 5,
                        Title = "Task 5",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-6"),
                        DueDate = DateTime.Parse("2018-2-16"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 6,
                        Title = "Task 6",
                        Parent = 5,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 7,
                        Title = "Task 7",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 8,
                        Title = "Task 8",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        ID = 9,
                        Title = "Task 9",
                        Parent = 8,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    }
            };

            Task.PerformActionOnChildrenTasks(taskList, 1, (m) => m.Task.Title += "_edit");
            Task.PerformActionOnChildrenTasks(taskList, 2, (m) => m.Task.Title += "_diff");
            Task.PerformActionOnChildrenTasks(taskList, 5, (m) => m.Task.Title += "_etc");

            Assert.AreEqual(taskList[0].Title, "Task 1_edit");
            Assert.AreEqual(taskList[1].Title, "Task 2_diff");
            Assert.AreEqual(taskList[2].Title, "Task 3_edit");
            Assert.AreEqual(taskList[3].Title, "Task 4_diff");
            Assert.AreEqual(taskList[4].Title, "Task 5_diff_etc");
            Assert.AreEqual(taskList[5].Title, "Task 6_diff_etc");
            Assert.AreEqual(taskList[6].Title, "Task 7_diff");
            Assert.AreEqual(taskList[7].Title, "Task 8_diff");
            Assert.AreEqual(taskList[8].Title, "Task 9_diff");
        }
    }
}
