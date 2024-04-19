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
    public partial class BuildingForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtBuilding, dataTableMaterial, dtKoef, dtGoods;
        DataSet dsBuilding;
        SqlDataAdapter dataAdapterBuilding, dataAdapterKoef;
        BindingSource bsBuilding, bsMaterial, bsKoef;
        bool add_items;
        Eventname events = null;
        public int ID_event { get; set; }
        private void BuildingForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.InitialImage;
            loadComboMaterial();
            LoadDataFromTable();
            LoadKoef();
        }
        public BuildingForm()
        {
            InitializeComponent();
            add_items = false;
        }
        public BuildingForm(int ID)
        {
            InitializeComponent();
            ID_event = ID;
            add_items = true;
        }


        void LoadKoef()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    dtKoef = new DataTable();
                    dataAdapterKoef = new SqlDataAdapter();
                    dataAdapterKoef.SelectCommand = new SqlCommand("SELECT * " +
                                                              "FROM tCase WHERE ID_event ="+ID_event, connection);
                    dataAdapterKoef.Fill(dtKoef);
                    bsKoef = new BindingSource();
                    bsKoef.DataSource = dtKoef;
                    dgvTypeTO.DataSource = bsKoef;
                    dgvTypeTO.Columns[1].HeaderText = "Исходы";
                    dgvTypeTO.Columns[0].Visible = false;
                    dgvTypeTO.Columns[2].HeaderText = "коэффициент";
                    dgvTypeTO.Columns[3].Visible = false;
                    dgvTypeTO.Columns[4].Visible = false;
                    dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        void LoadDataFromTable()
        {
            try
            {
                connection.Close();
                connection.Open();
                SqlCommand SelectCommand = new SqlCommand(" SELECT tEvent.ID_event, " +
      " tEvent.event_name," +
      " tEvent.event_date," +
      " tEvent.ID_category," +
      " tCategory.category_name," +
      " tEvent.info," +
     " tEvent.photo, " +
     " tEvent.status " +
                "FROM tEvent JOIN tCategory ON tEvent.ID_category = tCategory.ID_category  WHERE ID_event ='" + ID_event+"'", connection);
                SqlDataReader reader = SelectCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    events = new Eventname();
                    events.ID_event = Convert.ToInt32(reader.GetValue(0));
                    events.event_name = Convert.ToString(reader.GetValue(1));
                    events.event_date = Convert.ToDateTime(reader.GetValue(2));
                    events.ID_category = Convert.ToInt32(reader.GetValue(3));
                    events.category_name = Convert.ToString(reader.GetValue(4));
                    events.info = Convert.ToString(reader.GetValue(5));
                    events.status = Convert.ToInt32(reader.GetValue(7));
                    if (reader.GetValue(6) != DBNull.Value)
                    {
                        events.photo = (byte[])reader.GetValue(6);
                        using (var ms = new MemoryStream(events.photo))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                    else
                       
                    pictureBox1.Image = pictureBox1.InitialImage;

                      tbID.Text = events.ID_event.ToString();
                    tbInfo.Text = events.info;
                    tbEvent.Text = events.event_name;
                    cmbSport.SelectedValue = events.ID_category;
                    cmbStatus.SelectedIndex = events.status;
                    dtpDate.Value= events.event_date;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            try
            {
                string filename = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(filename);// читаем файл в строку
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки файла", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {
            bool b = true;
            if (events.status == 2) b = false;
            RoomsFlat roomsFlat = new RoomsFlat(ID_event, events.event_name, b);
            roomsFlat.ShowDialog();

            LoadKoef();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
            }
        }

        void UpdateWins(int IDc)
        {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand commandUpdate = new SqlCommand("UPDATE tCase SET" +
                                                                  " win=@win WHERE ID_event = @ID_event",
                            connection);
                        commandUpdate.Parameters.AddWithValue("@win", false);
                        commandUpdate.Parameters.AddWithValue("@ID_event", IDc);
                        commandUpdate.ExecuteNonQuery();
                     commandUpdate = new SqlCommand("UPDATE tStavka SET" +
                                                                 " status=@status, " +
                                                                 " win=@win WHERE ID_case IN (SELECT ID_case From tCase WHERE ID_event = @ID_event)", connection);
                    commandUpdate.Parameters.AddWithValue("@status", false);
                    commandUpdate.Parameters.AddWithValue("@win", 0);
                    commandUpdate.Parameters.AddWithValue("@ID_event", IDc);
                    commandUpdate.ExecuteNonQuery();

                    //     MessageBox.Show("Запись обновлена");
                }
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((tbEvent.Text == "")
              || (tbInfo.Text == "")
                || (cmbSport.SelectedIndex == -1) || (cmbStatus.SelectedIndex == -1)
              )
            {
                MessageBox.Show("Ключевые поля пустые");
                return;
            }
            if (add_items == true)
            {
                UpdateData();
            }
            else
            {
                SaveData();
            }
           if (cmbStatus.SelectedIndex<2) UpdateWins(ID_event);
        }

        byte[] ConvertInBytes(Image img)
        {

            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                bytes = ms.ToArray();
            }
            return bytes;
        }
        void loadComboMaterial()
        {

            SqlDataAdapter dataAdapterMaterial = new SqlDataAdapter("SELECT * from tCategory Order by category_name", connection);
            dataTableMaterial = new DataTable();
            dataAdapterMaterial.Fill(dataTableMaterial);
            bsMaterial = new BindingSource();
            bsMaterial.DataSource = dataTableMaterial;
            cmbSport.DataSource = dataTableMaterial;
            cmbSport.ValueMember = "ID_category";
            cmbSport.DisplayMember = "category_name";
        }
     
        void SaveData()
        {
           
            int ID_SS = 0;
            try
            {
                connection.Close();
                connection.Open();
        SqlCommand commandInsert = new SqlCommand("INSERT INTO [tEvent] VALUES(" +
                                "@event_name," +
                                "@event_date," +
                                "@ID_category," +
                                "@info," +
                                "@photo," +
                                "@status" +
                                "); SELECT SCOPE_IDENTITY()", connection);
                commandInsert.Parameters.AddWithValue("@event_name", tbEvent.Text);
                commandInsert.Parameters.AddWithValue("@event_date", dtpDate.Value);
                commandInsert.Parameters.AddWithValue("@ID_category", Convert.ToInt32(cmbSport.SelectedValue));
                commandInsert.Parameters.AddWithValue("@info", tbInfo.Text);
                commandInsert.Parameters.AddWithValue("@photo", ConvertInBytes(pictureBox1.Image));
                commandInsert.Parameters.AddWithValue("@status", Convert.ToInt32(cmbStatus.SelectedIndex));
                ID_event =Convert.ToInt32(commandInsert.ExecuteScalar());
                MessageBox.Show("Запись добавлена");
                add_items = true;
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
            try
            {
                connection.Close();
                connection.Open();
                SqlCommand commandUpdate = new SqlCommand("UPDATE tEvent SET" +
                                 " event_name=@event_name," +
                                "event_date=@event_date," +
                                "ID_category=@ID_category," +
                                "info=@info," +
                                "photo=@photo," +
                                "status=@status" +
                                "  WHERE ID_event= @IDSS", connection);
                commandUpdate.Parameters.AddWithValue("@event_name", tbEvent.Text);
                commandUpdate.Parameters.AddWithValue("@event_date", dtpDate.Value);
                commandUpdate.Parameters.AddWithValue("@ID_category", Convert.ToInt32(cmbSport.SelectedValue));
                commandUpdate.Parameters.AddWithValue("@info", tbInfo.Text);
                commandUpdate.Parameters.AddWithValue("@photo", ConvertInBytes(pictureBox1.Image));
                commandUpdate.Parameters.AddWithValue("@status", Convert.ToInt32(cmbStatus.SelectedIndex));
                commandUpdate.Parameters.AddWithValue("@IDSS", ID_event);
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