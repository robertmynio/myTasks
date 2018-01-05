using Microsoft.AspNetCore.Mvc;
using myTasks.Models;
using myTasks.Persistence;

namespace myTasks.Controllers
{
    [Route("api/tasks")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_taskRepository.GetAll());
        }

        [HttpGet("{id}", Name = "GetTask")]
        public IActionResult GetById(long id)
        {
            var item = _taskRepository.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _taskRepository.Add(item);
            if (!_taskRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return CreatedAtRoute("GetTask", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TaskItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_taskRepository.Exists(id))
            {
                return NotFound();
            }

            item.Id = id;
            _taskRepository.Update(item);
            if (!_taskRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!_taskRepository.Exists(id))
            {
                return NotFound();
            }

            _taskRepository.Delete(id);
            if (!_taskRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
