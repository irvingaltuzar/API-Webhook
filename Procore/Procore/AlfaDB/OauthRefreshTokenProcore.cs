using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class OauthRefreshTokenProcore
{
    public long Id { get; set; }

    public string AccessTokenId { get; set; } = null!;

    public string RefreshTokenId { get; set; } = null!;

    public string TokenType { get; set; } = null!;

    public bool? Revoked { get; set; }

    public int? ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
