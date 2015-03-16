using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerSweeper.Web.Facebook;
using PowerSweeper.DataTypes;
using PowerSweeper.DataAccessLayer;

namespace PowerSweeper.Web
{
    public partial class FBLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FacebookLogic fbLogic = new FacebookLogic();
            if (Request["code"] == null)
            {
                fbLogic.GetUserInfo(null);
            }
            else
            {
                try
                {
                    User currentUser = fbLogic.GetUserInfo(Request["code"].ToString());

                    UsersManager usersManager = new UsersManager();
                    User existingUser = usersManager.GetUserByFbUserId(currentUser.FBUserId);
                    if (existingUser != null)
                    {
                        existingUser.LoginCounter++;
                        usersManager.UpdateUser(existingUser);
                    }
                    else
                    {
                        usersManager.AddUser(currentUser);
                    }

                    Session["CurrentUser"] = currentUser;
                }
                catch { }

                Response.Redirect("Index.aspx");
            }
        }
    }
}