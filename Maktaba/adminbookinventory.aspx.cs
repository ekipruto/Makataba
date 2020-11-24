using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maktaba
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        string Strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        static string GlobalFilepath;
        static int GlobalActualStock, GlobalCurrentStock, GlobaIssuedBooks;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillAuthorPublisherValues();
            }
            GridView1.DataBind();
        }
        //Go Button On Click Event 
        protected void Button4_Click(object sender, EventArgs e)
        {
            GetBookById();
        }
        //Add New book Button on Click
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CheckIfBookExist())
            {
                Response.Write("<script>alert('Book with this Id already exist,Try another book id');</script>");
            }
            else
            {
                AddNewBook();
            }

        }
        //Update Button On Click Event
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (CheckIfBookExist())
            {
                updateBookByID();
            }
            else
            {
                Response.Write("<script>alert('Book with this ID does not exist');</script>");
            }

        }
        //Delete Button On Click Event
        protected void Button2_Click(object sender, EventArgs e)
        {
            deleteBookByID();
        }
        //User Defined Values

        void deleteBookByID()
        {
            if (CheckIfBookExist())
            {
                try
                {
                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("DELETE Book WHERE BookId='" + TxtBookId.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Write("<script>alert('Book Deleted Successfully');</script>");

                    GridView1.DataBind();

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }

            }
            else
            {
                Response.Write("<script>alert('Invalid Book ID');</script>");
            }
        }

        void updateBookByID()
        {

            if (CheckIfBookExist())
            {
                try
                {

                    int ActualStock = Convert.ToInt32(TxtActualStock.Text.Trim());
                    int CurrentStock = Convert.ToInt32(TxtCurrentStock.Text.Trim());

                    if (GlobalActualStock == ActualStock)
                    {

                    }
                    else
                    {
                        if (ActualStock < GlobalActualStock)
                        {
                            Response.Write("<script>alert('Actual Stock value cannot be less than the Issued books');</script>");
                            return;


                        }
                        else
                        {
                            CurrentStock = ActualStock - GlobaIssuedBooks;
                            TxtCurrentStock.Text = "" + CurrentStock;
                        }
                    }

                    string genres = "";
                    foreach (int i in ListBGenre.GetSelectedIndices())
                    {
                        genres = genres + ListBGenre.Items[i] + ",";
                    }
                    genres = genres.Remove(genres.Length - 1);

                    string filepath = "~/BookInventory/books1";
                    string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    if (filename == "" || filename == null)
                    {
                        filepath = GlobalFilepath;

                    }
                    else
                    {
                        FileUpload1.SaveAs(Server.MapPath("BookInventory/" + filename));
                        filepath = "~/BookInventory/" + filename;
                    }

                    SqlConnection con = new SqlConnection(Strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE Book set BookName=@BookName, Genre=@Genre, AuthorName=@AuthorName, " +
                        "PublisherName=@PublisherName, PublishDate=@PublishDate, Language=@Language, Edition=@Edition, " +
                        "BookCost=@BookCost, NoofPages=@NoofPages, BookDescription=@BookDescription, ActualStock=@ActualStock, " +
                        "CurrentStock=@CurrentStock, BookImgLink=@BookImgLink where BookId='" + TxtBookId.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@BookName", TxtBookName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Genre", genres);
                    cmd.Parameters.AddWithValue("@AuthorName", DPLAuthorName.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@PublisherName", DPLLanguage.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@PublishDate", TxtPublishDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@Language", DPLLanguage.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@Edition", TxtEdition.Text.Trim());
                    cmd.Parameters.AddWithValue("@BookCost", TxtBookCost.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoofPages", TxtPages.Text.Trim());
                    cmd.Parameters.AddWithValue("@BookDescription", TxtBookDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@ActualStock", TxtActualStock.ToString());
                    cmd.Parameters.AddWithValue("@CurrentStock", TxtCurrentStock.ToString());
                    cmd.Parameters.AddWithValue("@BookImgLink", filepath);


                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    Response.Write("<script>alert('Book Updated Successfully');</script>");


                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid Book ID');</script>");
            }
        }

        void GetBookById()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from  Book where BookId='"+TxtBookId.Text.Trim()+"';", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if(dt.Rows.Count>=1)
                {
                    TxtBookName.Text = dt.Rows[0]["BookName"].ToString();
                    TxtPublishDate.Text = dt.Rows[0]["PublishDate"].ToString();
                    TxtEdition.Text = dt.Rows[0]["Edition"].ToString();
                    TxtBookCost.Text = dt.Rows[0]["BookCost"].ToString().Trim();
                    TxtPages.Text = dt.Rows[0]["NoofPages"].ToString().Trim();
                    TxtActualStock.Text = dt.Rows[0]["ActualStock"].ToString().Trim();
                    TxtCurrentStock.Text = dt.Rows[0]["CurrentStock"].ToString().Trim();
                    TxtBookDescription.Text = dt.Rows[0]["BookDesc"].ToString();
                    TxtIssuedbooks.Text = "" + (Convert.ToInt32(dt.Rows[0]["ActualStock"].ToString()) - Convert.ToInt32(dt.Rows[0]["CurrentStock"].ToString()));

                    DPLLanguage.SelectedValue = dt.Rows[0]["Language"].ToString().Trim();
                    DPLPublisherName.SelectedValue = dt.Rows[0]["PublisherName"].ToString().Trim();
                    DPLAuthorName.SelectedValue = dt.Rows[0]["AuthorName"].ToString().Trim();

                    ListBGenre.ClearSelection();

                    string[] genre = dt.Rows[0]["genre"].ToString().Trim().Split(',');
                    for (int i = 0; i < genre.Length; i++)
                    {
                        for (int j = 0; j < ListBGenre.Items.Count; j++)
                        {
                            if (ListBGenre.Items[j].ToString() == genre[i])
                            {
                                ListBGenre.Items[j].Selected = true;

                            }
                        }
                    }

                    GlobalActualStock = Convert.ToInt32(dt.Rows[0]["ActualStock"].ToString().Trim());
                    GlobalCurrentStock = Convert.ToInt32(dt.Rows[0]["CurrentStock"].ToString().Trim());
                    GlobaIssuedBooks = GlobalActualStock - GlobalCurrentStock;
                    GlobalFilepath = dt.Rows[0]["BookImgLink"].ToString();
                }
                else
                {
                    Response.Write("<script>alert('Invalid book Id');</script>");
                }

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        void FillAuthorPublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT AuthorName from  Author;", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                DPLAuthorName.DataSource = dt;
                DPLAuthorName.DataValueField = "AuthorName";
                DPLAuthorName.DataBind();

                cmd = new SqlCommand("SELECT PublisherName from  Publisher;", con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);

                DPLPublisherName.DataSource = dt;
                DPLPublisherName.DataValueField = "PublisherName";
                DPLPublisherName.DataBind();

            }

            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        //Checking if Book Already Exist
        bool CheckIfBookExist()
        {
            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from Book where BookId='" + TxtBookId.Text.Trim() + "' or BookName='" + TxtBookName.Text.Trim() + "';", con);
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
        void AddNewBook()
        {
            //Multiselectbox Genres
            string genres = "";
            foreach(int i in ListBGenre.GetSelectedIndices())
            {
                genres=genres+ListBGenre.Items[i]+";";
            }
            //genres=Cow,Mbuzi,Kondoo,
            //To remove the last comma use this code below
            genres = genres.Remove(genres.Length - 1);

            //File Upload
            string filepath = "~/BookInventory/books1.png";
            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.SaveAs(Server.MapPath("BookInventory/" + filename));
            filepath = "~/BookInventory/" + filename;

            try
            {
                SqlConnection con = new SqlConnection(Strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Insert into Book (BookId,BookName,Genre,AuthoName,PublisherName,PublishDate," +
                    "Language,Edition,BookCost,NoofPages,BookDesc,ActualStock,CurrentStock,BookImgLink) values(@BookId,@BookName," +
                    "@Genre,@AuthorName,@PublisherName,@PublishDate,@Language,@Edition,@BookCost,@NoofPages,@BookDesc,@ActualStock," +
                    "@CurrentStock,@BookImgLink)", con);
                cmd.Parameters.AddWithValue("@BookId",TxtBookId.Text.Trim());
                cmd.Parameters.AddWithValue("@BookName", TxtBookName.Text.Trim());
                cmd.Parameters.AddWithValue("@Genre", genres);
                cmd.Parameters.AddWithValue("@AuthorName", DPLAuthorName.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@PublisherName", DPLPublisherName.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@PublishDate", TxtPublishDate.Text.Trim());
                cmd.Parameters.AddWithValue("@Language", DPLLanguage.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Edition", TxtEdition.Text.Trim());
                cmd.Parameters.AddWithValue("@BookCost", TxtBookCost.Text.Trim());
                cmd.Parameters.AddWithValue("@NoofPages", TxtPages.Text.Trim());
                cmd.Parameters.AddWithValue("@BookDesc", TxtBookDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@ActualStock", TxtActualStock.Text.Trim());
                cmd.Parameters.AddWithValue("@CurrentStock", TxtActualStock.Text.Trim());
                cmd.Parameters.AddWithValue("@BookImgLink", filepath);

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book added successfully.');</script>");
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }        
    }
}