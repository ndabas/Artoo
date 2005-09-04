using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
    
[Serializable]
public class UsersAdminForm : Form
{
        
    private ListView UsersList;
    private Button AddButton;
    private Button EditButton;
    private Button DelButton;
    
    private ArtooBase ArtooDataSet;
        
    public UsersAdminForm()
    {
        InitializeComponent();
        ArtooDataSet = new ArtooBase();
        ArtooDataSet.ReadXml("ArtooBase.xml");
        IEnumerator users = ArtooDataSet.User.GetEnumerator();
        while(users.MoveNext())
        {
            UsersList.Items.Add((users.Current as ArtooBase.UserRow).id);
        }
    }
        
    protected override void Dispose( bool disposing )
    {
        base.Dispose( disposing );
    }
        
    public void InitializeComponent()
    {
        this.UsersList = new ListView();
        this.AddButton = new Button();
        this.EditButton = new Button();
        this.DelButton = new Button();
        this.SuspendLayout();
        
        this.UsersList.View = View.List;
        this.UsersList.Location = new Point(12, 12);
        this.UsersList.Size = new Size(260, 254);
        
        this.AddButton.Size = new Size(96, 24);
        this.AddButton.Location = new Point(282, 12);
        this.AddButton.Text = "Add User";
        
        this.EditButton.Size = new Size(96, 24);
        this.EditButton.Location = new Point(282, 48);
        this.EditButton.Text = "Edit User";
        
        this.DelButton.Size = new Size(96, 24);
        this.DelButton.Location = new Point(282, 84);
        this.DelButton.Text = "Remove User";
        
        this.Text = "Artoo Instant Messenger Users Admin";
        this.Size = new Size(400, 312);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MinimumSize = new Size(400, 312);
        this.Font = new Font("Tahoma", 8);
            
        this.Controls.AddRange(new System.Windows.Forms.Control[]
		{
		    this.UsersList,
		    this.AddButton,
		    this.EditButton,
		    this.DelButton
		});
		
		this.ResumeLayout(false);
    }
        
    [STAThread]
    static void Main(string[] args)
    {
        Application.Run(new UsersAdminForm());
    }
        
    
        
        
}