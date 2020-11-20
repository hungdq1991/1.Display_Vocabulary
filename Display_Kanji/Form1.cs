using System;
using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Windows.Forms;

namespace Display_Kanji
{
    public partial class Form1 : Form
    {
        public DataTable _dtTemp = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open File Data";
            theDialog.Filter = "Files Excel|*.xlsx";

            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try {
                    //Read the Excel file as byte array
                    byte[] bin = File.ReadAllBytes(theDialog.FileName);

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    //create a new Excel package in a memorystream
                    using (MemoryStream stream = new MemoryStream(bin))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        if ((myStream = theDialog.OpenFile()) != null)
                        {
                            _dtTemp = ExcelPackageToDataTable(excelPackage);

                            openForm_Main(_dtTemp);
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } finally {
                    if (myStream != null)
                        myStream.Close();
                }
            }
        }

        private void openForm_Main(DataTable dt)
        {
            Form_Main form1 = new Form_Main(dt);

            // Define the border style of the form to a dialog box.
            form1.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            form1.MaximizeBox = false;

            // Set the MinimizeBox to false to remove the minimize box.
            //form1.MinimizeBox = false;

            //Form.TopMost will work unless the other program is creating topmost windows.
            form1.TopMost = true;

            // Set the start position of the form to the center of the screen.
            form1.StartPosition = FormStartPosition.CenterScreen;

            this.Hide();

            // Display the form as a modal dialog box.
            form1.ShowDialog();
        }

        public static DataTable ExcelPackageToDataTable(ExcelPackage excelPackage)
        {
            DataTable dt = new DataTable();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

            //check if the worksheet is completely empty
            if (worksheet.Dimension == null)
            {
                return dt;
            }

            dt.Columns.Add("Kanji");
            dt.Columns.Add("HanViet");
            dt.Columns.Add("Nghia");
            dt.Columns.Add("Status");
            dt.Columns["Status"].DataType = typeof(int);

            //start adding the contents of the excel file to the datatable
            for (int i = 3; i <= worksheet.Dimension.End.Row; i++)
            {
                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
                DataRow newRow = dt.NewRow();
                //Loop all cells in the row
                int index = 1;
                foreach (var cell in row)
                {
                    switch (index)
                    {
                        case 1:
                            newRow["Kanji"] = cell.Value;
                            index++;
                            break;
                        case 2:
                            newRow["HanViet"] = cell.Value;
                            index++;
                            break;
                        case 3:
                            newRow["Nghia"] = cell.Value;
                            index++;
                            break;
                        case 4:
                            newRow["Status"] = cell.Value;
                            index++;
                            break;
                    }
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }
    }
}
