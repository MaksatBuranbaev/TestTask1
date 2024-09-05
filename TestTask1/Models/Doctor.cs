namespace TestTask1.Models;

public partial class Doctor
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int OfficeId { get; set; }
    public int SpecializationId { get; set; }
    public int? DistrictId { get; set; }

    public virtual Office Office { get; set; }
    public virtual Specialization Specialization { get; set; }
    public virtual District District { get; set; }
}
