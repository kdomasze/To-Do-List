using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Pages.Entries;

namespace ToDoList.Utilities
{
    public static class HtmlExtentions
    {
        public static IHtmlContent PrintEntry(this IHtmlHelper helper, EntryItem entry)
        {
            StringBuilder output = new StringBuilder();
            if(entry.Children.Count > 0)
            {
                foreach(EntryItem item in entry.Children)
                {
                    output.Append("<tr>");
                    output.Append("<td>");
                    output.Append(item.Entry.Title);
                    output.Append("</td>");
                    output.Append("<td>");
                    output.Append(item.Entry.CreationDate);
                    output.Append("</td>");
                    output.Append("<td>");
                    output.Append(item.Entry.DueDate);
                    output.Append("</td>");
                    output.Append("<td>");
                    output.Append(item.Entry.Completed);
                    output.Append("</td>");
                    output.Append("<td>");
                    //<a asp-page="./Edit" asp-route-id="@item.Entry.ID">Edit</a> |
                    //<a asp-page="./Details" asp-route-id="@item.Entry.ID">Details</a> |
                    //<a asp-page="./Delete" asp - route - id = "@item.Entry.ID" > Delete </ a >
                    output.Append("</td>");
                    output.Append("</tr>");
                    output.Append(PrintEntry(helper, item));
                }
            }

            return new HtmlString(output.ToString());
        }
    }
}
