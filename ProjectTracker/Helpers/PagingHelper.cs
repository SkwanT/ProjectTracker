using System;
using System.Text;
using System.Web.Mvc;

namespace ProjectTracker.Helpers
{
    public static class PagingHelper
    {

        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            int pageGap = (pagingInfo.TotalPages - pagingInfo.CurrentPage - 5) > 0 ? (pagingInfo.TotalPages - pagingInfo.CurrentPage - 5) : 0;
            int min = pagingInfo.CurrentPage - 4 < 1 ? 1 : pagingInfo.CurrentPage - 4;
            int max = (pagingInfo.CurrentPage + 10) > pagingInfo.TotalPages ? (pagingInfo.TotalPages - pageGap) : (pagingInfo.CurrentPage < 6 ? 10 : pagingInfo.CurrentPage + 5);


            for (int i = min; i <= max; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }

                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}