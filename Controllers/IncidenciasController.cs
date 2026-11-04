using Microsoft.AspNetCore.Mvc;
using PracticaApi.Models;
using PracticaApi.Enums;

namespace PracticaApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
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

    [HttpPost()]
    public ActionResult<Incidencia> Post([FromBody] Incidencia incidencia)
    {
        if(string.IsNullOrEmpty(incidencia.Titulo) || string.IsNullOrEmpty(incidencia.Descripcion) || string.IsNullOrEmpty(incidencia.TecnicoAsignado) || incidencia.Prioridad <= 0)
        {
            return BadRequest("Error en los datos introducidos.");
        }

        incidencia.Id = siguienteId;
        // Lo hace en el constructor de la clase Incidencia.
        incidencia.FechaCreacion = DateTime.Now;
        incidencia.Estado = EstadoIncidencia.Abierta;

        incidencias.Add(incidencia);
        siguienteId++;
        return CreatedAtAction(nameof(GetById), new {id = incidencia.Id}, incidencia);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put([FromBody] Incidencia incidencia, int id)
    {
        if(string.IsNullOrEmpty(incidencia.Titulo) || string.IsNullOrEmpty(incidencia.Descripcion) || string.IsNullOrEmpty(incidencia.TecnicoAsignado) || incidencia.Prioridad <= 0)
        {
            return BadRequest("Error en los datos introducidos.");
        }

        Incidencia? incidenciaModificar = incidencias.FirstOrDefault(i => i.Id == id);

        if (incidenciaModificar == null)
        {
            return NotFound();
        }

        incidenciaModificar.Titulo = incidencia.Titulo;
        incidenciaModificar.Descripcion = incidencia.Descripcion;
        incidenciaModificar.TecnicoAsignado = incidencia.TecnicoAsignado;
        incidenciaModificar.Prioridad = incidencia.Prioridad;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        Incidencia? incidenciaEliminar = incidencias.FirstOrDefault(i => i.Id == id);

        if(incidenciaEliminar == null)
        {
            return NotFound();
        }

        incidencias.Remove(incidenciaEliminar);
        return NoContent();
    }

    [HttpPut("{id:int}/resolver")]
    public ActionResult PutResolver(int id)
    {
        Incidencia? incidenciaResolver = incidencias.FirstOrDefault(i => i.Id == id);

        if(incidenciaResolver == null)
        {
            return NotFound();
        }

        if(incidenciaResolver.Estado == EstadoIncidencia.Resuelta)
        {
            return Conflict();
        }

        incidenciaResolver.Estado = EstadoIncidencia.Resuelta;
        return NoContent();
    }
}