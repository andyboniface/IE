using System;
namespace IE.CommonSrc.Configuration
{
    public class Region
    {
        public Region( int id, string regionName)
        {
            Id = id;
            Name = regionName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
