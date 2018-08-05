//using System;
//using System.Linq;
//using Cherokee.DAL;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cherokee.Test
//{
//    [TestClass]
//    public class RoleTest
//    {
//        [TestMethod]
//        public void CheckRoles()
//        {
//            using (UnitOfWork unit = new UnitOfWork())
//            {
//                // Arrange
//                // Act
//                int roles = unit.Roles.Get().ToList().Count();
                
//                // Assert
//                Assert.AreEqual(2, roles);
//            }
//        }
//    }
//}
