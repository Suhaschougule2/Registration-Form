using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Registration_Form
{
 
    public partial class Form1 : Form
    {
        
        List<Employee> employees = new List<Employee>();

        private string filePath;
        public string imagePath;
        private int indexRow;
        

        public Form1()
        {
            InitializeComponent();
            filePath = "EmployeeData.json";



            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("Employee Registration Form", Application.ExecutablePath.ToString());
            //MessageBox.Show("You have been successfully saved.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Load  JsonFile Data in datagridview 
            string json = File.ReadAllText(filePath);
            dataGridView1.DataSource = JsonConvert.DeserializeObject<DataTable>(json);


            //Unique id generator     

            Guid guid = Guid.NewGuid();
            label8.Text = guid.ToString().Substring(0, 3);                          //id in string 

        }




        //Upload Image Button
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                imagePath = openFileDialog.FileName;
            }
        }


        
        //Save Button
        private void button2_Click(object sender, EventArgs e)
        {



            //Add data into same json file
            string jsonD = File.ReadAllText(filePath);
            var employees = JsonConvert.DeserializeObject<List<Employee>>(jsonD) ?? new List<Employee>();





            //Unique id generator

            Guid guid = Guid.NewGuid();
            label8.Text = guid.ToString().Substring(0, 3);                          //id in string 




            //Add employee
            try
            {


                //Dublicate Mobile Number validation
                string MobileNo = Convert.ToString(textBox5.Text.Trim());
                if (employees.Where(x => x.Mobile == MobileNo).Any())
                {
                    MessageBox.Show("Mobile Number Already Exists");
                    return;
                }



                //Dublicate EmailID validation
                string EmailID = Convert.ToString(textBox6.Text.Trim());
                if (employees.Where(x => x.Email == EmailID).Any())
                {
                    MessageBox.Show("Email ID Already Exists");
                    return;
                }



                //Adding Employees
                employees.Add(new Employee()
                {
                    ID = label8.Text,                              //save unique id in ID column
                    Name = textBox4.Text,
                    Age = Convert.ToInt32(textBox2.Text),
                    Address = textBox3.Text,
                    Gender = comboBox1.Text,
                    Mobile = textBox5.Text,
                    Email = textBox6.Text,
                    Image = File.ReadAllBytes(imagePath),
                    Imagepath = imagePath,
                    
                });




                //Email validation
                var exp = new Regex(@"\b[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}\b", RegexOptions.IgnoreCase);
                if (exp.IsMatch(textBox6.Text) == false)
                {
                    MessageBox.Show("Enter Valid Email ID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //Mobile number validation
                else if ((textBox5.Text.Length > 11) || (textBox5.Text.Length < 10))
                {
                    MessageBox.Show("Enter Valid Mobile Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    //save  file as json

                    string jsonS = JsonConvert.SerializeObject(employees, Formatting.Indented);
                    File.WriteAllText(filePath, jsonS);
                    MessageBox.Show("Employee Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }




                //Load Newly added Data in datagridview
                string json = File.ReadAllText(filePath);
                dataGridView1.DataSource = JsonConvert.DeserializeObject<DataTable>(json);
            }

            catch 
            {
                MessageBox.Show("Please Enter Valid Information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }



        //Display Data in text boxes by Selecting cell of datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexRow];

            label8.Text = row.Cells[0].Value.ToString();
            textBox4.Text = row.Cells[1].Value.ToString();
            textBox2.Text = row.Cells[2].Value.ToString();
            textBox3.Text = row.Cells[3].Value.ToString();
            comboBox1.Text = row.Cells[4].Value.ToString();
            textBox5.Text = row.Cells[5].Value.ToString();
            textBox6.Text = row.Cells[6].Value.ToString();
            pictureBox1.Image = new Bitmap(row.Cells[8].Value.ToString());
            
        }




        //Search using searchbox-textbox1 text
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource !=null)
            {

                //Search using Name
                string Name = textBox1.Text.Trim();
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Name like '%" + Name + "%'");


                //Search using Email
                //string EmailID = textBox1.Text.Trim();
                //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Email like '%" + EmailID + "%'");



                //Search using Mobile Number
                //string Mobile = textBox1.Text.Trim();
               //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Mobile like '%" + Mobile + "%'");


            }
        }




        //Reset Button (Clear All text Boxes)
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.Text = " ";
            pictureBox1.Image = null;
        }




        //Count Form Button
        private void button4_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText(filePath);
            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(json);
            int count = employees.Count;
            MessageBox.Show($"Number of Employees: {count}");

        }



    }
}
