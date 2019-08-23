using DbGeneric.Contracts;
using DbGeneric.Entity;
using DbGeneric.Repository;
using System;

namespace ExecuteGenericDb
{
    class Program
    {
        static void Main(string[] args)
        {
            IStudentRepository studentRepository = new StudentRepository();
            Student student = new Student
            {
                Id=5,
                FullName="Makunda Prasad Kafle",
                StudyLevel="PHD"
            };
            studentRepository.Update(student);
            int studentCount = studentRepository.QueryForCount();

            Console.ReadKey();
        }
    }
}
