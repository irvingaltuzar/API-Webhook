using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class ProcoreConfiguration
{
    public long Id { get; set; }

    public string CompanyId { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string ServiceUrl { get; set; } = null!;

    public string ServiceUrlLogin { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? FolderCompanyId { get; set; }
}
