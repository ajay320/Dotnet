using Mono.TextTemplating;

namespace Task10Field.Models.Binding
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public ICollection<State> States { get; set; }
    }
}
