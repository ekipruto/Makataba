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
    public partial class WebForm7 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
        //Go Button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            getMemberByID();
        }
        //Active Button
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            UpdateMemberStatusById("Active");
        }
        //Pending Button
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            UpdateMemberStatusById("Pending");
        }
        //Inactive Button
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            UpdateMemberStatusById("Inactive");
        }
        //Delete Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            DeleteMemberById();
        }

        //User Defined Functions

        //Inline with Go Button
        void getMemberByID()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                SqlCommand cmd = new SqlCommand("select * from Members where MemberId='" + TxtMemberId.Text.Trim() + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TxtName.Text = dr.GetValue(0).ToString();
                        TxtAccStatus.Text = dr.GetValue(10).ToString();
                        TxtDOB.Text = dr.GetValue(1).ToString();
                        TxtContacts.Text = dr.GetValue(2).ToString();
                        TxtEmail.Text = dr.GetValue(3).ToString();
                        TxtState.Text = dr.GetValue(4).ToString();
                        TxtCity.Text = dr.GetValue(5).ToString();
                        TxtPincode.Text = dr.GetValue(6).ToString();
                        TxtPostalAddress.Text = dr.GetValue(7).ToString();

                    }

                }
                else
                {
                    Response.Write("<script>alert('Invalid credentials');</script>");
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        void UpdateMemberStatusById(string Status)
        {
            if (CheckIfMemberExist())
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();

                    }
                    SqlCommand cmd = new SqlCommand("Update Members set AccStatus='" + Status + "' where MemberId='" + TxtMemberId.Text.Trim() + "'", con);
                    SqlDataReader dr = cmd.ExecuteReader();

                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    ClearFields();

                    Response.Write("<script>alert('Member Status Updated Successfully');</script>");

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid Member Id');</script>");
            }

        }
        //Delete Member
        void DeleteMemberById()
        {
            if (CheckIfMemberExist())
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("Delete Members where MemberId='" + TxtMemberId.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Member Deleted Successful');</script>");
                    ClearFields();
                    GridView1.DataBind();

                }

                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid MemberId');</script>");
            }
            
        }
        //Check If Member Exist
        bool CheckIfMemberExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM Members where MemberId='" + TxtMemberId.Text.Trim() + "';", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                return false;
            }

        }

        void ClearFields()
        {
            TxtMemberId.Text = "";
            TxtName.Text = "";
            TxtDOB.Text = "";
            TxtContacts.Text = "";
            TxtCity.Text = "";
            TxtEmail.Text = "";
            TxtAccStatus.Text = "";
            TxtPincode.Text = "";
            TxtPostalAddress.Text = "";
            TxtState.Text = "";
        }
    }
}