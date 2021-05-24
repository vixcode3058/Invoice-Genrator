using System.Collections.ObjectModel;

namespace Invoice_Genrator
{
    public class GenrateInvoice
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPostalCode { get; set; }
        public string ClientCity { get; set; }
        public ObservableCollection<ListItemCollection> ItemList { get; set; }
        public string grossAmount { get; set; }
    }
}