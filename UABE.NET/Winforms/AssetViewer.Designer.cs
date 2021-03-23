namespace UABE.NET.Winforms
{
    partial class AssetViewer
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetViewer));
            this.assetsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameField = new System.Windows.Forms.TextBox();
            this.pathField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fileField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.typeField = new System.Windows.Forms.TextBox();
            this.viewDataButton = new System.Windows.Forms.Button();
            this.exportRawButton = new System.Windows.Forms.Button();
            this.exportDumpButton = new System.Windows.Forms.Button();
            this.importRawButton = new System.Windows.Forms.Button();
            this.importDumpButton = new System.Windows.Forms.Button();
            this.pluginsButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modMakerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createStandaloneexeInstallerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInstallerPackageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryContentSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monobehaviourSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continueSearchF3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToAssetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dependenciesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assetList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetsLabel
            // 
            this.assetsLabel.AutoSize = true;
            this.assetsLabel.Location = new System.Drawing.Point(12, 22);
            this.assetsLabel.Name = "assetsLabel";
            this.assetsLabel.Size = new System.Drawing.Size(41, 12);
            this.assetsLabel.TabIndex = 1;
            this.assetsLabel.Text = "Assets";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(542, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // nameField
            // 
            this.nameField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameField.Location = new System.Drawing.Point(545, 52);
            this.nameField.Name = "nameField";
            this.nameField.ReadOnly = true;
            this.nameField.Size = new System.Drawing.Size(181, 21);
            this.nameField.TabIndex = 3;
            // 
            // pathField
            // 
            this.pathField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pathField.Location = new System.Drawing.Point(545, 88);
            this.pathField.Name = "pathField";
            this.pathField.ReadOnly = true;
            this.pathField.Size = new System.Drawing.Size(181, 21);
            this.pathField.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(542, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Path ID";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(542, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "File ID";
            // 
            // fileField
            // 
            this.fileField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileField.Location = new System.Drawing.Point(545, 124);
            this.fileField.Name = "fileField";
            this.fileField.ReadOnly = true;
            this.fileField.Size = new System.Drawing.Size(181, 21);
            this.fileField.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(542, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "Type";
            // 
            // typeField
            // 
            this.typeField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeField.Location = new System.Drawing.Point(545, 160);
            this.typeField.Name = "typeField";
            this.typeField.ReadOnly = true;
            this.typeField.Size = new System.Drawing.Size(181, 21);
            this.typeField.TabIndex = 9;
            // 
            // viewDataButton
            // 
            this.viewDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewDataButton.Location = new System.Drawing.Point(545, 184);
            this.viewDataButton.Name = "viewDataButton";
            this.viewDataButton.Size = new System.Drawing.Size(181, 21);
            this.viewDataButton.TabIndex = 10;
            this.viewDataButton.Text = "View Data";
            this.viewDataButton.UseVisualStyleBackColor = true;
            this.viewDataButton.Click += new System.EventHandler(this.viewDataButton_Click);
            // 
            // exportRawButton
            // 
            this.exportRawButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportRawButton.Location = new System.Drawing.Point(545, 210);
            this.exportRawButton.Name = "exportRawButton";
            this.exportRawButton.Size = new System.Drawing.Size(181, 21);
            this.exportRawButton.TabIndex = 11;
            this.exportRawButton.Text = "Export Raw";
            this.exportRawButton.UseVisualStyleBackColor = true;
            this.exportRawButton.Click += new System.EventHandler(this.exportRawButton_Click);
            // 
            // exportDumpButton
            // 
            this.exportDumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportDumpButton.Location = new System.Drawing.Point(545, 237);
            this.exportDumpButton.Name = "exportDumpButton";
            this.exportDumpButton.Size = new System.Drawing.Size(181, 21);
            this.exportDumpButton.TabIndex = 12;
            this.exportDumpButton.Text = "Export Dump";
            this.exportDumpButton.UseVisualStyleBackColor = true;
            this.exportDumpButton.Click += new System.EventHandler(this.exportDumpButton_Click);
            // 
            // importRawButton
            // 
            this.importRawButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importRawButton.Location = new System.Drawing.Point(545, 264);
            this.importRawButton.Name = "importRawButton";
            this.importRawButton.Size = new System.Drawing.Size(181, 21);
            this.importRawButton.TabIndex = 13;
            this.importRawButton.Text = "Import Raw";
            this.importRawButton.UseVisualStyleBackColor = true;
            this.importRawButton.Click += new System.EventHandler(this.importRawButton_Click);
            // 
            // importDumpButton
            // 
            this.importDumpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importDumpButton.Location = new System.Drawing.Point(545, 291);
            this.importDumpButton.Name = "importDumpButton";
            this.importDumpButton.Size = new System.Drawing.Size(181, 21);
            this.importDumpButton.TabIndex = 14;
            this.importDumpButton.Text = "Import Dump";
            this.importDumpButton.UseVisualStyleBackColor = true;
            this.importDumpButton.Click += new System.EventHandler(this.importDumpButton_Click);
            // 
            // pluginsButton
            // 
            this.pluginsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginsButton.Location = new System.Drawing.Point(545, 318);
            this.pluginsButton.Name = "pluginsButton";
            this.pluginsButton.Size = new System.Drawing.Size(181, 21);
            this.pluginsButton.TabIndex = 15;
            this.pluginsButton.Text = "Plugins";
            this.pluginsButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Location = new System.Drawing.Point(545, 344);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(181, 21);
            this.removeButton.TabIndex = 16;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(545, 371);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(181, 21);
            this.addButton.TabIndex = 17;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(738, 25);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.modMakerToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            // 
            // modMakerToolStripMenuItem
            // 
            this.modMakerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createStandaloneexeInstallerToolStripMenuItem,
            this.createInstallerPackageFileToolStripMenuItem});
            this.modMakerToolStripMenuItem.Enabled = false;
            this.modMakerToolStripMenuItem.Name = "modMakerToolStripMenuItem";
            this.modMakerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.modMakerToolStripMenuItem.Text = "Mod Maker";
            // 
            // createStandaloneexeInstallerToolStripMenuItem
            // 
            this.createStandaloneexeInstallerToolStripMenuItem.Name = "createStandaloneexeInstallerToolStripMenuItem";
            this.createStandaloneexeInstallerToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.createStandaloneexeInstallerToolStripMenuItem.Text = "Create standalone .exe installer";
            // 
            // createInstallerPackageFileToolStripMenuItem
            // 
            this.createInstallerPackageFileToolStripMenuItem.Name = "createInstallerPackageFileToolStripMenuItem";
            this.createInstallerPackageFileToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.createInstallerPackageFileToolStripMenuItem.Text = "Create installer package file";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchByNameToolStripMenuItem,
            this.continueSearchF3ToolStripMenuItem,
            this.goToAssetToolStripMenuItem,
            this.dependenciesToolStripMenuItem,
            this.windowToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // searchByNameToolStripMenuItem
            // 
            this.searchByNameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byNameToolStripMenuItem,
            this.binaryContentSearchToolStripMenuItem,
            this.monobehaviourSearchToolStripMenuItem,
            this.transformSearchToolStripMenuItem});
            this.searchByNameToolStripMenuItem.Name = "searchByNameToolStripMenuItem";
            this.searchByNameToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.searchByNameToolStripMenuItem.Text = "Search...";
            // 
            // byNameToolStripMenuItem
            // 
            this.byNameToolStripMenuItem.Name = "byNameToolStripMenuItem";
            this.byNameToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.byNameToolStripMenuItem.Text = "Asset by name";
            this.byNameToolStripMenuItem.Click += new System.EventHandler(this.byNameToolStripMenuItem_Click);
            // 
            // binaryContentSearchToolStripMenuItem
            // 
            this.binaryContentSearchToolStripMenuItem.Name = "binaryContentSearchToolStripMenuItem";
            this.binaryContentSearchToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.binaryContentSearchToolStripMenuItem.Text = "Content Search";
            this.binaryContentSearchToolStripMenuItem.Click += new System.EventHandler(this.binaryContentSearchToolStripMenuItem_Click);
            // 
            // monobehaviourSearchToolStripMenuItem
            // 
            this.monobehaviourSearchToolStripMenuItem.Name = "monobehaviourSearchToolStripMenuItem";
            this.monobehaviourSearchToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.monobehaviourSearchToolStripMenuItem.Text = "Monobehaviour Search";
            this.monobehaviourSearchToolStripMenuItem.Click += new System.EventHandler(this.monobehaviourSearchToolStripMenuItem_Click);
            // 
            // transformSearchToolStripMenuItem
            // 
            this.transformSearchToolStripMenuItem.Name = "transformSearchToolStripMenuItem";
            this.transformSearchToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.transformSearchToolStripMenuItem.Text = "Transform Search";
            this.transformSearchToolStripMenuItem.Click += new System.EventHandler(this.transformSearchToolStripMenuItem_Click);
            // 
            // continueSearchF3ToolStripMenuItem
            // 
            this.continueSearchF3ToolStripMenuItem.Name = "continueSearchF3ToolStripMenuItem";
            this.continueSearchF3ToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.continueSearchF3ToolStripMenuItem.Text = "Continue search (F3)";
            // 
            // goToAssetToolStripMenuItem
            // 
            this.goToAssetToolStripMenuItem.Name = "goToAssetToolStripMenuItem";
            this.goToAssetToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.goToAssetToolStripMenuItem.Text = "Go to asset";
            // 
            // dependenciesToolStripMenuItem
            // 
            this.dependenciesToolStripMenuItem.Name = "dependenciesToolStripMenuItem";
            this.dependenciesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.dependenciesToolStripMenuItem.Text = "Dependencies";
            this.dependenciesToolStripMenuItem.Click += new System.EventHandler(this.dependenciesToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assetPreviewToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // assetPreviewToolStripMenuItem
            // 
            this.assetPreviewToolStripMenuItem.Name = "assetPreviewToolStripMenuItem";
            this.assetPreviewToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.assetPreviewToolStripMenuItem.Text = "Asset Preview";
            // 
            // assetList
            // 
            this.assetList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.assetList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.assetList.Location = new System.Drawing.Point(12, 37);
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(527, 355);
            this.assetList.TabIndex = 19;
            this.assetList.UseCompatibleStateImageBehavior = false;
            this.assetList.View = System.Windows.Forms.View.Details;
            this.assetList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.assetList_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "File ID";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Path ID";
            this.columnHeader4.Width = 85;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Size";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Modified";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // AssetViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 402);
            this.Controls.Add(this.assetList);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.pluginsButton);
            this.Controls.Add(this.importDumpButton);
            this.Controls.Add(this.importRawButton);
            this.Controls.Add(this.exportDumpButton);
            this.Controls.Add(this.exportRawButton);
            this.Controls.Add(this.viewDataButton);
            this.Controls.Add(this.typeField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.fileField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pathField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.assetsLabel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AssetViewer";
            this.Text = "Assets Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AssetViewer_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label assetsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameField;
        private System.Windows.Forms.TextBox pathField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox typeField;
        private System.Windows.Forms.Button viewDataButton;
        private System.Windows.Forms.Button exportRawButton;
        private System.Windows.Forms.Button exportDumpButton;
        private System.Windows.Forms.Button importRawButton;
        private System.Windows.Forms.Button importDumpButton;
        private System.Windows.Forms.Button pluginsButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modMakerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createStandaloneexeInstallerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createInstallerPackageFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchByNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continueSearchF3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToAssetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dependenciesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assetPreviewToolStripMenuItem;
        private System.Windows.Forms.ListView assetList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStripMenuItem byNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryContentSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monobehaviourSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    }
}