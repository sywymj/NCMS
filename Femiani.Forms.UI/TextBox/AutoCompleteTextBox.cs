using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;

namespace Femiani.Forms.UI.Input
{
	/// <summary>
	/// Summary description for AutoCompleteTextBox.
	/// </summary>
	[Serializable]
	public class AutoCompleteTextBox : TextBox
	{

		#region Classes and Structures

		public enum EntryMode
		{
			Text,
			List
		}

		/// <summary>
		/// This is the class we will use to hook mouse events.
		/// </summary>
		private class WinHook : NativeWindow
		{
			private AutoCompleteTextBox tb;

			/// <summary>
			/// Initializes a new instance of <see cref="WinHook"/>
			/// </summary>
			/// <param name="tbox">The <see cref="AutoCompleteTextBox"/> the hook is running for.</param>
			public WinHook(AutoCompleteTextBox tbox)
			{
				this.tb = tbox;
			}

			/// <summary>
			/// Look for any kind of mouse activity that is not in the
			/// text box itself, and hide the popup if it is visible.
			/// </summary>
			/// <param name="m"></param>
			protected override void WndProc(ref Message m)
			{
				switch (m.Msg)
				{
					case Win32.Messages.WM_LBUTTONDOWN:
					case Win32.Messages.WM_LBUTTONDBLCLK:
					case Win32.Messages.WM_MBUTTONDOWN:
					case Win32.Messages.WM_MBUTTONDBLCLK:
					case Win32.Messages.WM_RBUTTONDOWN:
					case Win32.Messages.WM_RBUTTONDBLCLK:
					case Win32.Messages.WM_NCLBUTTONDOWN:
					case Win32.Messages.WM_NCMBUTTONDOWN:
					case Win32.Messages.WM_NCRBUTTONDOWN:
					{
						// Lets check to see where the event took place
						Form form = tb.FindForm();
						Point p = form.PointToScreen(new Point((int)m.LParam));
						Point p2 = tb.PointToScreen(new Point(0, 0));
						Rectangle rect = new Rectangle(p2, tb.Size);
						// Hide the popup if it is not in the text box
						if (!rect.Contains(p))
						{
							tb.HideList();
						}
					} break;
					case Win32.Messages.WM_SIZE:
					case Win32.Messages.WM_MOVE:
					{
						tb.HideList();
					} break;
					// This is the message that gets sent when a childcontrol gets activity
					case Win32.Messages.WM_PARENTNOTIFY:
					{
						switch ((int)m.WParam)
						{
							case Win32.Messages.WM_LBUTTONDOWN:
							case Win32.Messages.WM_LBUTTONDBLCLK:
							case Win32.Messages.WM_MBUTTONDOWN:
							case Win32.Messages.WM_MBUTTONDBLCLK:
							case Win32.Messages.WM_RBUTTONDOWN:
							case Win32.Messages.WM_RBUTTONDBLCLK:
							case Win32.Messages.WM_NCLBUTTONDOWN:
							case Win32.Messages.WM_NCMBUTTONDOWN:
							case Win32.Messages.WM_NCRBUTTONDOWN:
							{
								// Same thing as before
								Form form = tb.FindForm();
								Point p = form.PointToScreen(new Point((int)m.LParam));
								Point p2 = tb.PointToScreen(new Point(0, 0));
								Rectangle rect = new Rectangle(p2, tb.Size);
								if (!rect.Contains(p))
								{
									tb.HideList();
								}
							} break;
						}
					} break;
				}
				
				base.WndProc (ref m);
			}
		}

		#endregion

		#region Members
        /// <summary>
        /// 
        /// </summary>
        /// 


        //*************************************
		//private ListBox list;
        private C1FlexGrid flexGrid;
		protected Form popup;
		private AutoCompleteTextBox.WinHook hook;
        

