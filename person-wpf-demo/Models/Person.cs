namespace person_wpf_demo.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
