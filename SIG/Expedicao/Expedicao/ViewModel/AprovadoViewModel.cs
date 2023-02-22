using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Expedicao
{
    internal class AprovadoViewModel
    {
        public async Task<IList<AprovadoModel>> GetAprovados()
        {
            IList<AprovadoModel> listAsync;
            try
            {
                using AppDatabase db = new();
                listAsync = (IList<AprovadoModel>)await db.Aprovados.OrderBy<AprovadoModel, string>(c => c.SiglaServ).ToListAsync<AprovadoModel>();
            }
            catch (Exception)
            {
                throw;
            }
            return listAsync;
        }
        public async Task<AprovadoModel> GetAprovadoAsync(string SiglaServ)
        {
            AprovadoModel aprovadoAsync;
            try
            {
                using AppDatabase db = new();
                aprovadoAsync = await db.Aprovados.FirstOrDefaultAsync<AprovadoModel>(ap => ap.SiglaServ == SiglaServ);
            }
            catch (Exception)
            {
                throw;
            }
          return aprovadoAsync;
        }
    }
}
