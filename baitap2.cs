using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Age: {Age}";
    }
}

class Program
{
    static void Main()
    {
        // Tạo danh sách học sinh
        List<Student> students = new List<Student>()
        {
            new Student{ Id = 1, Name = "Thinh", Age = 16 },
            new Student{ Id = 2, Name = "Phuong", Age = 18 },
            new Student{ Id = 3, Name = "Hoang", Age = 20 },
            new Student{ Id = 4, Name = "Dat", Age = 15 },
            new Student{ Id = 5, Name = "Tuan", Age = 17 }
        };

        Console.WriteLine("=== a. Danh sách toàn bộ học sinh ===");
        students.ForEach(s => Console.WriteLine(s));

        Console.WriteLine("\n=== b. Học sinh tuổi từ 15 đến 18 ===");
        var age15to18 = students.Where(s => s.Age >= 15 && s.Age <= 18);
        foreach (var s in age15to18)
            Console.WriteLine(s);

        Console.WriteLine("\n=== c. Học sinh có tên bắt đầu bằng 'A' ===");
        var nameStartA = students.Where(s => s.Name.StartsWith("A"));
        foreach (var s in nameStartA)
            Console.WriteLine(s);

        Console.WriteLine("\n=== d. Tổng tuổi tất cả học sinh ===");
        int totalAge = students.Sum(s => s.Age);
        Console.WriteLine("Tổng tuổi: " + totalAge);

        Console.WriteLine("\n=== e. Học sinh có tuổi lớn nhất ===");
        int maxAge = students.Max(s => s.Age);
        var oldest = students.Where(s => s.Age == maxAge);
        foreach (var s in oldest)
            Console.WriteLine(s);

        Console.WriteLine("\n=== f. Sắp xếp danh sách theo tuổi tăng dần ===");
        var sorted = students.OrderBy(s => s.Age);
        foreach (var s in sorted)
            Console.WriteLine(s);
    }
}
