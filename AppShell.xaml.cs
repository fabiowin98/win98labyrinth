namespace org.dgl.win98labyrinth
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
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
