using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Input;
using Wpf.DataForm.Library.DataForm.FormFill;
using Wpf.DataForm.Sample.DataObjects;
using Microsoft.Win32;

namespace Wpf.DataForm.Sample.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        #region Fields

        private bool _isValidationSuccess;

        #endregion

        #region Properties

        public Person Person { get; set; }
        public string Layout { get; set; }

        public bool IsValidationSuccess
        {
            get { return _isValidationSuccess; }
            set
            {
                if (value != _isValidationSuccess)
                {
                    _isValidationSuccess = value;
                    OnPropertyChanged("IsValidationSuccess");
                }
            }
        }

        public ICommand FillOutDataCommand { get; private set; }
        public ICommand FillDataSaveCommand { get; private set; }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            Person = new Person();

            //Person.FirstName = "John";
            //Person.LastName = "Doe";
            Person.JoinDate = DateTime.Now;

            Person.Age = 44;
            Person.City = "Neustadt";

            Person.MarkUnmodified();

            // Set layout
            Layout = Properties.Resources.SampleLayout1;

            FillOutDataCommand = new RelayCommand(FillOutDataCommandExecute);
            FillDataSaveCommand = new RelayCommand(FillDataSaveCommandExecute);
        }

        #endregion

        #region Methods

        private void FillOutDataCommandExecute(object parameter)
        {
            var ff = parameter as IFormFiller;
            if (ff == null)
            {
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML file (*.xml)|*.xml";
            if (ofd.ShowDialog() == true)
            {
                using (Stream stream = ofd.OpenFile())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(IDictionary<string, object>));
                    var data = serializer.ReadObject(stream) as IDictionary<string, object>;

                    ff.SetData(data);
                }
            }
        }

        private void FillDataSaveCommandExecute(object parameter)
        {
            var ff = parameter as IFormFiller;
            if (ff == null)
            {
                return;
            }

            var data = ff.GetData();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML file (*.xml)|*.xml";
            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(IDictionary<string, object>));
                    serializer.WriteObject(stream, data);
                }
            }
        }

        #endregion
    }
}
