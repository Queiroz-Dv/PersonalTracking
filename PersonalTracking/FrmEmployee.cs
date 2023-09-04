﻿using BLL;
using DAL;
using DAL.DTO;
using PersonalTracking.Entities;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PersonalTracking
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        EmployeeDTO dto = new EmployeeDTO();
        public EmployeeDetailDTO detail = new EmployeeDetailDTO();
        public bool isUpdate = false;

        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cmbDeparment.DataSource = dto.Departments;
            cmbDeparment.DisplayMember = "DepartmentName";
            cmbDeparment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDeparment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            if (isUpdate)
            {
                txtName.Text = detail.Name;
                txtSurname.Text = detail.Surname;
                txtUserNo.Text = detail.UserNo.ToString();
                txtPassword.Text = detail.Password;
                chAdmin.Checked = Convert.ToBoolean(detail.isAdmin);
                txtAddres.Text = detail.Address;
                dateTimePicker2.Value = Convert.ToDateTime(detail.BirthDay);
                cmbDeparment.SelectedValue = detail.DepartmentID;
                cmbPosition.SelectedValue = detail.PositionID;
                txtSalary.Text = detail.Salary.ToString();
                if (!UserStatic.isAdmin)
                {
                    chAdmin.Enabled = false;
                    txtUserNo.Enabled = false;
                    txtSalary.Enabled = false;
                    cmbDeparment.Enabled = false;
                    cmbPosition.Enabled = false;
                }
            }
        }

        bool combofull = false;
        private void cmbDeparment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                int departmentID = Convert.ToInt32(cmbDeparment.SelectedValue);
                //cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User Number is Empty");

            else if (txtPassword.Text.Trim() == "")
                MessageBox.Show("Password is Empty");
            else if (txtName.Text.Trim() == "")
                MessageBox.Show("Name is Empty");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Surname is Empty");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Salary is Empty");
            else if (cmbDeparment.SelectedIndex == -1)
                MessageBox.Show("Select a Department");
            else if (cmbPosition.SelectedIndex == -1)
                MessageBox.Show("Select a Position");
            else
            {
                if (!isUpdate)
                {
                    if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text)))
                        MessageBox.Show("This user is used by another employee please change it");
                    else
                    {
                        EMPLOYEE employee = new EMPLOYEE
                        {
                            UserNo = Convert.ToInt32(txtUserNo.Text),
                            Password = txtPassword.Text,
                            isAdmin = chAdmin.Checked,
                            Name = txtName.Text,
                            Surname = txtSurname.Text,
                            Salary = Convert.ToInt32(txtSalary.Text),
                            DepartmentID = Convert.ToInt32(cmbDeparment.SelectedValue),
                            PositionID = Convert.ToInt32(cmbPosition.SelectedValue),
                            Adress = txtAddres.Text,
                            BirthDay = dateTimePicker2.Value,
                        };

                        EmployeeBLL.AddEmployee(employee);
                        MessageBox.Show("Employee was added");
                        txtUserNo.Clear();
                        txtPassword.Clear();
                        chAdmin.Checked = false;
                        txtName.Clear();
                        txtSurname.Clear();
                        txtSalary.Clear();
                        txtAddres.Clear();
                        combofull = false;
                        cmbDeparment.SelectedIndex = -1;
                        cmbPosition.DataSource = dto.Positions;
                        cmbPosition.SelectedIndex = -1;
                        combofull = true;
                        dateTimePicker2.Value = DateTime.Today;
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        EMPLOYEE employee = new EMPLOYEE();

                        employee.ID = detail.EmployeeID;
                        employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                        employee.Name = txtName.Text;
                        employee.Surname = txtSurname.Text;
                        employee.isAdmin = chAdmin.Checked;
                        employee.Password = txtPassword.Text;
                        employee.Adress = txtAddres.Text;
                        employee.BirthDay = dateTimePicker2.Value;
                        employee.DepartmentID = Convert.ToInt32(cmbDeparment.SelectedValue);
                        employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                        employee.Salary = Convert.ToInt32(txtSalary.Text);
                        EmployeeBLL.UpdateEmployee(employee);
                        MessageBox.Show("Employee was updated");
                        this.Close();


                    }
                }

            }
        }

        bool isUnique = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("User Number is Empty");
            else
            {
                isUnique = EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text));
                if (!isUnique)
                    MessageBox.Show("This user is used by another employee please change it");
                else
                    MessageBox.Show("This user is usable");
            }
        }
    }
}
