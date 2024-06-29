using System.ComponentModel.DataAnnotations;

namespace ExamenPOO.Database.Entities;

public class TaskEntity
{
    public Guid Id { get; set; }

    [Display(Name = "Descripcion")]
    [MinLength(10, ErrorMessage = "La {0} debe tener al menos {1} caracteres")]
    [Required(ErrorMessage = "La {0} de la tarea es requerido")]
    public string Description { get; set; }

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "El {0} de la tarea es requerido")]
    public bool Status { get; set; }

    [Display(Name = "Prioridad")]
    [Required(ErrorMessage = "La {0} de la tarea es requerido")]
    public string Priority { get; set; }

    [Display(Name = "Tiempo estimado")]
    [Range(1, int.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 1.")]     //OJO CON ESTA LINEA
    [Required(ErrorMessage = "El {0} de la tarea es requerido")]
    public int RequiredTime { get; set; }
}
