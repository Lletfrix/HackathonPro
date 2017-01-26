using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HackathonPro
{
    public partial class ProblemPage : ContentPage
    {
        public ProblemPage(object problem)
        {
            InitializeComponent();
            BindingContext = problem;
        }
    }
}
