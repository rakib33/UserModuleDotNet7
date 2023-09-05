namespace UserManagementCore.Models
{
    public class TestItems
    {
        public int id { get; set; }
        public string idk { get; set; } = null!;
    }

    public class Student {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Class> Classes { get; set; } = null!;
    }
    public class Class {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
