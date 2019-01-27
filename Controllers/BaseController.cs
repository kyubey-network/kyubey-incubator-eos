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
        [Obsolete]
        [NonAction]
        protected IActionResult ApiResultOld<T>(T ret, int code = 200)
        {
            Response.StatusCode = code;
            return Json(new ApiResult<T>
            {
                code = code,
                data = ret,
                msg = "ok"
            });
        }

        [Obsolete]
        [NonAction]
        protected IActionResult ApiResultOld(int code, string msg)
        {
            Response.StatusCode = code;
            return Json(new ApiResult
            {
                code = code,
                msg = msg
            });
        }


        [NonAction]
        protected IActionResult ApiResult<T>(T ret, int code = 200)
        {
            Response.StatusCode = code;
            return Json(ret);
        }

        [NonAction]
        protected IActionResult ApiResult(int code, string msg)
        {
            Response.StatusCode = code;
            return Json(null);
        }
    }
}
