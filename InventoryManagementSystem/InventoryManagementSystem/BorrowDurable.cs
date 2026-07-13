using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class BorrowDurable : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public static BorrowDurable Form2Instance;
        public Label txt;

        public BorrowDurable()
        {
            InitializeComponent();
            LoadEquipmentName();
            Form2Instance = this;
            txt = label10;
        }
        public void LoadEquipmentName()
        {
            cmbequname.Items.Clear();
            cm = new SqlCommand("SELECT Durable_name FROM Durable_Arcticles Where quantity > 0", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cmbequname.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        int A, B, total;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //Show Borrower_ID 
                    cm = new SqlCommand("SELECT Borrower_ID FROM Borrower Where name = '" + label10.Text + "'", con);
                    con.Open();
                    dr = cm.ExecuteReader();
                    dr.Read();
                    label2.Text = dr[0].ToString();
                    dr.Close();
                    con.Close();

                    cm = new SqlCommand("INSERT INTO Borrow(Borrower_ID,BorrowDate,ReturnDate,status)VALUES(@Borrower_ID, @BorrowDate, @ReturnDate, 'Borrow')", con);
                    cm.Parameters.AddWithValue("@Borrower_ID", Convert.ToInt32(label2.Text));
                    cm.Parameters.AddWithValue("@BorrowDate", txtbrdate.Text);
                    cm.Parameters.AddWithValue("@ReturnDate", txtrtdate.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                    //Show Borrow_ID
                    cm = new SqlCommand("SELECT Borrow_ID FROM Borrow Where Borrower_ID = '" + label2.Text + "' AND BorrowDate = '" + txtbrdate.Text + "' AND ReturnDate = '" + txtrtdate.Text + "' ", con);
                    con.Open();
                    dr = cm.ExecuteReader();
                    dr.Read();
                    label14.Text = dr[0].ToString();
                    dr.Close();
                    con.Close();

                    //Show Durable_ID
                    cm = new SqlCommand("SELECT Durable_ID FROM Durable_Arcticles Where Durable_name = '" + cmbequname.Text + "'", con);
                    con.Open();
                    dr = cm.ExecuteReader();
                    dr.Read();
                    label5.Text = dr[0].ToString();
                    dr.Close();
                    con.Close();

                    cm = new SqlCommand("INSERT INTO Borrow_Detail_Durable(Borrow_ID,Durable_ID,quantity)VALUES(@Borrow_ID, @Durable_ID, @quantity)", con);
                    cm.Parameters.AddWithValue("@Borrow_ID", Convert.ToInt32(label14.Text));
                    cm.Parameters.AddWithValue("@Durable_ID", label5.Text);
                    cm.Parameters.AddWithValue("@quantity", Convert.ToInt32(txtquantity.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                    //Show Quantity
                    cm = new SqlCommand("SELECT quantity FROM Durable_Arcticles Where Durable_ID = '" + label5.Text + "'", con);
                    con.Open();
                    dr = cm.ExecuteReader();
                    dr.Read();
                    label3.Text = dr[0].ToString();
                    dr.Close();
                    con.Close();


                    A = Convert.ToInt32(label3.Text);
                    B = Convert.ToInt32(txtquantity.Text);
                    total = A - B;
                    label4.Text = total.ToString();

                    cm = new SqlCommand("UPDATE Durable_Arcticles SET quantity = '" + Convert.ToInt32(label4.Text) + "' WHERE Durable_ID LIKE '" + label5.Text + "' ", con);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Form has been successfully!");
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
        }
        public void Clear()
        {
            cmbequname.Text = "";
            txtquantity.Clear();
            txtbrdate.Clear();
            txtrtdate.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void BorrowForm_Load(object sender, EventArgs e)
        {

        }
    }
    
}
