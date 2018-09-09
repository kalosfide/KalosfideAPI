using KalosfideAPI.Erreurs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Controllers
{
    public class ErreurController : Controller
    {
        [Route("api/erreur/{code}")]
        public IActionResult Error(int code)
        {
            HttpContext.Response.StatusCode = code;
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
