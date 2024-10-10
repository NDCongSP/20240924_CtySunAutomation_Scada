using EasyScada.Core;
using EasyScada.Winforms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunAutomation.Controls
{
    public partial class MonitorPanel : UserControl
    {
        Dictionary<string, EasyLabel> _tagToLabel = new Dictionary<string, EasyLabel>();

        public MonitorPanel()
        {
            InitializeComponent();

            Load += MonitorPanel_Load;
        }

        private void MonitorPanel_Load(object sender, EventArgs e)
        {
            foreach (var tagLabel in FindStatusControl())
            {
                if (!string.IsNullOrWhiteSpace(tagLabel.TagPath))
                {
                    _tagToLabel[tagLabel.TagPath] = tagLabel;
                    tagLabel.LinkedTag.QualityChanged += LinkedTag_QualityChanged;
                }
                UpdateLabelStatus(tagLabel);
            }
        }

        private void UpdateLabelStatus(EasyLabel easyLabel)
        {
            var qualityLabel = easyLabel.Tag as Label;
            if (string.IsNullOrEmpty(easyLabel.TagPath) && easyLabel.LinkedTag == null)
            {
                easyLabel.Text = "";
                qualityLabel.BackColor = Color.DarkGray;
                qualityLabel.Text = "N/A";
                qualityLabel.ForeColor = Color.Black;
            }
            else
            {
                var quality = easyLabel.LinkedTag.Quality;
                switch (quality)
                {
                    case Quality.Uncertain:
                    case Quality.Bad:
                        qualityLabel.Text = "ER";
                        qualityLabel.BackColor = Color.Red;
                        qualityLabel.ForeColor = Color.White;
                        break;
                    case Quality.Good:
                        qualityLabel.Text = "OK";
                        qualityLabel.BackColor = Color.Green;
                        qualityLabel.ForeColor = Color.White;
                        break;
                    default:
                        break;
                }
            }
        }

        private void LinkedTag_QualityChanged(object sender, TagQualityChangedEventArgs e)
        {
            if (_tagToLabel.ContainsKey(e.Tag.Path))
            {
                var tagLabel = _tagToLabel[e.Tag.Path];
                this.Invoke(new Action(() =>
                {
                    UpdateLabelStatus(tagLabel);
                }));
            }
        }

        private IEnumerable<EasyLabel> FindStatusControl()
        {
            foreach (var control in Controls)
            {
                Control statusControl = null;
                EasyLabel tagLabel = null;

                foreach (var child in (control as Control).Controls)
                {
                    if (child is Control childControl)
                    {
                        if (childControl.Text == "OK")
                        {
                            statusControl = childControl;
                        }

                        if (childControl is EasyLabel easyLabel)
                        {
                            tagLabel = easyLabel;
                        }

                        if (tagLabel != null && childControl != null)
                        {
                            tagLabel.Tag = statusControl;
                            yield return tagLabel;
                            break;
                        }
                    }
                }
            }
        }
    }
}
