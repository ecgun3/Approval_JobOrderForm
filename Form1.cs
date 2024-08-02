using System.Data;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace İsEmri_Alıcı
{
    public partial class Form1 : Form
    {
        SqlConnection connectionString = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);

        private DataTable dt;
        public Form1()
        {
            InitializeComponent();

            //Keypress
            isEmriNo2.KeyPress += new KeyPressEventHandler(isEmriNo2_KeyPress);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Combobox'u dolduralım: 
            FillIsEmriNoComboBox();

            isEmriNo2.Focus();
        }

        private void FillIsEmriNoComboBox()
        {
            try
            {
                connectionString.Open();

                //SQL
                string query = "SELECT İsEmriNo FROM IsTB";

                SqlCommand command = new SqlCommand(query, connectionString);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isEmriNo2.Items.Add(reader["İsEmriNo"].ToString());
                }

                reader.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Veriler akınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }

        //KeyPress Olaylar:
        private void isEmriNo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlar ve geri silme karakterine izin 
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Sadece rakam giriniz.");
            }
        }

        private void VeriGetirbtn_Click(object sender, System.EventArgs e)
        {
            #region try-catch (Eşleştirme silme)
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                if (connectionString.State != ConnectionState.Open)
                    connectionString.Open();


                //SQL Query:
                string query = "SELECT * FROM IsTB WHERE İsEmriNo = @isEmriNo2";

                adapter.SelectCommand = new SqlCommand(query, connectionString);

                // @isEmriNo parametresini TextBox'tan al
                adapter.SelectCommand.Parameters.AddWithValue("@isEmriNo2", isEmriNo2.Text);

                //İş numarası girmeden veri getir butonuna basarksak:
                if (string.IsNullOrWhiteSpace(isEmriNo2.Text))
                    MessageBox.Show("Lütfen bir İş Emri Numarası giriniz");
                else
                { 
                    dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        //İsEmriNo3 adlı textbox'a SQL'de İsEmriNo adlı sütundakini getir
                        İsEmriNo3.Text = row["İsEmriNo"].ToString();

                        //İsiTalepEdenTxtbox2 adlı texbox'a SQL de İsiTalepEden adlı sütun 
                        İsiTalepEdenTxtbox2.Text = row["İsiTalepEden"] != DBNull.Value ? row["İsiTalepEden"].ToString() : string.Empty;

                        //VerilenIsTanım2Txtbox adlı textboxa SQL de VerilenIsinTanimi adlı sütun
                        VerilenIsTanım2Txtbox.Text = row["VerilenIsinTanimi"] != DBNull.Value ? row["VerilenIsinTanimi"].ToString() : string.Empty;

                        //İsTanim2 adlı textbox SQL de IsTanimi
                        İsTanim2.Text = row["IsTanimi"] != DBNull.Value ? row["IsTanimi"].ToString() : string.Empty;

                        //ProjeKodutxtBox2 adlı textbox SQL de ProjeKodu
                        ProjeKodutxtBox2.Text = row["ProjeKodu"] != DBNull.Value ? row["ProjeKodu"].ToString() : string.Empty;

                        //EvetChckBox2 adlı checkbox,Eğer SQL 'de Oneri adlı sütunda Evet yazıyorsa seçili gelecek
                        EvetChckBox2.Checked = row["Oneri"].ToString().Equals("Evet", StringComparison.OrdinalIgnoreCase);

                        //HayırChckBox2 adlı checlbox, eğer SQL ' de Oneri adlı sütunda Hayor yazıyorsa seçili olacak.
                        HayırChckBox2.Checked = row["Oneri"].ToString().Equals("Hayır", StringComparison.OrdinalIgnoreCase);

                        //OneriNo2 adlı textbox eğer OneriNo NULL değil ise SQL ' de doldurulsun. Null ise boş kalsın
                        OneriNo2.Text = row["OneriNo"] != DBNull.Value ? row["OneriNo"].ToString() : string.Empty;

                        //IsinYapildigiBlmTxtbox adlı textbox SQL'de IsınYapildigiBolumden gelecek
                        IsinYapildigiBlmTxtbox.Text = row["IsınYapildigiBolum"] != DBNull.Value ? row["IsınYapildigiBolum"].ToString() : string.Empty;

                        //ProDurumuSeriChckBox2 adlı checkbox, ProjeDurumu adlı sütunda Seri yazıyorsa seçili olsun
                        ProDurumuSeriChckBox2.Checked = row["ProjeDurumu"].ToString().Equals("Seri", StringComparison.OrdinalIgnoreCase);

                        //ProDurumuProjeChckbox2 adlı chkcbox, ProjeDurumu adlı sütunda Proje yazıyorsa seçili olsun 
                        ProDurumuProjeChckbox2.Checked = row["ProjeDurumu"].ToString().Equals("Proje", StringComparison.OrdinalIgnoreCase);

                        //IsiVerenBolum2 adlı texbox, IsıVerenBolum adlı sütundan gelecek
                        IsiVerenBolum2.Text = row["IsıVerenBolum"] != DBNull.Value ? row["IsıVerenBolum"].ToString() : string.Empty;

                        //GondermeTarihitxtbox2 adlı textbox, SQL 'DE GondermeTarihi adlı sütundan gesin 
                        GondermeTarihitxtbox2.Text = row["GondermeTarihi"] != DBNull.Value ? row["GondermeTarihi"].ToString() : string.Empty;

                    }
                    else
                    {
                        MessageBox.Show("İş emri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri alınamadı: " + ex.Message);
            }
            finally
            {
                if (connectionString.State != ConnectionState.Closed)
                {
                    connectionString.Close();
                }
                adapter.Dispose();
                adapter = null;
            }
            #endregion
        }

        //Checkbox durumları:
        private void UygunChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(UygunDeilChckBox.Checked)
                UygunChckBox.Checked = false;
        }

        private void UygunDeilChckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = UygunDeilChckBox.Checked;

            UygunOlanTarihLabel.Visible = isChecked;
            dateTimePicker2.Visible = isChecked;

            //Eğer Uygun seçeneği seçildiyse Diğerleri görünmez kalsın 
            if (!isChecked)
            {
                dateTimePicker2.Visible = false;
                UygunOlanTarihLabel.Visible = false;
            }

            //Uygun seçili ise uygundeğil aynı anda seçilemez olsun (Seçiliden çıkartman gerekiyor diğerini seçmek için)
            if (UygunChckBox.Checked)
                UygunDeilChckBox.Checked = false;
        }

        //Onay Butonu
        private void OnayButon_Click(object sender, EventArgs e)
        {
            SebepLabel.Visible = false;
            SebebiTxt.Visible = false;
        }

        //OnayNo butonu
        private void OnayNoButon_Click(object sender, EventArgs e)
        {
            SebebiTxt.Visible = true;
            SebepLabel.Visible = true;
            
            SebebiTxt.Focus();
        }

        private void FormKayıtButon_Click(object sender, EventArgs e)
        {
            Kaydet();

            //Temizlemeler:
            OnayVerentxtbox.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now; //İstenen Teslim tarihi 
            UygunChckBox.Checked = false;
            UygunDeilChckBox.Checked = false;
            UygunOlanTarihLabel.Visible = false;
            dateTimePicker2.Value = DateTime.Now; //uygun teslim tarihi
            dateTimePicker2.Visible = false;
            SebepLabel.Visible = false;
            KaliphaneTextbox.Text = string.Empty;
            AciklamaTextbox.Text = string.Empty;
            OnayButon.Checked = false;
            SebebiTxt.Text = string.Empty;
            OnayNoButon.Checked = false;
        }

        #region TRY-CATCH
        private void Kaydet()
        {
            try
            {
                connectionString.Open();

                // SQL sorgusu
                string query = @"INSERT INTO OnayTB (İsEmriNo,
                                                    İsTalepOnayiVeren, 
                                                    İstenenTeslimTarihi, 
                                                    İsTeslimTarihiUygunlugu,
                                                    UygunOlanTeslimTarihi,
                                                    KaliphaneAmiri,
                                                    Aciklama,
                                                    OnayDurumu,
                                                    Sebebi)

                                VALUES  (@İsEmriNo,
                                        @İsTalepOnayiVeren,
                                        @İstenenTeslimTarihi,
                                        @İsTeslimTarihiUygunlugu, 
                                        @UygunOlanTeslimTarihi, 
                                        @KaliphaneAmiri,
                                        @Aciklama,
                                        @OnayDurumu,
                                        @Sebebi)";

                SqlCommand command = new SqlCommand(query, connectionString);

                //İsEmriNo (int)
                command.Parameters.AddWithValue("@İsEmriNo", İsEmriNo3.Text);

                //İsTalepOnayiVeren
                command.Parameters.AddWithValue("@İsTalepOnayiVeren", OnayVerentxtbox.Text);

                //İstenenTeslimTarihi --> dateTimePicker1
                command.Parameters.AddWithValue("@İstenenTeslimTarihi", dateTimePicker1.Value);

                //İsTeslimTarihiUygunlugu (Checkbox)
                if (UygunChckBox.Checked)
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu", "Uygun");
                else if (UygunDeilChckBox.Checked)
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu", "Uygun Değil");
                else
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu",DBNull.Value);

                //UygunOlanTeslimTarihi --> dateTimePicker
                command.Parameters.AddWithValue("@UygunOlanTeslimTarihi", dateTimePicker2.Value);

                //KaliphaneAmiri
                command.Parameters.AddWithValue("@KaliphaneAmiri", KaliphaneTextbox.Text);

                //Aciklama
                command.Parameters.AddWithValue("@Aciklama",AciklamaTextbox.Text);

                //Onay Butonu
                if (OnayButon.Checked)
                {
                    command.Parameters.AddWithValue("@OnayDurumu", "Onaylıyorum");
                    command.Parameters.AddWithValue("@Sebebi", DBNull.Value);
                }
                else if (OnayNoButon.Checked)
                {
                    command.Parameters.AddWithValue("@OnayDurumu", "Onaylamıyorum");
                    command.Parameters.AddWithValue("@Sebebi", SebebiTxt.Text);
                }
                else
                {
                    MessageBox.Show("Onay butonları boş bırakılamaz!");
                    return;
                }

                command.ExecuteNonQuery();
                MessageBox.Show("Veritabanına başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına kaydedilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }
        #endregion

    }
}
