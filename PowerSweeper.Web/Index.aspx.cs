using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using PowerSweeper.DataTypes;

namespace PowerSweeper.Web
{
    public partial class Index : System.Web.UI.Page
    {
        public string InitParams { get; set; }

        public User CurrentUser
        {
            get { return (User)Session["CurrentUser"]; }
            set { Session["CurrentUser"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);

            SaveSilverlightDeploymentSettings(ParamInitParams);
        }

        private void SaveSilverlightDeploymentSettings(Literal litSettings)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            StringBuilder SB = new StringBuilder();
            SB.Append("<param name=\"InitParams\" value=\"");
            int SettingCount = appSettings.Count;
            for (int Idex = 0; Idex < SettingCount; Idex++)
            {
                SB.Append(appSettings.GetKey(Idex));
                SB.Append("=");
                SB.Append(appSettings[Idex]);
                SB.Append(",");
            }
            if (this.CurrentUser != null)
            {
                SB.Append("UserName=" + this.CurrentUser.Name);
            }
            SB.Append(",IpAddress=" + HttpContext.Current.Request.UserHostAddress);
            SB.Append("\" />");
            litSettings.Text = SB.ToString();
        }

    }

}