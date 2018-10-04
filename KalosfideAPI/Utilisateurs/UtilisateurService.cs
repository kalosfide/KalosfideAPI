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

        #region UtilisateurAvec

        public async Task<Utilisateur> UtilisateurAvecListeRoles(ApplicationUser applicationUser)
        {
            return await _context.Utilisateur
                .Where(u => u.UserId == applicationUser.Id)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync();
        }

        public async Task<Utilisateur> UtilisateurAvecRoleSelectionné(ApplicationUser applicationUser)
        {
            return await _context.Utilisateur
                .Where(u => u.UserId == applicationUser.Id)
                .Include(u => u.RoleSélectionné)
                .FirstOrDefaultAsync();
        }

        public async Task<Utilisateur> UtilisateurAvecRoleSelectionné(string userName)
        {
            ApplicationUser applicationUser = await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            if (applicationUser == null)
            {
                return null;
            }
            return await UtilisateurAvecRoleSelectionné(applicationUser);
        }

        #endregion // UtilisateurAvec

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

        public async Task<RetourDeService<Utilisateur>> Enregistre(ApplicationUser applicationUser, string password)
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
                    Max = await _context.Utilisateur.MaxAsync(u => long.Parse(u.UtilisateurId)) + 1;
                }
                else
                {
                    Max = 1;
                }
                Utilisateur utilisateur = new Utilisateur
                {
                    UserId = applicationUser.Id,
                    UtilisateurId = Max.ToString(),
                    Etat = EtatUtilisateur.Nouveau
                };
                _context.Utilisateur.Add(utilisateur);
                ChangementEtatUtilisateur changement = new ChangementEtatUtilisateur
                {
                    UtilisateurId = utilisateur.UtilisateurId,
                    Etat = EtatUtilisateur.Nouveau,
                    Date = DateTime.Now
                };
                _context.JournalEtatUtilisateur.Add(changement);
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
                    Max = await _context.Utilisateur.MaxAsync(u => long.Parse(u.UtilisateurId)) + 1;
                }
                else
                {
                    Max = 1;
                }
                Utilisateur utilisateur = new Utilisateur
                {
                    UserId = applicationUser.Id,
                    UtilisateurId = Max.ToString(),
                    Etat = EtatUtilisateur.Nouveau
                };
                _context.Utilisateur.Add(utilisateur);
                ChangementEtatUtilisateur changement = new ChangementEtatUtilisateur
                {
                    UtilisateurId = utilisateur.UtilisateurId,
                    Etat = EtatUtilisateur.Nouveau,
                    Date = DateTime.Now
                };
                _context.JournalEtatUtilisateur.Add(changement);
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

        public async Task<RetourDeService<Utilisateur>> Edite(Utilisateur utilisateur)
        {
            _context.Update(utilisateur);
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

        public void ChangeRoleSansSauver(Utilisateur utilisateur, Role role)
        {
            utilisateur.RoleSélectionnéNo = role.RoleNo;
            utilisateur.RoleSélectionné = role;
        }

        public async Task<RetourDeService<Utilisateur>> ChangeRole(Utilisateur utilisateur, Role role)
        {
            ChangeRoleSansSauver(utilisateur, role);
            return await SaveChangesAsync(utilisateur);
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
            ChangementEtatUtilisateur changement = new ChangementEtatUtilisateur
            {
                UtilisateurId = utilisateur.UtilisateurId,
                Date = DateTime.Now,
                Etat = état
            };
            _context.JournalEtatUtilisateur.Add(changement);
            utilisateur.Etat = état;
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

        public async Task<bool> PeutAjouterRole(Utilisateur utilisateur, Client client)
        {
            var existe = await _context.Role.Where(role => role.UtilisateurId == utilisateur.UtilisateurId)
                .Join(_context.Client, role => role.ClientId, client1 => client1.RoleId, (role, client1) => client1)
                .Where(client1 => client1.Nom == client.Nom && client1.FournisseurId == client.FournisseurId).AnyAsync();
            return !existe;
        }

        public async Task<bool> PeutAjouterRole(Utilisateur utilisateur, Fournisseur fournisseur)
        {
            var existe = await _context.Role.Where(role => role.UtilisateurId == utilisateur.UtilisateurId)
                .Join(_context.Client, role => role.ClientId, fournisseur1 => fournisseur1.RoleId, (role, fournisseur1) => fournisseur1)
                .Where(fournisseur1 => fournisseur1.Nom == fournisseur.Nom).AnyAsync();
            return !existe;
        }

    }

}
