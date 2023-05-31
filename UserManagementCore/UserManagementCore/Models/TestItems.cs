namespace UserManagementCore.Models
{
    public class TestItems
    {
        public int id { get; set; }
        public string idk { get; set; }
    }

    public class Student {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }
    public class Class {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
