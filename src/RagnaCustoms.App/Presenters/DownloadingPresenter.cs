﻿using RagnaCustoms.Models;
using RagnaCustoms.Views;

namespace RagnaCustoms.Presenters
{
    public class DownloadingPresenter : IDownloadingPresenter
    {
        protected virtual IDownloadingView View { get; }
        protected virtual ISongProvider SongProvider { get; }

        public DownloadingPresenter(ISongProvider songProvider)
        {
            SongProvider = songProvider;
        }
        public DownloadingPresenter(IDownloadingView view, ISongProvider songProvider) : this(songProvider)
        {
            View = view;
            View.Presenter = this;
        }

        public virtual void Show() => View.Show();
        public virtual void ShowAsPopup() => View.ShowAsPopup();
        public virtual void Close() => View.Close();

        public virtual void Download(int songId, bool autoClose = false)
        {
            View.DownloadPercent = default;

            SongProvider.DownloadAsync(songId, DownloadProgressChanged, DownloadCompleted, DownloadTitle, autoClose);
        }
        public virtual void DownloadTitle(string downloadTitle)
        {
            View.Title = downloadTitle;
        }

        public virtual void DownloadProgressChanged(int downloadPercent)
        {
            View.DownloadPercent = downloadPercent;
        }

        public virtual void DownloadCompleted(bool autoClose = false)
        {
            if (!autoClose)
            {
                View.ShowSuccessMessage("Download completed successfuly.", "Information");
            }
            View.Close();
        }
    }
}
