using KalosfideAPI.Data;
using KalosfideAPI.Data.Keys;
using KalosfideAPI.Partages;
using KalosfideAPI.Partages.KeyParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Livraisons
{
    public interface ILivraisonService : IKeyUidRnoNoService<Livraison, LivraisonVue>
    {
        void CréePremièreLivraison(AKeyUidRno keyFournisseur);
        Task<LivraisonVue> LivraisonEnCours(AKeyUidRno keyFournisseur);

    }
}
