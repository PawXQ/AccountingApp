using Accounting.Components;
using Accounting.Forms;
using Accounting.Utility;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting
{
    internal static class Program
    {
        public class Student
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Heigh { get; set; }
        }

        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public int Height { get; set; }
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Student student = new Student() { Name = "AAA", Age = 18, Heigh = 200 };
            IEnumerable<Student> students = new List<Student>()
            {
                new Student() { Name = "AAA", Age = 18, Heigh = 200 },
                new Student() { Name = "BBB", Age = 19, Heigh = 201 },
                new Student() { Name = "CCC", Age = 20, Heigh = 202 },
                new Student() { Name = "DDD", Age = 21, Heigh = 203 },
            };


            //var config = new MapperConfiguration(cfg =>
            //cfg.CreateMap<Student, User>()
            //);
            //var mapper = config.CreateMapper();

            //---------------------

            //var config = new MapperConfiguration(cfg =>
            //cfg.CreateMap<Student, User>()
            //   .ForMember(x => x.Height, y => y.MapFrom(z => z.Heigh))
            //);
            //var mapper = config.CreateMapper();

            //var user = mapper.Map<User>(student);

            //---------------------

            var user = Utility.Mapper.Map<Student, User>(student, cfg =>
            {
                cfg.ForMember(x => x.Height, y => y.MapFrom(z => z.Heigh));
            });

            //---------------------

            List<User> users = Utility.Mapper.Map<Student, User>(students, cfg =>
            {
                cfg.ForMember(x => x.Height, y => y.MapFrom(z => z.Heigh));
            }).ToList();

            users.ForEach(x =>
            {
                Console.WriteLine($"user: {x.Name}");
                Console.WriteLine($"user: {x.Age}");
                Console.WriteLine($"user: {x.Height}");
            });

            //Console.WriteLine($"user: {user.Name}");
            //Console.WriteLine($"user: {user.Age}");
            //Console.WriteLine($"user: {user.Height}");




            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form AccountBookForm = SignletoForm.CreateForm("AccountBookForm");
            Application.Run(AccountBookForm);
        }
    }
}
