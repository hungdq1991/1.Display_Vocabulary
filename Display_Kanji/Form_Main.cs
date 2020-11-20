using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Display_Kanji
{
    public partial class Form_Main : Form
    {
        DataTable dtKanji = new DataTable();
        int position_display_kanji = 0;

        public Form_Main(DataTable dataTable)
        {
            InitializeComponent();
            dtKanji = dataTable;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            Display_Kanji();
        }

        private void Display_Kanji()
        {
            String detail = dtKanji.Rows[position_display_kanji].Field<String>("Nghia").Replace(", ", Environment.NewLine);
            lbl_Kanji.Text = dtKanji.Rows[position_display_kanji].Field<String>("Kanji");
            txt_HanViet.Text = dtKanji.Rows[position_display_kanji].Field<String>("HanViet");
            txt_Detail.Text = ToUpperFirstLetter(detail);
        }

        public string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        private void Select_position()
        {
            //if (position_display_kanji == 0)
            //dtKanji.DefaultView.Sort = "Preferance ASC";
            position_display_kanji++;
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string exitMessageText = "Do you want to save your progress??";
            string exitCaption = "Confirm";
            MessageBoxButtons button = MessageBoxButtons.YesNo;
            DialogResult res = MessageBox.Show(exitMessageText, exitCaption, button, MessageBoxIcon.Exclamation);
            if (res == DialogResult.Yes)
            {
                
            } else if (res == DialogResult.No) {
                this.Hide();
            }
            
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Select_position();

            Display_Kanji();
        }
    }
}
