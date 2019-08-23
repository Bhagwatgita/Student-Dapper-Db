using DbGeneric.BaseRepository;
using DbGeneric.Contracts;
using DbGeneric.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbGeneric.Repository
{
    public class StudentRepository : BaseRepository<Student>,IStudentRepository
    {
        
    }
}
