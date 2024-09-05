namespace TestTask1.Dto
{
    public class DoctorCreateDto
    {
        public string FullName { get; set; }
        public int OfficeId { get; set; }
        public int SpecializationId { get; set; }
        public int? DistrictId { get; set; }
    }
}
