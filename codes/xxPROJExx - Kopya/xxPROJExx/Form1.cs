using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxPROJExx
{
    public partial class Form1 : Form
    {
        NumberStyles sayiStili;
        CultureInfo culture;


        SerialPort serialPort;
        List<Button> yonBuzzerLedSudanCikButonlari; 
        List<Button> sensorButonlari;
        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            sayiStili = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
            culture = CultureInfo.CreateSpecificCulture("en-GB");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort.PortName = "COM9";  
            yonBuzzerLedSudanCikButonlari = new List<Button>();  
            sensorButonlari = new List<Button>();
            yonBuzzerLedSudanCikButonlari.Add(buzzerDurdurButton); 
            yonBuzzerLedSudanCikButonlari.Add(buzzerCalButton);
            yonBuzzerLedSudanCikButonlari.Add(sagaButton);
            yonBuzzerLedSudanCikButonlari.Add(solaButton);
            yonBuzzerLedSudanCikButonlari.Add(geriButton);
            yonBuzzerLedSudanCikButonlari.Add(sudanCikButton);
            yonBuzzerLedSudanCikButonlari.Add(ledYakButton);
            yonBuzzerLedSudanCikButonlari.Add(ledSondurButton);
            sensorButonlari.Add(mesafeOlcButton);
            sensorButonlari.Add(sesAlgilaButton);
            sensorButonlari.Add(yagmurAlgilaButton);
            sensorButonlari.Add(sicaklikAlgilaButton);
            baglantiKapatButton.Enabled = false;  

        }

        private void baglanButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen) 
                {

                    serialPort.Open();  
                    AraclariSifirla(); 

                    baglantiLabel.Text = "Bağlantı kuruldu.Yapabileceğiniz işlemlerin ilgili butonları aktifleştirildi."; 
                    baglantiKapatLabel.Text = "Bağlantıyı kapatmak için lütfen butona tıklayınız";   

                    buttonDurumRenkAyari(baglanButton, Color.PaleGreen, false); 
                    buttonDurumRenkAyari(baglantiKapatButton, Color.WhiteSmoke, true);

                    foreach (Button btn in sensorButonlari)
                    {
                        btn.Enabled = true;
                        btn.BackColor = Color.Gold;
                    }
                    buttonDurumRenkAyari(ileriButton, Color.Gold, true);
                    buttonDurumRenkAyari(durButton, Color.Gold, true);

                }
            }
            catch
            {
                baglantiLabel.Text = "Seri port hatası!";
            }
        }

        private void baglantiKapatButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("s");  
            serialPort.Close();     

            buttonDurumRenkAyari(baglantiKapatButton, Color.Tomato, false);
            buttonDurumRenkAyari(baglanButton, Color.WhiteSmoke, true);

            baglantiKapatLabel.Text = "Bağlantı kapatıldı."; 
            baglantiLabel.Text = "Bağlantı kurmak için lütfen butona tıklayınız";  
            AraclariSifirla();
            foreach (Button btn in sensorButonlari)
            {
                btn.Enabled = false;        
                btn.BackColor = Color.Silver;
            }

            buttonDurumRenkAyari(ileriButton, Color.Silver, false);
            buttonDurumRenkAyari(durButton, Color.Silver, false);
        }
        private void buttonDurumRenkAyari(Button buton, Color renk, bool durum)
        {
            buton.Enabled = durum;
            buton.BackColor = renk;
        }

        private void AraclariSifirla() 
        {                                              
            bildirimLabel.Text = "";
            mesafeDegerLabel.Text = "";
            yagmurDegerLabel.Text = "";
            sicaklikDegerLabel.Text = "";
            foreach (Button btn in yonBuzzerLedSudanCikButonlari)
            {
                btn.Enabled = false;        
                btn.BackColor = Color.Silver;

            }

            mesafeOlcTimer.Stop();
            sesAlgilaTimer.Stop();
            yagmurAlgilaTimer.Stop();
            sicaklikAlgilaTimer.Stop();

        }

        private void ileriButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("w");
        }

        private void sagaButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("d");
        }

        private void solaButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("a");
        }

        private void geriButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("x");
        }

        private void durButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("s");
        }

        private void buzzerDurdurButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("b");
        }

        private void buzzerCalButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("g");
        }

        private void mesafeOlcButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("m");
            AraclariSifirla();
            mesafeOlcTimer.Start();
        }

        private void mesafeOlcTimer_Tick(object sender, EventArgs e)
        {

            double mesafe;
            string mesafeString = serialPort.ReadExisting();
            bool mesafeDurum = Double.TryParse(mesafeString, sayiStili, culture, out mesafe);
            if (mesafeDurum)
            {

                mesafeDegerLabel.Text = "Mesafe: " + mesafe + " cm";
                if (mesafe <= 50 && mesafe != 0)
                {
                    bildirimLabel.Text = "DİKKAT! Bir cisme 50 cm veya daha az mesafede yakınsınız! Yapabileceğiniz işlemlerin ilgili butonları aktifleştirildi.";
                    serialPort.Write("s");
                    buttonDurumRenkAyari(sagaButton, Color.Gold, true);
                    buttonDurumRenkAyari(solaButton, Color.Gold, true);
                    mesafeOlcTimer.Stop();
                }
                else
                {
                    serialPort.Write("m");
                }

            }
            else
            {
                serialPort.Write("m");
            }

        }

        private void sesAlgilaButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("v");
            AraclariSifirla();
            sesAlgilaTimer.Start();
        }

        private void sesAlgilaTimer_Tick(object sender, EventArgs e)
        {
            int sesDeger;
            string sesString = serialPort.ReadExisting();
            bool sesDurum = Int32.TryParse(sesString, out sesDeger);
            if (sesDurum)
            {
                if (sesDeger == 111)
                {
                    bildirimLabel.Text = "Ses Algılandı! Yapabileceğiniz işlemlerin ilgili butonları aktifleştirildi.";
                    serialPort.Write("s");
                    buttonDurumRenkAyari(sagaButton, Color.Gold, true);
                    buttonDurumRenkAyari(solaButton, Color.Gold, true);
                    buttonDurumRenkAyari(geriButton, Color.Gold, true);
                    buttonDurumRenkAyari(buzzerCalButton, Color.Gold, true);
                    buttonDurumRenkAyari(buzzerDurdurButton, Color.Gold, true);
                    sesAlgilaTimer.Stop();
                }


            }
           

        }

        private void yagmurAlgilaButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("r");
            AraclariSifirla();
            yagmurAlgilaTimer.Start();
        }

        private void yagmurAlgilaTimer_Tick(object sender, EventArgs e)
        {
            int suDegeri;
            string suString = serialPort.ReadExisting();
            bool suDurum = Int32.TryParse(suString, out suDegeri);
            if (suDurum)
            {
                yagmurDegerLabel.Text = "Okunan su değeri: " + suDegeri;
                if (suDegeri < 400)
                {
                    serialPort.Write("s");
                    bildirimLabel.Text = "Su algılandı! Yapabileceğiniz işlemlerin ilgili butonları aktifleştirildi.";
                    buttonDurumRenkAyari(sudanCikButton, Color.Gold, true);
                    yagmurAlgilaTimer.Stop();
                }
                else
                {
                    bildirimLabel.Text = "Su ile temas edilmiyor.";
                    serialPort.Write("r");
                }

            }
            else
            {
                serialPort.Write("r");
            }

        }

        private void sicaklikAlgilaButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("c");
            AraclariSifirla();
            sicaklikAlgilaTimer.Start();
        }

        private void sicaklikAlgilaTimer_Tick(object sender, EventArgs e)
        {
            double sicaklikDegeri;
            string sicaklikString = serialPort.ReadExisting();
            bool sicaklikDurum = Double.TryParse(sicaklikString, sayiStili, culture, out sicaklikDegeri);
            if (sicaklikDurum)
            {

                sicaklikDegerLabel.Text = "Sicaklik: " + sicaklikDegeri + "°C";
                if (sicaklikDegeri >= 24)
                {
                    serialPort.Write("s");
                    bildirimLabel.Text = "Sıcaklık 24 °C veya 24 °C üstünde! Yapabileceğiniz işlemlerin ilgili butonları aktifleştirildi.";
                    buttonDurumRenkAyari(sagaButton, Color.Gold, true);
                    buttonDurumRenkAyari(solaButton, Color.Gold, true);
                    buttonDurumRenkAyari(geriButton, Color.Gold, true);
                    buttonDurumRenkAyari(ledYakButton, Color.Gold, true);
                    buttonDurumRenkAyari(ledSondurButton, Color.Gold, true);
                    sicaklikAlgilaTimer.Stop();
                }
                else
                {
                    serialPort.Write("c");
                }

            }
            else
            {
                serialPort.Write("c");
            }

        }

        private void cikisButton_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write("s");  //Çıkmadan önce arabayı durduruyoruz
            }
            this.Close(); //Uygulamayı kapatıyoruz
        }

        private void sudanCikButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("w");
            AraclariSifirla();
            sudanCikTimer.Start();
        }

        private void sudanCikTimer_Tick(object sender, EventArgs e)
        {
            int suDegeri;
            string suString = serialPort.ReadExisting();
            bool suDurum = Int32.TryParse(suString, out suDegeri);
            if (suDurum)
            {
                yagmurDegerLabel.Text = "Okunan su değeri: " + suDegeri;
                if (suDegeri > 400)
                {
                    bildirimLabel.Text = "Su ile temas edilmiyor.";
                    buttonDurumRenkAyari(ileriButton, Color.Gold, true);
                    sudanCikTimer.Stop();

                }
                else
                {
                    bildirimLabel.Text = "Hala su ile temas halindesiniz!";
                    serialPort.Write("r");
                }

            }
            else
            {
                serialPort.Write("r");
            }
        }

        private void ledYakButton_Click(object sender, EventArgs e)
        {
            serialPort.Write("y");
        }

        private void ledSondurButton_Click(object sender, EventArgs e)
        {

            serialPort.Write("l");
        }
    }
}
