using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using myTasks.Controllers;
using myTasks.Models;
using myTasks.Persistence;
using Xunit;

namespace myTasks.Tests
{
    public class TaskControllerTest
    {
        private TaskItem[] _testData = new[]
        {
            new TaskItem() { Name = "Buy new TV." },
            new TaskItem() { Name = "Pay bills." },
            new TaskItem() { Name = "Send package.", IsComplete = true },
            new TaskItem() { Name = "a", IsComplete = false }
        };

        [Fact]
        public void GetAllReturnsAllTasks()
        {
            var controller = CreateTaskController("GetAllReturnsAllTasks");

            var tasks = GetAllTasks(controller);

            Assert.Equal(4, tasks.Count());
        }

        private static IEnumerable<TaskItem> GetAllTasks(TaskController controller)
        {
            var result = controller.GetAll() as OkObjectResult;
            return result.Value as IEnumerable<TaskItem>;
        }

        private TaskController CreateTaskController(string databaseName)
        {
            return new TaskController(new InMemoryTaskRepository(databaseName, _testData));
        }

        [Fact]
        public void GetAllReturnsListOfTasksOrderedByName()
        {
            var controller = CreateTaskController("GetAllReturnsAllTasks");

            var tasks = GetAllTasks(controller);

            var orderedTaskList = tasks.OrderBy(t => t.Name);
            Assert.Equal(orderedTaskList, tasks);
        }

        [Fact]
        public void GetByIdReturnsTaskWithMatchingId()
        {
            var controller = CreateTaskController("GetByIdReturnsTaskWithMatchingId");
            var taskId = GetTaskIdForTask(controller, _testData[3].Name);

            var task = GetTaskById(controller, taskId);

            Assert.Equal(_testData[3].Name, task.Name);
            Assert.Equal(_testData[3].IsComplete, task.IsComplete);
        }

        private long GetTaskIdForTask(TaskController controller, string taskName)
        {
            var allTasks = GetAllTasks(controller);
            return allTasks.FirstOrDefault(t => t.Name == taskName).Id;
        }

        private static TaskItem GetTaskById(TaskController controller, long id)
        {
            var result = controller.GetById(id) as OkObjectResult;
            return result.Value as TaskItem;
        }

        [Fact] 
        public void GetByIdWithIncorrectIdReturnsNotFoundStatus()
        {
            var controller = CreateTaskController("GetByIdWithIncorrectIdReturnsNotFound");

            var result = controller.GetById(500);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateReturnsCreatedAtRouteStatus()
        {
            var controller = CreateTaskController("CreateReturnsCreatedAtRouteStatus");
            var taskItem = new TaskItemDto { Name = "NewTask", IsComplete = false };

            var createResult = controller.Create(taskItem);

            Assert.IsType<CreatedAtRouteResult>(createResult);
        }

        [Fact]
        public void CreateReturnsExpectedValueInCreatedAtRouteStatus()
        {
            var controller = CreateTaskController("CreateReturnsExpectedValueInCreatedAtRouteStatus");
            var taskItem = new TaskItemDto { Name = "NewTask", IsComplete = false };

            var createResult = controller.Create(taskItem) as CreatedAtRouteResult;

            var createdTask = createResult.Value as TaskItem;
            Assert.Equal(taskItem.Name, createdTask.Name);
            Assert.Equal(taskItem.IsComplete, createdTask.IsComplete);
        }

        [Fact]
        public void CreateAddsTaskToTaskList()
        {
            var controller = CreateTaskController("CreateAddsTaskToTaskList");
            var taskItem = new TaskItemDto { Name = "NewTask", IsComplete = false };

            controller.Create(taskItem);

            var tasks = GetAllTasks(controller);
            Assert.Equal(5, tasks.Count());
        }

        [Fact]
        public void CreateReturnsBadRequestForInvalidData()
        {
            var controller = CreateTaskController("CreateReturnsBadRequestForInvalidData");

            var createResult = controller.Create(null);

            Assert.IsType<BadRequestResult>(createResult);
        }

        [Fact]
        public void UpdateReturnsBadRequestForInvalidData()
        {
            var controller = CreateTaskController("UpdateReturnsBadRequestForInvalidData");

            var updateResult = controller.Update(1, null);

            Assert.IsType<BadRequestResult>(updateResult);
        }

        [Fact]
        public void UpdateReturnsBadRequestForInvalidId()
        {
            var controller = CreateTaskController("UpdateReturnsBadRequestForInvalidId");
            var taskItem = new TaskItemDto { Name = "NewTask", IsComplete = false };

            var updateResult = controller.Update(1, taskItem);

            Assert.IsType<NotFoundResult>(updateResult);
        }

        [Fact]
        public void UpdateReturnsNoContentStatus()
        {
            var controller = CreateTaskController("UpdateReturnsNoContentStatus");
            var taskItem = new TaskItemDto { Name = "NewTask", IsComplete = false };
            var id = GetTaskIdForTask(controller, _testData[3].Name);

            var updateResult = controller.Update(id, taskItem);

            Assert.IsType<NoContentResult>(updateResult);
        }

        [Fact]
        public void UpdateCorrectlyModifiesTask()
        {
            string newName = "NewName";
            var controller = CreateTaskController("UpdateCorrectlyModifiesTask");
            var taskItem = new TaskItemDto { Name = newName };
            var id = GetTaskIdForTask(controller, _testData[3].Name);

            controller.Update(id, taskItem);

            var updatedTaskItem = GetTaskById(controller, id);
            Assert.Equal(newName, updatedTaskItem.Name);
        }

        [Fact]
        public void DeleteReturnsNoContentStatus()
        {
            var controller = CreateTaskController("DeleteReturnsOk");
            var taskId = GetTaskIdForTask(controller, _testData[3].Name);

            var deleteResult = controller.Delete(taskId) as NoContentResult;

            Assert.NotNull(deleteResult);
        }

        [Fact]
        public void DeleteRemovesTaskFromTaskList()
        {
            var controller = CreateTaskController("DeleteRemovesTaskFromTaskList");

            controller.Delete(1);

            var tasks = GetAllTasks(controller);
            Assert.Equal(3, tasks.Count());
        }

        [Fact]
        public void DeleteWithIncorrectIdReturnsNotFoundStatus()
        {
            var controller = CreateTaskController("DeleteWithIncorrectIdReturnsNotFound");

            var result = controller.Delete(500);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
