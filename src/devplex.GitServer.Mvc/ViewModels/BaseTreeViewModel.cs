namespace devplex.GitServer.Mvc.ViewModels
{
    public class BaseTreeViewModel<TTree>
    {
        public string Branch { get; set; }
        public string RepositoryPath { get; set; }

        public TTree Tree { get; set; }
    }
}