﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XmlExplorer.TreeView;

namespace XmlExplorer.Controls
{
	public partial class ErrorWindow : DockContent
	{
		#region Variables

		private List<Error> _errors;

		private ErrorsHeader header;

		#endregion

		#region Constructors

		public ErrorWindow()
		{
			InitializeComponent();

			this.listView.ItemActivate += new EventHandler(listViewExpressions_ItemActivate);

			header = new ErrorsHeader();

			header.BrowseClicked += new EventHandler(header_BrowseClicked);

			this.elementHost.Child = header;
		}

		#endregion

		#region Properties

		public List<Error> Errors
		{
			get
			{
				return _errors;
			}

			set
			{
				_errors = value;
				this.LoadErrors(_errors);
			}
		}

		public ErrorsHeader Header
		{
			get
			{
				return this.header;
			}
		}

		#endregion

		#region Events

		public event EventHandler<EventArgs<Error>> ErrorActivated;

		public event EventHandler BrowseClicked;

		#endregion

		#region Methods

		private void AutoSizeListViewColumns(ListView listView)
		{
			foreach (ColumnHeader header in listView.Columns)
			{
				header.Width = -1;
				int width = header.Width;
				header.Width = -2;
				if (width > header.Width)
					header.Width = width;
			}
		}

		public void LoadErrors(List<Error> errors)
		{
			try
			{
				this.listView.BeginUpdate();

				this.listView.Items.Clear();

				if (errors != null)
				{
					foreach (Error error in errors)
					{
						ListViewItem item = new ListViewItem(error.DefaultOrder.ToString());
						item.SubItems.Add(error.Description);
						item.SubItems.Add(error.File);
						item.Tag = error;
						item.ImageKey = error.Category.ToString();
						this.listView.Items.Add(item);
					}
				}
			}
			finally
			{
				this.listView.AutoResizeColumns();
				this.listView.EndUpdate();
			}
		}

		#endregion

		#region Event Handlers

		void listViewExpressions_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				if (this.listView.SelectedItems.Count < 1)
					return;

				ListViewItem item = this.listView.SelectedItems[0];

				Error error = item.Tag as Error;
				if (error == null)
					return;

				this.ErrorActivated(this, new EventArgs<Error>(error));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				ExceptionDialog.ShowDialog(this, ex);
			}
		}

		void header_BrowseClicked(object sender, EventArgs e)
		{
			try
			{
				if (this.BrowseClicked != null)
					this.BrowseClicked(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				ExceptionDialog.ShowDialog(this, ex);
			}
		}

		#endregion
	}
}
