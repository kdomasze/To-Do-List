using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ToDoList.Pages.Entries;

namespace ToDoList.Utilities
{
    public static class HtmlExtentions
    {
        public static IHtmlContent PrintEntry(this IHtmlHelper<ViewTestModel> helper, EntryItem entry, int startMargin = 20)
        {
            /*
            <div style="margin-left: 20px;">
                <ul class="entry-item">
                    <li class="entry-title">@Html.DisplayFor(modelItem => item.Entry.Title)</li>
                    <li class="entry-creation-date">@Html.DisplayFor(modelItem => item.Entry.CreationDate)</li>
                    <li class="entry-due-date">@Html.DisplayFor(modelItem => item.Entry.DueDate)</li>
                    <li class="entry-complete">@Html.DisplayFor(modelItem => item.Entry.Completed)</li>
                    <li class="entry-details"><a asp-page="./Details" asp-route-id="@item.Entry.ID">Details</a></li>
                    <li class="entry-delete"><a asp-page="./Delete" asp-route-id="@item.Entry.ID">Delete</a></li>
                </ul>
            </div> 
            */

            StringBuilder output = new StringBuilder();
            if (entry.Children.Count > 0)
            {
                foreach (EntryItem item in entry.Children)
                {
                    TagBuilder divStyled = new TagBuilder("div");
                    divStyled.AddCssClass("entry-container");
                    divStyled.MergeAttribute("style", $"padding-left: {startMargin}px;");

                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("entry-item");

                    TagBuilder liTitle = new TagBuilder("li");
                    liTitle.AddCssClass("entry-title");
                    liTitle.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.Title));

                    TagBuilder liDueDate = new TagBuilder("li");
                    liDueDate.AddCssClass("entry-due-date");
                    liDueDate.InnerHtml.Append("Due: ");
                    liDueDate.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.DueDate));

                    TagBuilder liCompleted = new TagBuilder("li");
                    liCompleted.AddCssClass("entry-completed");
                    liCompleted.InnerHtml.AppendHtml(helper.DisplayFor(modelItem => item.Entry.Completed));

                    TagBuilder liDetails = new TagBuilder("li");
                    liDetails.AddCssClass("entry-details");

                    TagBuilder aDetails = new TagBuilder("a");
                    aDetails.MergeAttribute("asp-page", "./Details");
                    aDetails.MergeAttribute("asp-route-id", item.Entry.ID.ToString());
                    aDetails.InnerHtml.Append("Details");

                    liDetails.InnerHtml.AppendHtml(aDetails);

                    TagBuilder liDelete = new TagBuilder("li");
                    liDelete.AddCssClass("entry-delete");

                    TagBuilder aDelete = new TagBuilder("a");
                    aDelete.MergeAttribute("asp-page", "./Delete");
                    aDelete.MergeAttribute("asp-route-id", item.Entry.ID.ToString());
                    aDelete.InnerHtml.Append("Delete");

                    liDelete.InnerHtml.AppendHtml(aDelete);

                    ul.InnerHtml.AppendHtml(liTitle);
                    ul.InnerHtml.AppendHtml(liDueDate);
                    ul.InnerHtml.AppendHtml(liCompleted);
                    ul.InnerHtml.AppendHtml(liDetails);
                    ul.InnerHtml.AppendHtml(liDelete);

                    divStyled.InnerHtml.AppendHtml(ul);

                    /*
                    output.Append($"<div style=\"margin-left: {startMargin}px; \">");
                    output.Append("<ul class=\"entry-item\">");
                    output.Append("<li class=\"entry-title\">");
                    output.Append(GetString(helper.DisplayFor(modelItem => item.Entry.Title)));
                    output.Append("</li>");
                    output.Append("<li class=\"entry-creation-date\">");
                    output.Append(GetString(helper.DisplayFor(modelItem => item.Entry.CreationDate)));
                    output.Append("</li>");
                    output.Append("<li class=\"entry-title\">");
                    output.Append(GetString(helper.DisplayFor(modelItem => item.Entry.DueDate)));
                    output.Append("</li>");
                    output.Append("<li class=\"entry-title\">");
                    output.Append(GetString(helper.DisplayFor(modelItem => item.Entry.Completed)));
                    output.Append("</li>");
                    output.Append("<li class=\"entry-title\">");
                    output.Append("<a asp-page=\"./Details\" asp-route-id=\"@item.Entry.ID\">Details</a>");
                    output.Append("</li>");
                    output.Append("<a asp-page=\"./Delete\" asp-route-id=\"@item.Entry.ID\">Delete</a>");
                    output.Append(item.Entry.Title);
                    output.Append("</li>");
                    output.Append("</ul>");
                    output.Append("</div>");
                    */
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
    }
}
