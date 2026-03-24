using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("Directors", Schema = "public")]
public class Director
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    // Навигационные свойства
    public virtual ICollection<SeriesDirector> SeriesDirectors { get; set; }
}
