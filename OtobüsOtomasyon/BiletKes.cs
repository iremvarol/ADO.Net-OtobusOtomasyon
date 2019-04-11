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
using Microsoft.VisualBasic;

namespace OtobüsOtomasyon
{
    public partial class BiletKes : Form
    {
        public BiletKes()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(Baglanti.sqlBaglanti());
        DataSet ds = new DataSet();

        private void BiletKes_Load(object sender, EventArgs e)
        {
            butonEkle();
            FormLoadIcerik();
            
        }

        
        //Butonları Evente Yükle
        private void butonEkle()
        {
            int i = 1;
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    control.Click += new EventHandler(button_Click);
                    control.Text = i.ToString();
                    control.Name = "btn" + i;
                    control.BackColor = Color.Gray;
                    i++;
                }
            }
        }
        //Buton Event
        private void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Biletkes VALUES(@Tarih,@SeferSaati,@Kalkis,@Varis,@KoltukNo,@AdSoyad)", conn);
            cmd.Parameters.AddWithValue("@Tarih", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@SeferSaati", cbSeferSaati.SelectedItem);
            cmd.Parameters.AddWithValue("@Kalkis", cbSehirKalkis.SelectedValue);
            cmd.Parameters.AddWithValue("@Varis", cbSehirVaris.SelectedValue);
            cmd.Parameters.AddWithValue("@KoltukNo", button.Text);
            cmd.Parameters.AddWithValue("@AdSoyad", Microsoft.VisualBasic.Interaction.InputBox("İsim ve Soyisim Giriniz:", "İsim Soyİsim", ""));
            SqlCommand command = new SqlCommand("Select fiyat from fiyatlandirma where kalkis = @kalkis and varis = @varis", conn);
            command.Parameters.AddWithValue("@Kalkis", cbSehirKalkis.SelectedValue);
            command.Parameters.AddWithValue("@Varis", cbSehirVaris.SelectedValue);

            int a = Convert.ToInt16(command.ExecuteScalar());
            DialogResult dialogResult = MessageBox.Show("Fiyatınız: " + a + ", Onaylıyor musunuz ", "Onay", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                cmd.ExecuteNonQuery();
                button.BackColor = Color.LightGreen;
                button.Enabled = false;
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

            conn.Close();

        }

        //Form Load İçerik
        private void FormLoadIcerik()
        {
            string[] seferSaat = { "19", "21", "23" };
            cbSeferSaati.Items.AddRange(seferSaat);
            cbSeferSaati.SelectedIndex = -1;

            string[] cinsiyet = { "Erkek", "Kadın" };
            CbCinsiyet.Items.AddRange(cinsiyet);
            CbCinsiyet.SelectedIndex = 0;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Sehir;SELECT * FROM Sehir", conn);
            adapter.Fill(ds, "Sehirler");
            adapter.Fill(ds, "Sehirler2");

            cbSehirKalkis.DataSource = ds.Tables["Sehirler"];
            cbSehirKalkis.DisplayMember = "SehirAdi";
            cbSehirKalkis.ValueMember = "SehirNo";
            cbSehirKalkis.SelectedIndex = -1;

            cbSehirVaris.DataSource = ds.Tables["Sehirler2"];
            cbSehirVaris.DisplayMember = "SehirAdi";
            cbSehirVaris.ValueMember = "SehirNo";
            cbSehirVaris.SelectedIndex = -1;

            dataGridView1.Visible = false;

            //cbSehirVaris.Visible = false;
            //dateTimePicker1.Visible = false;
            //cbSeferSaati.Visible = false;

            dateTimePicker1.Value = dateTimePicker1.MinDate;
        }
       
        //dtpickerdan zaman seçme ve buton devre dışı bırakma
        private void ZamanVeKoltukSecipButonDevreDısıBırakma()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    control.BackColor = Color.Gray;
                    control.Enabled = true;
                }
            }

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            string dtpValue = dateTimePicker1.Text.ToString();
            string sub = dtpValue.Substring(0, 10);

            conn.Open();

            SqlDataAdapter da;
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT KoltukNumarasi FROM Biletkes WHERE SeferTarihi LIKE '%' + @tarih + '%' AND SeferSaati LIKE '%' + @SeferSaati + '%' AND KalkisYeri=@kalkis AND VarisYeri=@Varis", conn);
            cmd.Parameters.AddWithValue("@tarih", sub);
            cmd.Parameters.AddWithValue("@SeferSaati", cbSeferSaati.SelectedItem);
            cmd.Parameters.AddWithValue("@kalkis", Convert.ToInt16(cbSehirKalkis.SelectedValue));
            cmd.Parameters.AddWithValue("@Varis", Convert.ToInt16(cbSehirVaris.SelectedValue));
            

            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == control.Text)
                        {
                            control.Enabled = false;
                            control.BackColor = Color.LightGreen;
                        }
                    }
                }
            }
            dataGridView1.DataSource = null;
            conn.Close();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            cbSeferSaati.Visible = true;
            try { ZamanVeKoltukSecipButonDevreDısıBırakma(); }catch(Exception ex)
            {
                conn.Close();
            }
            
        }
        private void cbSeferSaati_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZamanVeKoltukSecipButonDevreDısıBırakma();
        }
        private void cbSehirKalkis_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSehirVaris.Visible = true;
        }

        private void cbSehirVaris_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = true;
        }
    }
}
