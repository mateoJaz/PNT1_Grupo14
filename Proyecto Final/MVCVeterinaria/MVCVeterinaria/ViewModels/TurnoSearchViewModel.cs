namespace MVCVeterinaria.ViewModels;

public class VeterinarioHeader
{
    public int VeterinarioId { get; set; }
    public string? NombreCompleto { get; set; }
}
public class TurnoSearchViewModel
{
    public int MascotaId { get; set; }
    public int ClienteId { get; set; }
    public string? NombreMascota { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public List<TurnoDisponible> TurnosDisponibles { get; set; } = new List<TurnoDisponible>();
    public int VeterinarioIdSeleccionado { get; set; }
    public DateTime FechaHoraSeleccionada { get; set; }
    public string? Detalle { get; set; }
    public List<VeterinarioHeader> EncabezadosVeterinarios => TurnosDisponibles
        .GroupBy(t => t.VeterinarioId)
        .Select(g => new VeterinarioHeader
        {
            VeterinarioId = g.Key,
            NombreCompleto = g.First().VeterinarioNombreCompleto
        })
        .OrderBy(v => v.NombreCompleto)
        .ToList();
}