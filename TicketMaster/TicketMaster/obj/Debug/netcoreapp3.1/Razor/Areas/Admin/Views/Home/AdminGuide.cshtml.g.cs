#pragma checksum "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4d491c459c52d2b5384423bfd7a764f593553c3c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Home_AdminGuide), @"mvc.1.0.view", @"/Areas/Admin/Views/Home/AdminGuide.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\_ViewImports.cshtml"
using TicketMaster;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\_ViewImports.cshtml"
using TicketMaster.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4d491c459c52d2b5384423bfd7a764f593553c3c", @"/Areas/Admin/Views/Home/AdminGuide.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ffa3ed648da138917bf25353005b227ca806f06", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Home_AdminGuide : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
  
    ViewData["Title"] = "AdminGuide";
    Layout = "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Admin Guide</h1>\r\n\r\n");
#nullable restore
#line 9 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
 if (ViewBag.guideError == null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div>\r\n        <h3 style=\"color:#fc5e3d;\"> Navigation bar guide</h3>\r\n        <text>");
#nullable restore
#line 13 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.navbarGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n    <div>\r\n        <h3 style=\"color:#f8723a;\"> Home guide</h3>\r\n        <text>");
#nullable restore
#line 17 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.homeGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n    <div>\r\n        <h3 style=\"color:#fca336;\"> Users guide</h3>\r\n        <text>");
#nullable restore
#line 21 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.userGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n    <div>\r\n        <h3 style=\"color:#fcb257;\"> Companies guide</h3>\r\n        <text>");
#nullable restore
#line 25 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.companyGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n    <div>\r\n        <h3 style=\"color:#fccc36;\"> Projects guide</h3>\r\n        <text>");
#nullable restore
#line 29 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.projectGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n    <div>\r\n        <h3 style=\"color:#fce536;\"> Tickets guide</h3>\r\n        <text>");
#nullable restore
#line 33 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
         Write(ViewBag.ticketGuide);

#line default
#line hidden
#nullable disable
            WriteLiteral("</text>\r\n    </div>\r\n");
#nullable restore
#line 35 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div>\r\n        <text class=\"text-danger\">\r\n            ");
#nullable restore
#line 40 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
       Write(ViewBag.guideError);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </text>\r\n    </div>\r\n");
#nullable restore
#line 43 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Admin\Views\Home\AdminGuide.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
