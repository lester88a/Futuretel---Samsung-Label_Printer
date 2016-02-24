using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Input;

namespace SamsungLabelPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //display the label printer
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                listBox1.Items.Add(printer);
            }
            
        }

        //move window around
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
        //exit application
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //switch text box
        private void textBoxPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxPartNo.Focus();

                e.Handled = true;
            }
        }

        private void textBoxPartNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                textBoxPONo.Focus();

                e.Handled = true;
            }
        }

        //boolean for check the generate button click
        private bool button1WasClicked = false;

        //generate button
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            //ensure the part order number is not empty
            if (textBoxPONo.Text.Length > 15)
            {
                MessageBox.Show("Sorry, IMIE only accept 15 digits!");
                textBoxPON.Text = "IMEI: " + "Error!";
            }
            else if (textBoxPONo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, IMIE cannot be empty!");
                textBoxPON.Text = "IMEI: " + "Error!";
            }
            else
            {
                textBoxPON.Text = "IMEI: " + textBoxPONo.Text;

                //ensure the part number is not empty
                if (textBoxPartNo.Text.Length <= 0)
                {
                    MessageBox.Show("Sorry, part code. cannot be empty!");
                    textBoxBarcode.Text = "*" + "Error!" + "*";
                    textBoxPN.Text = "Part Code: " + "Error!";
                }
                else
                {
                    textBoxAll.Text = textBoxPONo.Text + textBoxPartNo.Text;
                    textBoxBarcode.Text = textBoxPONo.Text + textBoxPartNo.Text;
                    textBoxPN.Text = "Part Code: " + textBoxPartNo.Text;
                    //textBoxDate.Text = DateTime.Now.ToString("MMM dd,yyyy");
                    button1WasClicked = true;
                }

            }
            //get selected copies
            //set number of copies to print
            int selectedValue = (int)comboBox1.SelectedIndex + 1;

            //show selected printer name
            //string printerName = listBox1.GetItemText(listBox1.SelectedItem);
            try
            {
                string printerName = listBox1.SelectedItem.ToString();
                toolStripStatusLabel1.Content = "Status:\t" + "Print Copies: " + selectedValue + ". Printer: " + printerName;
            }
            catch (Exception)
            {

                MessageBox.Show("Please select a printer!");
                toolStripStatusLabel1.Content = "Status:\t" + "Please select a printer!";
            }


        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            textBoxPONo.Focus();

            if (textBoxPONo.Text.Length > 15)
            {
                MessageBox.Show("Sorry, IMIE only accept maximum 15 digits!");
            }
            else if (textBoxPONo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, IMIE cannot be empty!");
            }
            else if (textBoxPartNo.Text.Length <= 0)
            {
                MessageBox.Show("Sorry, part code. cannot be empty!");
            }
            else if (button1WasClicked == false)
            {
                MessageBox.Show("Sorry, you have to generate before print!");
            }
            else
            {
                //get selected printer name
                string printerName = listBox1.SelectedItem.ToString();

                //print doc object
                System.Drawing.Printing.PrintDocument myPrintDocument1 = new System.Drawing.Printing.PrintDocument();

                //PrintDialog myPrinDialog1 = new PrintDialog();

                myPrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(myPrintDocument2_PrintPage);

                //use the selected printer name
                myPrintDocument1.PrinterSettings.PrinterName = printerName;

                //myPrintDocument1.DefaultPageSettings.Landscape = true;

                //myPrintDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Label2", 29, 20);

                //set number of copies to print
                int selectedValue = (int)comboBox1.SelectedIndex + 1;
                myPrintDocument1.PrinterSettings.Copies = (short)selectedValue;
                try
                {
                    toolStripStatusLabel1.Content = "Status:\t" + "Printing...";
                    myPrintDocument1.Print();
                    toolStripStatusLabel1.Content = "Status:\t" + "Succeed.";
                }
                catch (Exception)
                {

                    MessageBox.Show("Please select a printer!");
                    toolStripStatusLabel1.Content = "Status:\t" + "Please select a printer!";
                }

            }

            textBoxPONo.Text = "";
            textBoxPartNo.Text = "";
        }

        private void myPrintDocument2_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            //set font
            Font printFont = new Font("Code 128", 22);
            Font printFont2 = new Font("Arial", 6);
            Font printFont3 = new Font("Arial", 5);

            //set the bar-code width
            Matrix m = new Matrix();
            m.Scale(1F, 0.4F); //0.7 times height than original height

            //begin scale the bar-code width
            e.Graphics.Transform = m;
            string textBoxBarcode128 = BarcodeConverter128.StringToBarcode(textBoxBarcode.Text);
            e.Graphics.DrawString(textBoxBarcode128, printFont, System.Drawing.Brushes.Black, 2, 2, new StringFormat());
            //reset the font to regular width
            e.Graphics.ResetTransform();

            e.Graphics.DrawString(textBoxBarcode.Text, printFont2, System.Drawing.Brushes.Black, 45, 13, new StringFormat());

            e.Graphics.DrawString(textBoxPON.Text, printFont3, System.Drawing.Brushes.Black, 5, 22, new StringFormat());
            e.Graphics.DrawString(textBoxPN.Text, printFont3, System.Drawing.Brushes.Black, 113, 22, new StringFormat());
            //e.Graphics.DrawString(DateTime.Now.ToString("MMM dd,yyyy"), printFont2, System.Drawing.Brushes.Black, 175, 20, new StringFormat());


        }
    }
}
