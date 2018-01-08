using FluentValidation;

namespace myTasks.Models
{
    public class TaskItemDtoValidator : AbstractValidator<TaskItemDto>
    {
        public TaskItemDtoValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Length(1, 200);
        }
    }
}
