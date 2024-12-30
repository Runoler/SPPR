using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WEB_253503_BARANCHIK.UI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly LinkGenerator linkGenerator;

        public PagerTagHelper(IUrlHelperFactory helperFactory, LinkGenerator linkGenerator)
        {
            _urlHelperFactory = helperFactory;
            this.linkGenerator = linkGenerator;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string Category { get; set; }
        public bool IsAdmin { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var paginationHtml = new TagBuilder("nav");
            paginationHtml.Attributes.Add("aria-label", "Page navigation example");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            // Previous button
            var prevLi = new TagBuilder("li");
            prevLi.AddCssClass("page-item");
            if (CurrentPage == 1) prevLi.AddCssClass("disabled");

            var prevLink = new TagBuilder("a");
            prevLink.AddCssClass("page-link");
            prevLink.Attributes.Add("href",
                IsAdmin ? urlHelper.Page("./Index", new { pageNo = CurrentPage - 1 }) : urlHelper.Action("Index", new { category = Category, pageNo = CurrentPage - 1 }));
            prevLink.InnerHtml.Append("Previous");
            prevLi.InnerHtml.AppendHtml(prevLink);
            ul.InnerHtml.AppendHtml(prevLi);



            // Page number links
            for (int i = 1; i <= TotalPages; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                if (CurrentPage == i) li.AddCssClass("active");

                var link = new TagBuilder("a");
                link.AddCssClass("page-link");
                link.Attributes.Add("href",
                    IsAdmin ? urlHelper.Page("./Index", new {pageNo = i }) : urlHelper.Action("Index", new { category = Category, pageNo = i }));
                link.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(link);
                ul.InnerHtml.AppendHtml(li);
            }

            // Next button
            var nextLi = new TagBuilder("li");
            nextLi.AddCssClass("page-item");
            if (CurrentPage == TotalPages) nextLi.AddCssClass("disabled");

            var nextLink = new TagBuilder("a");
            nextLink.AddCssClass("page-link");
            nextLink.Attributes.Add("href",
                IsAdmin ? urlHelper.Page("./Index", new { pageNo = CurrentPage + 1 }) : urlHelper.Action("Index", new { category = Category, pageNo = CurrentPage + 1 }));
            nextLink.InnerHtml.Append("Next");
            nextLi.InnerHtml.AppendHtml(nextLink);
            ul.InnerHtml.AppendHtml(nextLi);

            paginationHtml.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(paginationHtml);
        }
    }
}
