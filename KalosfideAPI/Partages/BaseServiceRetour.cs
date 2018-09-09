using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Partages
{
    public enum BaseServiceRetourType
    {
        Ok,
        IdentityError,
        ConcurrencyError,
        NotFound,
        Indéterminé
    }

    public class BaseServiceRetour
    {
        public BaseServiceRetourType Type { get; set; }
        public Object Objet { get; set; }

        public bool Ok { get { return this.Type == BaseServiceRetourType.Ok; } }
        public bool ConcurrencyError { get { return this.Type == BaseServiceRetourType.ConcurrencyError; } }
        public bool IdentityError { get { return this.Type == BaseServiceRetourType.IdentityError; } }

        public BaseServiceRetour(BaseServiceRetourType type)
        {
            Type = type;
        }

        public BaseServiceRetour(Object Objet)
        {
            Type = BaseServiceRetourType.Ok;
            this.Objet = Objet;
        }

    }

    public class BaseServiceRetour<T> : BaseServiceRetour where T: class
    {
        public T Entité
        {
            get => Objet as T;
            set => base.Objet = value;
        }
        public BaseServiceRetour(BaseServiceRetourType type) : base(type) { }

        public BaseServiceRetour(T Entité) : base(Entité) { }
    }
}
