using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

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

            celestialObjectFound.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObject.Id == id).ToList();
            return Ok(celestialObjectFound);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjectFound = _context.CelestialObjects.Where(co => co.Name.Equals(name)).ToList();
            if (celestialObjectFound.Count == 0) return NotFound();

            foreach (var item in celestialObjectFound)
            {
                item.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObject.Id == item.Id).ToList();
            }
            return Ok(celestialObjectFound);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCelestialObjectFound = _context.CelestialObjects.ToList();
            foreach (var item in allCelestialObjectFound)
            {
                item.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObject.Id == item.Id).ToList();
            }
            return Ok(allCelestialObjectFound);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(CelestialObject celestialObject, int id)
        {
            var celestialObjectFound = _context.CelestialObjects.Find(id);
            if (celestialObjectFound == null) return NotFound();

            celestialObjectFound.Name = celestialObject.Name;
            celestialObjectFound.OrbitalPeriod = celestialObject.OrbitalPeriod;
            celestialObjectFound.OrbitedObject = celestialObject.OrbitedObject;

            _context.CelestialObjects.Update(celestialObjectFound);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObjectFound = _context.CelestialObjects.SingleOrDefault(co => co.Id == id);
            if (celestialObjectFound == null) return NotFound();

            celestialObjectFound.Name = name;
            _context.Update(celestialObjectFound);
            _context.SaveChanges();

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var celestialObjectFound = _context.CelestialObjects.SingleOrDefault(co => co.Id == id);
            

            
        //    if (celestialObjectFound == null) return NotFound();
        //}
    }
}
