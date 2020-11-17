using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net.Core.Authorization.Filter
{
    /// <summary>
    /// 权限过滤器
    /// </summary>
    public class AuthenticationTest: IAuthorizationFilter, IAllowAnonymousFilter, IOrderedFilter
    {
        public int Order { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new { StatusCodeResult = StatusCodes.Status401Unauthorized, Title = "401", Time = DateTime.Now,test="测试" });
           // context.Result = new StatusCodeResult(401);
            Console.WriteLine("我是Authorization过滤器");
            //请求的地址
            var url = context.HttpContext.Request.Path.Value;
            #region 打印头部信息
            var heads = context.HttpContext.Request.Headers;
            string msg = string.Empty;

            foreach (var item in heads)
            {
                msg += item.Key + ":" + item.Value + "\r\n";
            }

            Console.WriteLine("我是heads：" + msg);
            #endregion

            Console.WriteLine("请求地址：" + url);
            //控制器
            var controller = ((object[])context.RouteData.Values.Values)[1];
            //方法
            var action = ((object[])context.RouteData.Values.Values)[0];

            if (controller.ToString() == "Login")
            {

            }
            else
            {
                //获取请求头是否有Bearer
                if (!ValidateToken(context.HttpContext.Request.Headers["token"]))
                {
                    //context.Result = new JsonResult(new { StatusCodeResult = StatusCodes.Status401Unauthorized, Title = "401", Time = DateTime.Now });
                    //context.Result =new StatusCodeResult(401);
                }

                if (IsHaveAllow(context.Filters))
                {
                    return;
                }

                #region Mvc中使用验证Cookie缓存用户
                //获取请求上下文中用户的信息

                //var calient = context.HttpContext.User.Claims.ToList();
                //if (calient.Count <= 0)
                //{
                //    context.Result = new JsonResult(new { StatusCodeResult = StatusCodes.Status200OK, Title = "未登录", Time = DateTime.Now });
                //}
                #endregion

            }
        }

        #region 验证Head是否含有Token
        public static bool ValidateToken(string head)
        {
            if (head == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 判断请求是不是都被允许
        public static bool IsHaveAllow(IList<IFilterMetadata> filers)
        {
            for (int i = 0; i < filers.Count; i++)
            {
                if (filers[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