        //添加的数据成员；
        private DataView mDataView;
        public DataView DataSource
        {
            get
            {
                return mDataView;
            }
            set
            {
                this.mDataView = value;
                this.flexGrid.DataSource = mDataView;
                this.flexGrid.Cols.Fixed = 0;
                this.flexGrid.AutoSizeCols();
                
            }
        }
        private bool mEnterToTab;
        public bool EnterToTab
        {
            get
            {
                return mEnterToTab;
            }
            set
            {
                this.mEnterToTab = value;
            }
        }
        
        private string mFilterStr;
        public string FilterStr
        {
            get
            {
                return mFilterStr;
            }
            set
            {
                this.mFilterStr=value;
            }
        }
        private string mObjectCol;
        public string ObjectCol
        {
            get
            {
                return mObjectCol;
            }
            set
            {
                this.mObjectCol = value;
            }
        }
        private object mObjectMemBer;
        public object ObjectMember
        {
            get
            {
                if (this.flexGrid.RowSel<=0)
                {
                    return null;
                } 
                else
                {
                    return this.flexGrid.Rows[this.flexGrid.RowSel][this.ObjectCol];
                }
            }
            
        }
        public bool checkValue()
        {
            if (this.flexGrid.RowSel<=0)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }
        private string  mValueCol;
        public string ValueCol
        {
            get
            {
                return mValueCol;
            }
            set
            {
                this.mValueCol =value;
            }
        }

		#endregion

		#region Properties

