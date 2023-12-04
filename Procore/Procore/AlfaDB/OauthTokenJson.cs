using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class OauthTokenJson
{
    public string access_token { get; set; } = null!;
    public string refresh_token { get; set; } = null!;

    public string token_type { get; set; } = null!;

    public int? expires_in { get; set; }

    public long? resource_owner_id { get; set; }

    public int? expires_in_seconds { get; set; }


}
