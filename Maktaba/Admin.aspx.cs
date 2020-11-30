using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maktaba
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM ADMIN WHERE username='" + TxtAdminId.Text.Trim() + "' AND Password='" + TxtAdminPassword.Text.Trim() + "'", con);
                SqlDataReader Dr = cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        //Response.Write("<script>alert('" + Dr.GetValue(0).ToString() + "');</script>");
                        Session["username"] = Dr.GetValue(0).ToString();
                        Session["Name"] = Dr.GetValue(2).ToString();
                        Session["role"] = "Admin";
                        //Session["Status"] = Dr.GetValue(10).ToString();

                    }
                    Response.Redirect("Admin.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Invalid Credentials');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}