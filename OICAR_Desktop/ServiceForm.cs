﻿
using ClassLibrary.DAL;
using ClassLibrary.Model;
using OICAR_Desktop.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OICAR_Desktop
{
    public partial class ServiceForm : Form
    {


        private UniteOfWork uow;

        private Panel _pnlContent;

        private CompanyLogin _companyLogin;

        public ServiceForm(Panel pnlContent, CompanyLogin companyLogin)
        {
            _companyLogin = companyLogin;
            _pnlContent = pnlContent;
            InitializeComponent();
            InitRepository();
            FillServiceList();
            GridCheck();


        }


        private void InitRepository()
        {
            uow = new UniteOfWork(new ModelContainer());
        }

        private void FillServiceList()
        {
            var company = uow.Company.GetCompanyForUser(_companyLogin.IdCompanyLogin);

            lbServices.DataSource = uow.ServiceTypes.GetServiceTypeForCompany(company.IdCompany);
            lbServices.DisplayMember = nameof(Service_Type.Name);
            lbServices.ValueMember = nameof(Service_Type.IdServiceType);
        }

        private void GridCheck()
        {
            if (lbServices.Items.Count == 0)
            {
                DisableGrid();
            }
            else
            {
                EnableGrid();
            }
        }



        private void lbServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbServices.SelectedIndex == -1)
            {

                return;
            }


            Service_Type service_Type = lbServices.SelectedItem as Service_Type;
            PopulateGrid(service_Type.IdServiceType);
        }



        private void PopulateGrid(int idServiceType)
        {


            if (lbServices.Items.Count == 0)
            {
                DisableGrid();
            }
            else
            {
                var company = uow.Company.GetCompanyForUser(_companyLogin.IdCompanyLogin);
                EnableGrid();
                dataGridView.DataSource = uow.Services.GetServiceByServiceType(idServiceType);
                SetupDataGrid();

            }


        }
        private void DisableGrid()
        {

            dataGridView.Visible = false;
            lblInfo.Visible = true;


        }
        private void EnableGrid()
        {
            dataGridView.Visible = true;
            lblInfo.Visible = false;
        }
        private void SetupDataGrid()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView.BackgroundColor = SystemColors.Control;


            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }




        private void btnDeleteServiceType_Click(object sender, EventArgs e)
        {
            if (lbServices.SelectedItem == null)
            {
                return;
            }
            var serviceType = lbServices.SelectedItem as Service_Type;
            var services = uow.Services.GetServiceByServiceType(serviceType.IdServiceType).ToList();


            DialogResult deleteServiceTypeResult = HelperMethods.DialogHelper("Želite li obrisati tip usluge ?", "Potvrda brisanja.", MessageBoxButtons.YesNo);

            if (deleteServiceTypeResult == DialogResult.Yes)
            {
                try
                {
                    foreach (var item in services)
                    {
                        uow.Services.Delete(item);
                    }

                    uow.ServiceTypes.Delete(serviceType);

                    uow.SaveChanges();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Greška kod brisanja. \n" + ex.Message);
                }

            }


            FillServiceList();
            GridCheck();

            //var appointments = uow.Apponitments.GetAppointmentsByService(services).ToList();


            //if (appointments.Count() > 0)
            //{
            //    if (DialogResult.Yes == MessageBox.Show("Usluga postoji u zakazanom terminu, želite li obrisati i te termine ?", "Potvrda brisanja.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            //    {

            //        foreach (var item in appointments)
            //        {
            //            uow.Apponitments.Delete(item);

            //        }
            //        foreach (var item in services)
            //        {
            //            uow.Services.Delete(item);
            //        }

            //        uow.ServiceTypes.Delete(serviceType);

            //    }
            //}
            //else
            //{
            //    if (DialogResult.Yes == MessageBox.Show("Želite li obrisati tip usluge ?", "Potvrda brisanja.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            //    {
            //        foreach (var item in services)
            //        {
            //            uow.Services.Delete(item);
            //        }
            //        uow.ServiceTypes.Delete(serviceType);
            //    }

            //}

            //try
            //{
            //    uow.SaveChanges();
            //}
            //catch (EntitySqlException ex)
            //{

            //    MessageBox.Show("Greška" + ex.Message);
            //}



        }






        private void btnAddService_Click(object sender, EventArgs e)
        {

            OpenFormHelper.OpenChildForm(new AddServiceForm(_companyLogin), _pnlContent);
        }


        private void btnDeleteService_Click(object sender, EventArgs e)
        {
            if (uow.Services.GetAll().Count() == 0)
            {
                return;
            }

            foreach (DataGridViewRow item in dataGridView.SelectedRows)
            {


                var id = int.Parse(item.Cells[0].Value.ToString());
                var service = uow.Services.GetById(id);

                DialogResult deleteServiceTypeResult = HelperMethods.DialogHelper("Želite li obrisati uslugu ?", "Potvrda brisanja.", MessageBoxButtons.YesNo);

                if (deleteServiceTypeResult == DialogResult.Yes)
                {
                    try
                    {
                        uow.Services.Delete(service);

                        uow.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Greška kod brisanja. \n" + ex.Message);
                    }

                }

                RefreshGrid();


                //    if (appointments.Count() > 0)
                //    {
                //        if (DialogResult.Yes == MessageBox.Show("Usluga postoji u zakazanom terminu, želite li obrisati i te termine ?", "Potvrda brisanja.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                //        {
                //            foreach (var appointment in appointments)
                //            {
                //                uow.Apponitments.Delete(appointment);
                //                uow.Services.Delete(service);

                //            }


                //        }
                //    }
                //    else
                //    {
                //        if (DialogResult.Yes == MessageBox.Show("Želite li obrisati  uslugu ?", "Potvrda brisanja.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                //        {
                //            uow.Services.Delete(service);

                //        }

                //    }

                //}

                //try
                //{
                //    uow.SaveChanges();
                //}
                //catch (Exception ex)
                //{

                //    MessageBox.Show("Greška." + ex.Message);
                //}

            }


        }

        private void RefreshGrid()
        {
            var selected = lbServices.SelectedItem as Service_Type;
            PopulateGrid(selected.IdServiceType);
        }

        private void btnUpdateService_Click(object sender, EventArgs e)
        {
            if (uow.Services.GetAll().Count() == 0)
            {
                return;
            }

            foreach (DataGridViewRow item in dataGridView.SelectedRows)
            {
                var id = int.Parse(item.Cells[0].Value.ToString());
                var service = uow.Services.GetById(id);
                var serviceType = lbServices.SelectedItem as Service_Type;
                var company = uow.Company.GetCompanyForUser(_companyLogin.IdCompanyLogin);

                UpdateServiceForm form = new UpdateServiceForm(service, serviceType,company);

                OpenFormHelper.OpenChildForm(form, _pnlContent);
                form.FormClosed += Form_FormClosed;

            }

        }
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            OpenFormHelper.OpenChildForm(new ServiceForm(_pnlContent, _companyLogin), _pnlContent);
        }


    }
}

