namespace BookStoreDK.Models.Models.Users
{
    public record Employee
    {
        public int EmployeeId { get; set; }
        public string NationalIdNumber { get; init; } = null!;

        public string? EmployeeName { get; init; } = null!;
        public string LoginId { get; init; } = null!;
        public string JobTitle { get; init; } = null!;
        public DateTime BirthDate { get; init; }
        public string MaritalStatus { get; init; } = null!;
        public string Gender { get; init; } = null!;

        public DateTime HireDate { get; init; }

        public short VacationHours { get; init; }
        public short SickLeaveHours { get; init; }
        public Guid rowguid { get; init; }
        public DateTime ModifiedDate { get; init; }


    }
}
