using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTests
{
    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork("TestData"))
            {
                try
                {
                    unitOfWork.Open();
                    var command = new GetTestDataCommand()
                                        .AddParameters("@EntityName", "Test");
                    Entities entities = command.Execute(unitOfWork).FirstOrDefault();

                    Assert.IsNotNull(entities);
                    Assert.AreEqual("Test", entities.EntityName);
                    Assert.AreEqual("Test entity for use with the data access code", entities.Description);
                }
                finally
                {
                    unitOfWork.Close();
                }
            }
        }
    }
}
