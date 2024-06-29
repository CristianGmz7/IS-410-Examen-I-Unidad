using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.Dtos.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El {0} de la tarea es requerido")]
    public bool Status { get; set; }
    public string Priority { get; set; }
    public int RequiredTime { get; set; }

}
