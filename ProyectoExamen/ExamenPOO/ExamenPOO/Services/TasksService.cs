using ExamenPOO.Database.Entities;
using ExamenPOO.Dtos.Tasks;
using ExamenPOO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Newtonsoft.Json;

namespace ExamenPOO.Services;

public class TasksService : ITasksService
{
    public readonly string _JSON_FILE;

    public TasksService()
    {
        _JSON_FILE = "SeedData/tasks.json";
    }
    public async Task<TaskDto> GetTaskByIdAsync(Guid id)
    {
        var tasks = await ReadTasksFromFilesAsync();
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<List<TaskDto>> GetTasksListAsync()
    {
        return await ReadTasksFromFilesAsync();
    }

    public async Task<bool> CreateAsync(TaskCreateDto dto)
    {
        var tasksDtos = await ReadTasksFromFilesAsync();

        //LOGICA DEL CODIGO DEL CHECKTASK

        var taskDto = new TaskDto
        {
            Id = Guid.NewGuid(),
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            RequiredTime = dto.RequiredTime
        };

        tasksDtos.Add(taskDto);

        await WriteTasksToFileAsync(tasksDtos);

        return true;
    }

    public async Task<bool> EditAsync(TaskEditDto dto, Guid id)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var existingProduct = tasksDto.FirstOrDefault(t => t.Id == id);

        if (existingProduct is null)
        {
            return false;
        }

        //logica del CHECKTASKS

        for(int i = 0; i < tasksDto.Count; i++)
        {
            if(tasksDto[i].Id == id)
            {
                tasksDto[i].Description = dto.Description;
                tasksDto[i].Status = dto.Status;
                tasksDto[i].Priority = dto.Priority;
                tasksDto[i].RequiredTime = dto.RequiredTime;
            }
        }

        await WriteTasksToFileAsync(tasksDto);

        return true;


    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var taskToDelete = tasksDto.FirstOrDefault(t => t.Id == id);

        if(taskToDelete is null)
        {
            return false;
        }

        tasksDto.Remove(taskToDelete);
        await WriteTasksToFileAsync(tasksDto);
        return true;
    }

    private async Task<List<TaskDto>> ReadTasksFromFilesAsync()
    {
        if (!File.Exists(_JSON_FILE))
        {
            return new List<TaskDto>();
        }

        var json = await File.ReadAllTextAsync(_JSON_FILE);

        var tasks = JsonConvert.DeserializeObject<List<TaskDto>>(json);

        var dtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            RequiredTime = t.RequiredTime,
        }).ToList();

        return dtos;
    }

    private async Task WriteTasksToFileAsync(List<TaskDto> taskDtos)
    {
        var tasks = taskDtos.Select(t => new TaskEntity
        {
            Id = t.Id,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            RequiredTime = t.RequiredTime,
        }).ToList();

        var json = JsonConvert.SerializeObject(tasks, Formatting.Indented);

        if (File.Exists(_JSON_FILE))
        {
            await File.WriteAllTextAsync(_JSON_FILE, json);
        }
    }

    //CONSIDERAR SI ES NECESARIO AGREGAR EL CHECKTASK

    //Nuevos metodos
    //Cambiar el estado de una tarea
    public async Task<TaskDto> ChangeStatusAsync(Guid id)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var taskToChange = tasksDto.FirstOrDefault(t => t.Id == id);

        if(taskToChange is null)
        {
            return null;
        }

        for (int i = 0; i < tasksDto.Count; i++)
        {
            if (tasksDto[i].Id == id)
            {
                if (tasksDto[i].Status == true)
                {
                    tasksDto[i].Status = false;
                }
                else
                {
                    tasksDto[i].Status = true;
                }
            }
        }

        await WriteTasksToFileAsync(tasksDto);

        return taskToChange;
    }

    //Listar tareas por prioridad
    public async Task<List<TaskDto>> GetTaskListByPriority(string priority)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var tasksPriority = new List<TaskDto>();

        for(int i = 0; i < tasksDto.Count; i++)
        {
            if (tasksDto[i].Priority == priority)
            {
                tasksPriority.Add(tasksDto[i]);
            }
        }

        return tasksPriority;
    }

    //Calcular tiempo total para completar tareas pendientes
    public async Task<int> CalculateTotalTime()
    {
        var tasksDto = await ReadTasksFromFilesAsync();
        int suma = 0;

        for(int i = 0; i < tasksDto.Count ; i++)
        {
            if (tasksDto[i].Status == true)
            {
                suma += tasksDto[i].RequiredTime;
            }
        }

        return suma;
    }

    //Calcular tiempo total por prioridad
    public async Task<int> CalculateTotalTimePriority(string priority)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var tasksPriority = new List<TaskDto>();
        int suma = 0;

        for(int i = 0; i < tasksDto.Count; i++)
        {
            if (tasksDto[i].Status == true && tasksDto[i].Priority == priority)
            {
                suma += tasksDto[i].RequiredTime;
            }
        }

        return suma;
    }

    //Listar tareas por estado
    public async Task<List<TaskDto>> GetTaskListByStatus(bool status)
    {
        var tasksDto = await ReadTasksFromFilesAsync();

        var tasksStatus = new List<TaskDto>();

        for(int i = 0; i < tasksDto.Count; i++)
        {
            if (tasksDto[i].Status == status)
            {
                tasksStatus.Add(tasksDto[i]);
            }
        }

        return tasksStatus;
    }
}
