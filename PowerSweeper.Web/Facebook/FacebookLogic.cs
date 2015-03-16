using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Globalization;
using PowerSweeper.Web.Facebook;
using PowerSweeper.DataTypes;

namespace PowerSweeper.Web.Facebook
{
    public class FacebookLogic
    {
        protected FacebookAPI Api = new FacebookAPI();
        public string authenticationUrl = "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}";
        public string authorizationUrl = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";
        public string pictureUrl = "https://graph.facebook.com/me/picture?type={0}&access_token={1}";

        public string AppId
        {
            get { return "359194397437105"; }
        }

        public string AppSecret
        {
            get { return "dbd092e1d1772c1434da4a5129b6c851"; }
        }

        public string RedirectUrl
        {
            get
            {
                //return string.Format("http://localhost:2879/FBLogin.aspx");
                return string.Format("http://www.walidaly.net/PowerSweeper/FBLogin.aspx");
            }
        }

        public User GetUserInfo(string code)
        {
            if (code == null)
            {
                authenticationUrl = string.Format(authenticationUrl, AppId, RedirectUrl);

                HttpContext.Current.Response.Redirect(authenticationUrl);

                return null;
            }
            else
            {
                //Get AccessToken
                Api.AccessToken = GetAccessToken(code);

                //Get Logged User Information
                JSONObject jsonLoggedUser = Api.Get("/me");

                //Return instance of FBUser
                User fbUser = new User();
                fbUser.FBUserId = jsonLoggedUser.Dictionary["id"].String;
                if (jsonLoggedUser.Dictionary.Keys.FirstOrDefault(k => k == "birthday") != null)
                {
                    fbUser.Birthday = DateTime.Parse(jsonLoggedUser.Dictionary["birthday"].String, CultureInfo.InvariantCulture);
                }
                if (jsonLoggedUser.Dictionary.Keys.FirstOrDefault(k => k == "gender") != null)
                {
                    fbUser.Gender = jsonLoggedUser.Dictionary["gender"].String;
                }
                if (jsonLoggedUser.Dictionary.Keys.FirstOrDefault(k => k == "name") != null)
                {
                    fbUser.Name = jsonLoggedUser.Dictionary["name"].String;
                }

                fbUser.SmallPhoto = Requests.GetResponseUri(string.Format(pictureUrl, "small", Api.AccessToken));
                return fbUser;
            }
        }

        private string GetAccessToken(string code)
        {
            authorizationUrl = string.Format(authorizationUrl, new object[] { AppId, RedirectUrl, AppSecret, code });

            string webResponse = Requests.GetResponse(authorizationUrl);

            if (!string.IsNullOrEmpty(webResponse))
            {
                return Regex.Split(webResponse, @"(=)|(&)")[2];
            }
            return string.Empty;
        }

    }
}
