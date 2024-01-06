using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace Zack.ASPNETCore;

/// <summary>
/// 提供事务管理功能
/// </summary>
public class UnitOfWorkFilter : IAsyncActionFilter
{
    /// <summary>
    /// 主要目的是通过使用事务范围确保数据的一致性。
    /// 当一个HTTP请求涉及多个数据库操作时，
    /// 要么所有操作都成功并提交，
    /// 要么在发生错误时全部撤销，
    /// 从而保证数据的完整性和一致性。
    /// </summary>
    /// <param name="actionDesc"></param>
    /// <returns></returns>
    private static UnitOfWorkAttribute? GetUoWAttr(ActionDescriptor actionDesc)
    {
        var caDesc = actionDesc as ControllerActionDescriptor;
        if (caDesc == null)
        {
            return null;
        }
        //try to get UnitOfWorkAttribute from controller,
        //if there is no UnitOfWorkAttribute on controller, 
        //try to get UnitOfWorkAttribute from action

        //两种场景
        //1.在控制器级别遇到UnitOfWorkAttribute
        //2.在单个动作方法级别遇到
        var uowAttr = caDesc.ControllerTypeInfo
            .GetCustomAttribute<UnitOfWorkAttribute>();
        if (uowAttr != null)
        {
            return uowAttr;
        }
        else
        {
            return caDesc.MethodInfo
                .GetCustomAttribute<UnitOfWorkAttribute>();
        }
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        //1.OnActionExecutionAsync
        var uowAttr = GetUoWAttr(context.ActionDescriptor);
        if (uowAttr == null)
        {
            await next();
            return;
        }
        //2.创建TransactionScope
        using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
        List<DbContext> dbCtxs = new List<DbContext>();
        foreach (var dbCtxType in uowAttr.DbContextTypes)
        {
            //用HttpContext的RequestServices
            //确保获取的是和请求相关的Scope实例
            var sp = context.HttpContext.RequestServices;
            DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
            dbCtxs.Add(dbCtx);
        }
        var result = await next();
        if (result.Exception == null)
        {
            foreach (var dbCtx in dbCtxs)
            {
                await dbCtx.SaveChangesAsync();
            }
            txScope.Complete();
        }
    }
}