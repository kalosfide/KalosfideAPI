using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Administrateurs;
using KalosfideAPI.Clients;
using KalosfideAPI.Data;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Erreurs;
using KalosfideAPI.Fournisseurs;
using KalosfideAPI.Partages;
using KalosfideAPI.Roles;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Sites;
using KalosfideAPI.Utilisateurs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

        public EnregistrementController(
            IJwtFabrique jwtFabrique,
            IUtilisateurService service, 
            IUtilisateurTransformation transformation,
            IRoleService roleService,
            IAdministrateurService administrateurService,
            IFournisseurService fournisseurService,
            ISiteService siteService,
            IClientService clientService
            ) : base(jwtFabrique, service, transformation)
        {
            _roleService = roleService;
            _administrateurService = administrateurService;
            _fournisseurService = fournisseurService;
            _siteService = siteService;
            _clientService = clientService;
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

                CarteUtilisateur carte = new CarteUtilisateur();
                carte.PrendClaims(HttpContext.User);

                bool permis = (carte.EstUtilisateurActif && carte.Uid == utilisateur.Uid) || carte.EstAdministrateur;
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
                    résultat.ActionResult = StatusCode(403);
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
                    return await _service.PeutAjouterRole(utilisateur, (vue as FournisseurVue).CréeFournisseur());
                case TypeDeRole.Client.Code:
                    return await _service.PeutAjouterRole(utilisateur, (vue as ClientVue).CréeClient());
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
                    break;
                case TypeDeRole.Fournisseur.Code:
                    résultat.Entité = (vue as FournisseurVue).CréeFournisseur();
                    résultat.Site = (vue as FournisseurVue).CréeSite();
                    résultat.Site.Uid = uid;
                    résultat.Site.Rno = rno;
                    résultat.Role.SiteUid = uid;
                    résultat.Role.SiteRno = rno;
                    break;
                case TypeDeRole.Client.Code:
                    résultat.Entité = (vue as ClientVue).CréeClient();
                    résultat.Role.SiteUid = (vue as ClientVue).SiteUid;
                    résultat.Role.SiteRno = (vue as ClientVue).SiteRno;
                    break;
                default:
                    break;
            }
            résultat.Entité.Uid = uid;
            résultat.Entité.Rno = rno;
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
                    break;
                case TypeDeRole.Client.Code:
                    _clientService.AjouteSansSauver(résultat.Entité as Client);
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

                Utilisateur utilisateur = résultat.Utilisateur;

                int roleNo = await _roleService.DernierNo(utilisateur.Uid) + 1;
                Role role = new Role
                {
                    Uid = utilisateur.Uid,
                    Rno = roleNo,
                };
                résultat.Role = role;

                CréeEntité(résultat, type, vue);

                _roleService.AjouteSansSauver(role);
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
        public async Task<IActionResult> Fournisseur(FournisseurVue vue)
        {
            return await Enregistre(TypeDeRole.Fournisseur.Code, vue);
        }

        [HttpPost("/api/enregistrement/client")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Client(ClientVue vue)
        {
            return await Enregistre(TypeDeRole.Client.Code, vue);
        }

    }
}