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
    public class FournisseurService : KeyUidRnoService<Fournisseur>, IFournisseurService
    {
        public FournisseurService(ApplicationContext context) : base(context)
        {
            _dbSet = _context.Fournisseur;
        }
    }
}
