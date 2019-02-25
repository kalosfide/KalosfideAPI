using System;
using System.Threading.Tasks;
using KalosfideAPI.Administrateurs;
using KalosfideAPI.Clients;
using KalosfideAPI.Commandes;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Fournisseurs;
using KalosfideAPI.Livraisons;
using KalosfideAPI.Partages;
using KalosfideAPI.Roles;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Sites;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KalosfideAPI.Enregistrement
{
    class RésultatEnregistrement
    {
        public bool ACréé;
        public ApplicationUser user;
        public Utilisateur Utilisateur;
        public Role Role;
        public AKeyUidRno Entité;
        public Site Site;
        public IActionResult ActionResult;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EnregistrementController : UtilisateurController
    {
        private readonly IRoleService _roleService;
        private readonly IAdministrateurService _administrateurService;
        private readonly IFournisseurService _fournisseurService;
        private readonly ISiteService _siteService;
        private readonly IClientService _clientService;
        private readonly ICommandeService _commandeService;
        private readonly ILivraisonService _livraisonService;

        public EnregistrementController(
            IJwtFabrique jwtFabrique,
            IUtilisateurService service, 
            IRoleService roleService,
            IAdministrateurService administrateurService,
            IFournisseurService fournisseurService,
            ISiteService siteService,
            IClientService clientService,
            ICommandeService commandeService,
            ILivraisonService livraisonService
            ) : base(jwtFabrique, service)
        {
            _roleService = roleService;
            _administrateurService = administrateurService;
            _fournisseurService = fournisseurService;
            _siteService = siteService;
            _clientService = clientService;
            _commandeService = commandeService;
            _livraisonService = livraisonService;
        }

        private async Task<RésultatEnregistrement> CréeUtilisateur(string type, VueBase vue)
        {
            RésultatEnregistrement résultat = new RésultatEnregistrement();
            ApplicationUser existant = await _service.TrouveParEmail(vue.Email);
            if (existant == null)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = vue.Email,
                    Email = vue.Email,
                };

                RetourDeService<Utilisateur> retour = await _service.CréeUtilisateur(applicationUser, vue.Password);
                if (retour.Ok)
                {
                    résultat.ACréé = true;
                    résultat.user = applicationUser;
                    résultat.Utilisateur = retour.Entité;
                }
                else
                {
                    résultat.ActionResult = SaveChangesActionResult(retour);
                }
            }
            else
            {
                Utilisateur utilisateur = await _service.UtilisateurDeUser(existant.Id);

                CarteUtilisateur carte = await _service.CréeCarteUtilisateur(HttpContext.User);

                bool permis = carte != null && ((carte.EstUtilisateurActif && carte.Uid == utilisateur.Uid) || carte.EstAdministrateur);
                if (permis)
                {
                    résultat.ACréé = false;
                    résultat.user = existant;
                    résultat.Utilisateur = utilisateur;
                    permis = await PeutAjouterRole(utilisateur, type, vue);
                    if (!permis)
                    {
                        résultat.ActionResult = BadRequest();
                    }
                }
                else
                {
                    résultat.ActionResult = Forbid();
                }
            }
            return résultat;
        }

        private async Task<bool> PeutAjouterRole(Utilisateur utilisateur, string type, VueBase vue)
        {
            switch (type)
            {
                case TypeDeRole.Administrateur.Code:
                    return true;
                case TypeDeRole.Fournisseur.Code:
                    return await _service.PeutAjouterRole(utilisateur, vue as EnregistrementFournisseurVue);
                case TypeDeRole.Client.Code:
                    return await _service.PeutAjouterRole(utilisateur, vue as EnregistrementClientVue);
                default:
                    break;
            }
            return false;
        }

        private void CréeEntité(RésultatEnregistrement résultat, string type, VueBase vue)
        {
            string uid = résultat.Role.Uid;
            int rno = résultat.Role.Rno;
            switch (type)
            {
                case TypeDeRole.Administrateur.Code:
                    résultat.Entité = (vue as AdministrateurVue).CréeAdministrateur();
                    résultat.Entité.Uid = uid;
                    résultat.Entité.Rno = rno;
                    break;
                case TypeDeRole.Fournisseur.Code:
                    résultat.Entité = _fournisseurService.CréeFournisseur(résultat.Role, vue as EnregistrementFournisseurVue);
                    résultat.Site = _siteService.CréeSite(résultat.Role, vue as EnregistrementFournisseurVue);
                    break;
                case TypeDeRole.Client.Code:
                    résultat.Entité = _clientService.CréeClient(résultat.Role, vue as EnregistrementClientVue);
                    break;
                default:
                    break;
            }
        }

        private async Task ValideEntité(RésultatEnregistrement résultat, string type)
        {
            switch (type)
            {
                case TypeDeRole.Administrateur.Code:
                    break;
                case TypeDeRole.Fournisseur.Code:
                    await _siteService.DValideAjoute()(résultat.Site, ModelState);
                    break;
                case TypeDeRole.Client.Code:
                    break;
                default:
                    break;
            }
        }

        private void AjouteEntitéSansSauver(RésultatEnregistrement résultat, string type)
        {
            switch (type)
            {
                case TypeDeRole.Administrateur.Code:
                    _administrateurService.AjouteSansSauver(résultat.Entité as Administrateur);
                    break;
                case TypeDeRole.Fournisseur.Code:
                    _fournisseurService.AjouteSansSauver(résultat.Entité as Fournisseur);
                    _siteService.AjouteSansSauver(résultat.Site);
                    _livraisonService.CréePremièreLivraison(résultat.Entité as Fournisseur);
                    break;
                case TypeDeRole.Client.Code:
                    _clientService.AjouteSansSauver(résultat.Entité as Client);
                    KeyUidRno keySite = new KeyUidRno
                    {
                        Uid = résultat.Role.SiteUid,
                        Rno = résultat.Role.SiteRno
                    };
                    _commandeService.CréePremièreCommande(résultat.Entité as Client, keySite);
                    break;
                default:
                    break;
            }
        }

        private async Task<IActionResult> Enregistre(string type, VueBase vue)
        {
            RésultatEnregistrement résultat = null;
            try
            {
                résultat = await CréeUtilisateur(type, vue);

                if (résultat.ActionResult != null)
                {
                    return résultat.ActionResult;
                }

                résultat.Role = await _roleService.CréeRole(résultat.Utilisateur);

                CréeEntité(résultat, type, vue);

                await ValideEntité(résultat, type);
                if (!ModelState.IsValid)
                {
                    if (résultat.ACréé)
                    {
                        await _service.Supprime(résultat.Utilisateur);
                    }
                    return BadRequest(ModelState);
                }

                _roleService.AjouteSansSauver(résultat.Role);
                AjouteEntitéSansSauver(résultat, type);

                RetourDeService retour = await _service.SaveChangesAsync();
                if (retour.Type != TypeRetourDeService.Ok && résultat.ACréé)
                {
                    await _service.Supprime(résultat.Utilisateur);
                    return SaveChangesActionResult(retour);
                }
            }
            catch (Exception ex)
            {
                if (résultat != null && résultat.ACréé)
                {
                    await _service.Supprime(résultat.Utilisateur);
                }
                throw (ex);
            }

            return await Connecte(résultat.user, true);
        }

        [HttpPost("/api/enregistrement/administrateur")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Administrateur(AdministrateurVue vue)
        {
            return await Enregistre(TypeDeRole.Administrateur.Code, vue);
        }

        [HttpPost("/api/enregistrement/fournisseur")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Fournisseur(EnregistrementFournisseurVue vue)
        {
            return await Enregistre(TypeDeRole.Fournisseur.Code, vue);
        }

        [HttpPost("/api/enregistrement/client")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Client(EnregistrementClientVue vue)
        {
            return await Enregistre(TypeDeRole.Client.Code, vue);
        }

        [HttpGet("/api/enregistrement/userNamePris/{nom}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> UserNamePris(string userName)
        {
            return Ok(await _service.UserNamePris(userName));
        }

        [HttpGet("/api/enregistrement/emailPris/{nom}")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(404)] // Not found
        [AllowAnonymous]
        public async Task<IActionResult> EmailPris(string userName)
        {
            return Ok(await _service.EmailPris(userName));
        }

    }
}