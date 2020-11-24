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
    public partial class adminauthormanagement : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
        // Go Button Click
        protected void Button1_Click(object sender, EventArgs e)
        {
            CheckAuthorById();
        }
        //Add Button Click
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (CheckIfAuthorExist())
            {
                Response.Write("<script>alert('Author with this Id already exist');</script>");

            }
            else
            {
                AddNewAuthor();
            }
        }
        //Update Button Click
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (CheckIfAuthorExist())
            {
                UpdateAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author with this Id doesn't exist');</script>");
            }
        }
        //Delete Button Click
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (CheckIfAuthorExist())
            {
                DeleteAuthor();
            }
            else
            {
                Response.Write("<script>alert('Author with this Id doesn't exist');</script>");
            }
        }
        //User Defined Methods


            //Add New Author
            void AddNewAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO Author(AuthorId,AuthorName) " +
                    "values(@AuthorId,@AuthorName)", con);
                cmd.Parameters.AddWithValue("@AuthorId", TxtAuthorId.Text.Trim());
                cmd.Parameters.AddWithValue("@AuthorName", TxtAuthorName.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Author Added Successful');</script>");
                ClearFields();
                GridView1.DataBind();

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        //Update Author
        void UpdateAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Update Author Set AuthorName=@AuthorName where AuthorId='"+TxtAuthorId.Text.Trim()+"'", con);
                cmd.Parameters.AddWithValue("@AuthorName", TxtAuthorName.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Author Updated Successful');</script>");
                ClearFields();
                GridView1.DataBind();

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        //Delete Author
        void DeleteAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Delete Author where AuthorId='" + TxtAuthorId.Text.Trim() + "'", con);
                cmd.Parameters.AddWithValue("@AuthorName", TxtAuthorName.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Author Deleted Successful');</script>");
                ClearFields();
                GridView1.DataBind();

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void ClearFields()
        {
            TxtAuthorId.Text = "";
            TxtAuthorName.Text = "";
        }

        //Check If Author Exist
        bool CheckIfAuthorExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM Author where AuthorId='" + TxtAuthorId.Text.Trim() + "';", con);
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

        //Check Author Exist By Id //Go Button
        void CheckAuthorById()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM Author where AuthorId='" + TxtAuthorId.Text.Trim() + "';", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TxtAuthorName.Text = dt.Rows[0][1].ToString();
                }
                else
                {

                }

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('Invalid Author Id');</script>");
            }


        }
    }
}