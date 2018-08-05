//using Cherokee.DAL.Entities;
//using Cherokee.DAL.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Cherokee.Test.DAL_tests.EmployeeTesting
//{
//    public class DeleteEmployeeTest
//    {
//        [TestClass]
//        public class DeleteTest
//        {
//            [TestMethod]
//            public void CheckDeleteEmployee()
//            {
//                using (UnitOfWork unit = new UnitOfWork())
//                {
//                    Employee test = unit.Employees.Get().FirstOrDefault();
//                    unit.Employees.Delete(test);
//                    unit.Save();
//                    Employee test1 = unit.Employees.Get(test.Id);
//                    Assert.IsNull(test1);
//                }
//            }
//        }
//    }
//}
