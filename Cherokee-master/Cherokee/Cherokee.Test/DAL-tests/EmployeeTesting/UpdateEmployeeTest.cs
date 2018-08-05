//using System;
//using Cherokee.DAL;
//using Cherokee.DAL.Entities;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cherokee.Test.EmployeeTesting
//{
//    [TestClass]
//    public class UpdateTest
//    {
//        [TestMethod]
//        public void CheckUpdateWithValidData()
//        {
//            using (UnitOfWork unit = new UnitOfWork())
//            {

//                Employee emp = unit.Employees.Get(1);
                
                
//                emp.FirstName = "Jane";
//                unit.Employees.Update(emp, 1);
//                unit.Save();
               
//                Assert.AreEqual("Jane", emp.FirstName);

//            }
//        }

//        //[TestMethod]
//        //public void CheckUpdateWithInvalidData()
//        //{
//        //    using (UnitOfWork unit = new UnitOfWork())
//        //    {
//        //        Employee emp = unit.Employees.Get(2);
                
//        //        emp.Position = unit.Roles.Get("DEV");
//        //        unit.Employees.Update(emp, emp.Id);
//        //        unit.Save();


//        //        Assert.AreNotEqual("DEV", emp.PositionId);
               
              

//        //    }
//        //}

//    }
//}
