using System.Collections.Generic;

namespace KalosfideAPI.Partages
{
    public interface ITransformation<T, TVue> where T: class where TVue: class
    {
        TVue CréeVue(T donnée);
        IEnumerable<TVue> CréeVues(IEnumerable<T> données);
        T CréeDonnée(TVue vue);
        void CopieVueDansDonnées(T donnée, TVue vue);
    }
}
