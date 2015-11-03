namespace Gymverksamhet_G3
{
    partial class Login
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
            this.textBox_Login_User = new System.Windows.Forms.TextBox();
            this.textBox_Login_Pass = new System.Windows.Forms.TextBox();
            this.button_Login = new System.Windows.Forms.Button();
            this.label_Login_User = new System.Windows.Forms.Label();
            this.label_Login_Pass = new System.Windows.Forms.Label();
            this.button_Login_Avsluta = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_Login_User
            // 
            this.textBox_Login_User.Location = new System.Drawing.Point(426, 111);
            this.textBox_Login_User.Name = "textBox_Login_User";
            this.textBox_Login_User.Size = new System.Drawing.Size(100, 20);
            this.textBox_Login_User.TabIndex = 0;
            // 
            // textBox_Login_Pass
            // 
            this.textBox_Login_Pass.Location = new System.Drawing.Point(426, 177);
            this.textBox_Login_Pass.Name = "textBox_Login_Pass";
            this.textBox_Login_Pass.Size = new System.Drawing.Size(100, 20);
            this.textBox_Login_Pass.TabIndex = 1;
            // 
            // button_Login
            // 
            this.button_Login.BackColor = System.Drawing.Color.LightGreen;
            this.button_Login.Location = new System.Drawing.Point(426, 216);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(75, 23);
            this.button_Login.TabIndex = 2;
            this.button_Login.Text = "Logga in";
            this.button_Login.UseVisualStyleBackColor = false;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // label_Login_User
            // 
            this.label_Login_User.AutoSize = true;
            this.label_Login_User.Location = new System.Drawing.Point(426, 92);
            this.label_Login_User.Name = "label_Login_User";
            this.label_Login_User.Size = new System.Drawing.Size(82, 13);
            this.label_Login_User.TabIndex = 3;
            this.label_Login_User.Text = "Användarnamn:";
            // 
            // label_Login_Pass
            // 
            this.label_Login_Pass.AutoSize = true;
            this.label_Login_Pass.Location = new System.Drawing.Point(426, 161);
            this.label_Login_Pass.Name = "label_Login_Pass";
            this.label_Login_Pass.Size = new System.Drawing.Size(54, 13);
            this.label_Login_Pass.TabIndex = 4;
            this.label_Login_Pass.Text = "Lösenord:";
            // 
            // button_Login_Avsluta
            // 
            this.button_Login_Avsluta.BackColor = System.Drawing.Color.LightGreen;
            this.button_Login_Avsluta.Location = new System.Drawing.Point(426, 255);
            this.button_Login_Avsluta.Name = "button_Login_Avsluta";
            this.button_Login_Avsluta.Size = new System.Drawing.Size(75, 23);
            this.button_Login_Avsluta.TabIndex = 5;
            this.button_Login_Avsluta.Text = "Avsluta";
            this.button_Login_Avsluta.UseVisualStyleBackColor = false;
            this.button_Login_Avsluta.Click += new System.EventHandler(this.button_Login_Avsluta_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(950, 374);
            this.Controls.Add(this.button_Login_Avsluta);
            this.Controls.Add(this.label_Login_Pass);
            this.Controls.Add(this.label_Login_User);
            this.Controls.Add(this.button_Login);
            this.Controls.Add(this.textBox_Login_Pass);
            this.Controls.Add(this.textBox_Login_User);
            this.Name = "Login";
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Login_User;
        private System.Windows.Forms.TextBox textBox_Login_Pass;
        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.Label label_Login_User;
        private System.Windows.Forms.Label label_Login_Pass;
        private System.Windows.Forms.Button button_Login_Avsluta;
    }
}