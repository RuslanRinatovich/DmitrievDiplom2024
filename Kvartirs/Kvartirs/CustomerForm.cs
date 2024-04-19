using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Kvartirs
{
    public partial class CustomerForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        BindingSource bsPeople;
        private DataTable dt;
        SqlDataAdapter dataAdapter, dataAdapterPost;

        bool add_items;
        public CustomerForm()
        {
            InitializeComponent();
            LoadDataFromTable();
            add_items = false;
  }

        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ClearItems();
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tCustomer Order by sFIO, passport ", connection);
                dt = new DataTable();
                sda.Fill(dt);
                bsPeople = new BindingSource();
                //= dataTable;
                bsPeople.DataSource = dt;
                bindingNavigator1.BindingSource = bsPeople;
                // Очистка
                // textBox1
                if (bsPeople.Count > 0) toolStripButton2.Enabled = true;
                else toolStripButton2.Enabled = false;
                tbSurName.DataBindings.Clear();
                tbSurName.DataBindings.Add(new Binding("Text", bsPeople, "sFIO"));
                dtpBithday.DataBindings.Clear();
                dtpBithday.DataBindings.Add(new Binding("Value", bsPeople, "Bithday"));
                mtbPassport.DataBindings.Clear();
                mtbPassport.DataBindings.Add(new Binding("Text", bsPeople, "Passport"));
                tbAdress.DataBindings.Clear();
                tbAdress.DataBindings.Add(new Binding("Text", bsPeople, "Address"));
                tbPhone.DataBindings.Clear();
                tbPhone.DataBindings.Add(new Binding("Text", bsPeople, "Phone"));

                tbID.DataBindings.Clear();
                tbID.DataBindings.Add(new Binding("Text", bsPeople, "Id_customer"));
                mtbBankaccount.DataBindings.Clear();
                mtbBankaccount.DataBindings.Add(new Binding("Text", bsPeople, "bank_account"));
                dgvTypeTO.DataSource = bsPeople;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTypeTO.Columns[1].HeaderText = "ФИО";
                dgvTypeTO.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTypeTO.Columns[2].HeaderText = "Дата рождения";
                dgvTypeTO.Columns[3].HeaderText = "Серия и номер паспорта";
                dgvTypeTO.Columns[4].HeaderText = "Адрес";
                dgvTypeTO.Columns[5].HeaderText = "Телефон";
                dgvTypeTO.Columns[6].HeaderText = "Банковская карта";
                add_items = false;
            }
        }
     
        private void tbSurName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) return;
            if (Char.IsSeparator(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }
      
        private void ClearItems()
        {
            tbSurName.Text = "";
            dtpBithday.Value = DateTime.Now;
            mtbPassport.Text = "";
            tbAdress.Text = "";
            tbPhone.Text = "";
            mtbBankaccount.Text = "";     
        }
        private void AddStripButton_Click(object sender, EventArgs e)
        {
            ClearItems();
            add_items = true;
           // saveToolStripButton.Enabled = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (add_items)
            {
                add_items = false;
                saveToolStripButton.Enabled = true;
                LoadDataFromTable();
            }
            else if (bsPeople.Count > 0)
            {
                int i = bsPeople.Position;
                string ID_SS = ((DataRowView)this.bsPeople.Current).Row["ID_customer"].ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание",
                            MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            LoadDataFromTable();
                            return;
                        }
                        if (result == DialogResult.Yes)
                        {

                            SqlCommand commandDelete = new SqlCommand("Delete From tCustomer where ID_customer = @ID_customer", connection);
                            commandDelete.Parameters.AddWithValue("@ID_customer", ID_SS);
                            commandDelete.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException exception)
                    {
                       // if ((uint)exception.ErrorCode == 0x80004005)
                            MessageBox.Show("Удаление прервано, есть связанные записи", "Error", MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                      //  else
                         //   MessageBox.Show(exception.ToString());
                    }
                    finally
                    {
                        LoadDataFromTable();
                    }
                }
            }

        }

        void InsertData()
        {
            string s = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tCustomer] VALUES(" +
                                                    "@sFIO," +
                                                    "@bithday," +
                                                    "@passport," +
                                                    "@address," +
                                                    "@phone," +
                                                    "@bank_account" +
                                                    ") ", connection);
                    commandInsert.Parameters.AddWithValue("@sFIO", tbSurName.Text);
                    commandInsert.Parameters.AddWithValue("@bithday", dtpBithday.Value.Date);
                    commandInsert.Parameters.AddWithValue("@passport", mtbPassport.Text);
                    commandInsert.Parameters.AddWithValue("@address", tbAdress.Text);
                    commandInsert.Parameters.AddWithValue("@phone", tbPhone.Text);
                    commandInsert.Parameters.AddWithValue("@bank_account", mtbBankaccount.Text);
                    s = mtbPassport.Text;
                    commandInsert.ExecuteNonQuery();
                    MessageBox.Show("Запись добавлена");
                    add_items = false;
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    bsPeople.Position = bsPeople.Find("passport", s);
                }
            }
        }

        void UpdateData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string x = "";
                connection.Open();
                try
                {
                    string ID_SS = ((DataRowView)this.bsPeople.Current).Row["ID_customer"].ToString();
                    x = ID_SS;
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tCustomer SET" +
                                                    " sFIO=@sFIO," +
                                                    "bithday=@bithday," +
                                                    "passport=@passport," +
                                                    "address=@address," +
                                                    "phone=@phone," +
                                                    "bank_account=@bank_account" +
                                                  
                                    "  WHERE ID_customer= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@sFIO", tbSurName.Text);
                    commandUpdate.Parameters.AddWithValue("@bithday", dtpBithday.Value.Date);
                    commandUpdate.Parameters.AddWithValue("@passport", mtbPassport.Text);
                    commandUpdate.Parameters.AddWithValue("@address", tbAdress.Text);
                    commandUpdate.Parameters.AddWithValue("@phone", tbPhone.Text);
                    commandUpdate.Parameters.AddWithValue("@bank_account", mtbBankaccount.Text);
                    commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                    commandUpdate.ExecuteNonQuery();
                    MessageBox.Show("Запись обновлена");
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    bsPeople.Position = bsPeople.Find("ID_customer", x);
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if ((mtbPassport.Text == "") || (tbSurName.Text == ""))
            {
                MessageBox.Show("Ключевые поля пустые", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (add_items)
            {
                InsertData();
            }
            else
            {
                UpdateData();
            }
        }

        private void tbPassport_TextChanged(object sender, EventArgs e)
        {
            //if (mtbPassport.Text == "" || tbSurName.Text == "") saveToolStripButton.Enabled = false;
            //else saveToolStripButton.Enabled = true;

        }
        private void dgvTypeTO_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
   //         bsPeople.Filter = "sFIO LIKE '%" + toolStripTextBox2.Text + "%'";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dgvTypeTO.RowCount > 0)
            {
                int i = Convert.ToInt32(tbID.Text);
                FlatForm flatForm = new FlatForm(i, tbSurName.Text);
                flatForm.ShowDialog();
            }

        }

        void ShowBuildingToChange()
        {
            if ((bsPeople.Count > 0) && (dgvTypeTO.SelectedRows.Count > 0))
            {
                int i = Convert.ToInt32(tbID.Text);
                FlatForm flatForm = new FlatForm(i, tbSurName.Text);
                flatForm.ShowDialog();
                // bsEvent.Position = bsEvent.Find("ID_event", (ID_SS));

            }
        }

        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dgGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

    }
}
