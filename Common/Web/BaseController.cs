using Common.DataAccess;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web;

/// <summary>
/// 
/// </summary>
public class BaseController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    protected readonly UnitOfWork UnitOfWork;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWork"></param>
    public BaseController(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork; 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected new IActionResult Ok()
    {
        if (this.Request is not { Method: "GET" })
        {
            UnitOfWork.Commit();
        }
        return base.Ok(Envelope.Ok());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected IActionResult Ok<T>(T result)
    {
        if (this.Request != null && this.Request.Method != "GET")
        {
            UnitOfWork.Commit();
        }
        return base.Ok(Envelope.Ok(result));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="commit"></param>
    /// <returns></returns>
    protected IActionResult Warning(string message, bool commit = false)
    {
        if (this.Request.Method != "GET" && commit)
        {
            UnitOfWork.Commit();
        }
        return base.Ok(Envelope.Error(message, Severity.Warning));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    protected IActionResult Error(string errorMessage)
    {
        return BadRequest(Envelope.Error(errorMessage, Severity.Error));
    }

    protected IActionResult FromResult(Result result)
    {
        return result.IsSuccess ? Ok() : Error(result.Error);
    }
    protected IActionResult FromResult<T>(Result<T> result)
    {
        return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
    }
}