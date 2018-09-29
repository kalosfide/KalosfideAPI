using KalosfideAPI.Erreurs;
using KalosfideAPI.Partages.KeyLong;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KalosfideAPI.SiteInfos
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiValidationFilter]
    [AllowAnonymous]
    public class SiteInfoController : KeyLongController<SiteInfo, SiteInfoVue>
    {

        public SiteInfoController(
            IAuthorizationService autorisation,
            ISiteInfoService service,
            ISiteInfoTransformation transformation
        ) : base(autorisation, service, transformation)
        {
        }

        ISiteInfoService _service { get { return base.__service as SiteInfoService; } }

        // POST api/siteInfo
        [HttpPost]
        [ProducesResponseType(201)] // created
        [ProducesResponseType(400)] // Bad request
        public new async Task<IActionResult> Ajoute(SiteInfoVue vue)
        {
            return await base.Ajoute(vue);
        }

        // GET api/siteInfo
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Lit(string id)
        {
            return await base.Lit(id);
        }

        // GET api/siteInfo
        [HttpGet]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        public new async Task<IActionResult> Lit()
        {
            return await base.Lit();
        }

        // PUT api/siteInfo
        [HttpPut]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(400)] // Bad request
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(500)] // 500 Internal Server Error
        public new async Task<IActionResult> Edite(SiteInfoVue vue)
        {
            return await base.Edite(vue);
        }
        
        // DELETE api/donnée/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(404)] // Not found
        [ProducesResponseType(500)] // 500 Internal Server Error
        public new async Task<IActionResult> Supprime(string param)
        {
            return await base.Supprime(param);
        }
    }
}