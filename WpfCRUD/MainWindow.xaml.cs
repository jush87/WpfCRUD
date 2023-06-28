using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace WpfCRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        public MainWindow(SqlConnection con)
        {
            this.con = con;
        }

        SqlConnection con = new SqlConnection("Data Source=GRAS_PC\\SQLEXPRESS;Initial Catalog=NewDB;Integrated Security=True");
       
        public void clearData()
        {
            name_txt.Clear();
            surname_txt.Clear();
            nation_txt.Clear();
            email_txt.Clear();
            search_txt.Clear();
        }

      
        public void LoadGrid()
        {

            SqlCommand cmd = new SqlCommand("SELECT * FROM FirstTable", con);
            
                DataTable dt = new DataTable();
            con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                con.Close();
                datagrid.ItemsSource = dt.DefaultView;
            
        }



        private void ClearDataBtn_Click(object sender, RoutedEventArgs e) => clearData();

        public bool isValid()
        {

            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (surname_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (email_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (nation_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirstTable VALUES (@Name, @Surname, @Email, @Nationality)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Surname", surname_txt.Text);
                    cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                    cmd.Parameters.AddWithValue("@Nationality", nation_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully registered", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();


                }

            }

            catch (SqlException ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM FirstTable where ID = "+search_txt.Text+" ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();
                con.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not Deleted" + ex.Message);

            }
            finally 
            { 
               con.Close();
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE FirstTable set Name= '"+name_txt.Text+"', Surname = '"+surname_txt.Text+ "', Email = '"+email_txt.Text+ "', Nationality= '"+nation_txt.Text+ "' WHERE ID ='"+search_txt.Text+"' ", con);
         try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been updated successfully", "Update", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        finally 
            {
                con.Close();
                clearData() ;
                LoadGrid();
            }
        
        }

    }
}
