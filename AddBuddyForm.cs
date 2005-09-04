using System;
using System.Windows.Forms;
using System.Drawing;
    
[Serializable]
public class AddBuddyForm : Form
{
        
    private Label AddBuddyLabel;
    public TextBox AddBuddyTextBox;
    private Button AddBuddyButton;
    private Button CloseButton;
    
    public AddBuddyForm()
    {
        InitializeComponent();
    }
        
    protected override void Dispose( bool disposing )
    {
        base.Dispose( disposing );
    }
        
    public void InitializeComponent()
    {
        this.AddBuddyLabel = new Label();
        this.AddBuddyTextBox = new TextBox();
        this.AddBuddyButton = new Button();
        this.CloseButton = new Button();
        this.SuspendLayout();
        
        this.Text = "Add Buddy";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Size = new Size(236, 156);
        this.MinimumSize = new Size(236, 156);
        this.Font = new Font("Tahoma", 8);
        
        this.AddBuddyLabel.Text = "Buddy ID:";
        this.AddBuddyLabel.Size = new Size(62, 24);
        this.AddBuddyLabel.Location = new Point(12, 12);
        
        this.AddBuddyTextBox.Size = new Size(130, 24);
        this.AddBuddyTextBox.Location = new Point(86, 12);
        
        this.AddBuddyButton.Text = "Add";
        this.AddBuddyButton.DialogResult = DialogResult.OK;
        this.AddBuddyButton.Size = new Size(96, 24);
        this.AddBuddyButton.Location = new Point(12, 84);
        
        this.CloseButton.Text = "Cancel";
        this.CloseButton.DialogResult = DialogResult.Cancel;
        this.CloseButton.Size = new Size(96, 24);
        this.CloseButton.Location = new Point(120, 84);
        
        this.AcceptButton = AddBuddyButton;
        this.CancelButton = CloseButton;
        
        this.Controls.AddRange(new System.Windows.Forms.Control[]
		{
		    this.AddBuddyLabel,
            this.AddBuddyTextBox,
            this.AddBuddyButton,
            this.CloseButton
		});
		    
        this.ResumeLayout(false);
    }
        
        
}  