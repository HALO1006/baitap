using Lab05.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
   public class StudentService
    {
        public List<Student> GetAll()
        {
            Model1 context = new Model1();
            return context.Student.ToList();
        }

        public List<Student> GetAllHasNoMajor()
        {
            Model1 context = new Model1();
            return context.Student.Where(s => s.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            Model1 context = new Model1();
            return context.Student.Where(s => s.MajorID == null && s.FacultyID == facultyID).ToList();
        }

        public Student FindById(string studentID)
        {
            Model1 context = new Model1();
            return context.Student.FirstOrDefault(s => s.StudentID == studentID);
        }

        public void InsertUpdate(Student s)
        {
            Model1 context = new Model1();
            context.Student.AddOrUpdate(s);
            context.SaveChanges();
        }

        public void Delete(string studentID)
        {
            Model1 context = new Model1();
            var student = context.Student.FirstOrDefault(p => p.StudentID == studentID);
            if (student != null)
            {
                context.Student.Remove(student);
                context.SaveChanges();
            }
        }

    }
}
