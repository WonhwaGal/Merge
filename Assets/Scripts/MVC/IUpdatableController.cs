namespace Code.MVC
{
    public interface IUpdatableController
    {
        public string Tag { get; }
        public void UpdateController(string tag);
    }
}