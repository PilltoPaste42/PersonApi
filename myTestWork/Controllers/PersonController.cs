#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myTestWork.Data;
using myTestWork.Models;

namespace myTestWork.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly myTestWorkContext _context;

        public PersonController(myTestWorkContext context)
        {
            _context = context;
        }

        // GET: api/v1/Person
        /// <summary>
        ///     Get all persons
        /// </summary>
        /// <returns>
        ///     Return collection of Person objects
        /// </returns>
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPerson()
        {
            List<Person> persons = _context.Person.ToList();

            foreach (var person in persons)
            {
                person.Skills = await _context.Skill
                    .Where(p => p.PersonID == person.PersonID)
                    .ToListAsync();
            }
            return persons;
        }

        // GET: api/v1/Person/5
        /// <summary>
        ///     Get one person with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     Return Person object
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            person.Skills = await _context.Skill.Where(p => p.PersonID == person.PersonID).ToListAsync();
            
            
            return person;
        }

        // PUT: api/v1/Person/5
        /// <summary>
        ///     Updating person data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.PersonID)
            {
                return BadRequest();
            }
            
            // Remove all old skills and add new skill to DB 
            _context.Skill.RemoveRange(_context.Skill.Where(p => p.PersonID == person.PersonID).ToArray());
            foreach(var item in person.Skills)
            {   
                await _context.Skill.AddAsync(item);
            }

            _context.Entry(person).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/Person
        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>
        ///     Return GET request for new Person object
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Person.Add(person);
            foreach (var skill in person.Skills)
            {
                _context.Skill.Add(skill);
            }


            await _context.SaveChangesAsync();

            return await GetPerson(person.PersonID);
        }

        // DELETE: api/v1/Person/5
        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return HTTP 204 if delete is successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(long id)
        {
            return _context.Person.Any(e => e.PersonID == id);
        }
    }
}
