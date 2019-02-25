using KalosfideAPI.Data.Keys;
using KalosfideAPI.Utilisateurs;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages.KeyParams
{
    public class KeyUidRnoNo2Controller<T, TVue> : KeyParamController<T, TVue, KeyParam> where T : AKeyUidRnoNo2 where TVue : AKeyUidRnoNo2
    {

        public KeyUidRnoNo2Controller(IKeyUidRnoNo2Service<T, TVue> service, IUtilisateurService utilisateurService) : base(service, utilisateurService)
        {
        }

        private IKeyUidRnoNo2Service<T, TVue> _service { get => __service as IKeyUidRnoNo2Service<T, TVue>; }

        protected override Task FixeKeyParamAjout(TVue vue)
        {
            return Task.CompletedTask;
        }

    }
}
