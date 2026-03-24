using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Domain.Status;

public class UpdateStatusRequest
{
    public string SeriesId { get; set; }
    public string Status { get; set; }
    public string DeviceId { get; set; }
}
