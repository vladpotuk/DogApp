using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace DogApp
{
    public partial class MainWindow : Window
    {
        private HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetRandomDog_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://dog.ceo/api/breeds/image/random";
            await FetchAndDisplayImage(url);
        }

        private async void ListAllBreeds_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://dog.ceo/api/breeds/list/all";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(responseData);
            var breeds = json["message"].ToObject<Dictionary<string, List<string>>>();

            breedList.Items.Clear();
            foreach (var breed in breeds.Keys)
            {
                breedList.Items.Add(breed);
            }
        }

        private async void GetByBreed_Click(object sender, RoutedEventArgs e)
        {
            if (breedList.SelectedItem == null)
            {
                MessageBox.Show("Please select a breed first.");
                return;
            }

            string breed = breedList.SelectedItem.ToString();
            string url = $"https://dog.ceo/api/breed/{breed}/images/random";
            await FetchAndDisplayImage(url);
        }

        private async void GetBySubBreed_Click(object sender, RoutedEventArgs e)
        {
            if (breedList.SelectedItem == null)
            {
                MessageBox.Show("Please select a breed first.");
                return;
            }

            string breed = breedList.SelectedItem.ToString();
            string url = $"https://dog.ceo/api/breed/{breed}/list";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(responseData);
            var subBreeds = json["message"].ToObject<List<string>>();

            if (subBreeds.Count == 0)
            {
                MessageBox.Show("No sub-breeds available for this breed.");
                return;
            }

            string subBreed = subBreeds[0];  
            url = $"https://dog.ceo/api/breed/{breed}/{subBreed}/images/random";
            await FetchAndDisplayImage(url);
        }

        private async void BrowseBreeds_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://dog.ceo/api/breeds/list/all";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(responseData);
            var breeds = json["message"].ToObject<Dictionary<string, List<string>>>();

            breedList.Items.Clear();
            foreach (var breed in breeds.Keys)
            {
                breedList.Items.Add(breed);
            }
        }

        private async Task FetchAndDisplayImage(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(responseData);
            string imageUrl = json["message"].ToString();

            dogImage.Source = new BitmapImage(new Uri(imageUrl));
        }
    }
}