		private AutoCompleteTextBox.EntryMode mode = EntryMode.Text;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public AutoCompleteTextBox.EntryMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		private AutoCompleteEntryCollection items = new AutoCompleteEntryCollection();
		[System.ComponentModel.Editor(typeof(AutoCompleteEntryCollection.AutoCompleteEntryCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public AutoCompleteEntryCollection Items
		{
			get
			{
				return this.items;
			}
			set
			{
				this.items = value;
			}
		}

		private AutoCompleteTriggerCollection triggers = new AutoCompleteTriggerCollection();
		[System.ComponentModel.Editor(typeof(AutoCompleteTriggerCollection.AutoCompleteTriggerCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public AutoCompleteTriggerCollection Triggers
		{
			get
			{
				return this.triggers;
			}
			set
			{
				this.triggers = value;
			}
		}

		[Browsable(true)]
		[Description("The width of the popup (-1 will auto-size the popup to the width of the textbox).")]
		public int PopupWidth
		{
			get
			{
				return this.popup.Width;
			}
			set
			{
				if (value == -1)
				{
					this.popup.Width = this.Width;
				} 
				else
				{
					this.popup.Width = value;
				}
			}
		}
        public int PopupHeight
        {
            get
            {
                return this.popup.Height;
            }
            set
            {
                this.popup.Height = value;
            }
        }
		public BorderStyle PopupBorderStyle
		{
			get
			{
                //***************************************************
				//return this.list.BorderStyle;
                //return this.flexGrid.BorderStyle;
                return BorderStyle.None;
            }
			set
			{
				//this.list.BorderStyle = value;
               // this.flexGrid.BorderStyle = value;
			}
		}

		private Point popOffset = new Point(12, 0);
		[Description("The popup defaults to the lower left edge of the textbox.")]
		public Point PopupOffset
		{
			get
			{
				return this.popOffset;
			}
			set
			{
				this.popOffset = value;
			}
		}

		private Color popSelectBackColor = SystemColors.Highlight;
		public Color PopupSelectionBackColor
		{
			get
			{
				return this.popSelectBackColor;
			}
			set
			{
				this.popSelectBackColor = value;
			}
		}

		private Color popSelectForeColor = SystemColors.HighlightText;
		public Color PopupSelectionForeColor
		{
			get
			{
				return this.popSelectForeColor;
			}
			set
			{
				this.popSelectForeColor = value;
			}
		}

		private bool triggersEnabled = true;
		protected bool TriggersEnabled
		{
			get
			{
				return this.triggersEnabled;
			}
			set
			{
				this.triggersEnabled = value;
			}
		}

		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				this.TriggersEnabled = false;
				base.Text = value;
				this.TriggersEnabled = true;
			}
		}

		#endregion
       

		public AutoCompleteTextBox()
		{
            //this.dv = new DataView();
            //this.filterStr = "";
            //this.valueCol = 0;
            this.EnterToTab = false;

            this.mObjectMemBer = null;

			// Create the form that will hold the list
			this.popup = new Form();
			this.popup.StartPosition = FormStartPosition.Manual;
			this.popup.ShowInTaskbar = false;
			this.popup.FormBorderStyle = FormBorderStyle.None;
			this.popup.TopMost = true;
			this.popup.Deactivate += new EventHandler(Popup_Deactivate);

            

            //************************************************************
			// Create the list box that will hold mathcing items
            //this.list = new ListBox();
            //this.list.Cursor = Cursors.Hand;
            //this.list.BorderStyle = BorderStyle.None;
            //this.list.SelectedIndexChanged += new EventHandler(List_SelectedIndexChanged);
            //this.list.MouseDown += new MouseEventHandler(List_MouseDown);
            //this.list.ItemHeight = 14;
            //this.list.DrawMode = DrawMode.OwnerDrawFixed;
            //this.list.DrawItem += new DrawItemEventHandler(List_DrawItem);
            //this.list.Dock = DockStyle.Fill;

		
			// Add the list box to the popup form

			//this.popup.Controls.Add(this.list);
            

            this.flexGrid = new C1FlexGrid();
            this.flexGrid.Cursor = Cursors.Hand;
            //this.flexGrid.BorderStyle = BorderStyle.None;
            this.flexGrid.AfterSelChange += new RangeEventHandler(flexGrid_AfterSelChange);
            this.flexGrid.MouseDown += new MouseEventHandler(flexGrid_MouseDown);
            this.flexGrid.Dock = DockStyle.Fill;
            this.flexGrid.SelectionMode = SelectionModeEnum.Row;
            this.flexGrid.DrawMode = DrawModeEnum.Normal;
            this.flexGrid.AllowEditing = false;
            this.flexGrid.ExtendLastCol = true;
            this.flexGrid.Cols.Fixed = 0;
            

            this.popup.Controls.Add(this.flexGrid);

            //this.flexGrid.DataSource = dv;
            //this.flexGrid.DataMember = "test";
            //this.flexGrid.DataSource = dv;


			// Add default triggers.
			this.triggers.Add(new TextLengthTrigger(2));
			this.triggers.Add(new ShortCutTrigger(Keys.Enter, TriggerState.SelectAndConsume));
            
			this.triggers.Add(new ShortCutTrigger(Keys.Tab, TriggerState.Select));
			this.triggers.Add(new ShortCutTrigger(Keys.Control | Keys.F, TriggerState.ShowAndConsume));
			this.triggers.Add(new ShortCutTrigger(Keys.Escape, TriggerState.HideAndConsume));
		}

        void flexGrid_MouseDown(object sender, MouseEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            //for (int i = 0; i < this.list.Items.Count; i++)
            //{
            //    if (this.list.GetItemRectangle(i).Contains(e.X, e.Y))
            //    {
            //        this.list.SelectedIndex = i;
            //        this.SelectCurrentItem();
            //    }
            //}
            //this.HideList();
           // if (this.Mode != EntryMode.List)
           // {
                SelectCurrentItem();
            //}
            this.HideList();
            
        }

        void flexGrid_AfterSelChange(object sender, RangeEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            //if (this.Mode != EntryMode.List)
            //{
            //    SelectCurrentItem();
            //}
        }

		protected virtual bool DefaultCmdKey(ref Message msg, Keys keyData)
		{

			bool val = base.ProcessCmdKey (ref msg, keyData);
            //bool val;

			if (this.TriggersEnabled)
			{
				switch (this.Triggers.OnCommandKey(keyData))
				{
					case TriggerState.ShowAndConsume:
					{
						val = true;
						this.ShowList();
                       
					} break;
					case TriggerState.Show:
					{
                        
						this.ShowList();
					} break;
					case TriggerState.HideAndConsume:
					{
						val = true;
						this.HideList();
					} break;
					case TriggerState.Hide:
					{
						this.HideList();
					} break;
					case TriggerState.SelectAndConsume:
					{
						if (this.popup.Visible == true)
						{
							val = true;
							this.SelectCurrentItem();
                            if (this.EnterToTab)
                            {
                                SendKeys.Send("{TAB}");
                            }
                            
						}
					} break;
					case TriggerState.Select:
					{
						if (this.popup.Visible == true)
						{
							this.SelectCurrentItem();
						}
					} break;
					default:
						break;
				}
			}
            //val = base.ProcessCmdKey(ref msg, keyData);
			return val;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Up:
				{
					this.Mode = EntryMode.List;
					//*******************************************
                    //if (this.list.SelectedIndex > 0)
                    //{
                    //    this.list.SelectedIndex--;
                    //}
                    if (this.flexGrid.RowSel>1)
                    {
                        //this.flexGrid.RowSel--;
                        this.flexGrid.Select(this.flexGrid.RowSel - 1, 0);
                    }
					return true;
				} break;
				case Keys.Down:
				{
					this.Mode = EntryMode.List;
                    //if (this.list.SelectedIndex < this.list.Items.Count - 1)
                    //{
                    //    this.list.SelectedIndex++;
                    //}
                    if (this.flexGrid.RowSel<this.flexGrid.Rows.Count-1)
                    {
                        //this.flexGrid.RowSel++;
                        this.flexGrid.Select(this.flexGrid.RowSel + 1, 0);
                        
                    }
					return true;
				} break;
				default:
				{
					return DefaultCmdKey(ref msg, keyData);
				} break;
			}
            
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);

			if (this.TriggersEnabled)
			{
				switch (this.Triggers.OnTextChanged(this.Text))
				{
					case TriggerState.Show:
					{
						this.ShowList();
					} break;
					case TriggerState.Hide:
					{
						this.HideList();
					} break;
					default:
					{
						this.UpdateList();
					} break;
				}
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);
            ///*************************************
            ///

			//if (!(this.Focused || this.popup.Focused || this.list.Focused))
            if (!(this.Focused || this.popup.Focused || this.flexGrid.Focused))
            {
				this.HideList();
			}
		}

		protected virtual void SelectCurrentItem()
		{
            //*****************************************
			//if (this.list.SelectedIndex == -1)
            if(this.flexGrid.RowSel<=0)
			{
				return;
			}

			this.Focus();
			//this.Text = this.list.SelectedItem.ToString();
            this.Text = this.flexGrid.Rows[this.flexGrid.RowSel][this.ValueCol].ToString();
            //this.ObjectMember = this.flexGrid.Rows[this.flexGrid.RowSel][this.ObjectCol];
            if (this.Text.Length > 0)
			{
				this.SelectionStart = this.Text.Length;
			}

			this.HideList();
		}

		protected virtual void ShowList()
		{
			if (this.popup.Visible == false)
			{
                //***********************************************
                this.flexGrid.RowSel = -1;
				//this.list.SelectedIndex = -1;
				this.UpdateList();
				Point p = this.PointToScreen(new Point(0,0));
				p.X += this.PopupOffset.X;
				p.Y += this.Height + this.PopupOffset.Y;
				this.popup.Location = p;
				if(this.flexGrid.Rows.Count>1)
                //if (this.list.Items.Count > 0)
				{
					this.popup.Show();
					if (this.hook == null)
					{
						this.hook = new WinHook(this);
						this.hook.AssignHandle(this.FindForm().Handle);
					}
					this.Focus();
				}
			} 
			else
			{
				this.UpdateList();
			}
		}

		protected virtual void HideList()
		{
            this.Mode = EntryMode.Text;
            if (this.hook != null)
                this.hook.ReleaseHandle();
            this.hook = null;
            this.popup.Hide();
		}

		protected virtual void UpdateList()
		{
            //string filterTemp = this.filterStr+this.Text+"%'";
            //this.dv.RowFilter = filterTemp;
            string filterStrTemp = string.Format(FilterStr, this.Text);
            this.DataSource.RowFilter = filterStrTemp;


            if (this.flexGrid.Rows.Count <= 1)
            {
                this.HideList();
            }
            else
            {
                this.flexGrid.RowSel = 1;
            }

            this.popup.Width = this.PopupWidth;

            //object selectedItem = this.list.SelectedItem;

            //this.list.Items.Clear();
            //this.list.Items.AddRange(this.FilterList(this.Items).ToObjectArray());

            //if (selectedItem != null &&
            //    this.list.Items.Contains(selectedItem))
            //{
            //    EntryMode oldMode = this.Mode;
            //    this.Mode = EntryMode.List;
            //    this.list.SelectedItem = selectedItem;
            //    this.Mode = oldMode;
            //}

            //if (this.list.Items.Count == 0)
            //{
            //    this.HideList();
            //} 
            //else
            //{
            //    int visItems = this.list.Items.Count;
            //    if (visItems > 8)
            //        visItems = 8;

            //    this.popup.Height = (visItems * this.list.ItemHeight) + 2;
            //    switch (this.BorderStyle)
            //    {
            //        case BorderStyle.FixedSingle:
            //        {
            //            this.popup.Height += 2;
            //        } break;
            //        case BorderStyle.Fixed3D:
            //        {
            //            this.popup.Height += 4;
            //        } break;
            //        case BorderStyle.None:
            //        default:
            //        {
            //        } break;
            //    }
				
            //    this.popup.Width = this.PopupWidth;

            //    if (this.list.Items.Count > 0 &&
            //        this.list.SelectedIndex == -1)
            //    {
            //        EntryMode oldMode = this.Mode;
            //        this.Mode = EntryMode.List;
            //        this.list.SelectedIndex = 0;
            //        this.Mode = oldMode;
            //    }

            //}
		}

		protected virtual AutoCompleteEntryCollection FilterList(AutoCompleteEntryCollection list)
		{
			AutoCompleteEntryCollection newList = new AutoCompleteEntryCollection();
			foreach (IAutoCompleteEntry entry in list)
			{
				foreach (string match in entry.MatchStrings)
				{
					if (match.ToUpper().StartsWith(this.Text.ToUpper()))
					{
						newList.Add(entry);
						break;
					}
				}
			}
			return newList;
		}

		private void List_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.Mode != EntryMode.List)
			{
				SelectCurrentItem();
			}
		}

