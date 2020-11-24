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
    public partial class WebForm3 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //On click SignUp Page
        protected void BtnSignup_Click(object sender, EventArgs e)
        {
            if (CheckIfMemberExist())
            {
                Response.Write("<script>alert('Ooops! Member with this ID already Exist,Kindly try another ID');</script>");
            }
            else
            {
                SignUpNewMember();
            }
        }

        //User Defined Methods
        bool CheckIfMemberExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM Members where MemberId='"+TxtMemberId.Text.Trim()+"';", con);
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
        void SignUpNewMember()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO Members(FullName,DOB,Contacts,Email,State,City,Pincode,FullAddress,MemberId,Password,AccStatus) " +
                    "values(@FullName,@DOB,@Contacts,@Email,@State,@City,@Pincode,@FullAddress,@MemberId,@Password,@AccStatus)", con);
                cmd.Parameters.AddWithValue("@FullName", TxtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@DOB", TxtDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@Contacts", TxtContact.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", TxtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@State", DPLState.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@City", TxtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@Pincode", TxtPincode.Text.Trim());
                cmd.Parameters.AddWithValue("@FullAddress", TxtFullAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@MemberId", TxtMemberId.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", TxtPassword1.Text.Trim());
                cmd.Parameters.AddWithValue("@AccStatus", "Pending");

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Successful Signup.Proceed to Login');</script>");

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('"+ex.Message+"');</script>");
            }
        }
    }
}