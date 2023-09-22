using System.ComponentModel.DataAnnotations;

namespace StudentDetails.Areas.LOC_City.Models
{
    public class CityModel
    {
        [Required(ErrorMessage = "ID is Requried")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Name is Requried")]
        public string? CityName { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public int CityCode { get; set; }

        public int StateID { get; set; }
        public int CountryID { get; set; }
    }
    public class LOC_CityDropDownModel
    {

        public int CityID { get; set; }
        public String CityName { get; set; }

    }
}
