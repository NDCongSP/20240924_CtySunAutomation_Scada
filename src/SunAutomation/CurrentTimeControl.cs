using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunAutomation
{
    public partial class CurrentTimeControl : UserControl
    {
        public CurrentTimeControl()
        {
            InitializeComponent();

            Load += CurrentTimeControl_Load;
        }

        private void CurrentTimeControl_Load(object sender, EventArgs e)
        {
            RefreshDateTime();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                RefreshDateTime();
            }
            catch { }
            finally
            {
                if (!this.FindForm().IsDisposed)
                {
                    timer1.Start();
                }
            }
        }

        private void RefreshDateTime()
        {
            var now = DateTime.Now;
            _lbCurrentDate.Text = $"{now:yyyy-MM-dd}";
            _lbCurrentTime.Text = $"{now:HH}   :   {now:mm}:   {now:ss}";
        }
    }
}
