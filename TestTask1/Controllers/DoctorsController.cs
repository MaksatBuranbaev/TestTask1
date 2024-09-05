using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask1.Data;
using TestTask1.Models;
using TestTask1.Dto;

namespace TestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorsPatientsDbContext _context;

        public DoctorsController(DoctorsPatientsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorListDto>>> GetDoctors(
            string sortBy = "FullName", int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Doctors.Include(d => d.Office)
                                        .Include(d => d.Specialization)
                                        .Include(d => d.District)
                                        .AsQueryable();

            switch (sortBy.ToLower())
            {
                case "fullname":
                    query = query.OrderBy(d => d.FullName);
                    break;
                case "officenumber":
                    query = query.OrderBy(d => d.Office.Number);
                    break;
                default:
                    query = query.OrderBy(d => d.FullName);
                    break;
            }

            var doctors = await query.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(d => new DoctorListDto
                                     {
                                         Id = d.Id,
                                         FullName = d.FullName,
                                         OfficeNumber = d.Office.Number,
                                         SpecializationName = d.Specialization.Name,
                                         DistrictNumber = d.District != null ? d.District.Number : null
                                     })
                                     .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorEditDto>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var doctorDto = new DoctorEditDto
            {
                Id = doctor.Id,
                FullName = doctor.FullName,
                OfficeId = doctor.OfficeId,
                SpecializationId = doctor.SpecializationId,
                DistrictId = doctor.DistrictId
            };

            return Ok(doctorDto);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> AddDoctor(DoctorCreateDto doctorDto)
        {
            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                OfficeId = doctorDto.OfficeId,
                SpecializationId = doctorDto.SpecializationId,
                DistrictId = doctorDto.DistrictId
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDoctor(int id, DoctorEditDto doctorDto)
        {
            if (id != doctorDto.Id)
            {
                return BadRequest();
            }

            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            doctor.FullName = doctorDto.FullName;
            doctor.OfficeId = doctorDto.OfficeId;
            doctor.SpecializationId = doctorDto.SpecializationId;
            doctor.DistrictId = doctorDto.DistrictId;

            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
