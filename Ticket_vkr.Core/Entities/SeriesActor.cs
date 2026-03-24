using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("SeriesActors", Schema = "public")]
[PrimaryKey(nameof(SeriesId), nameof(ActorId))]
public class SeriesActor
{
    public Guid SeriesId { get; set; }
    public int ActorId { get; set; }

    [ForeignKey("SeriesId")]
    public virtual Series Series { get; set; }

    [ForeignKey("ActorId")]
    public virtual Actor Actor { get; set; }
}
