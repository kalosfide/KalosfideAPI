using KalosfideAPI.Data;
using KalosfideAPI.Partages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using KalosfideAPI.Erreurs;
using System.Linq.Expressions;
using KalosfideAPI.Sécurité;
using KalosfideAPI.Data.Constantes;

namespace KalosfideAPI.Utilisateurs
{

    public class UtilisateurService : BaseService<Utilisateur>, IUtilisateurService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UtilisateurService(
            ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            ) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            DValidation = _Validation;
        }

        #region Validation

        public async Task<ErreurDeModel> _Validation(Utilisateur donnée)
        {
            if (await EstDoublon(donnée))
            {
                return new ErreurDeModel
                {
                    Code = "Doublon_Nom",
                    Description = $"Le nom {donnée.ApplicationUser.UserName} est déjà utilisé."
                };
            }
            return null;
        }
        public async Task<bool> EstDoublon(Utilisateur donnée)
        {
            var doublon = await _context.Users.Where(s => s.UserName == donnée.ApplicationUser.UserName && s.Id != donnée.UserId).FirstOrDefaultAsync();
            return doublon != null;
        }

        public async Task<bool> NomUnique(string nom)
        {
            var doublon = await _context.Users.Where(s => s.UserName == nom).FirstOrDefaultAsync();
            return doublon == null;
        }

        #endregion // Validation

        public async Task<ApplicationUser> TrouveParNom(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApplicationUser> TrouveParEmail(string eMail)
        {
            return await _userManager.FindByEmailAsync(eMail);
        }

        public async Task<ApplicationUser> ApplicationUserVérifié(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                ApplicationUser userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(userToVerify);
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ApplicationUser>(null);
        }

        public async Task Connecte(ApplicationUser user, bool persistant)
        {
            await _signInManager.SignInAsync(user, persistant);
        }

        public async Task Déconnecte()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<CarteUtilisateur> CréeCarteUtilisateur(ApplicationUser user)
        {
            var utilisateur = await _context.Utilisateur.Where(u => u.UserId == user.Id)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Site)
                .FirstOrDefaultAsync();

            var sites = utilisateur.Roles.Select(role => role.Site).ToList();
            CarteUtilisateur utilisation = new CarteUtilisateur
            {
                UserId = user.Id,
                UserName = user.UserName,
                Uid = utilisateur.Uid,
                Etat = utilisateur.Etat,
                Roles = utilisateur.Roles.Select(role => new CarteRole
                {
                    Rno = role.Rno,
                    Etat = role.Etat,
                    NomSite = role.Site.NomSite
                }).ToList()
            };
            return utilisation;
        }

        public async Task<bool> PeutAjouterRole(Utilisateur utilisateur, Client client)
        {
            var existe = await _context.Role.Where(role => role.Uid == utilisateur.Uid)
                .Join(_context.Client, role => new { role.Uid, role.Rno }, client1 => new { client1.Uid, client1.Rno }, (role, client1) => client1)
                .Where(client1 => client1.Nom == client.Nom && client1.FournisseurId == client.FournisseurId).AnyAsync();
            return !existe;
        }

        public async Task<bool> PeutAjouterRole(Utilisateur utilisateur, Fournisseur fournisseur)
        {
            var existe = await _context.Role.Where(role => role.Uid == utilisateur.Uid)
                .Join(_context.Client, role => new { role.Uid, role.Rno }, fournisseur1 => new { fournisseur1.Uid, fournisseur1.Rno }, (role, fournisseur1) => fournisseur1)
                .Where(fournisseur1 => fournisseur1.Nom == fournisseur.Nom).AnyAsync();
            return !existe;
        }

        public async Task<RetourDeService<Utilisateur>> CréeUtilisateur(ApplicationUser applicationUser, string password)
        {
            try
            {
                var identityResult = await _userManager.CreateAsync(applicationUser, password);
                if (!identityResult.Succeeded)
                {
                    return new RetourDeService<Utilisateur>(TypeRetourDeService.IdentityError);
                }
                await _context.SaveChangesAsync();
                long Max;
                if(await _context.Utilisateur.AnyAsync())
                {
                    Max = await _context.Utilisateur.MaxAsync(u => long.Parse(u.Uid)) + 1;
                }
                else
                {
                    Max = 1;
                }
                Utilisateur utilisateur = new Utilisateur
                {
                    UserId = applicationUser.Id,
                    Uid = Max.ToString(),
                };
                _context.Utilisateur.Add(utilisateur);
                EtatUtilisateur changement = new EtatUtilisateur
                {
                    Uid = utilisateur.Uid,
                    Etat = TypeEtatUtilisateur.Nouveau,
                    Date = DateTime.Now
                };
                _context.EtatUtilisateur.Add(changement);
                await _context.SaveChangesAsync();
                return new RetourDeService<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new RetourDeService<Utilisateur>(TypeRetourDeService.ConcurrencyError);
            }
            catch (Exception)
            {
                return new RetourDeService<Utilisateur>(TypeRetourDeService.Indéterminé);
            }
        }

        public async Task<Utilisateur> Lit(string id)
        {
            Utilisateur utilisateur = await _context.Utilisateur.FindAsync(id);
            if (utilisateur != null)
            {
                ApplicationUser applicationUser = await _context.Users.FindAsync(utilisateur.UserId);
                utilisateur.ApplicationUser = applicationUser;
            }
            return utilisateur;
        }

        public async Task<Utilisateur> UtilisateurDeUser(string userId)
        {
            return await _context.Utilisateur.Where(utilisateur => utilisateur.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<Utilisateur>> Lit()
        {
            return await _context.Utilisateur
                .Include(u => u.ApplicationUser)
                .ToListAsync();
        }

        public async Task<RetourDeService<Utilisateur>> Supprime(Utilisateur utilisateur)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(utilisateur.UserId);
            var r = await _userManager.DeleteAsync(applicationUser);
            _context.Remove(applicationUser);
            return await SaveChangesAsync(utilisateur);
        }

        public async Task<RetourDeService<Utilisateur>> ChangeEtat(Utilisateur utilisateur, string état)
        {
            EtatUtilisateur changement = new EtatUtilisateur
            {
                Uid = utilisateur.Uid,
                Date = DateTime.Now,
                Etat = état
            };
            _context.EtatUtilisateur.Add(changement);
            try
            {
                await _context.SaveChangesAsync();
                return new RetourDeService<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new RetourDeService<Utilisateur>(TypeRetourDeService.ConcurrencyError);
            }
            catch (Exception)
            {
                return new RetourDeService<Utilisateur>(TypeRetourDeService.Indéterminé);
            }
        }

    }

}
