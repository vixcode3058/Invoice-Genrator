using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace Invoice_Genrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CompanyProfile companyProfile;
        List<GenrateInvoice> list;

        static CompanyInformation companyInformation;
        double grossTotal = 0.0;
        private ObservableCollection<ListItemCollection> itemsCollection;
        private ObservableCollection<ClientInformation> clientInformation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            companyInformation = new CompanyInformation();
            itemsCollection = new ObservableCollection<ListItemCollection>();

            getDate();
            if (!File.Exists("CompanyProfile.xml"))
            {
                companyInformation.ShowDialog();
            }
            else
            {

                companyProfile = Storage.ReadXml<CompanyProfile>("CompanyProfile.xml");
                InitilizeField();
            }
            if (!File.Exists("Invoice.xml"))
            {
              
                list = new List<GenrateInvoice>();
                TextBlock_Invoice.Text = "1";

            }
            else
            {
                list = Storage.ReadXml<List<GenrateInvoice>>("Invoice.xml");
                TextBlock_Invoice.Text = (list.Count + 1) + "";
                LBx_invoiceList.ItemsSource = list;

            }
            if (!File.Exists("ClientInformation.xml"))
            {
                clientInformation = new ObservableCollection<ClientInformation>();
            }
            else
            {
                clientInformation = Storage.ReadXml<ObservableCollection<ClientInformation>>("ClientInformation.xml");
                LBx_clientList.ItemsSource = clientInformation;
            }

        }

        private void InitilizeField()
        {
            TBx_companyName.Text = companyProfile.CompanyName;
            TBx_companyAddress.Text = companyProfile.CompanyAddress;
            TBx_companyEmail.Text = companyProfile.CompanyEmail;
            TBx_contact.Text = companyProfile.CompanyContact;
        }
        
        private void BTn_updateCompanyInfo_Click(object sender, RoutedEventArgs e)
        {
            var companyName = TBx_companyName.Text;
            var companyAddress = TBx_companyAddress.Text;
            var companyEmail = TBx_companyEmail.Text;
            var companyContact = TBx_contact.Text;

            if (companyName != null)
            {
                if (companyAddress != null)
                {
                    if (companyEmail != null & CheckEmail())
                    {
                        if (companyContact != null & NumberValidationForComapny(TBx_contact))
                        {
                            companyProfile.CompanyName = companyName;
                            companyProfile.CompanyAddress = companyAddress;
                            companyProfile.CompanyEmail = companyEmail;
                            companyProfile.CompanyContact = companyContact;

                            Storage.WriteXML<CompanyProfile>(companyProfile, "CompanyProfile.xml");
                            MessageBox.Show("Information Updated Successfully");
                        }
                        else
                        {
                            MessageBox.Show("Please Enter the Valid Contact Number");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter the Valid Email");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter the Comapny Address");

                }
            }
            else
            {
                MessageBox.Show("Please Enter the Comapny Name");
            }
        }

        private bool NumberValidationForComapny(TextBox bx_contact)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(bx_contact.Text, "[^0-9]"))
            {
                return false;
            }
            return true;
        }

        // Check Validation to genrate Invoice
        private void BTn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (TBx_clientName.Text != "")
            {
                if (TBx_clientAddress.Text != "")
                {
                    if (TBx_postalCode.Text != "")
                    {
                        if (TBx_City.Text != "")
                        {
                            genrateInvoice(sender);
                        }
                        else
                        {
                            MessageBox.Show("Please enter City.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter Postal Code.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter Client Address.");
                }
            }
            else
            {
                MessageBox.Show("Please enter Client Name.");
            }

        }
        // Genrate Invoice and write Data in XML and Also create the client List
        private void genrateInvoice(object sender)
        {

            GenrateInvoice invoice = new GenrateInvoice();
            invoice.InvoiceNumber = TextBlock_Invoice.Text;
            invoice.InvoiceDate = TextBlock_InvoiceDate.Text;
            invoice.ClientName = TBx_clientName.Text;
            invoice.ClientAddress = TBx_clientAddress.Text;
            invoice.ClientPostalCode = TBx_postalCode.Text;
            invoice.ClientCity = TBx_City.Text;
            invoice.ItemList = itemsCollection;
            invoice.grossAmount = TextBlock_grossTotal.Text;

            list.Add(invoice);
            TextBlock_Invoice.Text = "";
            TBx_City.Text = "";
            TBx_clientAddress.Text = "";
            TBx_clientName.Text = "";
            TBx_postalCode.Text = "";
            grossTotal = 0.0;
            TextBlock_grossTotal.Text = "00.00";

            DG_itemDetails.ItemsSource = null;
            itemsCollection = new ObservableCollection<ListItemCollection>();
            MessageBox.Show("Invoice Genrated");
            TextBlock_Invoice.Text = (list.Count + 1).ToString();

        }
        private void BTn_clientInfoUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        // To Add Items In the Item Grid
        private void BTn_addItem_Click(object sender, RoutedEventArgs e)
        {
            if (TBx_description.Text != "")
            {
                if (TBx_price.Text != "")
                {

                    if (TBx_quantity.Text != "")
                    {
                        int total = (int.Parse(TBx_price.Text) * int.Parse(TBx_quantity.Text));
                        grossTotal = grossTotal + total;
                        TextBlock_grossTotal.Text = grossTotal.ToString();

                        var item = new ListItemCollection { Description = TBx_description.Text, Price = TBx_price.Text, Quantity = TBx_quantity.Text, Total = total.ToString() };
                        itemsCollection.Add(item);
                        DG_itemDetails.ItemsSource = itemsCollection;

                        TBx_description.Text = "";
                        TBx_price.Text = "";
                        TBx_quantity.Text = "";

                    }
                    else
                    {
                        MessageBox.Show("Please Enter the Quantity");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter the Price");
                }
            }
            else
            {
                MessageBox.Show("Please Enter the Item Name");
            }

        }
        private void NumberValidation(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                (sender as TextBox).Text = (sender as TextBox).Text.Remove((sender as TextBox).Text.Length - 1);
            }
        }

        // Set the Company Details in the field when window activated
        private void Window_Activated(object sender, EventArgs e)
        {
            if (!File.Exists("CompanyProfile.xml"))
            {
                CompanyInformation companyInformation = new CompanyInformation();
                companyInformation.ShowDialog();
            }
            else
            {

                companyProfile = Storage.ReadXml<CompanyProfile>("CompanyProfile.xml");
                InitilizeField();
            }
        }
        // Remove item from Data Grid
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DG_itemDetails.SelectedItem == null)
            {
                MessageBox.Show("Please Select the item to Remove");
            }
            else
            {
                grossTotal = 0.0;
                itemsCollection.RemoveAt(DG_itemDetails.SelectedIndex);
                foreach (var item in itemsCollection)
                {
                    grossTotal = (grossTotal + int.Parse(item.Total));
                }
                TextBlock_grossTotal.Text = grossTotal.ToString();
                DG_itemDetails.ItemsSource = itemsCollection;
            }
        }

        // Filter for Invoice Histroy
        private void TBx_searchInvoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            var lst = from s in list where s.ClientName.ToLower().Contains(TBx_searchInvoice.Text.ToLower()) select s;
            LBx_invoiceList.ItemsSource = lst;
        }

        private void BTn_add_client(object sender, RoutedEventArgs e)
        {
            var client = new ClientInformation { ClientName = "Please Edit", ClientAddress = "Please Edit", ClientCity = "Please Edit", ClientPostal = " Please Edit" };


            clientInformation.Add(client);
            LBx_clientList.ItemsSource = clientInformation;

        }

        private void BTn_remove_client(object sender, RoutedEventArgs e)
        {
            if (LBx_clientList.SelectedItem == null)
            {
                MessageBox.Show("Please Select client to Remove");
            }
            else
            {
                clientInformation.Remove(LBx_clientList.SelectedItem as ClientInformation);
                MessageBox.Show("Client Removed");
            }
        }

        private void getDate()
        {
            DateTime date = DateTime.Today;
            TextBlock_InvoiceDate.Text = date.ToString("d");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Storage.WriteXML<ObservableCollection<ClientInformation>>(clientInformation, "ClientInformation.xml");
            Storage.WriteXML<List<GenrateInvoice>>(list, "Invoice.xml");
        }
        private bool CheckEmail()
        {
            try
            {
                MailAddress m = new MailAddress(TBx_companyEmail.Text);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
