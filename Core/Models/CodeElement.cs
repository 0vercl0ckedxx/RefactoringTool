namespace Core.Models
{
    public abstract class CodeElement
    {
        public string Name { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
    }
}
