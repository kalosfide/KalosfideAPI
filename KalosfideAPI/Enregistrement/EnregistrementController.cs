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
        public IActionResult ActionResult;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class EnregistrementController : UtilisateurController
    {
        private readonly IRoleService _roleService;
        private readonly IAdministrateurService _administrateurService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IClientService _clientService;

        public EnregistrementController(
            UserManager<ApplicationUser> userManager, 
            IJwtFabrique jwtFabrique,
            IUtilisateurService service, 
            IUtilisateurTransformation transformation,
            IRoleService roleService,
            IAdministrateurService administrateurService,
            IFournisseurService fournisseurService,
            IClientService clientService
            ) : base(userManager, jwtFabrique, service, transformation)
        {
            _roleService = roleService;
            _administrateurService = administrateurService;
            _fournisseurService = fournisseurService;
            _clientService = clientService;
        }

        private async Task<RésultatEnregistrement> CréeUtilisateur(string type, VueBase vue)
        {
            RésultatEnregistrement résultat = new RésultatEnregistrement();
            ApplicationUser existant = await _userManager.FindByEmailAsync(vue.Email);
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

                var revendications = RevendicationsFabrique.Revendications(HttpContext.User);
                bool permis = (revendications.EstUtilisateurActif && revendications.UtilisateurId == utilisateur.UtilisateurId) || revendications.EstAdministrateur;
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
                    return await _service.PeutAjouterRole(utilisateur, (vue as FournisseurVue).CréeFournisseur());
                case TypeDeRole.Client.Code:
                    return await _service.PeutAjouterRole(utilisateur, (vue as ClientVue).CréeClient());
                default:
                    break;
            }
            return false;
        }

        private void CréeEntitéSansSauver(RésultatEnregistrement résultat, string type, VueBase vue)
        {
            AKeyRId entité = null;
            string roleId = résultat.Role.RoleId;
            switch (type)
            {
                case TypeDeRole.Administrateur.Code:
                    entité = (vue as AdministrateurVue).CréeAdministrateur();
                    _administrateurService.AjouteSansSauver(entité as Administrateur);
                    résultat.Role.AdministrateurId = roleId;
                    break;
                case TypeDeRole.Fournisseur.Code:
                    entité = (vue as FournisseurVue).CréeFournisseur();
                    _fournisseurService.AjouteSansSauver(entité as Fournisseur);
                    résultat.Role.FournisseurId = roleId;
                    break;
                case TypeDeRole.Client.Code:
                    entité = (vue as ClientVue).CréeClient();
                    _clientService.AjouteSansSauver(entité as Client);
                    résultat.Role.ClientId = roleId;
                    break;
                default:
                    break;
            }
            if (entité != null)
            {
                entité.RoleId = roleId;
            }
        }

        private async Task<IActionResult> Enregistre(string type, VueBase vue)
        {
            RésultatEnregistrement résultat = await CréeUtilisateur(type, vue);

            if (résultat.ActionResult != null)
            {
                return résultat.ActionResult;
            }

            Utilisateur utilisateur = résultat.Utilisateur;

            Role role = new Role
            {
                UtilisateurId = utilisateur.UtilisateurId,
                RoleNo = await _roleService.DernierNo(new KeyUId { UtilisateurId = utilisateur.UtilisateurId }),
            };
            _roleService.AjouteSansSauver(role);
            résultat.Role = role;

            CréeEntitéSansSauver(résultat, type, vue);

            _service.ChangeRoleSansSauver(utilisateur, role);

            RetourDeService<Utilisateur> retour = await _service.SaveChangesAsync(utilisateur);

            if (!retour.Ok && résultat.ACréé)
            {
                await _service.Supprime(résultat.Utilisateur);
                return SaveChangesActionResult(retour);
            }

            JwtRéponse jwtRéponse = await _jwtFabrique.CréeReponse(résultat.user, utilisateur);
            return new OkObjectResult(jwtRéponse);
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
            return await Enregistre(TypeDeRole.Administrateur.Code, vue);
        }

        [HttpPost("/api/enregistrement/client")]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)] // Bad request
        [AllowAnonymous]
        public async Task<IActionResult> Client(ClientVue vue)
        {
            return await Enregistre(TypeDeRole.Administrateur.Code, vue);
        }

    }
}