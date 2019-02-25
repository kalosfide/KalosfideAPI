using KalosfideAPI.Data;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KalosfideAPI.Fournisseurs
{
    class GèreEtat : Partages.KeyParams.GéreEtat<Fournisseur, FournisseurVue, EtatFournisseur>
    {
        public GèreEtat(DbSet<EtatFournisseur> dbSetEtat) : base(dbSetEtat)
        { }
        protected override EtatFournisseur CréeEtatAjout(Fournisseur donnée)
        {
            EtatFournisseur etat = new EtatFournisseur
            {
                Nom = donnée.Nom,
                Adresse = donnée.Adresse,
                Date = DateTime.Now
            };
            return etat;
        }
        protected override EtatFournisseur CréeEtatEdite(Fournisseur donnée, FournisseurVue vue)
        {
            bool modifié = false;
            EtatFournisseur état = new EtatFournisseur
            {
                Date = DateTime.Now
            };
            if (vue.Nom != null && donnée.Nom != vue.Nom)
            {
                donnée.Nom = vue.Nom;
                état.Nom = vue.Nom;
                modifié = true;
            }
            if (vue.Adresse != null && donnée.Adresse != vue.Adresse)
            {
                donnée.Adresse = vue.Adresse;
                état.Adresse = vue.Adresse;
                modifié = true;
            }
            return modifié ? état : null;
        }
    }

    public class FournisseurService : KeyUidRnoService<Fournisseur, FournisseurVue, EtatFournisseur>, IFournisseurService
    {
        public FournisseurService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Fournisseur;
            _géreEtat = new GèreEtat(_context.EtatFournisseur);
        }

        public Fournisseur CréeFournisseur(Role role, EnregistrementFournisseurVue fournisseurVue)
        {
            Fournisseur fournisseur = new Fournisseur
            {
                Uid = role.Uid,
                Rno = role.Rno,
                Nom = fournisseurVue.Nom,
                Adresse = fournisseurVue.Adresse
            };
            return fournisseur;
        }

        public override Task CopieVueDansDonnées(Fournisseur donnée, FournisseurVue vue)
        {
            // inutilisé
            return Task.CompletedTask;
        }

        public override Fournisseur NouvelleDonnée()
        {
            return new Fournisseur();
        }

        public override FournisseurVue CréeVue(Fournisseur donnée)
        {
            FournisseurVue vue = new FournisseurVue
            {
                Nom = donnée.Nom,
                Adresse = donnée.Adresse,
            };
            vue.CopieKey(donnée.KeyParam);
            return vue;
        }
    }
}
