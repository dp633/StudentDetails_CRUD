using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using StudentDetails.Areas.LOC_Country.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace StudentDetails.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{Action}")]
    public class LOC_Country : Controller
    {

        #region SelectAll
        public IActionResult CountrySelectAll()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDB = new SqlDatabase(connectionstr);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_COUNTRY_SELECTALL");
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
            SqlCommand dbCommand = new SqlCommand("PR_LOC_COUNTRY_DELETE", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@CountryId", id);

            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("CountrySelectAll");

        }
        #endregion

        //public IActionResult Country_Add()
        //{
        //    return View();

        //}

        public IActionResult Country_Add(int? id)
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
                SqlCommand dbCommand = new SqlCommand("PR_LOC_COUNTRY_SelectByPK", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@CountryId", id);
                SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                dt.Load(sqlDataReader);
                CountryModel model = new CountryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.CountryName = dr["CountryName"].ToString();
                    model.CountryCode = dr["CountryCode"].ToString();
                }
                return View(model);
            }

        }


        #region Insert
        [HttpPost]
        public IActionResult AddCountry(CountryModel modelCountryModel)
        {
            if (modelCountryModel.CountryID == 0)
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_COUNTRY_INSERT";
                objCmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelCountryModel.CountryName;
                objCmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelCountryModel.CountryCode;
                objCmd.ExecuteNonQuery();
                connection.Close();
                TempData["CountryInsertMsg"] = "Record Inserted Successfully";
                return RedirectToAction("Country_Add");
            }
            else
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                DataTable dt = new DataTable();
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_LOC_COUNTRY_UPDATE", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                //SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                //dt.Load(sqlDataReader);
                CountryModel model = new CountryModel();
                dbCommand.Parameters.AddWithValue("@CountryID", SqlDbType.Int).Value = modelCountryModel.CountryID;
                dbCommand.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelCountryModel.CountryName;
                dbCommand.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelCountryModel.CountryCode;
                dbCommand.ExecuteNonQuery();
                TempData["CountryInsertMsg"] = "Record Updated Successfully";

                return RedirectToAction("Country_Add");

            }

        }

        #endregion

    }
}
