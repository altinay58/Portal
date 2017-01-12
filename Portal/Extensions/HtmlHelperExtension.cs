using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.Mvc;
using System.Web.Routing;
using mwm = Microsoft.Web.Mvc.Internal;
using System.Web.Mvc;
using System.Linq.Expressions;
using Portal.Models;
using System.Text;

namespace Portal.Extensions
{
    public static class HtmlHelperExtension
    {
        public static string BuildUrlFromExpression<TController>(RequestContext context, RouteCollection routeCollection, Expression<Action<TController>> action) where TController : Controller
        {
            RouteValueDictionary routeValues = mwm.ExpressionHelper.GetRouteValuesFromExpression(action);
            VirtualPathData vpd = routeCollection.GetVirtualPathForArea(context, routeValues);//.GetVirtualPath(context, routeValues);
            return (vpd == null) ? null : vpd.VirtualPath;
        }
        public static string BuildUrlFromExpressionFG<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
        {
            return BuildUrlFromExpression<TController>(helper.ViewContext.RequestContext, helper.RouteCollection, action);
        }
        public static IHtmlString Pager(this HtmlHelper htmlHelper,
      PaginatedList pagingData, string queryData, Func<int?, HtmlHelper, string, string> fun)
        {
            StringBuilder html = new StringBuilder("");
            html.Append("<ul class=\"pagination\">");
            if (pagingData.TotalPages > 1)
            {


                // Previous Page

                if (pagingData.PageIndex > 1)
                {
                    if (pagingData.PageIndex == 2)
                    {

                        html.AppendFormat("<li><a href=\"{0}\"  >", fun(null, htmlHelper, queryData));
                        html.Append(" << önceki </a></li>");

                    }
                    else
                    {
                        html.AppendFormat("<li><a href=\"{0}\"  >", fun(pagingData.PageIndex - 1, htmlHelper, queryData));
                        html.Append(" << önceki </a></li>");
                    }

                }
                else
                {
                    //html.Append("<span class=\"last\" > << önceki</span>");
                }

                // first page
                if (pagingData.PageIndex > 1)
                {
                    html.AppendFormat("<li><a href=\"{0}\"   >", fun(null, htmlHelper, queryData));
                    html.Append("1</a></li>");
                }
                if (pagingData.PageIndex > 4)
                    html.Append("<li><span class=\"extend\">...</span></li>");
                // current page previous 2
                for (int i = pagingData.PageIndex - 2; i < pagingData.PageIndex; i++)
                {
                    if (i > 1)
                    {
                        html.AppendFormat("<li><a href=\"{0}\"   >{1}", fun(i, htmlHelper, queryData), i);
                        html.Append("</a></li>");
                    }
                }
                // current page
                if (pagingData.PageIndex != pagingData.TotalPages)
                    html.AppendFormat("<li><span class=\"current-page\">{0}</span></li>", pagingData.PageIndex);


                // current page next 2
                for (int i = pagingData.PageIndex + 1; i < pagingData.PageIndex + 3; i++)
                {
                    if (i < pagingData.TotalPages)
                    {
                        html.AppendFormat("<li><a href=\"{0}\"   >{1}", fun(i, htmlHelper, queryData), i);
                        html.Append("</a></li>");
                    }
                }
                if ((pagingData.PageIndex + 2) < pagingData.TotalPages - 1)
                    html.Append("<li><span class=\"extend\">...</span></li>");
                // last Page
                if (pagingData.PageIndex == pagingData.TotalPages)
                {
                    html.AppendFormat("<li><span class=\"active\">{0}</span><li>", pagingData.PageIndex);

                }
                else
                {
                    html.AppendFormat("<li><a href=\"{0}\"   >{1}</a></li>", fun(pagingData.TotalPages, htmlHelper, queryData), pagingData.TotalPages);
                }
                // Next Page
                if (pagingData.PageIndex < pagingData.TotalPages)
                {

                    html.AppendFormat("<li><a href=\"{0}\"  >", fun(pagingData.PageIndex + 1, htmlHelper, queryData));

                    html.Append("sonraki >> </a></li>");
                }
                else
                {

                    //html.Append("<span class=\"AtEnd\" > sonraki >> </span>");
                }

                html.Append("</ul>");
                html.Append("<br style=\"clear:both\" />");
            }
            return htmlHelper.Raw(html.ToString());

        }
        public static IHtmlString MetaDescription(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Raw(string.Format("\"{0}\"", "işletim sistelerinin çözüm merkezi."));
        }
        public static IHtmlString MetaKeyboard(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Raw(string.Format("\"{0}\"", "windows xp,windows 7,windows vista,windows server 2003,windows server 2008,windows ce 6.0,windows phone7,pardus,ubuntu,android,macosx,symbianOs"));
        }

    }
}