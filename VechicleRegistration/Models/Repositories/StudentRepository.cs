using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VechicleRegistration.Models.Repositories
{
    public class StudentRepository : Repository<Student>
    {
        public Student GetByMatricNo(int matricno)
        {
            return DbSet.Where(x => x.MatricNumber == matricno).FirstOrDefault();

        }



    }
}