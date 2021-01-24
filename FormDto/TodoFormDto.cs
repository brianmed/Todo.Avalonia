namespace Todo.FormDto
{
    public class TodoFormDto
    {
        public long TodoEntityId { get; set; }

        public string? Title { get; set; }

        public int DayCreated { get; set; }

        public bool IsDone { get; set; }
    }
}