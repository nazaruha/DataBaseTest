namespace WorkingDatabaseWF
{
    partial class MainForm
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
            this.btn_GenerateTable = new System.Windows.Forms.Button();
            this.dgUsers = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_GenerateTable
            // 
            this.btn_GenerateTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_GenerateTable.Location = new System.Drawing.Point(990, 541);
            this.btn_GenerateTable.Name = "btn_GenerateTable";
            this.btn_GenerateTable.Size = new System.Drawing.Size(281, 82);
            this.btn_GenerateTable.TabIndex = 0;
            this.btn_GenerateTable.Text = "Generate tables in DB";
            this.btn_GenerateTable.UseVisualStyleBackColor = true;
            this.btn_GenerateTable.Click += new System.EventHandler(this.btn_GenerateTable_Click);
            // 
            // dgUsers
            // 
            this.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUsers.Location = new System.Drawing.Point(32, 21);
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.RowHeadersWidth = 62;
            this.dgUsers.RowTemplate.Height = 28;
            this.dgUsers.Size = new System.Drawing.Size(1158, 500);
            this.dgUsers.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 635);
            this.Controls.Add(this.dgUsers);
            this.Controls.Add(this.btn_GenerateTable);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_GenerateTable;
        private System.Windows.Forms.DataGridView dgUsers;
    }
}

