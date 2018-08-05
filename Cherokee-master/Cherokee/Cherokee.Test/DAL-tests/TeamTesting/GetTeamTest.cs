//using System;
//using Cherokee.DAL.Entities;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cherokee.Test.UnitCRUD_Operations
//{
//    [TestClass]
//    public class GetTest
//    {
//       [TestMethod]
//        public void CheckGetValidData()
//        {
//            using (UnitOfWork unit = new UnitOfWork())
//            {
//                Team test = unit.Teams.Get("A");
//                Assert.AreEqual("Alpha", test.Name);
//            }
//        }

//        [TestMethod]
//        public void CheckGetInvalidData()
//        {
//            using (UnitOfWork unit = new UnitOfWork())
//            {
//                Team test = unit.Teams.Get("C");
//                Assert.IsNull(test);
//            }
//        }
//    }
//}
