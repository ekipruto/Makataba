using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maktaba
{   
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["role"].Equals(""))
                {
                    LkBtnAdminLogin.Visible = true;
                    LkBtnAuthorMgt.Visible = false;
                    LkBtnPublisherMgt.Visible = false;
                    LkBtnMemberMgt.Visible = false;
                    LkBtnSignout.Visible = false;
                    LkBtnUserLogin.Visible = true;
                    LkBtnSignup.Visible = true;
                    LkBtnHeloUser.Visible = false;
                }
                else if (Session["role"].Equals("User"))
                {
                    LkBtnAdminLogin.Visible = true;
                    LkBtnAuthorMgt.Visible = false;
                    LkBtnPublisherMgt.Visible = false;
                    LkBtnMemberMgt.Visible = false;
                    LkBtnSignout.Visible = true;
                    LkBtnUserLogin.Visible = true;
                    LkBtnSignup.Visible = true;
                    LkBtnHeloUser.Visible = true;
                    LkBtnHeloUser.Text = "Heloo " + Session["username"].ToString();
                }
                else
                {
                    LkBtnAdminLogin.Visible = false;
                    LkBtnAuthorMgt.Visible = true;
                    LkBtnPublisherMgt.Visible = true;
                    LkBtnMemberMgt.Visible = true;
                    LkBtnSignout.Visible = true;
                    LkBtnHeloUser.Visible = true;
                    LkBtnHeloUser.Text = "Heloo Admin ";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

        }

        protected void LkBtnAdminLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }

        protected void LkBtnAuthorMgt_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminauthormanagement.aspx");
        }

        protected void LkBtnPublisherMgt_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminpublishermanagement.aspx");
        }

        protected void LkBtnBookinventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookinventory.aspx");
        }

        protected void LkBtnBookIssuing_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminbookissuing.aspx");
        }

        protected void LkBtnMemberMgt_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminmembermanagement.aspx");
        }

        protected void LkBtnUserLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void LkBtnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Signup.aspx");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {

        }

        protected void LkBtnSignout_Click(object sender, EventArgs e)
        {
            Session["username"] = "";
            Session["Fullname"] = "";
            Session["role"] = "";
            Session["Status"] = "";

            LkBtnAdminLogin.Visible = true;
            LkBtnAuthorMgt.Visible = false;
            LkBtnPublisherMgt.Visible = false;
            LkBtnMemberMgt.Visible = false;
            LkBtnSignout.Visible = false;
            LkBtnUserLogin.Visible = true;
            LkBtnSignup.Visible = true;
            LkBtnHeloUser.Visible = false;

            Response.Redirect("Home.aspx");

        }

        protected void LkBtnViewbooks_Click(object sender, EventArgs e)
        {

        }

        protected void LkBtnHeloUser_Click(object sender, EventArgs e)
        {

        }
    }
}