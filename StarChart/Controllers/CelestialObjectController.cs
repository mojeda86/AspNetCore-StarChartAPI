using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObjectFound = _context.CelestialObjects.SingleOrDefault(co => co.Id == id);
            if (celestialObjectFound == null) return NotFound();

            celestialObjectFound.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == id).ToList();
            return Ok(celestialObjectFound);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjectFound = _context.CelestialObjects.Where(co => co.Name.Equals(name)).ToList();
            if (celestialObjectFound.Count == 0) return NotFound();

            foreach (var item in celestialObjectFound)
            {
                item.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObjectFound);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCelestialObjectFound = _context.CelestialObjects;
            foreach (var item in allCelestialObjectFound)
            {
                item.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(allCelestialObjectFound);
        }
    }
}
