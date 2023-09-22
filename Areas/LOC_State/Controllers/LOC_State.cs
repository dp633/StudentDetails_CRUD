using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using StudentDetails.Areas.LOC_Country.Models;
using StudentDetails.Areas.LOC_State.Models;

namespace StudentDetails.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/{Controller}/{Action}")]
    public class LOC_State : Controller
    {
        #region SelectAll

        public IActionResult StateAddEdit()
        {
            var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
            SqlDatabase sqlDB = new SqlDatabase(connectionstr);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_LOC_STATE_SELECTALL");
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
            SqlCommand dbCommand = new SqlCommand("PR_LOC_STATE_DELETE", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@StateID", id);

            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("StateAddEdit");

        }
        #endregion


        #region Update
        public IActionResult State_Add(int? id) {


            FillDropDownMenu();
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
                SqlCommand dbCommand = new SqlCommand("PR_LOC_STATE_SelectByPK", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@StateId", id);
                SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                dt.Load(sqlDataReader);
                StateModel model = new StateModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                    model.StateName = dr["StateName"].ToString();
                    model.StateCode = dr["StateCode"].ToString();
                    model.CountryID = Convert.ToInt32(dr["CountryID"]);
                }
                return View(model);
            }
        }
        #endregion


        #region Insert
        public IActionResult AddState(StateModel modelStateModel)
        {
            if (modelStateModel.StateID == 0)
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_STATE_INSERT";

                objCmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelStateModel.StateName;
                objCmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelStateModel.StateCode;
                objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelStateModel.CountryID;

                objCmd.ExecuteNonQuery();
                connection.Close();
                TempData["CountryInsertMsg"] = "Record Inserted Successfully";
                return RedirectToAction("State_Add");
            }
            else
            {
                var connectionstr = "Data Source=DESKTOP-DVN2N38\\SQLEXPRESS;Initial Catalog=22010101614;Integrated Security=True";
                DataTable dt = new DataTable();
                SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
                SqlConnection connection = new SqlConnection(connectionstr);

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_LOC_STATE_UPDATE", connection);

                dbCommand.CommandType = CommandType.StoredProcedure;
                //SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                //dt.Load(sqlDataReader);
                CountryModel model = new CountryModel();
                dbCommand.Parameters.AddWithValue("@StateID", SqlDbType.Int).Value = modelStateModel.StateID;
                dbCommand.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelStateModel.StateName;
                dbCommand.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelStateModel.StateCode;
                dbCommand.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelStateModel.CountryID;

                dbCommand.ExecuteNonQuery();
                TempData["CountryInsertMsg"] = "Record Updated Successfully";

                return RedirectToAction("State_Add");

            }

        }
        #endregion


        #region DropDownCountry
        public void FillDropDownMenu()
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
        #endregion

    }
}
