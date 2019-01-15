using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Andoromeda.Kyubey.Incubator.Models;

namespace Andoromeda.Kyubey.Incubator.Controllers
{
    public abstract class BaseController : Controller
    {
        [NonAction]
        protected IActionResult ApiResult<T>(T ret, object request = null, int code = 200)
        {
            Response.StatusCode = code;
            return Json(new ApiResult<T>
            {
                code = code,
                data = ret,
                msg = "ok",
                request = request
            });
        }

        [NonAction]
        protected IActionResult ApiResult(int code, string msg, object request = null)
        {
            Response.StatusCode = code;
            return Json(new ApiResult
            {
                code = code,
                msg = msg,
                request = request
            });
        }
    }
}
