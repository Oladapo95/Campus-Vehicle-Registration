using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VechicleRegistration.Context;

namespace VechicleRegistration.Models.Repositories
{
    public class Repository<T> where T : class
    {
        public VRContext context = new VRContext();

        protected DbSet<T> DbSet
        {
            get; set;
        }

        public Repository()
        {
            DbSet = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return DbSet.ToList();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public T Get(int id)
        {
            return DbSet.Find(id);
        }

    }
}