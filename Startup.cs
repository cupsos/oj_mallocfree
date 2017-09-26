using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace oj_mallocfree
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            ResultMaker.Init();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseStatusCodePages();

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/result.html" && context.Request.Method == "POST")
                {

                    TagBuilder page = new TagBuilder("html");
                    TagBuilder body = new TagBuilder("body");

                    string source = context.Request.Form["source"];
                    string test = context.Request.Form["test"];
                    ResultMaker rm = new ResultMaker(source, test);

                    if (!rm.isInitSuccess)
                        body.InnerHtml.Append(rm.InitFailLog);
                    else
                    {
                        rm.runProgram();
                        foreach(string line in rm.getMemoryLog().Split('\n'))
                        {
                            body.InnerHtml.Append(line);
                            body.InnerHtml.AppendHtmlLine("<br />");
                        }
                    }
                    page.InnerHtml.AppendHtml(body);

                    using (StringWriter writer = new StringWriter())
                    {
                        page.WriteTo(writer, HtmlEncoder.Default);
                        await context.Response.WriteAsync(writer.ToString());
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            });
        }
    }
}
