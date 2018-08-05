//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity.Validation;
//using Cherokee.DAL.Entities;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Cherokee.Test.UnitCRUD_Operations
//{
//    [TestClass]
//    public class InsertTest
//    {
//        //[TestMethod]
//        //public void InsertEmployeeWithValidData()
//        //{
//        //    using (UnitOfWork unit = new UnitOfWork())
//        //    {
//        //        Employee emp = new Employee()
//        //        {
//        //            FirstName = "Insert",
//        //            LastName = "Test",
//        //            Position = unit.Roles.Get("UX")
//        //        };
//        //        unit.Employees.Insert(emp);
//        //        unit.Save();
//        //        Employee test =  unit.Employees.Get(emp.Id);
//        //        Assert.AreEqual("Insert", test.FirstName);
//        //        Assert.AreEqual("Test", test.LastName);
//        //        Assert.AreEqual("UX", test.PositionId);

//        //        // most important testing
//        //        //Assert.ThrowsException<DbEntityValidationException>(() => unit.Save());

//        //    }
//        //}
        
//        // Assert.
//        //dbvalidationexception

//        //[TestMethod]
//        //public void InsertEmployeeWithInvalidData()
//        //{
//        //    using (UnitOfWork unit = new UnitOfWork())
//        //    {
//        //        Employee emp = new Employee()
//        //        {
//        //            FirstName = "Insert",
//        //            LastName = "Test",
//        //            Position = unit.Roles.Get("VRTLAR")
//        //        };
//        //        unit.Employees.Insert(emp);
//        //        unit.Save();
//        //        Employee test = unit.Employees.Get(emp.Id);
                
//        //        Assert.AreNotEqual("VRTLAR", test.PositionId);


//        //    }
//        //}
//    }
//}
