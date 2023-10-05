using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration_Form
{
    public class Employee
    {
        internal MemoryStream ms;

        public string ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public byte[] Image { get; set; }                                    //for byte array

        public string Imagepath { get; set; }                             //string path of image





/*
        public Employee(int id, string name, int age, string address, string gender, string image) 
        {
            ID = id;
            Name = name;
            Age = age;
            Address = address;
            Gender = gender;
            Image = image
        }
*/


        public Employee() 
        {
            
        }

    }

   
}
