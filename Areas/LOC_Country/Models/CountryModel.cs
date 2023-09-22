using System.ComponentModel.DataAnnotations;

namespace StudentDetails.Areas.LOC_Country.Models
{
    public class CountryModel
    {
        
            public int CountryID { get; set; }

            [Required(ErrorMessage = "Name is Requried")]
            public string? CountryName { get; set; }

            [Required(ErrorMessage = "Code is Requried")]
            public string? CountryCode { get; set; }
        
    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }

        public string? CountryName { get; set; }
    }
}
