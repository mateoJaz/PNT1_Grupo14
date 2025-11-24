namespace MVCVeterinaria.ViewModels;

public class VeterinarioHeader
{
    public int VeterinarioId { get; set; }
    public string NombreCompleto { get; set; }
}

// 2. VIEWMODEL PRINCIPAL
public class TurnoSearchViewModel
{
    // Datos de entrada
    public int MascotaId { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    // Datos de salida (la lista plana de tu servicio)
    // Asegúrate de que tienes definida la clase TurnoDisponible en tu proyecto.
    public List<TurnoDisponible> TurnosDisponibles { get; set; } = new List<TurnoDisponible>();

    // Propiedad auxiliar (Calculada): Genera la lista única de veterinarios (columnas de la matriz)
    public List<VeterinarioHeader> EncabezadosVeterinarios => TurnosDisponibles
        // Agrupamos por ID del veterinario para tener uno por columna
        .GroupBy(t => t.VeterinarioId)
        .Select(g => new VeterinarioHeader
        {
            VeterinarioId = g.Key,
            // Tomamos el nombre del primer elemento de cada grupo
            NombreCompleto = g.First().VeterinarioNombreCompleto
        })
        .OrderBy(v => v.NombreCompleto)
        .ToList();
}