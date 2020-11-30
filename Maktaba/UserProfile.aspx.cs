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
    public partial class WebForm4 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"].ToString() == "" || Session["username"] == null)
                {
                    Response.Write("<script>alert('Session Expired Login Again');</script>");
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    getUserBookData();

                    if (!Page.IsPostBack)
                    {
                        getUserPersonalDetails();
                    }

                }
            }
            catch (Exception ex)
            {

                Response.Write("<script>alert('Session Expired Login Again');</script>");
                Response.Redirect("Login.aspx");
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Check your condition here //Due Date for book returning//cell five is the positiontable column for duedate
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
                    DateTime today = DateTime.Today;
                    if (today > dt)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        //Update Button
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (Session["username"].ToString() == "" || Session["username"] == null)
            {
                Response.Write("<script>alert('Session Expired Login Again');</script>");
                Response.Redirect("Login.aspx");
            }
            else
            {
                updateUserPersonalDetails();

            }
        }
        //Go/Select Button
        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        // user defined function


        void updateUserPersonalDetails()
        {
            string password = "";
            if (TxtNewPassword.Text.Trim() == "")
            {
                password = TxtOldPassword.Text.Trim();
            }
            else
            {
                password = TxtNewPassword.Text.Trim();
            }
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand("update Members set FullName=@FullName, DOB=@DOB, Contacts=@Contacts, Email=@Email, State=@State," +
                    " City=@City, Pincode=@Pincode, FullAddress=@FullAddress, Password=@Password, AccStatus=@AccStatus " +
                    "WHERE MemberId='" + Session["username"].ToString().Trim() + "'", con);

                cmd.Parameters.AddWithValue("@FullName", TxtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@DOB", TxtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@Contacts", TxtContacts.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", TxtEmailId.Text.Trim());
                cmd.Parameters.AddWithValue("@State", DPLState.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@City", TxtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@Pincode", TxtPincode.Text.Trim());
                cmd.Parameters.AddWithValue("@FullAddress", TxtFAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@AccStatus", "pending");

                int result = cmd.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {

                    Response.Write("<script>alert('Your Details Updated Successfully');</script>");
                    getUserPersonalDetails();
                    getUserBookData();
                }
                else
                {
                    Response.Write("<script>alert('Invaid entry');</script>");
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }


        void getUserPersonalDetails()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from Members where MemberId='" + Session["username"].ToString() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                TxtFullName.Text = dt.Rows[0]["FullName"].ToString();
                TxtDOB.Text = dt.Rows[0]["DOB"].ToString();
                TxtContacts.Text = dt.Rows[0]["Contacts"].ToString();
                TxtEmailId.Text = dt.Rows[0]["Email"].ToString();
                DPLState.SelectedValue = dt.Rows[0]["State"].ToString().Trim();
                TxtCity.Text = dt.Rows[0]["City"].ToString();
                TxtPincode.Text = dt.Rows[0]["Pincode"].ToString();
                TxtFAddress.Text = dt.Rows[0]["FullAddress"].ToString();
                TxtUserId.Text = dt.Rows[0]["MemeberId"].ToString();
                TxtOldPassword.Text = dt.Rows[0]["Password"].ToString();

                Label1.Text = dt.Rows[0]["AccStatus"].ToString().Trim();

                if (dt.Rows[0]["AccStatus"].ToString().Trim() == "active")
                {
                    Label1.Attributes.Add("class", "badge badge-pill badge-success");
                }
                else if (dt.Rows[0]["AccStatus"].ToString().Trim() == "pending")
                {
                    Label1.Attributes.Add("class", "badge badge-pill badge-warning");
                }
                else if (dt.Rows[0]["AccStatus"].ToString().Trim() == "Inactive")
                {
                    Label1.Attributes.Add("class", "badge badge-pill badge-danger");
                }
                else
                {
                    Label1.Attributes.Add("class", "badge badge-pill badge-info");
                }


            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");

            }
        }

        void getUserBookData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from BookIssue where MemberId='" + Session["username"].ToString() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();


            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");

            }
        }
    }
}