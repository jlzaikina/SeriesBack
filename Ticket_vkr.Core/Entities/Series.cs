using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("Series", Schema = "public")]
public class Series
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string PosterPath { get; set; }
    public string Plot { get; set; }
    public int ReleaseYearStart { get; set; }
    public int ReleaseYearEnd { get; set; }
    public string Country { get; set; }
    public int SeasonsCount { get; set; }
    public string EpisodeLength { get; set; }
    public double Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<SeriesGenre> SeriesGenres { get; set; }
}
