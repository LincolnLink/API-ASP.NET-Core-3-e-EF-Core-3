﻿using Domain.Entities;
using Domain.Interface.RepositoryBase;
using Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Service.Services
{
    public class QuestionsService : IQuestionsService
    {

        private IRepository<Questions> _repository;

        public QuestionsService(IRepository<Questions> repository)
        {
            this._repository = repository;
        }


        public async Task<bool> Delete(Guid id)
        {
          return await this._repository.DeleteAsync(id);
        }

        public async Task<Questions> Get(Guid id)
        {
            return await this._repository.SelectAsync(id);
        }

        public async Task<List<Questions>> GetAll()
        {
            return (List<Questions>)await this._repository.SelectAcync();
        }

        public async Task<Questions> Post(Questions q)
        {
            List<Questions> indexValue = await GetAll();

            int? indexValueInt = indexValue.Where(i => i.ChapterId == q.ChapterId).Count();

            if(indexValueInt == null)
            {
                q.Index = 0;
            }
            else { q.Index = (int)indexValueInt; }

            return await this._repository.InsertAsync(q);
        }

        public async Task<Questions> Put(Questions q)
        {
            return await this._repository.UpdateAsync(q);
        }

        public async Task<List<Questions>> PutAll(Questions[] c)
        {
            return await this._repository.UpdateAllAsync(c.ToList());
        }
    }
}
