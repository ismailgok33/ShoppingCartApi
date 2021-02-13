
namespace CicekSepetiTask.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public int Stock { get; set; }
    }
}
