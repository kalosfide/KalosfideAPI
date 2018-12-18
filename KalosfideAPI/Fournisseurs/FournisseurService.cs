using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Fournisseurs
{
    public class FournisseurService : KeyUidRnoService<Fournisseur, FournisseurVue>, IFournisseurService
    {
        public FournisseurService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Fournisseur;
        }

        public override Task CopieVueDansDonnées(Fournisseur donnée, FournisseurVue vue)
        {
            donnée.Nom = vue.Nom;
            donnée.Adresse = vue.Adresse;
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
            FixeVueKey(donnée, vue);
            return vue;
        }
    }
}
