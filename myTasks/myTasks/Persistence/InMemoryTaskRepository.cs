using Microsoft.EntityFrameworkCore;
using myTasks.Models;
using System.Collections.Generic;
using System.Linq;

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
            _context.TaskItems.Add(new TaskItem() { Name = "Buy new TV." });
            _context.TaskItems.Add(new TaskItem() { Name = "Pay bills." });
            _context.TaskItems.Add(new TaskItem() { Name = "Send package." });
            _context.SaveChanges();
        }

        public void Add(TaskItem item)
        {
            _context.TaskItems.Add(item);
            _context.SaveChanges();
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.TaskItems.ToList();
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
