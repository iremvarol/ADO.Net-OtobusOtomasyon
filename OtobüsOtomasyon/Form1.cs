using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OtobüsOtomasyon
{
    public partial class Form1 : Form
    {
        SqlConnection connection = new SqlConnection(Baglanti.sqlBaglanti());


        private void btnBiletKes_Click(object sender, EventArgs e)
        {
            BiletKes blt = new BiletKes();
            blt.Show();
        }




        public Form1()
        {
            InitializeComponent();
        }
    }
}