		private void List_MouseDown(object sender, MouseEventArgs e)
		{
            //for (int i=0; i<this.list.Items.Count; i++)
            //{
            //    if (this.list.GetItemRectangle(i).Contains(e.X, e.Y))
            //    {
            //        this.list.SelectedIndex = i;
            //        this.SelectCurrentItem();
            //    }
            //}
            //this.HideList();
		}

		private void List_DrawItem(object sender, DrawItemEventArgs e)
		{
            //Color bColor = e.BackColor;
            //if (e.State == DrawItemState.Selected)
            //{
            //    e.Graphics.FillRectangle(new SolidBrush(this.PopupSelectionBackColor), e.Bounds);
            //    e.Graphics.DrawString(this.list.Items[e.Index].ToString(), e.Font, new SolidBrush(this.PopupSelectionForeColor), e.Bounds, StringFormat.GenericDefault);
            //} 
            //else
            //{
            //    e.DrawBackground();
            //    e.Graphics.DrawString(this.list.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            //}
		}

		private void Popup_Deactivate(object sender, EventArgs e)
		{
			//if (!(this.Focused || this.popup.Focused || this.list.Focused))
            if (!(this.Focused || this.popup.Focused || this.flexGrid.Focused))
			{
				this.HideList();
			}
		}

	}
}
