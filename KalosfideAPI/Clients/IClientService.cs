using KalosfideAPI.Data;
using KalosfideAPI.Enregistrement;
using KalosfideAPI.Partages.KeyParams;

namespace KalosfideAPI.Clients
{
    public interface IClientService : IKeyUidRnoService<Client, ClientVue>
    {
        Client CréeClient(Role role, EnregistrementClientVue clientVue);
    }
}
