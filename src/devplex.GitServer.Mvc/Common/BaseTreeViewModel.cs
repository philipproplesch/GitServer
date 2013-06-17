namespace devplex.GitServer.Mvc.Common
{
    public class BaseTreeViewModel<TTree> : BaseRepositoryDetails
    {
        public TTree Tree { get; set; }
    }
}