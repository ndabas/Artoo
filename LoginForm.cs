using System;
using System.Windows.Forms;
using System.Drawing;
    
[Serializable]
public class LoginForm : Form
{
        
    private Label LoginLabel;
    private Label PasswordLabel;
    public TextBox LoginTextBox;
    public TextBox PasswordTextBox;
    private Button LoginButton;
    private Button CloseButton;
    
    public LoginForm()
    {
        InitializeComponent();
    }
        
    protected override void Dispose( bool disposing )
    {
        base.Dispose( disposing );
    }
        
    public void InitializeComponent()
    {
        this.LoginLabel = new Label();
        this.PasswordLabel = new Label();
        this.LoginTextBox = new TextBox();
        this.PasswordTextBox = new TextBox();
        this.LoginButton = new Button();
        this.CloseButton = new Button();
        this.SuspendLayout();
        
        this.Text = "Login";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Size = new Size(236, 156);
        this.MinimumSize = new Size(236, 156);
        this.Font = new Font("Tahoma", 8);
        
        this.LoginLabel.Text = "Login:";
        this.LoginLabel.Size = new Size(62, 24);
        this.LoginLabel.Location = new Point(12, 12);
        
        this.PasswordLabel.Text = "Password:";
        this.PasswordLabel.Size = new Size(62, 24);
        this.PasswordLabel.Location = new Point(12, 48);
        
        this.LoginTextBox.Size = new Size(130, 24);
        this.LoginTextBox.Location = new Point(86, 12);
        
        this.PasswordTextBox.Size = new Size(130, 24);
        this.PasswordTextBox.Location = new Point(86, 48);
        this.PasswordTextBox.PasswordChar = '*';
        
        this.LoginButton.Text = "Login";
        this.LoginButton.DialogResult = DialogResult.OK;
        this.LoginButton.Size = new Size(96, 24);
        this.LoginButton.Location = new Point(12, 84);
        
        this.CloseButton.Text = "Cancel";
        this.CloseButton.DialogResult = DialogResult.Cancel;
        this.CloseButton.Size = new Size(96, 24);
        this.CloseButton.Location = new Point(120, 84);
        
        this.AcceptButton = LoginButton;
        this.CancelButton = CloseButton;
        
        this.Controls.AddRange(new System.Windows.Forms.Control[]
		{
		    this.LoginLabel,
            this.PasswordLabel,
            this.LoginTextBox,
            this.PasswordTextBox,
            this.LoginButton,
            this.CloseButton
		});
		    
        this.ResumeLayout(false);
    }
        
        
}  