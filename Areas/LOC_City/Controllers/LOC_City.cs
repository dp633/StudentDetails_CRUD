using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using StudentDetails.Areas.LOC_City.Models;
using StudentDetails.Areas.LOC_Country.Models;
using StudentDetails.Areas.LOC_State.Models;

namespace StudentDetails.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/{Controller}/{Action}")]
    public class LOC_City : Controller
    {


        #region SelectAll
        public IActionResult CityAddEdit()
        {
            FillStateDropDownMenu();
            FillCountryDropDownMenu();
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDB = new SqlDatabase(connectionstr);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_CITY_SELECTALL");
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
            SqlCommand dbCommand = new SqlCommand("PR_LOC_CITY_DELETE", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@CityID", id);

            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("CityAddEdit");

        }
        #endregion

        public IActionResult City_Add(int? id)
        {
            FillStateDropDownMenu();
            FillCountryDropDownMenu();
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
                SqlCommand dbCommand = new SqlCommand("PR_LOC_CITY_SelectByPK", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@CityID", id);
                SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                dt.Load(sqlDataReader);
                CityModel model = new CityModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.CityName = dr["CityName"].ToString();
                    model.CityCode = Convert.ToInt32(dr["CityCode"]);
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                }
                return View(model);
            }

        }


        #region Insert
        [HttpPost]
        public IActionResult AddCity(CityModel modelCityModel)
        {
            if (modelCityModel.CityID == 0)
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_CITY_INSERT";
                objCmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelCityModel.CityName;
                objCmd.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelCityModel.CityCode;
                objCmd.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelCityModel.CountryID;
                objCmd.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelCityModel.StateID;
                objCmd.ExecuteNonQuery();
                connection.Close();
                TempData["CityInsertMsg"] = "Record Inserted Successfully";
                return RedirectToAction("City_Add");
            }
            else
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                DataTable dt = new DataTable();
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_LOC_CITY_UPDATE", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                //SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                //dt.Load(sqlDataReader);
                CityModel model = new CityModel();
                dbCommand.Parameters.AddWithValue("@CityID", SqlDbType.Int).Value = modelCityModel.CityID;
                dbCommand.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelCityModel.CityName;
                dbCommand.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelCityModel.CityCode;

                dbCommand.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelCityModel.CountryID;
                dbCommand.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelCityModel.StateID;

                dbCommand.ExecuteNonQuery();
                TempData["CityInsertMsg"] = "Record Updated Successfully";

                return RedirectToAction("City_Add");

            }
        }
        #endregion

        public void FillCountryDropDownMenu()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";

            List<LOC_CountryDropDownModel> lOC_CountryDropDowns = new List<LOC_CountryDropDownModel>();


            SqlConnection sqlConnection = new SqlConnection(connectionstr);


            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.CommandText = "CountryDropDownList";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();


            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_CountryDropDownModel lOC_CountryDropDownModel = new LOC_CountryDropDownModel();

                    {
                        lOC_CountryDropDownModel.CountryID = Convert.ToInt32(sqlDataReader["CountryID"]);
                        lOC_CountryDropDownModel.CountryName = sqlDataReader["CountryName"].ToString();
                    };
                    lOC_CountryDropDowns.Add(lOC_CountryDropDownModel);

                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.CountryList = lOC_CountryDropDowns;



        }

        public void FillStateDropDownMenu()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";

            List<LOC_StateDropDownModel> lOC_StateDropDowns = new List<LOC_StateDropDownModel>();


            SqlConnection sqlConnection = new SqlConnection(connectionstr);


            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.CommandText = "StateDropDownList";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();


            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_StateDropDownModel lOC_StateDropDownModel = new LOC_StateDropDownModel();

                    {
                        lOC_StateDropDownModel.StateID = Convert.ToInt32(sqlDataReader["StateID"]);
                        lOC_StateDropDownModel.StateName = sqlDataReader["StateName"].ToString();
                    };
                    lOC_StateDropDowns.Add(lOC_StateDropDownModel);

                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.StateList = lOC_StateDropDowns;



        }

    }
}
