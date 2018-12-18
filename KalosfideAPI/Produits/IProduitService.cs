using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages.KeyParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Produits
{
    public interface IProduitService: IKeyUidRnoNoService<Produit, ProduitVue>
    {
        Task<bool> NomPris(string nom);
        Task<bool> NomPrisParAutre(AKeyUidRnoNo key, string nom);
    }
}
