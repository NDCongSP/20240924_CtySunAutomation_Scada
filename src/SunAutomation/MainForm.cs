using EasyScada.Core;
using EasyScada.Winforms.Controls;
using SunAutomation.Controls;
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
    public partial class MainForm : Form
    {
        EasyDriverConnector _easyDriverConnector;
        SettingsPanel _setingsPanel = new SettingsPanel();
        MonitorPanel _monitorPanel = new MonitorPanel();

        Control _activePanel;

        public MainForm()
        {            
            InitializeComponent();
            Text = "KhoiHung Tech";

            _easyDriverConnector = new EasyDriverConnector();
            _easyDriverConnector.ConnectionStatusChaged += _easyDriverConnector_ConnectionStatusChaged;
            _easyDriverConnector.BeginInit();
            _easyDriverConnector.EndInit();
            _lbStatus.Text = _easyDriverConnector.ConnectionStatus.ToString();

            Load += MainForm_Load;

            _btnMain.Click += _btnMain_Click;
            _btnMonitor.Click += _btnMonitor_Click;
            _btnSetup.Click += _btnSetup_Click;
            
        }

        private void _easyDriverConnector_ConnectionStatusChaged(object sender, ConnectionStatusChangedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    _lbConnectionStatus.Text = e.NewStatus.ToString();
                    _lbConnectionStatus.ForeColor = GetConnectionStatusColor(e.NewStatus);
                }));
            }
            else
            {
                _lbConnectionStatus.Text = e.NewStatus.ToString();
                _lbConnectionStatus.ForeColor = GetConnectionStatusColor(e.NewStatus);
            }           
        }

        private Color GetConnectionStatusColor(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connected:
                    return Color.Lime;
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Reconnecting:
                    return Color.Orange;
                case ConnectionStatus.Disconnected:
                    return Color.Red;
                default:
                    return Color.White;
            }
        }

        private void _btnMonitor_Click(object sender, EventArgs e)
        {
            if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
            {
                MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ShowPanel(_monitorPanel);
        }

        private void _btnMain_Click(object sender, EventArgs e)
        {
            if (_activePanel != null)
            {
                _activePanel.Visible = false;
                _activePanel.SendToBack();                
            }
        }

        private void _btnSetup_Click(object sender, EventArgs e)
        {
            if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
            {
                MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ShowPanel(_setingsPanel);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var serverAddress = _easyDriverConnector.ServerAddress;
        }

        private bool ContainsControl(Control control)
        {
            foreach (Control item in _panelMain.Controls)
            {
                if (item == control)
                    return true;
            }
            return false;
        }

        private void ShowPanel(Control panel)
        {
            if (_activePanel != null)
            {
                _activePanel.Visible = false;
                _activePanel.SendToBack();
            }

            if (!ContainsControl(panel))
            {
                _panelMain.Controls.Add(panel);
            }

            panel.Dock = DockStyle.Fill; ;
            panel.Visible = true;
            _activePanel = panel;
            panel.BringToFront();
        }

        private bool _isBusy = false;

        private async void _btnStart_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            _isBusy = true;
            try
            {
                if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
                {
                    MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tag = _easyDriverConnector.GetTag("Local Station/Channel1/Device1/Start_stop");
                if (tag == null)
                {
                    MessageBox.Show("Không tìm thấy tag Start_stop", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await tag.WriteAsync("1234");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không ghi được lệnh Start. {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
            }

        }

        private async void _btnStop_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            _isBusy = true;
            try
            {
                if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
                {
                    MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tag = _easyDriverConnector.GetTag("Local Station/Channel1/Device1/Start_stop");
                if (tag == null)
                {
                    MessageBox.Show("Không tìm thấy tag Start_stop", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await tag.WriteAsync("4321");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không ghi được lệnh Start. {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private async void _btnInc1_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            _isBusy = true;
            try
            {
                if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
                {
                    MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //
                var tag = _easyDriverConnector.GetTag("Local Station/Channel1/Device1/Target");
                if (tag == null)
                {
                    MessageBox.Show("Không tìm thấy tag Target", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(tag.Value, out double targetValue))
                {
                    MessageBox.Show("Target không có giá trị", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                targetValue++;

                await tag.WriteAsync($"{targetValue}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không ghi được lệnh Start. {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private async void _btnDec1_Click(object sender, EventArgs e)
        {
            if (_isBusy) return;

            _isBusy = true;
            try
            {
                if (_easyDriverConnector.ConnectionStatus != ConnectionStatus.Connected)
                {
                    MessageBox.Show("Mất kết nối với server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tag = _easyDriverConnector.GetTag("Local Station/Channel1/Device1/Target");
                if (tag == null)
                {
                    MessageBox.Show("Không tìm thấy tag Target", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(tag.Value, out double targetValue))
                {
                    MessageBox.Show("Target không có giá trị", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                targetValue--;

                await tag.WriteAsync($"{targetValue}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không ghi được lệnh Start. {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
