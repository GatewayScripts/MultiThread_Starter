using BatchReplan.Services;
using BatchReplan.ViewModels;
using BatchReplan.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using esapi = VMS.TPS.Common.Model.API;

namespace BatchReplan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private esapi.Application esapiApp;
        private EsapiWorker esapiWorker;
        private MainView mainView;
        private MainViewModel mainViewModel;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                using (esapiApp = esapi.Application.CreateApplication())
                {
                    esapiWorker = new EsapiWorker(esapiApp);

                    //This new queue of tasks will prevent the script from exiting until the new window is closed.
                    DispatcherFrame frame = new DispatcherFrame();
                    RunOnNewStaThread(() =>
                    {
                        //This method won't return until the window is closed
                        InitializeAndStartMainWindow();
                        //End the queue so that the script can exit.
                        frame.Continue = false;
                    });
                    //Start the new queue, waiting until the window is closed.
                    Dispatcher.PushFrame(frame);
                    esapiApp.ClosePatient();
                    Shutdown();
                }
            }
            catch(Exception ex)
            {
                //TODO output ex to log
            }
        }

        private void InitializeAndStartMainWindow()
        {
            mainView = new MainView();
            mainViewModel = new MainViewModel(esapiWorker);
            mainView.DataContext = mainViewModel;
            mainView.ShowDialog();
        }

        private void RunOnNewStaThread(Action value)
        {
            Thread thread = new Thread(() => value());
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
