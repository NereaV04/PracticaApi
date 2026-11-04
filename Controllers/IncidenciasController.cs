using Microsoft.AspNetCore.Mvc;
using PracticaApi.Models;
using PracticaApi.Enums;

namespace PracticaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IncidenciasController : ControllerBase
{
    private static readonly List<Incidencia> incidencias = new List<Incidencia>
    {
        new Incidencia { Id = 1, Titulo = "Fall en la conexión de red.", Descripcion = "No se puede acceder a internet desde la oficina.", TecnicoAsignado = "Pepe Lopez", FechaCreacion = new DateTime(2023, 4, 28), Prioridad = 1, Estado = EstadoIncidencia.Abierta },
        new Incidencia { Id = 2, Titulo = "Error en la impresora.", Descripcion = "La impresora no imprime correctamente.", TecnicoAsignado = "Maria Garcia", FechaCreacion = DateTime.Now, Prioridad = 2, Estado = EstadoIncidencia.Abierta },
        new Incidencia { Id = 3, Titulo = "Problema con la refrigeración.", Descripcion = "El ordenador se sobrecalienta demasiado.", TecnicoAsignado = "Juan Perez", FechaCreacion = new DateTime(2000, 12, 1), Prioridad = 3, Estado = EstadoIncidencia.Resuelta },
    };

    private static int siguienteId = incidencias.Count() + 1;

    [HttpGet]
    public ActionResult<List<Incidencia>> GetAll()
    {
        return Ok(incidencias);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Incidencia> GetById(int id)
    {
        foreach(var incidencia in incidencias)
        {
            if(incidencia.Id == id)
            {
                return Ok(incidencia);
            }
        }

        return NotFound();
    }
}