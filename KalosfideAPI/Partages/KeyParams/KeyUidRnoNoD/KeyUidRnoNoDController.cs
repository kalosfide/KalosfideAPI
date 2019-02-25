using KalosfideAPI.Data.Keys;
using KalosfideAPI.Utilisateurs;
using System;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public class KeyUidRnoNoDController<T, TVue> : KeyParamController<T, TVue, KeyParam> where T : AKeyUidRnoNoD where TVue : AKeyUidRnoNoD
    {

        public KeyUidRnoNoDController(IKeyUidRnoNoDService<T, TVue> service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
        }

        private IKeyUidRnoNoDService<T, TVue> _service { get => __service as IKeyUidRnoNoDService<T, TVue>; }

        protected override Task FixeKeyParamAjout(TVue vue)
        {
            vue.Date = DateTime.Now;
            return Task.CompletedTask;
        }

    }
}
