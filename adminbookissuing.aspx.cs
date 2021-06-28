    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    namespace WebApplication3
    {
        public partial class adminbookissuing : System.Web.UI.Page
        {
            string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            protected void Page_Load(object sender, EventArgs e)
            {
                GridView1.DataBind();
            }

            // issue book
            protected void Button2_Click(object sender, EventArgs e)
            {
                if (checkIfBookExist() && checkIfMemberExist())
                {

                    if (checkIfIssueEntryExist())
                    {
                        Response.Write("<script>alert('This Member already has this book');</script>");
                    }
                    else
                    {
                        issueBook();
                    }

                }
                else
                {
                    Response.Write("<script>alert('Wrong Book ID or Member ID');</script>");
                }
            }
            // return book
            protected void Button4_Click(object sender, EventArgs e)
            {
                if (checkIfBookExist() && checkIfMemberExist())
                {

                    if (checkIfIssueEntryExist())
                    {
                        returnBook();
                    }
                    else
                    {
                        Response.Write("<script>alert('This Entry does not exist');</script>");
                    }

                }
                else
                {
                    Response.Write("<script>alert('Wrong Book ID or Member ID');</script>");
                }
            }

            // go button click event
            protected void Button1_Click(object sender, EventArgs e)
            {
                getNames();
            }




            // user defined function

            void returnBook()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }


                    SqlCommand cmd = new SqlCommand("Delete from book_issue_table WHERE book_id='" + TextBox1.Text.Trim() + "' AND member_id='" + TextBox2.Text.Trim() + "'", con);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {

                        cmd = new SqlCommand("update book_master_tbl set current_stock = current_stock+1 WHERE book_id='" + TextBox1.Text.Trim() + "'", con);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        Response.Write("<script>alert('Book Returned Successfully');</script>");
                        GridView1.DataBind();

                        con.Close();

                    }
                    else
                    {
                        Response.Write("<script>alert('Error - Invalid details');</script>");
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }

            void issueBook()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("INSERT INTO book_issue_table(member_id,member_name,book_id,book_name,issue_date,due_date) values(@member_id,@member_name,@book_id,@book_name,@issue_date,@due_date)", con);

                    cmd.Parameters.AddWithValue("@member_id", TextBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@member_name", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_id", TextBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_name", TextBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@issue_date", TextBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@due_date", TextBox6.Text.Trim());

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("update  book_master_tbl set current_stock = current_stock-1 WHERE book_id='" + TextBox1.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();

                    con.Close();
                    Response.Write("<script>alert('Book Issued Successfully');</script>");

                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }

            bool checkIfBookExist()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("select * from book_master_tbl WHERE book_id='" + TextBox1.Text.Trim() + "' AND current_stock >0", con);
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
                    return false;
                }

            }

            bool checkIfMemberExist()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("select full_name from member_master_table WHERE member_id='" + TextBox2.Text.Trim() + "'", con);
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
                    return false;
                }

            }

            bool checkIfIssueEntryExist()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("select * from book_issue_table WHERE member_id='" + TextBox2.Text.Trim() + "' AND book_id='" + TextBox1.Text.Trim() + "'", con);
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
                    return false;
                }

            }



            void getNames()
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand("select book_name from book_master_tbl WHERE book_id='" + TextBox1.Text.Trim() + "'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count >= 1)
                    {
                        TextBox4.Text = dt.Rows[0]["book_name"].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('Wrong Book ID');</script>");
                    }

                    cmd = new SqlCommand("select full_name from member_master_table WHERE member_id='" + TextBox2.Text.Trim() + "'", con);
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count >= 1)
                    {
                        TextBox3.Text = dt.Rows[0]["full_name"].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('Wrong User ID');</script>");
                    }


                }
                catch (Exception ex)
                {

                }
            }

            protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //Check your condition here
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
        }
    }