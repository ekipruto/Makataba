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
    public partial class WebForm5 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
        //Add Publisher Button Click
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists())
            {
                Response.Write("<script>alert('Publisher Already Exist with this ID.');</script>");
            }
            else
            {
                addNewPublisher();
            }
        }
         // Update Publisher
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists()) {
                updatePublisherByID();
            } 
            else 
            {
            Response.Write("<script>alert('Publisher with this ID does not exist');</script>");
            }
        }
        //Delete Publisher
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists())
            {
                deletePublisherByID();
            }
            else
            {
                Response.Write("<script>alert('Publisher with this ID does not exist');</script>");
            }
        }
        //Go Button
        protected void Button1_Click(object sender, EventArgs e)
        {
            getPublisherByID();
        }


        // user defined functions

            void getPublisherByID()
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("SELECT * from Publisher where PublisherId='" + TxtPublisherId.Text.Trim() + "';", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        TxtPublisherName.Text = dt.Rows[0][1].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('Publisher with this ID does not exist.');</script>");
                    }


                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");

                }
            }

            bool checkIfPublisherExists()
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("SELECT * from Publisher where PublisherId='" + TxtPublisherId.Text.Trim() + "';", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

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

            void addNewPublisher()
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("INSERT INTO Publisher(PublisherId,PublisherName) values(@publisherid,@publishername)", con);

                    cmd.Parameters.AddWithValue("@publisherid", TxtPublisherId.Text.Trim());
                    cmd.Parameters.AddWithValue("@publishername", TxtPublisherName.Text.Trim());


                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Publisher added successfully.');</script>");
                    GridView1.DataBind();

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }

            void updatePublisherByID()
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }


                    SqlCommand cmd = new SqlCommand("update Publisher set PublisherName=@publishername WHERE publisherid='" + TxtPublisherId.Text.Trim() + "'", con);
                    cmd.Parameters.AddWithValue("@publishername", TxtPublisherName.Text.Trim());
                    int result = cmd.ExecuteNonQuery();
                    con.Close();
                    if (result > 0)
                    {

                        Response.Write("<script>alert('Publisher Updated Successfully');</script>");
                        GridView1.DataBind();
                    }
                    else
                    {
                        Response.Write("<script>alert('Publisher ID does not Exist');</script>");
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }

            void deletePublisherByID()
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }


                    SqlCommand cmd = new SqlCommand("Delete Publisher WHERE publisherid='" + TxtPublisherId.Text.Trim() + "'", con);
                    int result = cmd.ExecuteNonQuery();
                    con.Close();
                    if (result > 0)
                    {

                        Response.Write("<script>alert('Publisher Deleted Successfully');</script>");
                        GridView1.DataBind();
                    }
                    else
                    {
                        Response.Write("<script>alert('Publisher ID does not Exist');</script>");
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }


    }
}
