using myTasks.Models;
using System.Collections.Generic;

namespace myTasks.Persistence
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetAll();
        
        TaskItem Find(long id);
        bool Exists(long id);
        void Add(TaskItem item);
        void Delete(long id);
        bool Save();
    }
}
