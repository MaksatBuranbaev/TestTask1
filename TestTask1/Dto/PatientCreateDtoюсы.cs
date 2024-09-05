namespace TestTask1.Dto
{
    public class PatientCreateDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int DistrictId { get; set; }
    }

}
