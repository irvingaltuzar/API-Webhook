using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class OauthTokensProcore
{
    public string Id { get; set; } = null!;

    public long? ResourceOwnerId { get; set; }

    public bool? Revoked { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
