namespace TodoApp.Models
{
    public class Todo
    {
        public long id { get; set; }

        public string description { get; set; } = "";

        public bool isChecked { get; set; }
    }
}
