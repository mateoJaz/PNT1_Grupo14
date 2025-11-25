using Microsoft.EntityFrameworkCore;
using MVCVeterinaria.Context;
using MVCVeterinaria.ViewModels;

public class TurnoService
{
    private readonly VeterinariaDatabaseContext _context;

    public TurnoService(VeterinariaDatabaseContext context)
    {
        _context = context;
    }
    public async Task<List<TurnoDisponible>> EncontrarTurnosDisponibles(DateTime fechaInicio, DateTime fechaFin)
    {
        const int DURACION_TURNO_HORAS = 1;
        var HORARIO_INICIO = new TimeSpan(9, 0, 0); 
        var HORARIO_FIN = new TimeSpan(18, 0, 0); 

        var fechaInicioDia = fechaInicio.Date;
        var fechaFinDia = fechaFin.Date;


        var todosVeterinarios = await _context.Veterinario.AsNoTracking().ToListAsync();

        var turnosOcupados = await _context.Turno
            .Where(t => t.FechaHorario >= fechaInicioDia && t.FechaHorario < fechaFinDia.AddDays(1))
            .AsNoTracking()
            .ToListAsync();


        var disponibilidadTotal = new List<TurnoDisponible>();

        for (var fechaDiaActual = fechaInicioDia; fechaDiaActual <= fechaFinDia; fechaDiaActual = fechaDiaActual.AddDays(1))
        {
            if (fechaDiaActual.DayOfWeek != DayOfWeek.Saturday && fechaDiaActual.DayOfWeek != DayOfWeek.Sunday) 
            {
                foreach (var veterinario in todosVeterinarios)
                {
                    var turnosOcupadosDelDia = turnosOcupados
                        .Where(t => t.VeterinarioId == veterinario.Id && t.FechaHorario.Date == fechaDiaActual.Date)
                        .Select(t => t.FechaHorario)
                        .ToHashSet();

                    var horaSlot = HORARIO_INICIO;
                    while (horaSlot < HORARIO_FIN)
                    {
                        var inicioSlot = fechaDiaActual.Date.Add(horaSlot);

                        if (inicioSlot >= DateTime.Now)
                        {
                            bool slotOcupado = turnosOcupadosDelDia.Contains(inicioSlot);

                            if (!slotOcupado)
                            {
                                disponibilidadTotal.Add(new TurnoDisponible
                                {
                                    VeterinarioId = veterinario.Id,
                                    VeterinarioNombreCompleto = $"{veterinario.Nombre} {veterinario.Apellido}",
                                    InicioSlot = inicioSlot
                                });
                            }
                        }

                        horaSlot = horaSlot.Add(TimeSpan.FromHours(DURACION_TURNO_HORAS));
                    }
                }
            }
        }
        return disponibilidadTotal;
    }
}