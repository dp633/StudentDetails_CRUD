using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using StudentDetails.Areas.LOC_Branch.Models;

namespace StudentDetails.Areas.LOC_Branch.Controllers
{
    [Area("LOC_Branch")]
    [Route("LOC_Branch/{Controller}/{Action}")]
    public class LOC_Branch : Controller
    {

        #region SelectAll
        public IActionResult BranchAddEdit()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDB = new SqlDatabase(connectionstr);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_MST_BRANCH_SELECTALL");
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
            SqlCommand dbCommand = new SqlCommand("PR_MST_BRANCH_DELETE", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@BranchID", id);

            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("BranchAddEdit");

        }
        #endregion


        public IActionResult Branch_Add(int? id)
        {
            if (id == null)
            {
                return View();
            }
            else
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                DataTable dt = new DataTable();
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_MST_BRANCH_SelectByPK", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@BranchID", id);
                SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                dt.Load(sqlDataReader);
                BranchModel model = new BranchModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.BranchID = Convert.ToInt32(dr["BranchID"]);
                    model.BranchName = dr["BranchName"].ToString();
                    model.BranchCode = dr["BranchCode"].ToString();
                }
                return View(model);
            }

        }


        #region Insert
        [HttpPost]
        public IActionResult AddBranch(BranchModel modelBranchModel)
        {
            if (modelBranchModel.BranchID == 0)
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_MST_BRANCH_INSERT";
                objCmd.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = modelBranchModel.BranchName;
                objCmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = modelBranchModel.BranchCode;
                objCmd.ExecuteNonQuery();
                connection.Close();
                TempData["BranchInsertMsg"] = "Record Inserted Successfully";
                return RedirectToAction("Branch_Add");
            }
            else
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                DataTable dt = new DataTable();
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_MST_BRANCH_UPDATE", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                //SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                //dt.Load(sqlDataReader);
                BranchModel model = new BranchModel();
                dbCommand.Parameters.AddWithValue("@BranchID", SqlDbType.Int).Value = modelBranchModel.BranchID;
                dbCommand.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = modelBranchModel.BranchName;
                dbCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = modelBranchModel.BranchCode;
                dbCommand.ExecuteNonQuery();
                TempData["BranchInsertMsg"] = "Record Updated Successfully";

                return RedirectToAction("Branch_Add");

            }

        }

        #endregion
    }
}
