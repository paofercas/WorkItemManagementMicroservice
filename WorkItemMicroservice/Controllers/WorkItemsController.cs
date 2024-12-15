using Microsoft.AspNetCore.Mvc;

using WorkItemMicroservice.Models;
using WorkItemMicroservice.Services;

namespace WorkItemMicroservice.Controllers
{
    [ApiController]
    [Route("api/workitems")]  //API route
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
        }


        //GET /api/workitems

        [HttpGet]
        public ActionResult<IEnumerable<WorkItem>> GetAllWorkItems()
        {
            return Ok(_workItemService.GetAllWorkItems());
        }

        //GET /api/workitems/{id}

        [HttpGet("{id}")]
        public ActionResult<WorkItem> GetWorkItem(int id)
        {
            var workItem = _workItemService.GetWorkItemById(id);
            if (workItem == null)
                return NotFound();
            return Ok(workItem);
        }

        //POST /api/workitems

        [HttpPost]
        public async Task<ActionResult<WorkItem>> CreateWorkItem([FromBody] WorkItem workItem)
        {
            try
            {
                var createdWorkItem = await _workItemService.CreateWorkItemAsync(workItem);
                return CreatedAtAction(nameof(GetWorkItem), new { id = createdWorkItem.Id }, createdWorkItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear WorkItem: {ex.Message}");
                Console.WriteLine($"Detalles del error: {ex.StackTrace}");
                return StatusCode(500, new { Message = workItem, Error = ex.Message });
            }
        }


        //PUT /api/workitems/{id}

        [HttpPut("{id}")]
        public IActionResult UpdateWorkItem(int id, [FromBody] WorkItem updatedWorkItem)
        {
            if (id != updatedWorkItem.Id)
                return BadRequest();
            var existingWorkItem = _workItemService.GetWorkItemById(id);
            if (existingWorkItem == null)
                return NotFound();
            _workItemService.UpdateWorkItem(updatedWorkItem);
            return NoContent();
        }

        //DELETE /api/workitems/{id}

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkItem(int id)
        {
            var existingWorkItem = _workItemService.GetWorkItemById(id);
            if (existingWorkItem == null)
                return NotFound();
            _workItemService.DeleteWorkItem(id);
            return NoContent();
        }
    }
}
