using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Invoice_Genrator
{
    /// <summary>
    /// Interaction logic for CompanyInformation.xaml
    /// </summary>
    public partial class CompanyInformation : Window
    {
        public CompanyInformation()
        {
            InitializeComponent();
        }

        private void BTn_submit_Click(object sender, RoutedEventArgs e)
        {
            var companyName = TBx_companyName.Text;
            var companyAddress = TBx_companyAddress.Text;
            var companyEmail = TBx_companyEmail.Text;
            var companyContact = TBx_companyContact.Text;

            if (!companyName.Equals(""))
            {
                if (!companyAddress.Equals(""))
                {
                    if (!companyEmail.Equals("") && CheckEmail())
                    {
                        if (!companyContact.Equals(""))
                        {
                            // List<CompanyProfile> profile = new List<CompanyProfile>();
                            CompanyProfile companyProfile = new CompanyProfile { CompanyName = companyName, CompanyAddress = companyAddress, CompanyContact = companyContact, CompanyEmail = companyEmail };

                            // profile.Add(companyProfile);
                            Storage.WriteXML<CompanyProfile>(companyProfile, "CompanyProfile.xml");
                            var w = Application.Current.Windows[1];
                            // MainWindow mainWindow = new MainWindow();
                            // mainWindow.DisableWindow();
                            w.Close();

                        }
                        else
                        {
                            MessageBox.Show("Please Enter the Company Contact");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter the valid Company Email");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter the Company Address");
                }
            }
            else
            {
                MessageBox.Show("Please Enter the Company Name");
            }

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

        private void TBx_companyContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                (sender as TextBox).Text = (sender as TextBox).Text.Remove((sender as TextBox).Text.Length - 1);
            }
        }

    }
}
