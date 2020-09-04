// MainForm.cs
using System.ComponentModel;
using System.Windows.Forms;

namespace PrintReportWithWPFUC
{
    // Form class
    public partial class MainForm : Form
    {
        // Model
        private ReportData _model = new ReportData();

        // Constructor
        public MainForm()
        {
            InitializeComponent();

            // Binding the model to a Text property
            tbxMessage.DataBindings.Add(new Binding(nameof(TextBox.Text), _model, nameof(ReportData.Message), false, DataSourceUpdateMode.OnPropertyChanged));

            // Binding the model to the WPF usercontrol
            reportControl.DataContext = _model;
        }

        // Print button event
        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            // Print start
            new PrintClass().Print(_model);
        }
    }

    // Model class
    public class ReportData : INotifyPropertyChanged
    {
        private string _message = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        // Message property
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    // Update value and notify changes to bound controls
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
    }
}