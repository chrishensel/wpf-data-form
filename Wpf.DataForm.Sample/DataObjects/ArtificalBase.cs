using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Wpf.DataForm.Sample.ViewModels;
using System.Windows;
using Wpf.Library.Data;

namespace Wpf.DataForm.Sample.DataObjects
{
    abstract class ArtificalBase : DataObjectBase
    {
        [Required()]
        [Display(Name = "Is valid")]
        public bool IsValid
        {
            get { return GetValue(() => IsValid); }
            set { SetValue(() => IsValid, value); }
        }

        [Required()]
        [Display(Name = "Comment")]
        public string Comment
        {
            get { return GetValue(() => Comment); }
            set { SetValue(() => Comment, value); }
        }

        [Display(Name = "My test")]
        public ICommand DoSomethingCommand { get; set; }

        public ArtificalBase()
        {
            DoSomethingCommand = new RelayCommand(param => MessageBox.Show("Hello world from 'DoSomethingCommand'!"));
        }
    }
}
