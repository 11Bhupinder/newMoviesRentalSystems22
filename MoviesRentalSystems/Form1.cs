using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MoviesRentalSystems
{
    public partial class Form1 : Form
    {
        public SqlConnection conn = null;
        public  bool con,showData;
        String frstName = "", lstName = "", Title = "";
        String issDate = "", RetnDate = "";
        DateTime iDate, rDate;
        String CstId = "";

        public Form1()
        {
            InitializeComponent();
            
            string connect = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
            try {
                if(con ==false)
                {
                    conn = new SqlConnection(connect);             // To Make Connection to the server.
                    conn.Open();
                    con = true;
                }
                    
                
                                             // Check the Connection Availabality.
                SqlCommand cmd;
                showData = false;
                String sqlMov = "Select MovieID,Title,Genre,Rental_Cost,Year From Movies";
                String sqlRen = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
                String sqlCus = "Select CustID,FirstName,LastName, Address, Phone From Customer";
                cmd = new SqlCommand(sqlMov,conn);
                SqlDataAdapter adpMov = new SqlDataAdapter(cmd);

                DataTable dtGrd = new DataTable();                // Create data Table to store movies Record in Table.
                adpMov.Fill(dtGrd);
                moviesGrid.DataSource = dtGrd;

                cmd = new SqlCommand(sqlRen, conn);
                SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
                DataTable dtGrd1 = new DataTable();               // Create data Table to store RentalMovies Record in Table.
                adpRen.Fill(dtGrd1);
                RentalGrid.DataSource = dtGrd1;


                cmd = new SqlCommand(sqlCus, conn);
                SqlDataAdapter adpCus = new SqlDataAdapter(cmd);
                DataTable dtGrd2 = new DataTable();                 // Create data Table to store CustomerName Record in Table.
                adpCus.Fill(dtGrd2);
                CustGrid.DataSource = dtGrd2;
                // dataReader = cmd.ExecuteReader();

                showData = true;



            }
            catch(Exception e)                            // It will check the Availablity of connection and through exception.
            {
                if (con == false)
                    conn = null;
                showData = false;
                MessageBox.Show("Connection Error");
            }
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AddressCust_TextChanged(object sender, EventArgs e)
        {

        }

        private void Cust_ID_TextChanged(object sender, EventArgs e)
        {
            
        }
       
        // After clicking on RentalGrid_CellContent, it will give the rented movie to selected customers.
        private void RentalGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            String mobl = "";
            frstName = (string)RentalGrid[1, e.RowIndex].Value;
            lstName = (string)RentalGrid[2, e.RowIndex].Value;
            Title = (string)RentalGrid[4, e.RowIndex].Value;
            String phne = "Select Phone From Customer Where FirstName='"+frstName+"' and LastName='"+lstName+"'";
            SqlCommand cmd = new SqlCommand(phne, conn);
            SqlDataReader dataReader= cmd.ExecuteReader();
            if(dataReader.Read())
            {
                mobl = dataReader.GetString(0);
            }
                    dataReader.Close();
                    Cust_ID.Text = RentalGrid[0, e.RowIndex].Value.ToString();
                    FirstNameCust.Text = frstName;
                    LastName_Cust.Text = lstName;
                    AddressCust.Text = (string)RentalGrid[3, e.RowIndex].Value;
                    PhoneCust.Text = mobl;
                    issDate = RentalGrid[6, e.RowIndex].Value.ToString();
                    iDate = Convert.ToDateTime(issDate);
                    RetnDate = RentalGrid[7, e.RowIndex].Value.ToString();
                    rDate = Convert.ToDateTime(RetnDate);
                    IssueMovieDate.Text = issDate;
                    ReturnedDate.Text = RetnDate;
               
            
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Selection\n First Select the Rental Movie to be Filled in Input box.");
            }


        }
        // After clicking on All Rented raddio Button , it will show the list of Rented Movies.
        private void radioButton_Allrented_CheckedChanged(object sender, EventArgs e)
        {
            tabControl1_movisRenatl.SelectedTab = RenatlTAb;
            String sqlRen = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
          
            SqlCommand cmd = new SqlCommand(sqlRen, conn);
            SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
            DataTable dtGrd1 = new DataTable();
            adpRen.Fill(dtGrd1);
            RentalGrid.DataSource = dtGrd1;
        }

       
        private void CustGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            CstId = CustGrid[0, e.RowIndex].Value.ToString();
            Cust_ID.Text = CstId;
             FirstNameCust.Text= (string)CustGrid[1, e.RowIndex].Value;
            LastName_Cust.Text = (string)CustGrid[2, e.RowIndex].Value;
            AddressCust.Text = (string)CustGrid[3, e.RowIndex].Value;
            PhoneCust.Text = (string)CustGrid[4, e.RowIndex].Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Selection\n Please Filled all Input box First");
            }
        }
        // After clicking on Delete Customer button, it will delete the Name of Customers from The Data Table.
        private void button2_Click(object sender, EventArgs e)
        {
            try { 
            String delQuery = "Delete From Customer where CustID ="+CstId+"";
            SqlCommand cmd = new SqlCommand(delQuery, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Customer deleted sucessfully");
                FirstNameCust.Text = "";
                LastName_Cust.Text = "";
                AddressCust.Text = "";
                PhoneCust.Text = "";
                
                dataReader.Close();
        }
            catch(Exception)
            {
                MessageBox.Show("Select Customer Which has not rented any movies to deletes. if Rented then First Delete from rented Movie Table.");
            }
}
        // After clicking on Update Customer button, it will Update the List in Customer After Adding new Customer to list.
        private void button1_Click(object sender, EventArgs e)
        {
            try { 
            tabControl1_movisRenatl.SelectedTab = customersTab;
            String sqlCus = "Select * From Customer";
            SqlCommand cmd = new SqlCommand(sqlCus, conn);
            SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
            DataTable dtGrd1 = new DataTable();
            adpRen.Fill(dtGrd1);
            CustGrid.DataSource = dtGrd1;
                MessageBox.Show("Customer list updated sucessfully");
            }
            catch (Exception)
            {
                MessageBox.Show("Customer Updation Error");
            }
        }
        // After clicking on Delete Movie button, it will Delete the Selected movie from Table.
        private void button_Deletemovie_Click(object sender, EventArgs e)
        {
            try { 
            String delQuery = "Delete From Movies where MovieID =" + MovId + "";
            SqlCommand cmd = new SqlCommand(delQuery, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Movie Deleted sucessfully");
                TitleMovie.Text = "";
                GenreMovie.Text = "";
                RentalCostMovie.Text = "";
                ReleaseDate.Text = "";
                dataReader.Close();
                
            }
            catch (Exception)
            {
                MessageBox.Show("First Delete the movies From rented List ");
            }
        }

        private void button_updatemovie_Click(object sender, EventArgs e)
        {
            try
            {
                tabControl1_movisRenatl.SelectedTab = MoviesTab;
                String sqlCus = "Select MovieID, Title,Genre,Rental_Cost,Year From Movies";
                SqlCommand cmd = new SqlCommand(sqlCus, conn);
                SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
                DataTable dtGrd1 = new DataTable();
                adpRen.Fill(dtGrd1);
                moviesGrid.DataSource = dtGrd1;
                MessageBox.Show("Movie Updated sucessfully");
            }
            
            catch (Exception)
            {
                MessageBox.Show("Selected movies is not Deleted prperly:Error");
            }
        }
        // After clicking on Add Customer button, it will add the Name of new Customer to the Data Table.
        private void button_AddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                String fstName = FirstNameCust.Text;
                String lstName = LastName_Cust.Text;
                String Addrss = AddressCust.Text;
                String phne = PhoneCust.Text;
                
               String insQry = "Insert into Customer ([FirstName], [LastName], [Address], [Phone]) Values('" + fstName + "','" + lstName + "','" + Addrss + "','" + phne + "')";
                SqlCommand cmd = new SqlCommand(insQry, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                MessageBox.Show("Customer Added sucessfully");
                    FirstNameCust.Text = "";
                    LastName_Cust.Text = "";
                    AddressCust.Text = "";
                    PhoneCust.Text = "";

                    dataReader.Close();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Add customer Name prperly: Error");
            }
        }
        // After clicking on Add Movie button, it will add New movie to the Data tAble. 
        private void button_Addmovie_Click(object sender, EventArgs e)
        {
            String Titlemov = TitleMovie.Text;
            String genre = GenreMovie.Text;
            String Rentalmov = RentalCostMovie.Text;
            String RelDate = ReleaseDate.Text;

            String insQry = "Insert into Movies ( [Title], [Genre], [Rental_Cost], [Year]) Values('" + Titlemov + "','" + genre + "','" + Rentalmov + "','" + RelDate + "')";
            SqlCommand cmd = new SqlCommand(insQry, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Movie Added sucessfully");
            TitleMovie.Text= "";
            GenreMovie.Text = "";
            RentalCostMovie.Text = "";
            ReleaseDate.Text = "";
            dataReader.Close();
        }
        // After clicking on Issue Movies  button, it will issued the Movies whatever Customer seleted Mobies from rent.
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                String insQuery = "Insert into RentedMovies ( [MovieIDFK], [CustIDFK], [DateRented], [DateReturned]) Values(" + MovId + "," + CstId + ",'" + iDate + "','" + rDate + "')";

                SqlCommand cmd = new SqlCommand(insQuery, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                String sqlRen = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
                MessageBox.Show("Movie issued sucessfully");
                FirstNameCust.Text = "";
                LastName_Cust.Text = "";
                AddressCust.Text = "";
                PhoneCust.Text = "";
                IssueMovieDate.Text = "";
                ReturnedDate.Text = "";
                TitleMovie.Text = "";
                GenreMovie.Text = "";
                RentalCostMovie.Text = "";
                ReleaseDate.Text = "";
                dataReader.Close();
                cmd = new SqlCommand(sqlRen, conn);
                SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
                DataTable dtGrd1 = new DataTable();
                adpRen.Fill(dtGrd1);
                RentalGrid.DataSource = dtGrd1;
            }
            catch (Exception)
            {
                MessageBox.Show("Select Customer and Movies First to Issue the Movies and also Enter return Date and  Issue date");
            }



        }
        // After clicking on moviesGrid_CellContent, it will Select movieID. 
        String MovId = "";
        private void moviesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            MovId = moviesGrid[0, e.RowIndex].Value.ToString();
            MovieID.Text = MovId;
            TitleMovie.Text = (string)moviesGrid[1, e.RowIndex].Value;
            GenreMovie.Text = moviesGrid[2, e.RowIndex].Value.ToString();
            RentalCostMovie.Text = moviesGrid[3, e.RowIndex].Value.ToString();
            ReleaseDate.Text = moviesGrid[4, e.RowIndex].Value.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("All field must be Filled\n Table have some Empty Field ");
            }

        }
        // After clicking on Return Movies button, it will Delete The Rented Customer information from data TAble List. 
        private void button4_Click(object sender, EventArgs e)
        {
            try { 
            String delQuery = "Delete From RentedMovies where CustIDFK =(Select CustID from Customer where FirstName ='"+frstName+"') and MovieIDFK =(Select MovieID from Movies where Title = '"+Title+"')";
            SqlCommand cmd = new SqlCommand(delQuery, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Customer Sucessfully Returned Renetted Movie");
                FirstNameCust.Text = "";
                LastName_Cust.Text = "";
                AddressCust.Text = "";
                PhoneCust.Text = "";
                IssueMovieDate.Text = "";
                ReturnedDate.Text = "";
             String sqlRen = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
            dataReader.Close();
            cmd = new SqlCommand(sqlRen, conn);
            SqlDataAdapter adpRen = new SqlDataAdapter(cmd);
            DataTable dtGrd1 = new DataTable();
            adpRen.Fill(dtGrd1);
            RentalGrid.DataSource = dtGrd1;
        }
            catch(Exception)
            {
                MessageBox.Show("Movies Is not Deleted Due to Bad Selection Format: Error");
            }

}
    }
}
