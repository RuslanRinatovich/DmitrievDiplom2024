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
    public partial class AllFlats : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtFlat, dataTableMaterial, dataTableBase;
        DataSet dsFlat;
        SqlDataAdapter dataAdapterFlat;
        BindingSource bsFlat, bsMaterial, bsBase;
        private int ID_customer { get; set; }

        public AllFlats()
        {
            InitializeComponent();
            LoadDataFromTable();
        }

        public AllFlats(int ID)
        {
            InitializeComponent();
            ID_customer = ID;
            LoadDataFromTable();
            
        }
    
        private void tsbExcel_Click(object sender, EventArgs e)
        {
            PrintExcel();
        }

        private void tbSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                      (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if ((tbSum.Text=="")||(bsFlat.Count ==0)|| (ID_customer == 0))
                return;
            SaveData();
        }

        void SaveData()
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsFlat.Current).Row["ID_Case"]);
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tStavka] VALUES(" +
                                                              "@ID_customer," +
                                                              "@ID_case," +
                                                              "@stavka_date," +
                                                              "@summ," +
                                                              "@win, @status)", connection);

                    commandInsert.Parameters.AddWithValue("@ID_customer", ID_customer);
                    commandInsert.Parameters.AddWithValue("@ID_case", ID_SS);
                    commandInsert.Parameters.AddWithValue("@stavka_date", DateTime.Now);
                    commandInsert.Parameters.AddWithValue("@summ", Convert.ToDouble(tbSum.Text));
                    commandInsert.Parameters.AddWithValue("@win", 0);
                    commandInsert.Parameters.AddWithValue("@status", false);
                    commandInsert.ExecuteNonQuery();
                    MessageBox.Show("Ставка сделана");
                    add_items = true;
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

        private void PrintExcel()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "Cases" + ".xltx";
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
                xlSheet.Name = "Ставки";
                int row = 2;
                int i = 0;
             
                if (dgvTypeTO.RowCount > 0)
                {
                    for (i = 0; i < dgvTypeTO.RowCount; i++)
                    {
                        string x = "";
                        xlSheet.Cells[row, 1] = (i+1).ToString();
                        xlSheet.Cells[row, 2] = dgvTypeTO.Rows[i].Cells[0].Value.ToString(); 
                        xlSheet.Cells[row, 3] = dgvTypeTO.Rows[i].Cells[1].Value.ToString(); 
                        xlSheet.Cells[row, 4] = dgvTypeTO.Rows[i].Cells[2].Value.ToString();
                        xlSheet.Cells[row, 5] = dgvTypeTO.Rows[i].Cells[4].Value.ToString();
                        xlSheet.Cells[row, 6] = dgvTypeTO.Rows[i].Cells[5].Value.ToString();
                       
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
        bool add_items;
      
        void LoadDataFromTable()
        {
            try
            {
                dtFlat = new DataTable();
                dataAdapterFlat = new SqlDataAdapter();
                //dataAdapterFlat.SelectCommand = new SqlCommand("select tEvent.ID_Event, " +
                //                                               " tEvent.event_date, " +
                //                                               " tEvent.event_name," + 
                //                                               "  tCase.ID_case," +
                //                                               " tCase.case_name, " +
                //                                               "tCase.koef " +
                //                                               " From tCase join tEvent on tCase.ID_Event = tEvent.ID_Event order by tEvent.event_date, event_name", connection);
                dataAdapterFlat.SelectCommand = new SqlCommand(" select tEvent.ID_Event," +
                                                               " tEvent.event_date," +
                                                               " tEvent.event_name," +
                                                               " tCase.ID_case," +
                                                               " tCase.case_name," +
                                                               " tCase.koef " +
                                                               " From tCase join tEvent on tCase.ID_Event = tEvent.ID_Event " +
                                                               " WHERE  (tEvent.status<2) and (tCase.ID_case NOT IN (SELECT tCase.ID_case FROM TCase" +
                                                               " JOIN tStavka on tCase.ID_case = tStavka.ID_case" +
                                                               " WHERE tStavka.ID_customer = "+ID_customer+")) order by tEvent.event_date, event_name", connection);
                dataAdapterFlat.Fill(dtFlat);
                bsFlat = new BindingSource();
                bsFlat.DataSource = dtFlat;
                bindingNavigator1.BindingSource = bsFlat;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTypeTO.DataSource = bsFlat;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.Columns[1].HeaderText = "Дата";
                dgvTypeTO.Columns[2].HeaderText = "Матч";
                dgvTypeTO.Columns[3].Visible = false;
                dgvTypeTO.Columns[4].HeaderText = "Исход";
                dgvTypeTO.Columns[5].HeaderText = "Коэффициент";
                add_items = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
