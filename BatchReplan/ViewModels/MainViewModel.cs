using BatchReplan.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace BatchReplan.ViewModels
{
    public class MainViewModel : BindableBase
    {
        //public string PatientName { get; private set; }
        private string _patientName;

        public string PatientName
        {
            get { return _patientName; }
            set { SetProperty(ref _patientName, value); }
        }
        public MainViewModel(EsapiWorker esapiWorker)
        {
            string pid_temp = "RapidPlan-02";
            esapiWorker.Run(xapp =>
            {
                Patient patient = xapp.OpenPatientById(pid_temp);
                PatientName = patient.Name;
            });
        }

        

    }
}
