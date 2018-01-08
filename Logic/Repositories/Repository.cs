using Group11.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Logic
{
    public abstract class Repository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public DbSet<T> Items => context.Set<T>();

        public T Get(T id)
        {
            return Items.Find(id);
        }

        public List<T> GetAll()
        {
            return Items.ToList();
        }

        public void Add(T item)
        {
            Items.Add(item);
        }

        public void Remove(T id)
        {
            var item = Get(id);
            Items.Remove(item);
        }

        public void Edit(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
