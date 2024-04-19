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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms.VisualStyles;
using static Kvartirs.models;
using Excel = Microsoft.Office.Interop.Excel;

namespace Kvartirs
{
    public partial class RoomsFlat : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtRooms;
        DataSet dsRoom;
        SqlDataAdapter dataAdapterRoom;
        BindingSource bsRoom, bsnew;
        private int ID_event = -1;
        private int ID_case = -1;
        private string event_name = "";
        private string address, fio;
        private bool add_items;
        Tarif t;

        public RoomsFlat(int tarif)
        {
            InitializeComponent();
            ID_case = tarif;
        }

        public RoomsFlat(int i, string s, bool b)
        {

            InitializeComponent();
            ID_event = i;
            event_name = s;
            tbEvent.Text = s;
            tbID.Text = i.ToString();
            LoadDataFromTable();
            ButtonsVisible(b);
            add_items = false;
        }


        void ButtonsVisible(bool b)
        {
            tsbAdd.Visible = b;
            //tsbChange.Visible = b;
            tsbDelete.Visible = b;
        }

        void LoadDataFromTable()
        {
            try
            {
                //ClearItems();
                dtRooms = new DataTable();
                dataAdapterRoom = new SqlDataAdapter();
                dataAdapterRoom.SelectCommand = new SqlCommand(" SELECT * " +
                "FROM tCase WHERE tCase.Id_event = " + ID_event + " ORder by case_name", connection);
                dataAdapterRoom.Fill(dtRooms);
                bsRoom = new BindingSource();
                bsRoom.DataSource = dtRooms;
                bindingNavigator1.BindingSource = bsRoom;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTypeTO.DataSource = bsRoom;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Исход";
                dgvTypeTO.Columns[2].HeaderText = "Коэффициент";
                dgvTypeTO.Columns[3].Visible = false;
                dgvTypeTO.Columns[4].HeaderText = "Исход выйграл";
                add_items = false;

                tbCase.DataBindings.Clear();
                tbCase.DataBindings.Add(new Binding("Text", bsRoom, "case_name"));
                nudKoef.DataBindings.Clear();
                nudKoef.DataBindings.Add(new Binding("Value", bsRoom, "koef"));
                chbWin.DataBindings.Clear();
                chbWin.DataBindings.Add(new Binding("Checked", bsRoom, "win"));
                tsbDelete.Enabled = true;
                if (bsRoom.Count <= 0) tsbDelete.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (add_items)
            {
                LoadDataFromTable();
                add_items = false;
                return;
            }
            if (bsRoom.Count > 0)
            {
                int i = bsRoom.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsRoom.Current).Row["ID_case"]);

                try
                {
                    DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        LoadDataFromTable();
                        return;
                    }
                    if (result == DialogResult.Yes)
                    {
                        connection.Close();
                        connection.Open();
                        SqlCommand commandDelete = new SqlCommand("Delete From tCase where ID_case = @ID_case", connection);
                        commandDelete.Parameters.AddWithValue("@ID_case", ID_SS);
                        commandDelete.ExecuteNonQuery();
                        ClearItems();
                    }
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                }
            }
        }
        private void tsbChange_Click(object sender, EventArgs e)
        {
            if (bsRoom.Count > 0) ID_case = Convert.ToInt32(((DataRowView)this.bsRoom.Current).Row["ID_case"]);
            if ((tbCase.Text == "") || (Convert.ToDouble(nudKoef.Value) == 0))
                return;
            if (add_items)
            {
                SaveData();
            }
            else
            {
                UpdateData();
            }
           
        }

        void ClearItems()
        {
            tbCase.Text = "";
            nudKoef.Text = "0";
            chbWin.Checked = false;
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            ClearItems();
            add_items = true;
            tsbDelete.Enabled = true;
        }

        void UpdateWins(int IDc)
        {
        //    if (bsRoom.Count == 0) return;
    //        int ID_SS = Convert.ToInt32(((DataRowView)this.bsRoom.Current).Row["ID_case"]);
            if (IsCaseExist(IDc))
            {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand commandUpdate = new SqlCommand("UPDATE tStavka SET" +
                                                                      " status=@status, " +
                                                                      " win=summ*@win WHERE ID_case = @ID_case",
                                connection);
                            commandUpdate.Parameters.AddWithValue("@status", true);
                            commandUpdate.Parameters.AddWithValue("@win", Convert.ToDouble(nudKoef.Value));
                            commandUpdate.Parameters.AddWithValue("@ID_case", IDc);
                            commandUpdate.ExecuteNonQuery();
                       //     MessageBox.Show("Запись обновлена");
                        }
                    }
                    catch (SqlException exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }
            }
        }

        void UpdateLosers(int IDc)
        {
            //if (bsRoom.Count == 0) return;
            //int ID_SS = Convert.ToInt32(((DataRowView)this.bsRoom.Current).Row["ID_case"]);
            if (IsCaseExist(IDc))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tStavka SET" +
                                                                  " status=@status, " +
                                                                  " win=@win WHERE ID_case = @ID_case", connection);
                        commandUpdate.Parameters.AddWithValue("@status", false);
                        commandUpdate.Parameters.AddWithValue("@win", 0);
                        commandUpdate.Parameters.AddWithValue("@ID_case", IDc);
                        commandUpdate.ExecuteNonQuery();
                       commandUpdate = new SqlCommand("UPDATE tCase SET win=@win WHERE ID_case = @ID_case", connection);
                        commandUpdate.Parameters.AddWithValue("@win", false);
                        commandUpdate.Parameters.AddWithValue("@ID_case", IDc);
                        commandUpdate.ExecuteNonQuery();
                        // MessageBox.Show("Запись обновлена");
                    }
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        void UpdateWinsAndLosers()
        {
            if (dgvTypeTO.RowCount == 0) return;
            for (int i = 0; i < dgvTypeTO.RowCount; i++)
            {
                int ID_SS = Convert.ToInt32(dgvTypeTO.Rows[i].Cells[0].Value);
                if (ID_SS == ID_case)
                    UpdateWins(ID_SS);
                else UpdateLosers(ID_SS);
                    
            }
            LoadDataFromTable();
        }

        bool IsCaseExist(int s)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand SelectCommand =
                        new SqlCommand("SELECT COUNT(*) FROM tStavka Where Id_case =" + s, connection);
                    int x = Convert.ToInt32(SelectCommand.ExecuteScalar());
                    if (x > 0) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        void SaveData()
        {
            try
            {
                connection.Close();
                connection.Open();
                SqlCommand commandInsert = new SqlCommand("INSERT INTO [tCase] VALUES" +
                        " (@case_name,@koef,@ID_event,@win)", connection);
                commandInsert.Parameters.AddWithValue("@case_name", tbCase.Text);
                commandInsert.Parameters.AddWithValue("@koef", Convert.ToDouble(nudKoef.Value));
                commandInsert.Parameters.AddWithValue("@ID_event", Convert.ToInt32(tbID.Text));
                commandInsert.Parameters.AddWithValue("@win", chbWin.Checked);
                commandInsert.ExecuteNonQuery();
                MessageBox.Show("Запись добавлена");
                if (chbWin.Checked) UpdateWinsAndLosers();
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
            }
        }
        void UpdateData()
        {
            if (bsRoom.Count > 0)
            {
                int i = bsRoom.Position;
                int ii = bsRoom.Position;
                try
                {
                    connection.Close();
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsRoom.Current).Row["ID_case"]);
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tCase SET" +
                        " case_name=@case_name, koef=@koef, " +
                        " ID_event=@ID_event, win=@win WHERE ID_case = @ID_case", connection);
                    commandUpdate.Parameters.AddWithValue("@case_name", tbCase.Text);
                    commandUpdate.Parameters.AddWithValue("@koef", Convert.ToDouble(nudKoef.Value));
                    commandUpdate.Parameters.AddWithValue("@ID_event", Convert.ToInt32(tbID.Text));
                    commandUpdate.Parameters.AddWithValue("@win", chbWin.Checked);
                    commandUpdate.Parameters.AddWithValue("@ID_case", ID_SS);
                    commandUpdate.ExecuteNonQuery();
                    MessageBox.Show("Запись обновлена");
                    if (chbWin.Checked) UpdateWinsAndLosers();
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    bsRoom.Position = ii;
                }
            }
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
    }
}
