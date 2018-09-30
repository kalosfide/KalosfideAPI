using KalosfideAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Produits
{
    public class ProduitService : Partages.KeyString.KeyUIdRNoNo.KeyUIdRNoNoService<Produit>, IProduitService
    {
        public ProduitService(ApplicationContext context, DbSet<Produit> dbSet) : base(context, dbSet)
        {
        }
    }
}
