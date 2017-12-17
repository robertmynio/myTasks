using myTasks.Models;
using System.Collections.Generic;

namespace myTasks.Persistence
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetAll();
        void Add(TaskItem item);
        TaskItem Find(long id);
    }
}
