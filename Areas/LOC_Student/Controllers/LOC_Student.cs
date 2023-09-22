using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using StudentDetails.Areas.LOC_Student.Models;
using StudentDetails.Areas.LOC_City.Models;
using StudentDetails.Areas.LOC_Branch.Models;

namespace StudentDetails.Areas.LOC_Student.Controllers
{
    [Area("LOC_Student")]
    [Route("LOC_Student/{Controller}/{Action}")]
    public class LOC_Student : Controller
    {
        public IConfiguration Configuration;
        public LOC_Student(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region SelectAll
        public IActionResult StudentList()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDB = new SqlDatabase(connectionstr);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_MST_STUDENT_SELECTALL");
            DataTable dt = new DataTable();
            using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
            {
                dt.Load(dr);
            }
            return View(dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            connection.Open();
            SqlCommand dbCommand = new SqlCommand("PR_MST_STUDENT_DELETE", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@StudentID", id);

            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("StudentList");

        }
        #endregion

        //public IActionResult ValidateForm()
        //{ 
        //    return View();
        //}
        //public IActionResult Save(StudentModel student)
        //{
        //    if (string.IsNullOrEmpty(student.StudentName))
        //    {
        //        TempData["MSG"] = "name is empty";
        //        return View("ValidateForm");
        //    }
        //    else if (student.StudentAge <= 0 || student.StudentAge>100)
        //    {
        //        TempData["MSG"] = "enter Valid age";
        //        return View("ValidateForm");
        //    }

        //    else if (student.StudentEmail == null)
        //    {
        //        TempData["MSG"] = "email is empty";
        //        return View("ValidateForm");
        //    }
        //    else
        //    {
        //        return View("Index");
        //    }
        //}

        #region ADDEDIT
        public IActionResult StudentAddEdit(int? StudentID)
        {
            FillCity_DropDownMenu();
            FillBranch_DropDownMenu();
            if (StudentID != null)
            {
                FillCity_DropDownMenu();
                FillBranch_DropDownMenu();
                string connectionstr = this.Configuration.GetConnectionString("MyConnection");
                DataTable dt = new DataTable();
                SqlConnection sqlconnection = new SqlConnection(connectionstr);
                sqlconnection.Open();
                SqlCommand objcmd = sqlconnection.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "PR_MST_STUDENT_SelectByPK";
                objcmd.Parameters.AddWithValue("StudentID", StudentID);
                SqlDataReader sqldatareader = objcmd.ExecuteReader();
                dt.Load(sqldatareader);
                StudentModel model = new StudentModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StudentID = Convert.ToInt32(dr["StudentID"]);
                    model.BranchID = Convert.ToInt32(dr["BranchID"]);
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.StudentName = dr["StudentName"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.MobileNoStudent = Convert.ToString(dr["MobileNoStudent"]);
                    model.MobileNoFather = Convert.ToString(dr["MobileNoFather"]);
                    model.Address = dr["Address"].ToString();
                    model.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    model.Age = Convert.ToInt32(dr["Age"]);
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    model.Gender = dr["Gender"].ToString();
                    model.Password = dr["Password"].ToString();
                }
                return View(model);
            }
            return View();
        }
        #endregion

        #region ADDEDIT METHOD
        public IActionResult AddEditMethod(StudentModel model)
        {

            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlconnection = new SqlConnection(connectionstr);
            sqlconnection.Open();
            SqlCommand objcmd = sqlconnection.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            if (model.StudentID == null)
            {
                objcmd.CommandText = "PR_MST_STUDENT_INSERT";
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            else
            {
                objcmd.CommandText = "PR_MST_STUDENT_UPDATE";
                objcmd.Parameters.AddWithValue("StudentID", model.StudentID);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            return RedirectToAction("StudentList");
        }
        #endregion

        
        #region CITY DROPDOWN
        public void FillCity_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_CityDropDownModel> lOC_CityDropDowns = new List<LOC_CityDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "CityDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_CityDropDownModel loc_CityDropDownModel = new LOC_CityDropDownModel();
                    {
                        loc_CityDropDownModel.CityID = Convert.ToInt32(sqlDataReader["CityID"]);
                        loc_CityDropDownModel.CityName = sqlDataReader["CityName"].ToString();
                    };
                    lOC_CityDropDowns.Add(loc_CityDropDownModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.CityList = lOC_CityDropDowns;
        }
        #endregion

        #region BRANCH DROPDOWN
        public void FillBranch_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_BranchDropDownModel> loc_BranchDropDowns = new List<LOC_BranchDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "BranchDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_BranchDropDownModel loc_BranchDropModel = new LOC_BranchDropDownModel();
                    {
                        loc_BranchDropModel.BranchID = Convert.ToInt32(sqlDataReader["BranchID"]);
                        loc_BranchDropModel.BranchName = sqlDataReader["BranchName"].ToString();
                    };
                    loc_BranchDropDowns.Add(loc_BranchDropModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.BranchList = loc_BranchDropDowns;
        }
        #endregion

    }
}
