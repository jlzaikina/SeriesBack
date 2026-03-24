using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("Actors", Schema = "public")]
public class Actor
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<SeriesActor> SeriesActors { get; set; }
}
