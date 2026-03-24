using PdfSharp.Charting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Entities;

[Table("News", Schema = "public")]
public class News
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Platform { get; set; }

    public DateTime PublicationDate { get; set; }

    public string Genre { get; set; }

    public string Status { get; set; }
}
