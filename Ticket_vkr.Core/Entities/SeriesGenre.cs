using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("SeriesGenres", Schema = "public")]
[PrimaryKey(nameof(SeriesId), nameof(GenreId))]
public class SeriesGenre
{
    [Column(Order = 0)]
    public Guid SeriesId { get; set; }

    [Column(Order = 1)]
    public int GenreId { get; set; }

    [ForeignKey("SeriesId")]
    public virtual Series Series { get; set; }

    [ForeignKey("GenreId")]
    public virtual Genres Genre { get; set; }
}
