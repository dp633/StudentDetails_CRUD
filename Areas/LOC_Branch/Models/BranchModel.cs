using System.ComponentModel.DataAnnotations;

namespace StudentDetails.Areas.LOC_Branch.Models
{
    public class BranchModel
    {

        public int BranchID { get; set; }

        [Required(ErrorMessage = "Name is Requried")]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public string? BranchCode { get; set; }

    }

    public class LOC_BranchDropDownModel
    {
        public int BranchID { get; set; }

        public string? BranchName { get; set; }
    }
}
