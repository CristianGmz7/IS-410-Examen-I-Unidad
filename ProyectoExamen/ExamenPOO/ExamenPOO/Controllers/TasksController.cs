using ExamenPOO.Dtos.Tasks;
using ExamenPOO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamenPOO.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _tasksService.GetTasksListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get (Guid id)
    {
        var task = await _tasksService.GetTaskByIdAsync(id);

        if(task == null)
        {
            return NotFound(new { Message = $"No se encontro la categoria: {id}" });
        }

        return Ok(task);
    }


    [HttpPost]
    public async Task<ActionResult> Create (TaskCreateDto dto)
    {
        //falta validacion que se hace en checktask
        bool response = await _tasksService.CreateAsync(dto);

        return StatusCode(201);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(TaskEditDto dto, Guid id)
    {
        //falta validacion que se hace en checktask
        var response = await _tasksService.EditAsync(dto, id);

        if (!response)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete (Guid id)
    {
        var response = await _tasksService.DeleteAsync(id);

        if (!response)
        {
            return NotFound();
        }

        return Ok();
    }
    //ANTERIORES SON LOS 5 ENDPOINTS COMUNES: GET ALL, GET BY ID, CREATE, UPDATE, DELETE
    //1. Cambiar estado de tarea
    //Creo que el llamado esta malo, alternar entre Put y Create
    [HttpPut("ChangeStatus{id}")]
    public async Task<ActionResult> ChangeStatus(Guid id)
    {
        var response = await _tasksService.ChangeStatusAsync(id);

        if (!response)
        {
            return NotFound();
        }

        return Ok();

    }

    //2. Listar tarea por prioridad
    [HttpGet("GetPriority{text}")]
    public async Task<ActionResult> GetByPriority(string text)
    {
        var tasks = await _tasksService.GetTaskListByPriority(text);

        return Ok(tasks);
    }

    [HttpGet("GetTotalTime")]
    public async Task<ActionResult> GetTotalTime()
    {
        var total = await _tasksService.CalculateTotalTime();

        return Ok(total);
    }

}
