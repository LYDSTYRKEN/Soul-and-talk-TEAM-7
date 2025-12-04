namespace Soul_and_talk.Model
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        // null = private customer
        public Institution Institution { get; set; }  // gør den bare ikke-nullable for at slippe for bøvl

        public bool IsPrivateCustomer
        {
            get { return Institution == null; }
        }
    }
}
