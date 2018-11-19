using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Controllers
{
    [Route("api/[controller]/[action]/{utilisateurId?}/{roleNo?}")]
    public class TestController : Controller
    {

        [HttpGet]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public bool Lit(string utilisateurId, int roleNo)
        {
            return true;
        }

    }
}
