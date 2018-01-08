namespace myTasks.Models
{
    public class TaskItem
    {
        public TaskItem()
        {
        }

        public TaskItem(TaskItemDto taskItemDto)
        {
            Name = taskItemDto.Name;
            IsComplete = taskItemDto.IsComplete;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public void UpdateFrom(TaskItemDto otherItem)
        {
            Name = otherItem.Name;
            IsComplete = otherItem.IsComplete;
        }
    }
}
