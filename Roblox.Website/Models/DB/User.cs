using System;
using System.Collections.Generic;

namespace Roblox.Website.Models.DB;

/// <summary>
/// Contains all of the ROBLOX users.
/// </summary>
public partial class User
{
    public DateTime Created { get; set; }

    public DateTime LastOnline { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
}
