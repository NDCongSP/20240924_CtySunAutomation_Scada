﻿using EasyScada.Core;
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
    }
}