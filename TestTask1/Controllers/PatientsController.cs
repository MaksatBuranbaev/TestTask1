using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask1.Models;
using TestTask1.Dto;
using TestTask1.Data;

namespace TestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DoctorsPatientsDbContext _context;

        public PatientsController(DoctorsPatientsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientListDto>>> GetPatients(
            string sortBy = "LastName", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Patients.Include(p => p.District).AsQueryable();

            switch (sortBy.ToLower())
            {
                case "lastname":
                    query = query.OrderBy(p => p.LastName);
                    break;
                case "firstname":
                    query = query.OrderBy(p => p.FirstName);
                    break;
                default:
                    query = query.OrderBy(p => p.LastName);
                    break;
            }

            var patients = await query.Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .Select(p => new PatientListDto
                                      {
                                          Id = p.Id,
                                          LastName = p.LastName,
                                          FirstName = p.FirstName,
                                          MiddleName = p.MiddleName,
                                          Address = p.Address,
                                          BirthDate = p.BirthDate,
                                          Gender = p.Gender,
                                          DistrictNumber = p.District.Number
                                      })
                                      .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientEditDto>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientDto = new PatientEditDto
            {
                Id = patient.Id,
                LastName = patient.LastName,
                FirstName = patient.FirstName,
                MiddleName = patient.MiddleName,
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                DistrictId = patient.DistrictId
            };

            return Ok(patientDto);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] PatientCreateDto patientDto)
        {
            // Проверка существования DistrictId
            if (!_context.Districts.Any(d => d.Id == patientDto.DistrictId))
            {
                return BadRequest("Invalid DistrictId.");
            }

            var patient = new Patient
            {
                LastName = patientDto.LastName,
                FirstName = patientDto.FirstName,
                MiddleName = patientDto.MiddleName,
                Address = patientDto.Address,
                BirthDate = patientDto.BirthDate,
                Gender = patientDto.Gender,
                DistrictId = patientDto.DistrictId
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditPatient(int id, PatientEditDto patientDto)
        {
            if (id != patientDto.Id)
            {
                return BadRequest();
            }

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.LastName = patientDto.LastName;
            patient.FirstName = patientDto.FirstName;
            patient.MiddleName = patientDto.MiddleName;
            patient.Address = patientDto.Address;
            patient.BirthDate = patientDto.BirthDate;
            patient.Gender = patientDto.Gender;
            patient.DistrictId = patientDto.DistrictId;

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
