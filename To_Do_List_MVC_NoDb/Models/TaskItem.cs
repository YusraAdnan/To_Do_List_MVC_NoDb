namespace To_Do_List_MVC_NoDb.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
