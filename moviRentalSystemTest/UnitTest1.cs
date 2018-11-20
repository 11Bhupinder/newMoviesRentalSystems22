
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesRentalSystems;


namespace moviRentalSystemTest
{
    [TestClass]
    public class UnitTest1
    {
        Form1 obj = new Form1();
        [TestMethod]
        public void TestMethod1()
        {
            
            if(obj.conn ==null)
            {
                obj.con = false;

            }
            else
            {
                obj.con = true;
                
            }
            Assert.IsTrue(obj.con);
        }

        [TestMethod]
        public void ShowData_Test()
        {
            bool rtrnMov = obj.showData;
            Assert.IsTrue(rtrnMov);

        }

    }
}
