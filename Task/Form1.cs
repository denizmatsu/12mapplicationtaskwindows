using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Task.DTO;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Export;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting.Preview;
namespace Task
{
    public partial class Form1 : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private readonly ApiConnection _apiConnection = new ApiConnection(new HttpClient(), Program.ApiUrl);
        public Form1()
        {
            var options = new XlsxExportOptions();
            options.TextExportMode = TextExportMode.Value;
            options.ExportMode = XlsxExportMode.SingleFile;
            options.SheetName = "Sheet1";
            options.ShowGridLines = true;
            InitializeComponent();
            var builder = new ConfigurationBuilder()
            .SetBasePath(Application.StartupPath);

        }

        public async void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                var products = await _apiConnection.GetStockNameAsync();
                comboBoxEdit1.Properties.Items.AddRange(products.Select(p => p.malKodu).ToArray());
                comboBoxEdit2.Properties.Items.AddRange(products.Select(p => p.malAdi).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving product list: " + ex.Message);
            }
        }

        private async void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEdit1.SelectedIndex != -1)
                {
                    var selectedMalKodu = comboBoxEdit1.SelectedItem.ToString();
                    var products = await _apiConnection.GetStockNameAsync();
                    var selectedProduct = products.FirstOrDefault(p => p.malKodu == selectedMalKodu);
                    if (selectedProduct != null)
                    {
                        comboBoxEdit2.SelectedItem = selectedProduct.malAdi;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving product details: " + ex.Message);
            }
        }

        private async void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEdit2.SelectedIndex != -1)
                {
                    var selectedMalAdi = comboBoxEdit2.SelectedItem.ToString();
                    var products = await _apiConnection.GetStockNameAsync();
                    var selectedProduct = products.FirstOrDefault(p => p.malAdi == selectedMalAdi);
                    if (selectedProduct != null)
                    {
                        comboBoxEdit1.SelectedItem = selectedProduct.malKodu;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving product details: " + ex.Message);
            }
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var malKodu = comboBoxEdit1.SelectedItem.ToString();
                var girisTarih = dateEdit1.DateTime.ToUniversalTime();
                var cikisTarih = dateEdit2.DateTime.ToUniversalTime();

                var filter = new FilterDTO
                {
                    malKodu = malKodu,
                    girisTarih = girisTarih,
                    cikisTarih = cikisTarih
                };
                var stock = await _apiConnection.GetStockStatementAsync(filter);
                gridControl1.DataSource = stock;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving stock statement: " + ex.Message);
            }

        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "output.xlsx";
                gridControl1.ExportToXlsx(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving stock statement: " + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "output.pdf";
                gridControl1.ExportToPdf(path);
                // Open the created XLSX file with the default application.
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while retrieving stock statement: " + ex.Message);
            }
        }
        private void gridView1_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "sira No")
            {
                e.Info.Caption = "Mal Kodu";
            }
            if (e.Column.FieldName == "siraNo")
            {
                e.Info.Caption = "Sıra No";
            }
            if (e.Column.FieldName == "evrakNo")
            {
                e.Info.Caption = "Evrak No";
            }
            if (e.Column.FieldName == "tarih")
            {
                e.Info.Caption = "Tarih";
            }
            if (e.Column.FieldName == "girisMiktar")
            {
                e.Info.Caption = "Giriş Miktar";
            }
            if (e.Column.FieldName == "cikisMiktar")
            {
                e.Info.Caption = "New Miktar";
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = gridControl1;
            link.CreateDocument();
            link.ShowPreview();
        }
    }


}
