using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Fournisseurs
{
    public class FournisseurService : KeyRIdService<Fournisseur>, IFournisseurService
    {
        public FournisseurService(ApplicationContext context, DbSet<Fournisseur> dbSet) : base(context, dbSet)
        {
        }
    }
}
