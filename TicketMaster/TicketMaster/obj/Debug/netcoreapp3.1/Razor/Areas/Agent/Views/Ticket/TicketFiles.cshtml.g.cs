#pragma checksum "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ea8793fb2a012e5af93aaae6a4c256c82f9bfbf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Agent_Views_Ticket_TicketFiles), @"mvc.1.0.view", @"/Areas/Agent/Views/Ticket/TicketFiles.cshtml")]
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
#line 1 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\_ViewImports.cshtml"
using TicketMaster;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\_ViewImports.cshtml"
using TicketMaster.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ea8793fb2a012e5af93aaae6a4c256c82f9bfbf", @"/Areas/Agent/Views/Ticket/TicketFiles.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ffa3ed648da138917bf25353005b227ca806f06", @"/Areas/Agent/Views/_ViewImports.cshtml")]
    public class Areas_Agent_Views_Ticket_TicketFiles : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TicketMaster.Areas.Admin.ViewModels.Ticket.DisplayAllTicketFilesViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
  
    ViewData["Title"] = "TicketFiles";
    Layout = "~/Areas/Agent/Views/Shared/_AgentLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1>Ticket Files</h1>
<div class=""row"">
    <div>
        <div class=""card"">
            <div class=""card-header"">
                <div class=""card-tools"">
                    <div class=""input-group input-group-sm"" style=""width: 300px;"">

                        <input type=""text"" id=""myInput"" onkeyup=""myFunction(0)"" class=""form-control float-right"" placeholder=""Search by id..."">

                    </div>
                </div>
            </div>
            <!-- /.card-header -->
            <div class=""card-body table-responsive p-0"">
                <table class=""table table-hover"">

                    <thead class=""text-nowrap"">
                        <tr>
                            <th>Id</th>
                            <th>Alt</th>
                            <th>...</th>
                            <th>Ticket Id</th>
                        </tr>
                    </thead>
                    <tbody id=""myTableBody"">
");
#nullable restore
#line 33 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                         foreach (var file in Model.Files)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <tr>\r\n                                <td>");
#nullable restore
#line 36 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                               Write(file.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 37 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                               Write(file.Alt);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 38 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                               Write(file.FileUpload);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                <td>");
#nullable restore
#line 39 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                               Write(file.TicketId);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n                            </tr>\r\n");
#nullable restore
#line 41 "F:\Projects\repos\TicketMaster\TicketMaster\Areas\Agent\Views\Ticket\TicketFiles.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </tbody>\r\n                </table>\r\n            </div>\r\n            <!-- /.card-body -->\r\n        </div>\r\n        <!-- /.card -->\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n\r\n        $(document).ready(myFuction(1));\r\n    </script>\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TicketMaster.Areas.Admin.ViewModels.Ticket.DisplayAllTicketFilesViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
