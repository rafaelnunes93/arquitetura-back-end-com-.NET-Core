using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Configurations
{
    public interface IAutheiticationService
    {
        string GerarToken(Models.Usuarios.UsuarioViewModelOutput usuarioViewModelOutput);
    }
}
