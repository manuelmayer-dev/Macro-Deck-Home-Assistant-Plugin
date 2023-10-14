
using SuchByte.MacroDeck.GUI.CustomControls;

namespace SuchByte.HomeAssistantPlugin.GUI
{
    partial class EntitySelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.entityList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.searchBox = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.SuspendLayout();
            // 
            // entityList
            // 
            this.entityList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.entityList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityList.ForeColor = System.Drawing.Color.White;
            this.entityList.FormattingEnabled = true;
            this.entityList.Location = new System.Drawing.Point(12, 89);
            this.entityList.Name = "entityList";
            this.entityList.Size = new System.Drawing.Size(487, 360);
            this.entityList.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(465, 48);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select all the entities you want to use in Macro Deck as a variable";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnOk.BorderRadius = 8;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(184)))));
            this.btnOk.Icon = null;
            this.btnOk.Location = new System.Drawing.Point(424, 455);
            this.btnOk.Name = "btnOk";
            this.btnOk.Progress = 0;
            this.btnOk.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.searchBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.searchBox.Icon = global::SuchByte.HomeAssistantPlugin.Properties.Resources.magnifying_glass;
            this.searchBox.Location = new System.Drawing.Point(249, 58);
            this.searchBox.Multiline = false;
            this.searchBox.Name = "searchBox";
            this.searchBox.Padding = new System.Windows.Forms.Padding(26, 5, 8, 5);
            this.searchBox.PasswordChar = false;
            this.searchBox.PlaceHolderColor = System.Drawing.Color.Gray;
            this.searchBox.PlaceHolderText = "Search entitiy";
            this.searchBox.ReadOnly = false;
            this.searchBox.SelectionStart = 0;
            this.searchBox.Size = new System.Drawing.Size(250, 25);
            this.searchBox.TabIndex = 6;
            this.searchBox.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // EntitySelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 490);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.entityList);
            this.Name = "EntitySelector";
            this.Text = "EntitySelector";
            this.Shown += new System.EventHandler(this.EntitySelector_Shown);
            this.Controls.SetChildIndex(this.entityList, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.searchBox, 0);
            this.ResumeLayout(false);

        }


        #endregion

        private RoundedTextBox searchBox;
        private System.Windows.Forms.CheckedListBox entityList;
        private System.Windows.Forms.Label label1;
        private MacroDeck.GUI.CustomControls.ButtonPrimary btnOk;
    }
}