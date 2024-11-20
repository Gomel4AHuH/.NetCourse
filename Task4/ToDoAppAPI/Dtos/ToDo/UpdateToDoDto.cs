
namespace ToDoAppAPI.Dtos.ToDo
{
    public record UpdateToDoDto
    {                
        public string Name { get; set; }
                
        public string Description { get; set; }

        public bool IsClosed { get; set; }

    }
}
