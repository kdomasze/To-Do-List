using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ToDoList.Pages.Entries;
using static ToDoList.Pages.Entries.IndexModel;

namespace ToDoList.Utilities
{
    public static class HtmlExtentions
    {
        /// <summary>
        /// Special helper function that prints the todo list entries recursively if there are any children entries to be printed
        /// </summary>
        /// <param name="helper">Reference to the IHtmlHelper.</param>
        /// <param name="entry">The entry item to check for children entries.</param>
        /// <param name="startMargin">the margin that the children elements will start their indent from.</param>
        /// <returns>The complete HTML content to be rendered</returns>
        public static IHtmlContent PrintEntry(this IHtmlHelper<IndexModel> helper, EntryItem entry, int startMargin = 20)
        {
            StringBuilder output = new StringBuilder();
            if (entry.Children.Count > 0)
            {
                foreach (EntryItem item in entry.Children)
                {
                    // generate div
                    TagBuilder divStyled = new TagBuilder("div");
                    divStyled.AddCssClass("entry-container");
                    divStyled.MergeAttribute("style", $"padding-left: {startMargin}px;");

                    // generate ul
                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("entry-item");

                    // generate li for title
                    TagBuilder liTitle = new TagBuilder("li");
                    liTitle.AddCssClass("entry-title");
                    liTitle.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.Title));

                    // generate li for due date
                    TagBuilder liDueDate = new TagBuilder("li");
                    liDueDate.AddCssClass("entry-due-date");
                    liDueDate.InnerHtml.Append("Due: ");
                    liDueDate.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.DueDate));

                    // generate li for completed
                    TagBuilder liCompleted = new TagBuilder("li");
                    liCompleted.AddCssClass("entry-completed");
                    liCompleted.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.Completed));

                    // details link
                    TagBuilder liDetails = BuildLiLink(helper, "entry-details", "Details", item.Entry.ID, "fa-info-circle");

                    // Delete link
                    TagBuilder liDelete = BuildLiLink(helper, "entry-delete", "Delete", item.Entry.ID, "fa-trash");

                    // Add link
                    TagBuilder liAdd = BuildLiLink(helper, "entry-add", "Create", item.Entry.ID, "fa-plus");

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
                    output.Append(PrintEntry(helper, item, startMargin + 16));
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
            a.MergeAttribute("href", $"./Entries/{action}?id={id}");

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
