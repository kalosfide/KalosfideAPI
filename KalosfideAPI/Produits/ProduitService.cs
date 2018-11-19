using KalosfideAPI.Data;
using KalosfideAPI.Partages.KeyParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Produits
{
    public class ProduitService : KeyUidRnoNoService<Produit>, IProduitService
    {
        public ProduitService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Produit;
        }
    }
}
