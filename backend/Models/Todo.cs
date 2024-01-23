namespace TodoApp.Models
{
    public class Todo
    {
        public long Id { get; set; }

        public string Description { get; set; } = "";

        public bool IsChecked { get; set; }
    }
}
