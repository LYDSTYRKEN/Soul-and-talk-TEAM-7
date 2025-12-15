using Soul_and_talk;
using Soul_and_talk.Model;
using Soul_and_talk.Model.BusinessLogic;

namespace Soul_and_talk_test
{
    [TestClass]
    public class HourlyRateTests
    {
        [TestMethod]
        public void HourlyRate_PublicInstitution_Physical_Return550()
        {
            // Arrange
            HourlyRate calculate = new HourlyRate();

            Institution inst = new Institution();
            inst.Type = InstitutionType.Public;

            Customer customer = new Customer();
            customer.Institution = inst;

            // Act (Method we are testing)
            decimal result = calculate.GetRatePerHour(customer, true); // true = physical

            // Assert (verify the result)
            Assert.AreEqual(550m, result);
        }
    }
}
