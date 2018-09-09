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

        public async Task<BaseServiceRetour<Utilisateur>> Enregistre(ApplicationUser applicationUser, string password)
        {
            try
            {
                var identityResult = await _userManager.CreateAsync(applicationUser, password);
                if (!identityResult.Succeeded)
                {
                    return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.IdentityError);
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
                return new BaseServiceRetour<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.Indéterminé);
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


        public async Task<List<Utilisateur>> Lit()
        {
            return await _context.Utilisateur
                .Include(u => u.ApplicationUser)
                .ToListAsync();
        }

        public async Task<BaseServiceRetour<Utilisateur>> Update(Utilisateur utilisateur)
        {
            _context.Update(utilisateur);
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<BaseServiceRetour<Utilisateur>> ChangeRoleActif(Utilisateur utilisateur, Role role)
        {
            utilisateur.RoleSélectionnéNo = role.No;
            try
            {
                await _context.SaveChangesAsync();
                utilisateur.RoleSélectionné = role;
//                await Sécurité.Revendications.FixeUtilisateurClaims(utilisateur.ApplicationUser, _userManager);

                return new BaseServiceRetour<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<BaseServiceRetour<Utilisateur>> Delete(Utilisateur utilisateur)
        {
            _context.Remove(utilisateur);
            try
            {
                await _context.SaveChangesAsync();
                return new BaseServiceRetour<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.Indéterminé);
            }
        }

        public async Task<BaseServiceRetour<Utilisateur>> ChangeEtat(Utilisateur utilisateur, string état)
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
                return new BaseServiceRetour<Utilisateur>(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.ConcurrencyError);
            }
            catch (Exception)
            {
                return new BaseServiceRetour<Utilisateur>(BaseServiceRetourType.Indéterminé);
            }
        }

    }

}
