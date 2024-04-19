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

namespace Kvartirs
{
    public partial class FlatForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtFlat, dtRaschet, dtTarif, dtBuildings, dtCustomer;
        DataSet dsFlat;
        Flat flat = null;
        SqlDataAdapter dataAdapterFlat, dataAdapterRaschet;
        BindingSource bsFlat, bsBuilding, bsTarif, bsCustomer, bsRaschet;

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

        }

       

        bool add_items;
        public int ID_customer { get; set; }
        private string fio;
        public FlatForm()
        {
            InitializeComponent();
            ID_customer = -1;
            add_items = false;
            LoadDataRashcet();
        }
        public FlatForm(int ID, string Client)
        {
            InitializeComponent();
            ID_customer = ID;
            add_items = true;
          LoadDataRashcet();
            this.Text = Client;
            tbId.Text = ID.ToString() + " "+ Client;
        }
        void LoadDataRashcet()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                try
                {
                    dtRaschet = new DataTable();
                    dataAdapterRaschet = new SqlDataAdapter();
                    dataAdapterRaschet.SelectCommand = new SqlCommand(" select " +
                                                                      " tEvent.ID_Event, " +
                                                                      " tEvent.event_date," +
                                                                      " tEvent.event_name," +
                                                                      " tCase.ID_case," +
                                                                      " tCase.case_name," +
                                                                      " tCase.koef," +
                                                                      " tStavka.ID_stavka," +
                                                                      " tStavka.stavka_date," +
                                                                      " tStavka.summ," +
                                                                      " tStavka.win," +
                                                                      " tStavka.status" +
                                                                      " From tStavka" +
                                                                      " join (tCase join tEvent on tCase.ID_Event" +
                                                                      " = tEvent.ID_Event) ON tStavka.ID_case=" +
                                                                      " tCase.ID_case WHERE tStavka.ID_customer ="+ID_customer+" order by tStavka.ID_stavka,tStavka.stavka_date, tEvent.event_date, event_name", connection);
                    dataAdapterRaschet.Fill(dtRaschet);
                    bsRaschet = new BindingSource();
                    bsRaschet.DataSource = dtRaschet;
                    bindingNavigator1.BindingSource = bsRaschet;
                    dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvTypeTO.DataSource = bsRaschet;
                    dgvTypeTO.Columns[0].Visible = false;
                    dgvTypeTO.Columns[1].HeaderText = "Дата и время события";
                    dgvTypeTO.Columns[2].HeaderText = "Событие";
                    dgvTypeTO.Columns[3].Visible = false;
                    dgvTypeTO.Columns[4].HeaderText = "Исход";
                    dgvTypeTO.Columns[5].HeaderText = "Коэф.";
                    dgvTypeTO.Columns[6].Visible = false;
                    dgvTypeTO.Columns[7].HeaderText = "Дата ставки";
                    dgvTypeTO.Columns[8].HeaderText = "Сумма ставки, руб.";
                    dgvTypeTO.Columns[9].HeaderText = "Сумма выйгр, руб.";
                    dgvTypeTO.Columns[10].HeaderText = "Ставка выйграла";

                   
                    GetSum();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }


        void GetSum()
        {
            tbCount.Text = dgvTypeTO.RowCount.ToString();
            double sum1 = 0, sum2 = 0;
            int k=0;
            if (dgvTypeTO.RowCount > 0)
            {
                for (int i = 0; i < dgvTypeTO.RowCount; i++)
                {
                    sum1 += Convert.ToDouble(dgvTypeTO.Rows[i].Cells[8].Value);
                    sum2 += Convert.ToDouble(dgvTypeTO.Rows[i].Cells[9].Value);
                    if (Convert.ToBoolean(dgvTypeTO.Rows[i].Cells[10].Value)) k++;
                }
            }
            tbMoneyIn.Text = sum1.ToString();
            tbMoneyWin.Text = sum2.ToString();
            tbCountWin.Text = k.ToString();
        }

        private void AddStripButton_Click(object sender, EventArgs e)
        {
            AllFlats allFlats = new AllFlats(ID_customer);
            allFlats.Text = tbId.Text;
            allFlats.ShowDialog();
            LoadDataRashcet();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (bsRaschet.Count > 0)
            {
                int i = bsRaschet.Position;
                string ID_SS = ((DataRowView)this.bsRaschet.Current).Row["ID_stavka"].ToString();
                
                    try
                    {
                        DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание",
                            MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            LoadDataRashcet();
                            return;
                        }
                        if (result == DialogResult.Yes)
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                SqlCommand commandDelete =
                                    new SqlCommand("Delete From tStavka where ID_stavka = @ID_stavka", connection);
                                commandDelete.Parameters.AddWithValue("@ID_stavka", ID_SS);
                                commandDelete.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (SqlException exception)
                    {
                        if ((uint)exception.ErrorCode == 0x80004005)
                            MessageBox.Show("Удаление прервано, есть связанные записи", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        else
                            MessageBox.Show(exception.ToString());
                    }
                    finally
                    {
                        LoadDataRashcet();
                    }
                }
            }

            //void SaveData()
            //    {
            //        if (!IsEmptyKadastr(tbId.Text))
            //        {
            //            MessageBox.Show("Такой кадастровый номер не существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //        int ID_SS = 0;
            //        try
            //        {
            //            connection.Close();
            //            connection.Open();
            //    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tFlat] VALUES(" +
            //                                    "@flat_number," +
            //                                    "@storey," +
            //                                    "@people_count," +
            //                                    "@square_flat," +
            //                                    "@estove," +
            //                                    "@bonus," +
            //                                    "@ID_kadastr," +
            //                                    "@ID_customer," +
            //                                    "@ID_tarif" +
            //                                    ") ; SELECT SCOPE_IDENTITY()", connection);

            //            commandInsert.Parameters.AddWithValue("@storey", Convert.ToInt32(tbCountWin.Text));
            //            commandInsert.Parameters.AddWithValue("@people_count", Convert.ToInt32(tbMoneyIn.Text));
            //            commandInsert.Parameters.AddWithValue("@square_flat", Convert.ToDouble(tbMoneyWin.Text));
            //            commandInsert.Parameters.AddWithValue("@ID_kadastr", tbId.Text);

            //            ID_customer = Convert.ToInt32(commandInsert.ExecuteScalar());
            //            MessageBox.Show("Запись добавлена");
            //            add_items = true;
            //        }
            //        catch (SqlException exception)
            //        {
            //            MessageBox.Show(exception.ToString());
            //        }
            //        finally
            //        {
            //            connection.Close();
            //        }
            //    }
            //    void UpdateData()
            //    {
            //        if (!IsEmptyKadastr(tbId.Text))
            //        {
            //            MessageBox.Show("Такой кадастровый номер не существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //        try
            //        {
            //            connection.Close();
            //            connection.Open();
            //            SqlCommand commandUpdate = new SqlCommand("UPDATE tFlat SET" +
            //                              " flat_number=@flat_number," +
            //                              "storey=@storey," +
            //                                    "people_count=@people_count," +
            //                                    "square_flat=@square_flat," +
            //                                    "estove=@estove," +
            //                                    "bonus=@bonus," +
            //                                    "ID_kadastr=@ID_kadastr," +
            //                                    "ID_customer=@ID_customer," +
            //                                    "ID_tarif=@ID_tarif" +
            //                            "  WHERE ID_flat= @IDSS", connection);
            //            commandUpdate.Parameters.AddWithValue("@storey", Convert.ToInt32(tbCountWin.Text));
            //            commandUpdate.Parameters.AddWithValue("@people_count", Convert.ToInt32(tbMoneyIn.Text));
            //            commandUpdate.Parameters.AddWithValue("@square_flat", Convert.ToDouble(tbMoneyWin.Text));
            //            commandUpdate.Parameters.AddWithValue("@ID_kadastr", tbId.Text);
            //            commandUpdate.Parameters.AddWithValue("@IDSS", flat.ID_flat);
            //            commandUpdate.ExecuteNonQuery();
            //            MessageBox.Show("Запись обновлена");
            //        }
            //        catch (SqlException exception)
            //        {
            //            MessageBox.Show(exception.ToString());
            //        }
            //        finally
            //        {
            //            connection.Close();
            //        }
            //    }

            //    private void toolStripButton1_Click(object sender, EventArgs e)
            //    {
            //        if ((tbId.Text == "")
            //           || (tbMoneyIn.Text == "")
            //           || (tbMoneyWin.Text == "")
            //           || (tbCountWin.Text == ""))
            //        {
            //            MessageBox.Show("Ключевые поля пустые");
            //            return;
            //        }
            //        if (add_items == true)
            //        {
            //            UpdateData();
            //        }
            //        else
            //        {
            //            SaveData();
            //        }
            //    }

            //    private void tbSquareRoom_KeyPress(object sender, KeyPressEventArgs e)
            //    {
            //        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            //          (e.KeyChar != ','))
            //        {
            //            e.Handled = true;
            //        }
            //    }
            //    private void tbFlatNumber_KeyPress(object sender, KeyPressEventArgs e)
            //    {
            //        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //        {
            //            e.Handled = true;
            //        }
            //    }

            //    bool IsEmptyKadastr(string s)
            //    {
            //        try
            //        {
            //            connection.Close();
            //            connection.Open();
            //            SqlCommand SelectCommand = new SqlCommand("SELECT COUNT(*) FROM tBuilding Where Id_kadastr ='" + s + "'", connection);
            //            int x = Convert.ToInt32(SelectCommand.ExecuteScalar());
            //            if (x > 0) return true;
            //            else return false;
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString());
            //            return false;
            //        }
            //    }


        }
}
