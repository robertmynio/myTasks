using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTasks.Models;

namespace myTasks.Persistence
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public InMemoryTaskRepository() 
            : this("InMemoryTaskRepository", Enumerable.Empty<TaskItem>())
        {
        }

        public InMemoryTaskRepository(string databaseName, IEnumerable<TaskItem> seedData)
        {
            var builder = new DbContextOptionsBuilder<TaskContext>();
            builder.UseInMemoryDatabase(databaseName);
            _context = new TaskContext(builder.Options);

            if (_context.TaskItems.Count() == 0)
            {
                InitializeDummyData(seedData);
            }
        }

        private void InitializeDummyData(IEnumerable<TaskItem> seedData)
        {
            if (!seedData.Any())
            {
                seedData = new[]
                {
                    new TaskItem() { Name = "Buy new TV." },
                    new TaskItem() { Name = "Pay bills." },
                    new TaskItem() { Name = "Send package." }
                };
            }
            _context.TaskItems.AddRange(seedData);
            _context.SaveChanges();
        }

        public void Add(TaskItem item)
        {
            _context.TaskItems.Add(item);
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.TaskItems.OrderBy(x => x.Name).ToList();
        }

        public TaskItem Find(long id)
        {
            return _context.TaskItems.FirstOrDefault(t => t.Id == id);
        }

        public bool Exists(long id)
        {
            return Find(id) != null;
        }

        public void Delete(long id)
        {
            var existingItem = Find(id);
            if (existingItem != null)
            {
                _context.TaskItems.Remove(existingItem);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        private class TaskContext : DbContext
        {
            public TaskContext(DbContextOptions<TaskContext> options)
                : base(options)
            {
            }

            public DbSet<TaskItem> TaskItems { get; set; }
        }
    }
}
