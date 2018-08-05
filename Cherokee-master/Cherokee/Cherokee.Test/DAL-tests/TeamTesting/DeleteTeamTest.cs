//using System;
//using Cherokee.DAL.Entities;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cherokee.Test.UnitCRUD_Operations
//{
//    [TestClass]
//    public class DeleteTest
//    {
//        [TestMethod]
//        public void CheckDeleteTeam()
//        {
//            using (UnitOfWork unit = new UnitOfWork())
//            {
//                Team test = unit.Teams.Get("B");
//                unit.Teams.Delete(test);
//                unit.Save();
//                Team test1 = unit.Teams.Get("B");
//                Assert.IsNull(test1);
//            }
//            //
//            // dbentityvalidationexception
//        }
//    }
//}

