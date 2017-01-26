using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace HackathonPro
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Problem> problems;

        public MainPage()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Subir", "upload.png", ()=>{
                this.Navigation.PushAsync(new UploadPage() { Title = "Subir" }, true);
            }));


            problems = new ObservableCollection<Problem>{};
            GetProblems("");

            // var problemsListView = new ListView();
            problemsListView.ItemsSource = problems;
        }

        public async Task GetProblems(string search)
        {
        	using (var client = new HttpClient())
        	{
                var url = string.Format("http://hackathon.hernandis.me/problems.json?query="+search);
        		var resp = await client.GetAsync(url);
        		if (resp.IsSuccessStatusCode)
        		{
        			problems.Clear();
        			var problemsJSON = JsonConvert.DeserializeObject<Problem[]>(resp.Content.ReadAsStringAsync().Result);
        			foreach (var problem in problemsJSON)
        			{
        				problems.Add(problem);
        			}
        		}
        	}
        }

        public void openProblem(object sender, EventArgs e)
        {
            var selectedProblem = problemsListView.SelectedItem;
            this.Navigation.PushAsync(new ProblemPage(selectedProblem) { Title = "Problemas" }, true);
        }

        public async void search(object sender, EventArgs e)
        {
            await GetProblems(searchBar.Text);
            //problems.Add(new Problem{ DisplayName = searchBar.Text });

        }


    }
}
