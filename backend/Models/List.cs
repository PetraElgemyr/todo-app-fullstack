namespace TodoApp.Models
{
    public class List
    {
        public long Id { get; set; }
        public string ListName { get; set; } = "";
        public long PersonId { get; set; }
    }
}
