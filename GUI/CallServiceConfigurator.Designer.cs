
using SuchByte.MacroDeck.GUI.CustomControls;

namespace SuchByte.HomeAssistantPlugin.GUI
{
    partial class CallServiceConfigurator
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.servicesBox = new SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.targetSelector = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.entityBox = new SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.targetSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // servicesBox
            // 
            this.servicesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.servicesBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.servicesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.servicesBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.servicesBox.Icon = null;
            this.servicesBox.Location = new System.Drawing.Point(129, -1);
            this.servicesBox.Name = "servicesBox";
            this.servicesBox.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.servicesBox.SelectedIndex = -1;
            this.servicesBox.SelectedItem = null;
            this.servicesBox.Size = new System.Drawing.Size(351, 26);
            this.servicesBox.TabIndex = 0;
            this.servicesBox.SelectedIndexChanged += new System.EventHandler(this.ServicesBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Service";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.targetSelector);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 114);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(709, 153);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // targetSelector
            // 
            this.targetSelector.Controls.Add(this.label2);
            this.targetSelector.Controls.Add(this.entityBox);
            this.targetSelector.Location = new System.Drawing.Point(3, 3);
            this.targetSelector.Name = "targetSelector";
            this.targetSelector.Size = new System.Drawing.Size(672, 36);
            this.targetSelector.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Entity";
            // 
            // entityBox
            // 
            this.entityBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.entityBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.entityBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.entityBox.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.entityBox.Icon = null;
            this.entityBox.Location = new System.Drawing.Point(123, 3);
            this.entityBox.Name = "entityBox";
            this.entityBox.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.entityBox.SelectedIndex = -1;
            this.entityBox.SelectedItem = null;
            this.entityBox.Size = new System.Drawing.Size(351, 26);
            this.entityBox.TabIndex = 2;
            this.entityBox.SelectedIndexChanged += new System.EventHandler(this.EntityBox_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDescription.Location = new System.Drawing.Point(3, 70);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(709, 41);
            this.lblDescription.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(129, 33);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(351, 23);
            this.lblName.TabIndex = 4;
            // 
            // CallServiceConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.servicesBox);
            this.Name = "CallServiceConfigurator";
            this.Load += new System.EventHandler(this.CallServiceConfigurator_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.targetSelector.ResumeLayout(false);
            this.targetSelector.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private RoundedComboBox servicesBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel targetSelector;
        private System.Windows.Forms.Label label2;
        private RoundedComboBox entityBox;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblName;
    }
}
