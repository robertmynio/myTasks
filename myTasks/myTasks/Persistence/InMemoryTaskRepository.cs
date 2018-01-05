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
        {
            var builder = new DbContextOptionsBuilder<TaskContext>();
            builder.UseInMemoryDatabase("InMemoryTaskRepository");
            _context = new TaskContext(builder.Options);

            if (_context.TaskItems.Count() == 0)
            {
                InitializeDummyData();
            }
        }

        private void InitializeDummyData()
        {
            _context.TaskItems.AddRange(
                new TaskItem() { Name = "Buy new TV." }, 
                new TaskItem() { Name = "Pay bills." }, 
                new TaskItem() { Name = "Send package." });
            _context.SaveChanges();
        }

        public void Add(TaskItem item)
        {
            ClearId(item);
            _context.TaskItems.Add(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Clears the Id on the item which forces the DbContext to generate a new Id upon insertion.
        /// </summary>
        /// <param name="item">The task item to clear the Id for.</param>
        private void ClearId(TaskItem item)
        {
            item.Id = 0;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.TaskItems.OrderBy(x => x.Name).ToList();
        }

        public TaskItem Find(long id)
        {
            return _context.TaskItems.FirstOrDefault(t => t.Id == id);
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
