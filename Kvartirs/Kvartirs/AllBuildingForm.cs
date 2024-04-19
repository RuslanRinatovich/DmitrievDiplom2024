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
using Excel = Microsoft.Office.Interop.Excel;
namespace Kvartirs
{
    public partial class AllBuildingForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);

        DataTable dtEvent, dtCategory, dataTableBase;
        SqlDataAdapter dataAdapterEvent;
        BindingSource bsEvent, bsCategory, bsBase;
        bool add_items;
        public AllBuildingForm()
        {
            InitializeComponent();
            LoadDataFromTable();
            loadComboMaterial();
            
            cmbStatus.SelectedIndex = 0;
            PaintRows();
        }
        void loadComboMaterial()
        {

            SqlDataAdapter dataAdapterMaterial = new SqlDataAdapter("SELECT * from tCategory Order by category_name", connection);
            dtCategory = new DataTable();
            dataAdapterMaterial.Fill(dtCategory);
            bsCategory = new BindingSource();
            bsCategory.DataSource = dtCategory;
            cmbSport.DataSource = dtCategory;
            cmbSport.ValueMember = "ID_category";
            cmbSport.DisplayMember = "category_name";
        }
       
        void LoadDataFromTable()
        {
            try
            {
                dtEvent = new DataTable();
                dataAdapterEvent = new SqlDataAdapter();
                dataAdapterEvent.SelectCommand = new SqlCommand(" SELECT RTRIM(STR(tEvent.ID_event)) AS ID_event, " +
           " tEvent.photo, " +
                    " tEvent.event_name," +
      " tEvent.event_date," +
      " tEvent.ID_category," +
      " tCategory.category_name," +
      " tEvent.info," +

     " tEvent.status " +
                "FROM tEvent JOIN tCategory ON tEvent.ID_category = tCategory.ID_category ", connection);
                dataAdapterEvent.Fill(dtEvent);
                dtEvent.Columns.Add("Added", typeof(string));
                bsEvent = new BindingSource();
                bsEvent.DataSource = dtEvent;
                bindingNavigator1.BindingSource = bsEvent;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTypeTO.DataSource = bsEvent;
                dgvTypeTO.Columns[0].HeaderText = "Номер события";
                dgvTypeTO.Columns[1].HeaderText = "фото";
                ((DataGridViewImageColumn)dgvTypeTO.Columns[1]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvTypeTO.Columns[2].HeaderText = "Событие";
                dgvTypeTO.Columns[3].HeaderText = "Дата и время";
                dgvTypeTO.Columns[4].Visible = false;
                dgvTypeTO.Columns[5].HeaderText = "Вид спорта";
                dgvTypeTO.Columns[6].HeaderText = "Информация";
                dgvTypeTO.Columns[8].HeaderText = "статус";
                dgvTypeTO.Columns[7].Visible = false;
                add_items = false;
                if (bsEvent.Count <= 0) tsbDelete.Enabled = false;
                else tsbDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void PaintRows()
        {
            if (dgvTypeTO.Rows.Count > 0)
            {
                for (int j = 0; j < dgvTypeTO.Rows.Count; j++)
                {
                    int k = Convert.ToInt32(dgvTypeTO.Rows[j].Cells[7].Value);
                    if (k == 0)
                    {
                        dgvTypeTO.Rows[j].Cells[8].Value = "не начался";
                        dgvTypeTO.Rows[j].Cells[8].Style.ForeColor = Color.Orange;
                    }
                    if (k == 1)
                    {
                        dgvTypeTO.Rows[j].Cells[8].Value = "идёт";
                        dgvTypeTO.Rows[j].Cells[8].Style.ForeColor = Color.Green;
                    }
                    if (k == 2)
                    {
                        dgvTypeTO.Rows[j].Cells[8].Value = "завершён";
                        dgvTypeTO.Rows[j].Cells[8].Style.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            BuildingForm building_form = new BuildingForm();
            building_form.ShowDialog();
            LoadDataFromTable();
            PaintRows();
        }
        void ShowBuildingToChange()
        {
            if ((bsEvent.Count > 0) && (dgvTypeTO.SelectedRows.Count > 0))
            {
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsEvent.Current).Row["ID_event"]);
                BuildingForm building_form = new BuildingForm(ID_SS);
                building_form.ShowDialog();
                LoadDataFromTable();
                PaintRows();
               // bsEvent.Position = bsEvent.Find("ID_event", (ID_SS));

            }
        }
        private void tsbChange_Click(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        private void chbMaterial_CheckedChanged(object sender, EventArgs e)
        {
                FilterData();
        }
        private void chbBase_CheckedChanged(object sender, EventArgs e)
        {
                FilterData();
        }
        private void cmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void cmbBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (bsEvent.Count > 0)
            {
                int i = bsEvent.Position;
                string ID_SS = Convert.ToString(((DataRowView)this.bsEvent.Current).Row["ID_event"]);
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
                        SqlCommand commandDelete = new SqlCommand("Delete From tEvent where ID_event = @ID_event", connection);
                        commandDelete.Parameters.AddWithValue("@ID_event", ID_SS);
                        commandDelete.ExecuteNonQuery();
                    }
                }
                catch (SqlException exception)
                {
                    if (exception.HResult == -2146232060)
                        MessageBox.Show("Ошибка удаления, есть связанные записи");
                    else MessageBox.Show(exception.Message);
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    PaintRows();
                }
            }
        }
        private void tsbExcel_Click(object sender, EventArgs e)
        {
            PrintExcel();
        }

        private void AllBuildingForm_Load(object sender, EventArgs e)
        {
            PaintRows();
        }

        private void PrintExcel()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "Events" + ".xltx";
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                //добавляем книгу
                xlApp.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing);
                //делаем временно неактивным документ
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                Excel.Range xlSheetRange;
                //выбираем лист на котором будем работать (Лист 1)
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                //Название листа
                xlSheet.Name = "Здания";
                int row = 2;
                int i = 0;
                //dgvTypeTO.Columns[0].HeaderText = "Номер события";
                //dgvTypeTO.Columns[1].HeaderText = "фото";
                //((DataGridViewImageColumn)dgvTypeTO.Columns[1]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                //dgvTypeTO.Columns[2].HeaderText = "Событие";
                //dgvTypeTO.Columns[3].HeaderText = "Дата и время";
                //dgvTypeTO.Columns[4].Visible = false;
                //dgvTypeTO.Columns[5].HeaderText = "Вид спорта";
                //dgvTypeTO.Columns[6].HeaderText = "Информация";
                //dgvTypeTO.Columns[8].HeaderText = "статус";
                if (dgvTypeTO.RowCount > 0)
                {
                    for (i = 0; i < dgvTypeTO.RowCount; i++)
                    {
                        xlSheet.Cells[row, 1] = dgvTypeTO.Rows[i].Cells[0].Value.ToString();
                        xlSheet.Cells[row, 2] = dgvTypeTO.Rows[i].Cells[2].Value.ToString(); 
                        xlSheet.Cells[row, 3] = dgvTypeTO.Rows[i].Cells[3].Value.ToString();
                        xlSheet.Cells[row, 4] = dgvTypeTO.Rows[i].Cells[5].Value.ToString();
                        xlSheet.Cells[row, 5] = dgvTypeTO.Rows[i].Cells[6].Value.ToString();
                        xlSheet.Cells[row, 6] = dgvTypeTO.Rows[i].Cells[8].Value.ToString();
                        row++;
                        Excel.Range r = xlSheet.get_Range("A" + row.ToString(), "F" + row.ToString());
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                row--;
                xlSheetRange = xlSheet.get_Range("A2:F" + row.ToString(), Type.Missing);
                xlSheetRange.Borders.LineStyle = true;
                row++;
                //выбираем всю область данных*/
                xlSheetRange = xlSheet.UsedRange;
                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Показываем ексель
                xlApp.Visible = true;
                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;
            }
        }
        private void tbKadastr_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        void FilterData()
        {
            bsEvent.RemoveFilter();
            var queries = new List<string>();
            if (chbMaterial.Checked)
            {
                queries.Add(string.Format("[ID_category]={0}", cmbSport.SelectedValue));
            }
            if (chbBase.Checked)
            {
                queries.Add(string.Format("[status]={0}", cmbStatus.SelectedIndex));
            }
            if (tbKadastr.Text != "")
            {
                queries.Add(string.Format("[ID_event] LIKE '%{0}%'", tbKadastr.Text));
            }
            if (queries.Count >= 1)
            {
                var queryFilter = String.Join(" AND ", queries);
                bsEvent.Filter = queryFilter;
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
