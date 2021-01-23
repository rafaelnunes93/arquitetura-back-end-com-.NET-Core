using curso.api.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Business.Repositories
{
    public interface ICursoRepository
    {
        void adicionar(Curso curso);

        void Commit();

        IList<Curso> ObterPorUsuario(int codigoUsuario);
    }
}
