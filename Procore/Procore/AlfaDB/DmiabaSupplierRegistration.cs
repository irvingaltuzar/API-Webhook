using System;
using System.Collections.Generic;

namespace Procore.AlfaDB;

public partial class DmiabaSupplierRegistration
{
    public int Id { get; set; }

    public string? Rfc { get; set; }

    public string BusinessName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public byte? Status { get; set; }

    public string TypePerson { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? TypeSupplier { get; set; }

    public string? Efo { get; set; }

    public DateTime? Date { get; set; }

    public string? StatusFiles { get; set; }

    public int? Bank { get; set; }

    public string BankAccount { get; set; } = null!;

    public string BankClabe { get; set; } = null!;

    public string? BankSwift { get; set; }

    public string? Zip { get; set; }

    public string Address { get; set; } = null!;

    public string Suburb { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Cp { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? BanEmail { get; set; }

    public string? ReferenciaIntelisis { get; set; }

    public string? Classification { get; set; }

    public string? WebPage { get; set; }

    public string CreditDays { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string? MotiveDown { get; set; }

    public int? UpdateUser { get; set; }

    public string? UserApproved { get; set; }

    public string? Import { get; set; }

    public int? ManualDown { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? User { get; set; }

    public string? SpecialityMain { get; set; }

    public string? ComercialName { get; set; }

    public int? ProcoreId { get; set; }

    public string? ContactReference { get; set; }
}
