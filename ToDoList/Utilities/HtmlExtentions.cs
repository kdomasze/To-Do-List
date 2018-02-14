using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Encodings.Web;
using ToDoList.Models;
using ToDoList.Pages.Tasks;

namespace ToDoList.Utilities
{
    public static class HtmlExtentions
    {
        /// <summary>
        /// Special helper function that prints the todo list tasks recursively if there are any children tasks to be printed
        /// </summary>
        /// <param name="helper">Reference to the IHtmlHelper.</param>
        /// <param name="Task">The task item to check for children tasks.</param>
        /// <param name="startMargin">the margin that the children elements will start their indent from.</param>
        /// <returns>The complete HTML content to be rendered</returns>
        public static IHtmlContent PrintTask(this IHtmlHelper<IndexModel> helper, TaskItem Task, int startMargin = 20)
        {
            StringBuilder output = new StringBuilder();
            if (Task.Children.Count > 0)
            {
                foreach (TaskItem item in Task.Children)
                {
                    if (item.Task.Completed) continue;

                    // generate div
                    TagBuilder divStyled = new TagBuilder("div");
                    divStyled.AddCssClass("task-container");
                    divStyled.MergeAttribute("style", $"padding-left: {startMargin}px;");

                    // generate ul
                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("task-item");

                    // generate li for title
                    TagBuilder liTitle = new TagBuilder("li");
                    liTitle.AddCssClass("task-title");
                    liTitle.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Task.Title));

                    // generate li for due date
                    TagBuilder liDueDate = new TagBuilder("li");
                    liDueDate.AddCssClass("task-due-date");
                    liDueDate.InnerHtml.Append("Due: ");
                    liDueDate.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Task.DueDate));

                    // details link
                    TagBuilder liCompleted = BuildLiLink(helper, "task-completed", "Done", item.Task.ID, "fa-check");

                    // details link
                    TagBuilder liDetails = BuildLiLink(helper, "task-details", "Details", item.Task.ID, "fa-info-circle");

                    // Delete link
                    TagBuilder liDelete = BuildLiLink(helper, "task-delete", "Delete", item.Task.ID, "fa-trash");

                    // Add link
                    TagBuilder liAdd = BuildLiLink(helper, "task-add", "Create", item.Task.ID, "fa-plus");

                    // assemble html
                    ul.InnerHtml.AppendHtml(liTitle);
                    ul.InnerHtml.AppendHtml(liDueDate);
                    ul.InnerHtml.AppendHtml(liCompleted);
                    ul.InnerHtml.AppendHtml(liDetails);
                    ul.InnerHtml.AppendHtml(liDelete);
                    ul.InnerHtml.AppendHtml(liAdd);

                    divStyled.InnerHtml.AppendHtml(ul);

                    // output to string
                    output.Append("<hr />");
                    output.Append(GetString(divStyled));
                    output.Append(PrintTask(helper, item, startMargin + 16));
                }
            }

            return new HtmlString(output.ToString());
        }

        private static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        private static TagBuilder BuildLiLink(IHtmlHelper<IndexModel> helper, string className, string action, int id, string fontAwesomeClass)
        {
            // generate li for details
            TagBuilder li = new TagBuilder("li");
            li.AddCssClass(className);

            // generate link for details
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", $"./{action}?id={id}");

            // generate font-awesome for details
            TagBuilder i = new TagBuilder("i");
            i.AddCssClass("fas");
            i.AddCssClass(fontAwesomeClass);

            // embed font-awesome and link in li
            a.InnerHtml.AppendHtml(i);
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }
}
