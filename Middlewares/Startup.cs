using System;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;

#region [ listing #1 ]

namespace Middlewares
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Func<IOwinContext, Task> middleware = async (IOwinContext context) =>
            {
                await context.Response.WriteAsync(" [middleware] ");
            };

            app.Run(middleware);
        }
    }
}

#endregion

#region [ listing #2 ]

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            Func<IOwinContext, Func<Task>, Task> middleware = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware - after] ");
//            };

//            app.Use<UseHandlerMiddleware>(middleware);
//        }
//    }
//}

#endregion

#region [ listing #3 - incorrect way to chain middlewares ]

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            Func<IOwinContext, Func<Task>, Task> middleware1 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware1 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware1 - after] ");
//            };

//            Func<IOwinContext, Task> middleware2 = async (IOwinContext context) =>
//            {
//                await context.Response.WriteAsync(" [middleware2] ");
//            };

//            app.Run(middleware2);
//            app.Use<UseHandlerMiddleware>(middleware1);
//        }
//    }
//}

#endregion

#region [ listing #4 - correct way to chain middlewares ]

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            Func<IOwinContext, Func<Task>, Task> middleware1 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware1 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware1 - after] ");
//            };

//            Func<IOwinContext, Task> middleware2 = async (IOwinContext context) =>
//            {
//                await context.Response.WriteAsync(" [middleware2] ");
//            };

//            app.Use<UseHandlerMiddleware>(middleware1);
//            app.Run(middleware2);
//        }
//    }
//}

#endregion

#region [ listing #5 - one more middleware ]

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            Func<IOwinContext, Func<Task>, Task> middleware1 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware1 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware1 - after] ");
//            };

//            Func<IOwinContext, Func<Task>, Task> middleware2 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware2 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware2 - after] ");
//            };

//            Func<IOwinContext, Task> middleware3 = async (IOwinContext context) =>
//            {
//                await context.Response.WriteAsync(" [middleware3] ");
//            };

//            app.Use<UseHandlerMiddleware>(middleware1);
//            app.Use<UseHandlerMiddleware>(middleware2);
//            app.Run(middleware3);
//        }
//    }
//}

#endregion

#region [ listing #6 - middleware2 can short circuit OWIN pipeline ]

// http://localhost:5000/owin
// or
// http://localhost:5000/owin?apiKey=123

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            Func<IOwinContext, Func<Task>, Task> middleware1 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware1 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware1 - after] ");
//            };

//            Func<IOwinContext, Func<Task>, Task> middleware2 = async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware2 - before ]");

//                if ((context.Request.Query["apiKey"] ?? string.Empty).Equals("123"))
//                {
//                    await next.Invoke();
//                }

//                await context.Response.WriteAsync(" [middleware2 - after] ");
//            };

//            Func<IOwinContext, Task> middleware3 = async (IOwinContext context) =>
//            {
//                await context.Response.WriteAsync(" [middleware3] ");
//            };

//            app.Use<UseHandlerMiddleware>(middleware1);
//            app.Use<UseHandlerMiddleware>(middleware2);
//            app.Run(middleware3);
//        }
//    }
//}

#endregion

#region [ listing #7 - coding style preference ]

// http://localhost:5000/owin
// or
// http://localhost:5000/owin?apiKey=123

//namespace Middlewares
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            app.Use(async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware1 - before ]");
//                await next.Invoke();
//                await context.Response.WriteAsync(" [middleware1 - after] ");
//            });

//            app.Use(async (IOwinContext context, Func<Task> next) =>
//            {
//                await context.Response.WriteAsync(" [middleware2 - before ]");

//                if ((context.Request.Query["apiKey"] ?? string.Empty).Equals("123"))
//                {
//                    await next.Invoke();
//                }

//                await context.Response.WriteAsync(" [middleware2 - after] ");
//            });

//            app.Run(async (IOwinContext context) =>
//            {
//                await context.Response.WriteAsync(" [middleware3] ");
//            });
//        }
//    }
//}

#endregion