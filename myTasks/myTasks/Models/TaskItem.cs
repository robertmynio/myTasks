namespace myTasks.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public void UpdateFrom(TaskItem otherItem)
        {
            Name = otherItem.Name;
            IsComplete = otherItem.IsComplete;
        }
    }
}
