using ExamenPOO.Dtos.Tasks;

namespace ExamenPOO.Services.Interfaces;

public interface ITasksService
{
    Task<List<TaskDto>> GetTasksListAsync();
    Task<TaskDto> GetTaskByIdAsync(Guid id);
    Task<bool> CreateAsync(TaskCreateDto dto);
    Task<bool> EditAsync(TaskEditDto dto, Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ChangeStatusAsync(Guid id);
    Task<List<TaskDto>> GetTaskListByPriority(string priority);
    Task<int> CalculateTotalTime();


}
