﻿using RagnaCustoms.App.Views;
using RagnaCustoms.Models;
using RagnaCustoms.Presenters;
using RagnaCustoms.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RagnaCustoms.Views
{
    public partial class SongForm : Form, ISongView
    {
        public SongPresenter Presenter { private get; set; }
        public IEnumerable<SongSearchModel> Songs
        {
            get => (IEnumerable<SongSearchModel>)SearchResultGridView.DataSource;
            set => SearchResultGridView.DataSource = value;
        }
        public bool SendScoreAutomatically
        {
            get => SendScoreAutomaticallyMenuItem.Checked;
            set => SendScoreAutomaticallyMenuItem.Checked = value;
        }
         public bool AutoCloseDownload
        {
            get => autoCloseDownloadToolStripMenuItem.Checked;
            set => autoCloseDownloadToolStripMenuItem.Checked = value;
        }
        public bool Overlay
        {
            get => overlayToolStripMenuItem.Checked;
            set => overlayToolStripMenuItem.Checked = value;
        }

        public SongForm()
        {
            InitializeComponent();

            SearchResultGridView.AutoGenerateColumns = false;

            Text += $" {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
        }

        public virtual void ShowAsPopup()
        {
            ShowDialog();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await Presenter.SearchOnlineAsync(SearchTextBox.Text);
        }

        private void SearchResultGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var song = (SongSearchModel)senderGrid.Rows[e.RowIndex].DataBoundItem;

                Presenter.DownloadAsync(song.Id);
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApiKeyMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog("Enter your API key :", "RagnaCustoms", Presenter.ApiKey);
        }

        private void SendScoreMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.SendScoreAutomatically = SendScoreAutomaticallyMenuItem.Checked;
        }

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Process.Start("explorer.exe", dir);
        }

        private void logScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.Views.LogsForm logsForm = new App.Views.LogsForm();
            logsForm.Show();
         private void autoCloseDownloadToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.AutoCloseDownload = autoCloseDownloadToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm()).ShowDialog();
        }

        private void SendScoreAutomaticallyMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var device = Oculus.GetDevice();
            if(device != null)
            {
                MessageBox.Show($"{device.Manufacturer} {device.Description} found", "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No compatible device found", "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void syncSongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = Oculus.SyncSongs();
            if (result == 0)
            {
                MessageBox.Show("Sync complete", "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == 1)
            {
                MessageBox.Show("No compatible device found", "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void compareSongsVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.CompareSongsAsync();
        }

        private void twitchBotToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            (new TwitchBotForm()).Show();
        }

        private void gotoOverlayUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Presenter.ApiKey))
            {
                MessageBox.Show("You need to set your API key first", "RagnaCustoms", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ProcessStartInfo sInfo = new ProcessStartInfo($"https://ragnacustoms.com/overlay/display/{Presenter.ApiKey}");
                Process.Start(sInfo);
            }
         
        }

        private void overlayToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Presenter.Overlay = overlayToolStripMenuItem.Checked;

        }

        private void configureApiKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Presenter.ApiKey = Prompt.ShowDialog("Enter your API key :", "RagnaCustoms", Presenter.ApiKey);
        }
    }
}
