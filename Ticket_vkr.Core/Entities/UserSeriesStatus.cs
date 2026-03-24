using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Ticket_vkr.UI.Entities;

[Table("UserSeriesStatuses", Schema = "public")]
[PrimaryKey(nameof(UserDeviceId), nameof(SeriesId))]
public class UserSeriesStatus
{
    [Column(Order = 0)] 
    public string UserDeviceId { get; set; }

    [Column(Order = 1)] 
    public string SeriesId { get; set; }

    public string Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}