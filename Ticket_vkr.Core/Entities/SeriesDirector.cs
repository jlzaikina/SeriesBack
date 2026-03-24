using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("SeriesDirectors", Schema = "public")]
[PrimaryKey(nameof(SeriesId), nameof(DirectorId))]
public class SeriesDirector
{
    public Guid SeriesId { get; set; }
    public int DirectorId { get; set; }

    [ForeignKey("SeriesId")]
    public virtual Series Series { get; set; }

    [ForeignKey("DirectorId")]
    public virtual Director Director { get; set; }
}
