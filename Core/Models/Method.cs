namespace Core.Models
{
    public class Method : CodeElement
    {
        public string ReturnType { get; set; }
        public List<Variable> Parameters { get; set; } = new();
        public string Body { get; set; }
    }
}
