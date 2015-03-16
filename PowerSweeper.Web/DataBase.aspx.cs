using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerSweeper.DataAccessLayer;

namespace PowerSweeper.Web
{
    public partial class DataBase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        protected void btnStopServer_Click(object sender, EventArgs e)
        {
            DataManager.StopServer();
        }

        protected void btnStopShowUsers_Click(object sender, EventArgs e)
        {
            LogRecordsManager logrecords = new LogRecordsManager();
            lblLogrecordsCount.Text = logrecords.GetAllLogRecords().Count().ToString();

            UsersManager usersManager = new UsersManager();
            grdUsers.DataSource = usersManager.GetAllUsers();
            grdUsers.DataBind();
        }
    }
}