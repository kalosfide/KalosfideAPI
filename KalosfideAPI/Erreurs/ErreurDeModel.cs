using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KalosfideAPI.Erreurs
{
    public class ErreurDeModel
    {
        public string Code;
        public string Description;

        public ModelStateDictionary AjouteAModelState(ModelStateDictionary modelState)
        {
            modelState.TryAddModelError(Code, Description);
            return modelState;
        }
    }
}
