using KalosfideAPI.Data.Keys;
using KalosfideAPI.Utilisateurs;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public class KeyUidRnoNoController<T, TVue> : KeyParamController<T, TVue, KeyParam> where T : AKeyUidRnoNo where TVue : AKeyUidRnoNo
    {

        public KeyUidRnoNoController(IKeyUidRnoNoService<T,TVue> service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
        }

        private IKeyUidRnoNoService<T,TVue> _service { get => __service as IKeyUidRnoNoService<T, TVue>; }

        protected async override Task FixeKeyParamAjout(TVue vue)
        {
            vue.No = await _service.DernierNo(vue.KeyParam) + 1;
        }

    }
}
