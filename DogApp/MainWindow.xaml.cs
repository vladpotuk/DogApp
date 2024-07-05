using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace DogApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://dog.ceo/api/breeds/image/random";
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(responseData);
            string xmlData = JsonConvert.DeserializeXmlNode(json.ToString(), "Root").OuterXml;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            string imageUrl = xmlDoc.SelectSingleNode("//message").InnerText;

            dogImage.Source = new BitmapImage(new Uri(imageUrl));
        }
    }
}
