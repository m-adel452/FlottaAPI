using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlottaAPI.Models;

public partial class Kikuldottjarmu
{
    public int Id { get; set; }

    public int GepjarmuId { get; set; }

    public int KikuldetesId { get; set; }

    public int Sofor { get; set; }
    [JsonIgnore]
    public virtual Gepjarmu Gepjarmu { get; set; } = null!;
    [JsonIgnore]
    public virtual Kikuldete Kikuldetes { get; set; } = null!;
    [JsonIgnore]
    public virtual Sofor SoforNavigation { get; set; } = null!;
}
