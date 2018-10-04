using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyString;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Produits
{
    public class ProduitService : KeyRIdNoService<Produit>, IProduitService
    {
        public ProduitService(ApplicationContext context, DbSet<Produit> dbSet) : base(context, dbSet)
        {
        }
    }
}
