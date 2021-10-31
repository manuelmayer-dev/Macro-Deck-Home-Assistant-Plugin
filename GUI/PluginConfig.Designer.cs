
using SuchByte.MacroDeck.GUI.CustomControls;

namespace SuchByte.HomeAssistantPlugin.GUI
{
    partial class PluginConfig
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
            Main.HomeAssistant.OnAuthFailed -= AuthFailed;
            Main.HomeAssistant.OnAuthSuccess -= AuthSuccess;
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
            this.btnOk = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.token = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputUrl = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkSSL = new System.Windows.Forms.CheckBox();
            this.btnVariables = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.SuspendLayout();
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
            this.btnOk.Location = new System.Drawing.Point(415, 108);
            this.btnOk.Name = "btnOk";
            this.btnOk.Progress = 0;
            this.btnOk.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // token
            // 
            this.token.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.token.Cursor = System.Windows.Forms.Cursors.Hand;
            this.token.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.token.ForeColor = System.Drawing.Color.White;
            this.token.Icon = null;
            this.token.Location = new System.Drawing.Point(83, 73);
            this.token.Multiline = false;
            this.token.Name = "token";
            this.token.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.token.PasswordChar = true;
            this.token.PlaceHolderColor = System.Drawing.Color.Gray;
            this.token.PlaceHolderText = "";
            this.token.ReadOnly = false;
            this.token.SelectionStart = 0;
            this.token.Size = new System.Drawing.Size(366, 29);
            this.token.TabIndex = 15;
            this.token.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 29);
            this.label4.TabIndex = 14;
            this.label4.Text = "Token:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(83, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "e.g. <ip address/hostname>:8123";
            // 
            // inputUrl
            // 
            this.inputUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.inputUrl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.inputUrl.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.inputUrl.ForeColor = System.Drawing.Color.White;
            this.inputUrl.Icon = null;
            this.inputUrl.Location = new System.Drawing.Point(83, 11);
            this.inputUrl.Multiline = false;
            this.inputUrl.Name = "inputUrl";
            this.inputUrl.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.inputUrl.PasswordChar = false;
            this.inputUrl.PlaceHolderColor = System.Drawing.Color.Gray;
            this.inputUrl.PlaceHolderText = "";
            this.inputUrl.ReadOnly = false;
            this.inputUrl.SelectionStart = 0;
            this.inputUrl.Size = new System.Drawing.Size(366, 29);
            this.inputUrl.TabIndex = 12;
            this.inputUrl.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "Host:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkSSL
            // 
            this.checkSSL.AutoSize = true;
            this.checkSSL.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkSSL.Location = new System.Drawing.Point(376, 44);
            this.checkSSL.Name = "checkSSL";
            this.checkSSL.Size = new System.Drawing.Size(54, 23);
            this.checkSSL.TabIndex = 17;
            this.checkSSL.Text = "SSL";
            this.checkSSL.UseVisualStyleBackColor = true;
            // 
            // btnVariables
            // 
            this.btnVariables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnVariables.BorderRadius = 8;
            this.btnVariables.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVariables.FlatAppearance.BorderSize = 0;
            this.btnVariables.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVariables.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnVariables.ForeColor = System.Drawing.Color.White;
            this.btnVariables.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(184)))));
            this.btnVariables.Icon = null;
            this.btnVariables.Location = new System.Drawing.Point(6, 108);
            this.btnVariables.Name = "btnVariables";
            this.btnVariables.Progress = 0;
            this.btnVariables.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnVariables.Size = new System.Drawing.Size(75, 25);
            this.btnVariables.TabIndex = 18;
            this.btnVariables.Text = "Variables";
            this.btnVariables.UseVisualStyleBackColor = false;
            this.btnVariables.Click += new System.EventHandler(this.BtnVariables_Click);
            // 
            // PluginConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 142);
            this.Controls.Add(this.btnVariables);
            this.Controls.Add(this.checkSSL);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.token);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputUrl);
            this.Controls.Add(this.label1);
            this.Name = "PluginConfig";
            this.Text = "PluginConfig";
            this.Load += new System.EventHandler(this.PluginConfig_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.inputUrl, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.token, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.checkSSL, 0);
            this.Controls.SetChildIndex(this.btnVariables, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MacroDeck.GUI.CustomControls.ButtonPrimary btnOk;
        private RoundedTextBox token;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private RoundedTextBox inputUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkSSL;
        private MacroDeck.GUI.CustomControls.ButtonPrimary btnVariables;
    }
}