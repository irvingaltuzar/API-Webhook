using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class CatSupplierNotification
{
    public int Id { get; set; }

    public string ResponsableUser { get; set; } = null!;

    public string? Mail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
