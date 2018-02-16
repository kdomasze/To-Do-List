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
        /// <param name="startPadding">the margin that the children elements will start their indent from.</param>
        /// <returns>The complete HTML content to be rendered</returns>
        public static IHtmlContent PrintTask(this IHtmlHelper<IndexModel> helper, TaskItem Task, int startPadding = 20)
        {
            StringBuilder output = new StringBuilder();
            if (Task.Children.Count > 0)
            {
                foreach (TaskItem item in Task.Children)
                {
                    if (item.Task.Completed) continue;

                    var content = helper.DisplayFor(modelItem => item.Task, "TaskItem", new { padding = startPadding });
                    
                    output.Append("<hr />");
                    output.Append(GetString(content));
                    output.Append(PrintTask(helper, item, startPadding + 16));
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
