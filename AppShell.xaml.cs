namespace org.dgl.win98labyrinth
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            VersionLabel.Text = "v1.0.2";
        }

        private void MenuItemNew_Clicked(object sender, EventArgs e)
        {
            if(Shell.Current.CurrentPage is MainPage mainPage)
            {
                mainPage.NewGame();
            }
        }

    }
}
