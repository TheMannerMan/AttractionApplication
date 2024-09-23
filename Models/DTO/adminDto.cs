using System;
namespace Models.DTO
{
    public class adminInfoDbDto
    {
        public int nrSeededUsers { get; set; } = 0;
        public int nrUnseededUsers { get; set; } = 0;

        public int nrSeededLocations { get; set; } = 0;
        public int nrUnseededLocations { get; set; } = 0;

        public int nrSeededAttractions { get; set; } = 0;
        public int nrUnseededAttractions { get; set; } = 0;

        public int nrSeededReviews{ get; set; } = 0;
        public int nrUnseededReviews { get; set; } = 0;
    }
}

