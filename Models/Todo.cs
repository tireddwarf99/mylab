using System.ComponentModel.DataAnnotations;
namespace DotNetCoreSqlDb.Models
{
    public class Todo
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Введіть опис завдання.")]
        [StringLength(300)]
        public string Description { get; set; } = default!;
    }
}
