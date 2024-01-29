using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegoTrainProject
{
	public partial class ListSelection : Form
	{
		private string _itemSelected = "";

		public string ListItemSelected { get { return _itemSelected; } }

		public ListSelection(string topText, List<string> items)
		{
			InitializeComponent();
			Text = topText;
			listBoxItems.ClearSelected();
			listBoxItems.DataSource = items;
		}

		public ListSelection(string topText, string[] items)
		{
			InitializeComponent();
			Text = topText;
			listBoxItems.ClearSelected();
			listBoxItems.DataSource = items;
		}

		private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			//listBoxItems.SelectedItem.
			_itemSelected = listBoxItems.GetItemText(listBoxItems.SelectedItem);
		}

		private void listBoxItems_SelectedValueChanged(object sender, EventArgs e)
		{

		}
	}
}
