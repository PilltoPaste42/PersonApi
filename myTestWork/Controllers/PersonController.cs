using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myTestWork.Data;
using myTestWork.Models;

namespace myTestWork.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly myTestWorkContext _context;

        public PersonController(myTestWorkContext context)
        {
            _context = context;
            context.Database.Migrate();
        }

        /// GET: api/v1/Person
        /// <summary>
        ///     Get all persons
        /// </summary>
        /// <returns>
        ///     Return collection of Person objects
        /// </returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonRequestDTO>>> GetPerson()
        {
            List<PersonRequestDTO> result = await _context.Person.Select(x => new PersonRequestDTO()
            {
                PersonID = x.PersonID,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Skills = _context.Skill.Where(p => p.PersonID == x.PersonID)
                .Select(s => new SkillRequestDTO()
                {
                    SkillID = s.SkillID,
                    Name = s.Name,
                    Level = s.Level
                }).ToList()
            }).ToListAsync();

            //List<Person> persons = _context.Person.ToList();

            //foreach (var person in persons)
            //{
            //    person.Skills = await _context.Skill
            //        .Where(p => p.PersonID == person.PersonID)
            //        .ToListAsync();
            //}

            //return persons;
            return Ok(result);
        }

        /// GET: api/v1/Person/5
        /// <summary>
        ///     Get one person with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     Return Person object
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonRequestDTO>> GetPerson(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Person? person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            PersonRequestDTO result = new PersonRequestDTO()
            {
                PersonID = person.PersonID,
                Name = person.Name,
                DisplayName = person.DisplayName,
                Skills = await _context.Skill.Where(p => p.PersonID == person.PersonID)
                .Select(p => new SkillRequestDTO()
                {
                    SkillID = p.SkillID,
                    Name = p.Name,
                    Level = p.Level
                }).ToListAsync()
            };
            //person.Skills = await _context.Skill.Where(p => p.PersonID == person.PersonID).ToListAsync();


            return Ok(result);
        }

        /// PUT: api/v1/Person/5
        /// <summary>
        ///     Updating person data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, PersonRequestDTO person)
        {
            if (id != person.PersonID)
            {
                return BadRequest();
            }

            Person? oldPerson = await _context.Person.FindAsync(id);
            if (oldPerson == null)
            {
                return NotFound();
            }

            oldPerson.Name = person.Name;
            oldPerson.DisplayName = person.DisplayName;
            foreach (SkillRequestDTO skill in person.Skills)
            {
                Skill? oldSkill = await _context.Skill.
                    Where(p => (p.PersonID == person.PersonID) && (p.SkillID == skill.SkillID))
                    .SingleOrDefaultAsync();

                if (oldSkill == null)
                {
                    return BadRequest($"SkillID {skill.SkillID} is invalid");
                }

                oldSkill.Name = skill.Name;
                oldSkill.Level = skill.Level;
            }
            await _context.SaveChangesAsync();
            return Ok();

            //// Remove all old skills and add new skill to DB 
            //_context.Skill.RemoveRange(_context.Skill.Where(p => p.PersonID == person.PersonID).ToArray());
            //foreach(var item in person.Skills)
            //{   
            //    await _context.Skill.AddAsync(item);
            //}

            //_context.Entry(person).State = EntityState.Modified;
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PersonExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
        }

        /// POST: api/v1/Person
        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>
        ///     Return GET request for new Person object
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<PersonRequestDTO>> PostPerson(PersonCreateDTO person)
        {
            Person newPerson = new Person()
            {
                Name = person.Name,
                DisplayName = person.DisplayName,
                Skills = new List<Skill>()
            };
            await _context.Person.AddAsync(newPerson);
            await _context.SaveChangesAsync();

            foreach (SkillCreateDTO skill in person.Skills)
            {
                Skill temp = new Skill
                {
                    Name = skill.Name,
                    Level = skill.Level,
                    PersonID = newPerson.PersonID,
                    Person = newPerson
                };

                await _context.AddAsync(temp);
                //newPerson.Skills.Add(temp);
            }
            await _context.SaveChangesAsync();

            return await GetPerson(newPerson.PersonID);
        }

        /// DELETE: api/v1/Person/5
        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return HTTP 204 if delete is successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            Person? person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PersonExists(long id)
        {
            return _context.Person.Any(e => e.PersonID == id);
        }
    }
}
