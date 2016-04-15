namespace NxBRE.InferenceEngine.Console {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;

	using System.Drawing;
	using System.Windows.Forms;
	
	using System.Text.RegularExpressions;

	
	using NxBRE.InferenceEngine.IO;
	using NxBRE.Util;

	/// <summary>
	/// 
	/// Basic Console for NxBRE Inference Engine
	/// 
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public class MainForm : System.Windows.Forms.Form {
		private System.Windows.Forms.MenuItem menuItemRetract;
		private System.Windows.Forms.MenuItem menuItemConsoleClear;
		private System.Windows.Forms.MenuItem menuItemLoadFacts;
		private System.Windows.Forms.MenuItem menuItemStatus;
		private System.Windows.Forms.MenuItem menuItemSystem;
		private System.Windows.Forms.MenuItem menuItemWMIsolated;
		private System.Windows.Forms.MenuItem menuItemVerbosityHigh;
		private System.Windows.Forms.MenuItem menuItemRunQuery;
        private System.Windows.Forms.MenuItem menuItemRunCustomQuery;
		private System.Windows.Forms.MenuItem menuItemVerbosity;
		private System.Windows.Forms.MenuItem menuItemShowModifications;
		private System.Windows.Forms.MenuItem menuItemFactsTools;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem menuItemAssert;
		private System.Windows.Forms.MenuItem menuItemWMCommit;
		private System.Windows.Forms.MenuItem menuItemShowDeductions;
		private System.Windows.Forms.MenuItem menuItemProcess;
		private System.Windows.Forms.MenuItem menuItemConsole;
		private System.Windows.Forms.MenuItem menuItemVerbosityMedium;
		private System.Windows.Forms.MenuItem menuItemEngine;
		private System.Windows.Forms.MenuItem menuItemLoad;
		private System.Windows.Forms.MenuItem menuItemSaveRuleBase;
		private System.Windows.Forms.MenuItem menuItemSaveFacts;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.MenuItem menuItemShowDeletions;
		private System.Windows.Forms.MenuItem menuItemWM;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.MenuItem menuItemVerbosityLow;
		private System.Windows.Forms.MenuItem menuItemWMIsolatedEmpty;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemListAssemblies;
		private System.Windows.Forms.MenuItem menuItemLoadRuleBase;
		private System.Windows.Forms.MenuItem menuItemWMGlobal;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemFacts;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemVerbositySilent;
		private System.Windows.Forms.TextBox textBoxConsole;
		
		private enum ActionType {Exit, LoadRuleBase, ClearConsole, DumpStatus, VerbositySilent,
														 VerbosityLow, VerbosityMedium, VerbosityHigh, LoadFacts, Process,
														 RunQuery, RunCustomQuery, DumpFacts, WMGlobal, WMIsolated, WMIsolatedEmpty, WMCommit,
														 ListAssemblies, SaveRuleBase, SaveFacts, AssertFact, RetractFact};
		
		private IEGUIFacade iegf;
		private ArrayList consoleLines;
		private ArrayList loadedAssemblyNames;
		private SourceLevels verbosity;
		
		private Regex specialCharMatcher = new Regex("[\x00-\x1f]");
			
		public MainForm()
		{
			loadedAssemblyNames = Utils.LoadAssemblies();
			InitializeComponent();

			iegf = new IEGUIFacade();
			verbosity = SourceLevels.Warning;
			TraceListener tl = new IEGUIConsoleTraceListener(new ConsoleWriter(ConsoleOut));
			Logger.InferenceEngineSource.Listeners.Add(tl);
			Logger.UtilSource.Listeners.Add(tl);
			consoleLines = new ArrayList();
			RefreshMenus();

			Status(Assembly.GetExecutingAssembly().GetName().Name +
			       " v" +
			       Assembly.GetExecutingAssembly().GetName().Version +
			       " ready");
		}

		
		/// <summary>
		/// This method is charge of processing all events.
		/// </summary>
		/// <param name="type">The ActionType to process.</param>
		private void Action(ActionType type) {
			switch(type) {
				case ActionType.Exit:
					if (DialogResult.Yes == MessageBox.Show(this,
																	                "Do you really want to exit?",
																	                "Leaving NxBRE Console",
																	                MessageBoxButtons.YesNo,
																	                MessageBoxIcon.Warning,
					                                        MessageBoxDefaultButton.Button2)) {
						Utils.SaveUserPreferences(iegf.UserPrefs);
						Application.Exit();
					}
					
					Status("Exit cancelled, let's infer some more!");
					break;
					
				case ActionType.LoadRuleBase:
					LoadRuleBase(false);
					ConsoleOut(String.Empty);
					break;
				
				case ActionType.LoadFacts:
					LoadRuleBase(true);
					ConsoleOut(String.Empty);
					break;
					
				case ActionType.SaveRuleBase:
					SaveRuleBase(false);
					ConsoleOut(String.Empty);
					break;
				
				case ActionType.SaveFacts:
					SaveRuleBase(true);
					ConsoleOut(String.Empty);
					break;
					
				case ActionType.ClearConsole:
					Status("Console cleared");
					ResetConsole();
					break;
					
				case ActionType.DumpStatus:
					ConsoleOut(iegf.Status);
					ConsoleOut(String.Empty);
					break;
				
				case ActionType.DumpFacts:
					if ((iegf.FactsCount > 100) &&
					    (DialogResult.No == MessageBox.Show(this,
																	                "There are " +
																	                iegf.FactsCount +
																	                " facts in the working memory. Do you really want to list them?",
																	                "Massive facts listing",
																	                MessageBoxButtons.YesNo,
																	                MessageBoxIcon.Warning,
																	                MessageBoxDefaultButton.Button2))) return;
					
					iegf.DumpFacts(new ConsoleWriter(ConsoleOut));
					ConsoleOut(String.Empty);
					break;
					
				case ActionType.VerbositySilent:
					verbosity = SourceLevels.Critical;
					RefreshMenus();
					break;
				
				case ActionType.VerbosityLow:
					verbosity = SourceLevels.Warning;
					RefreshMenus();
					break;
				
				case ActionType.VerbosityMedium:
					verbosity = SourceLevels.Information;
					RefreshMenus();
					break;
				
				case ActionType.VerbosityHigh:
					verbosity = SourceLevels.Verbose;
					RefreshMenus();
					break;
					
				case ActionType.Process:
					Status("Processing started");
					iegf.Process(new NewFactEvent(HandleNewFactEvent),
					             new NewFactEvent(HandleDelFactEvent),
					             new NewFactEvent(HandleModFactEvent));
					ConsoleOut(String.Empty);
					RefreshMenus();
					Status("Processing done");
					break;

				case ActionType.AssertFact:
				case ActionType.RetractFact:
					try {
						string operation = (type == ActionType.AssertFact)?"Assertion":"Retraction";
						string fact = iegf.PromptFactOperation(this, operation);
						
						if (fact != String.Empty) {
							ConsoleOut(((type == ActionType.AssertFact)?"+ ":"- ") + fact);
							RefreshMenus();
							ConsoleOut(String.Empty);
							Status(operation + " done");
						}
					} catch(Exception e) {
						ConsoleOut(e.Message);
						ConsoleOut(String.Empty);
					}
					break;
				
				case ActionType.RunQuery:
					QueryListForm qlf = new QueryListForm(iegf.QueryLabels);
					if (qlf.ShowDialog(this) == DialogResult.OK ) {
						ConsoleOut(iegf.RunQuery(qlf.SelectedQueryIndex));
						ConsoleOut(String.Empty);
						Status("Query executed");
					}
					else Status(String.Empty);
					break;

                case ActionType.RunCustomQuery:
                    try
                    {
                        string queryResult = iegf.PromptQueryOperation(this);

                        if (queryResult != String.Empty)
                        {
                            ConsoleOut(queryResult);
                            ConsoleOut(String.Empty);
                            Status("Custom Query executed");
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleOut(e.Message);
                        ConsoleOut(String.Empty);
                    }
                    break;
				
				case ActionType.WMGlobal:
					iegf.WMType = WorkingMemoryTypes.Global;
					RefreshMenus();
					Action(ActionType.DumpStatus);
					break;
				
				case ActionType.WMIsolated:
					iegf.WMType = WorkingMemoryTypes.Isolated;
					RefreshMenus();
					Action(ActionType.DumpStatus);
					break;
				
				case ActionType.WMIsolatedEmpty:
					iegf.WMType = WorkingMemoryTypes.IsolatedEmpty;
					RefreshMenus();
					Action(ActionType.DumpStatus);
					break;
				
				case ActionType.WMCommit:
					iegf.WMCommit();
					RefreshMenus();
					Action(ActionType.DumpStatus);
					break;
					
				case ActionType.ListAssemblies:
					ConsoleOut("Assemblies loaded in the current domain:");
					foreach(AssemblyName assemblyName in loadedAssemblyNames)
						ConsoleOut(" " + assemblyName.Name + "(" + assemblyName.Version + ")");
					ConsoleOut(String.Empty);
					Status(String.Empty);
					break;
			}
		}
		
		private void HandleNewFactEvent(NewFactEventArgs nfea) 
	  {
			if (menuItemShowDeductions.Checked) ConsoleOut("+ " + nfea.Fact);
	  }

		private void HandleDelFactEvent(NewFactEventArgs nfea) 
	  {
			if (menuItemShowDeletions.Checked) ConsoleOut("- " + nfea.Fact);
	  }

		private void HandleModFactEvent(NewFactEventArgs nfea) 
	  {
			if (menuItemShowModifications.Checked) {
				ConsoleOut("< " + nfea.Fact);
				ConsoleOut("> " + nfea.OtherFact);
			}
	  }
		
		private void LoadRuleBase(bool factsOnly) {
			openFileDialog.Title = factsOnly?"Load Facts":"Load RuleBase";
			
			if (DialogResult.OK == openFileDialog.ShowDialog(this)) {
				if (!factsOnly) ResetConsole();
				
				try {
					iegf.LoadRuleBase(this, openFileDialog.FileName, factsOnly);
					Status("Loaded " +
					       (factsOnly?"Facts":"RuleBase") +
					       " " +
					       openFileDialog.FileName);
					ConsoleOut(String.Empty);
					ConsoleOut(iegf.Status);
				}
				catch(Exception e) {
					Status("Error in RuleBase " + openFileDialog.FileName);
					MessageBox.Show(this,
								          "Error in RuleBase " + openFileDialog.FileName + "\n\n" + e.Message,
								          "Error in RuleBase",
								          MessageBoxButtons.OK,
								          MessageBoxIcon.Error);
				}
			}
			RefreshMenus();
		}
	  
		private void SaveRuleBase(bool factsOnly) {
			saveFileDialog.Title = factsOnly?"Save Facts":"Save RuleBase";
			
			if (DialogResult.OK == saveFileDialog.ShowDialog(this)) {
				try {
					iegf.SaveRuleBase(this, saveFileDialog.FileName, factsOnly);
					Status("Saved " +
					       (factsOnly?"Facts":"RuleBase") +
					       " " +
					       saveFileDialog.FileName);
					ConsoleOut(String.Empty);
					ConsoleOut("Saved " + (factsOnly?"Facts":"RuleBase"));
				}
				catch(Exception e) {
					Status("Error in RuleBase " + saveFileDialog.FileName);
					MessageBox.Show(this,
								          "Error in RuleBase " + saveFileDialog.FileName + "\n\n" + e.Message,
								          "Error in RuleBase",
								          MessageBoxButtons.OK,
								          MessageBoxIcon.Error);
				}
			}
			RefreshMenus();
		}
		
		private void RefreshMenus() {
			RefreshTraces();
			
			menuItemLoadFacts.Enabled = iegf.HasImplications;
			menuItemSaveRuleBase.Enabled = iegf.Valid;
			menuItemSaveFacts.Enabled = iegf.HasFacts;
			menuItemSave.Enabled = menuItemSaveRuleBase.Enabled | menuItemSaveFacts.Enabled;
			
			menuItemProcess.Enabled = iegf.HasImplications;
			menuItemAssert.Enabled = iegf.HasImplications;
			menuItemRunQuery.Enabled = iegf.HasQueries;
            menuItemRunCustomQuery.Enabled = iegf.HasFacts;
			menuItemStatus.Enabled = iegf.Valid;
			menuItemFacts.Enabled = iegf.HasFacts;
			menuItemRetract.Enabled = iegf.HasFacts;
			menuItemEngine.Enabled = menuItemProcess.Enabled |
															 menuItemRunQuery.Enabled |
                                                             menuItemRunCustomQuery.Enabled |
															 menuItemStatus.Enabled |
															 menuItemFacts.Enabled;
			
			menuItemWMCommit.Enabled = (iegf.Valid) && (iegf.WMType != WorkingMemoryTypes.Global);
			
			menuItemVerbositySilent.Checked = (verbosity == SourceLevels.Critical);
			menuItemVerbosityLow.Checked = (verbosity == SourceLevels.Warning);
			menuItemVerbosityMedium.Checked = (verbosity == SourceLevels.Information);
			menuItemVerbosityHigh.Checked = (verbosity == SourceLevels.Verbose);
		}
		
		private void RefreshTraces() {
			Logger.InferenceEngineSource.Switch.Level = verbosity;
			Logger.UtilSource.Switch.Level = verbosity;
			Logger.RefreshBooleanSwitches();
		}
		
		private void Status(string msg) {
			statusBar.Text = msg;
		}
		
		private void ConsoleOut(string lines) {
			foreach(string line in lines.Split('|')) consoleLines.Add(specialCharMatcher.Replace(line, " "));
			textBoxConsole.Lines = (string[]) consoleLines.ToArray(typeof(string));
			textBoxConsole.SelectionStart = textBoxConsole.Text.Length;
			textBoxConsole.ScrollToCaret();
		}
		
		private void ResetConsole() {
			consoleLines = new ArrayList();
			textBoxConsole.Clear();
		}
		
		public bool AskYesNoQuestion(string title, string question) {
			return (DialogResult.Yes == MessageBox.Show(this, question, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question));
		}
		
		public string PromptForString(string title, string prompt, string defaultValue) {
 			return ajma.Utils.InputBox.Show(this, prompt, title, defaultValue, 0, 0);			
		}
		
		public string[] PromptForVisioPageNameSelection(IList<string> pageNames) {
			PageListForm plf = new PageListForm(pageNames);
			if (plf.ShowDialog(this) == DialogResult.OK)
				return plf.SelectedPages;
			else
				return null;
		}
		
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new MainForm());
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.textBoxConsole = new System.Windows.Forms.TextBox();
			this.menuItemVerbositySilent = new System.Windows.Forms.MenuItem();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemLoad = new System.Windows.Forms.MenuItem();
			this.menuItemLoadRuleBase = new System.Windows.Forms.MenuItem();
			this.menuItemLoadFacts = new System.Windows.Forms.MenuItem();
			this.menuItemSave = new System.Windows.Forms.MenuItem();
			this.menuItemSaveRuleBase = new System.Windows.Forms.MenuItem();
			this.menuItemSaveFacts = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemEngine = new System.Windows.Forms.MenuItem();
			this.menuItemWM = new System.Windows.Forms.MenuItem();
			this.menuItemWMGlobal = new System.Windows.Forms.MenuItem();
			this.menuItemWMIsolated = new System.Windows.Forms.MenuItem();
			this.menuItemWMIsolatedEmpty = new System.Windows.Forms.MenuItem();
			this.menuItemWMCommit = new System.Windows.Forms.MenuItem();
			this.menuItemProcess = new System.Windows.Forms.MenuItem();
			this.menuItemRunQuery = new System.Windows.Forms.MenuItem();
            this.menuItemRunCustomQuery = new System.Windows.Forms.MenuItem();
			this.menuItemStatus = new System.Windows.Forms.MenuItem();
			this.menuItemFactsTools = new System.Windows.Forms.MenuItem();
			this.menuItemAssert = new System.Windows.Forms.MenuItem();
			this.menuItemRetract = new System.Windows.Forms.MenuItem();
			this.menuItemFacts = new System.Windows.Forms.MenuItem();
			this.menuItemConsole = new System.Windows.Forms.MenuItem();
			this.menuItemConsoleClear = new System.Windows.Forms.MenuItem();
			this.menuItemShowDeductions = new System.Windows.Forms.MenuItem();
			this.menuItemShowDeletions = new System.Windows.Forms.MenuItem();
			this.menuItemShowModifications = new System.Windows.Forms.MenuItem();
			this.menuItemVerbosity = new System.Windows.Forms.MenuItem();
			this.menuItemVerbosityLow = new System.Windows.Forms.MenuItem();
			this.menuItemVerbosityMedium = new System.Windows.Forms.MenuItem();
			this.menuItemVerbosityHigh = new System.Windows.Forms.MenuItem();
			this.menuItemSystem = new System.Windows.Forms.MenuItem();
			this.menuItemListAssemblies = new System.Windows.Forms.MenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// textBoxConsole
			// 
			this.textBoxConsole.BackColor = System.Drawing.SystemColors.Info;
			this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxConsole.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxConsole.Location = new System.Drawing.Point(0, 0);
			this.textBoxConsole.Multiline = true;
			this.textBoxConsole.Name = "textBoxConsole";
			this.textBoxConsole.ReadOnly = true;
			this.textBoxConsole.ShortcutsEnabled = true;
			this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxConsole.Size = new System.Drawing.Size(816, 542);
			this.textBoxConsole.TabIndex = 1;
			// 
			// menuItemVerbositySilent
			// 
			this.menuItemVerbositySilent.Index = 0;
			this.menuItemVerbositySilent.RadioCheck = true;
			this.menuItemVerbositySilent.Text = "Silent";
			this.menuItemVerbositySilent.Click += new System.EventHandler(this.MenuItemVerbositySilentClick);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemFile,
									this.menuItemEngine,
									this.menuItemConsole,
									this.menuItemSystem});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemLoad,
									this.menuItemSave,
									this.menuItemExit});
			this.menuItemFile.Text = "File";
			// 
			// menuItemLoad
			// 
			this.menuItemLoad.Index = 0;
			this.menuItemLoad.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemLoadRuleBase,
									this.menuItemLoadFacts});
			this.menuItemLoad.Text = "Load";
			// 
			// menuItemLoadRuleBase
			// 
			this.menuItemLoadRuleBase.Index = 0;
			this.menuItemLoadRuleBase.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuItemLoadRuleBase.Text = "RuleBase";
			this.menuItemLoadRuleBase.Click += new System.EventHandler(this.MenuItemLoadRuleBaseClick);
			// 
			// menuItemLoadFacts
			// 
			this.menuItemLoadFacts.Enabled = false;
			this.menuItemLoadFacts.Index = 1;
			this.menuItemLoadFacts.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.menuItemLoadFacts.Text = "Facts";
			this.menuItemLoadFacts.Click += new System.EventHandler(this.MenuItemLoadFactsClick);
			// 
			// menuItemSave
			// 
			this.menuItemSave.Enabled = false;
			this.menuItemSave.Index = 1;
			this.menuItemSave.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemSaveRuleBase,
									this.menuItemSaveFacts});
			this.menuItemSave.Text = "Save";
			// 
			// menuItemSaveRuleBase
			// 
			this.menuItemSaveRuleBase.Index = 0;
			this.menuItemSaveRuleBase.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItemSaveRuleBase.Text = "RuleBase";
			this.menuItemSaveRuleBase.Click += new System.EventHandler(this.MenuItemSaveRuleBaseClick);
			// 
			// menuItemSaveFacts
			// 
			this.menuItemSaveFacts.Index = 1;
			this.menuItemSaveFacts.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.menuItemSaveFacts.Text = "Facts";
			this.menuItemSaveFacts.Click += new System.EventHandler(this.MenuItemSaveFactsClick);
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 2;
			this.menuItemExit.Shortcut = System.Windows.Forms.Shortcut.CtrlF4;
			this.menuItemExit.Text = "Exit";
			this.menuItemExit.Click += new System.EventHandler(this.MenuItemExitClick);
			// 
			// menuItemEngine
			// 
			this.menuItemEngine.Index = 1;
			this.menuItemEngine.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemWM,
									this.menuItemProcess,
									this.menuItemRunQuery,
                                    this.menuItemRunCustomQuery,
									this.menuItemStatus,
									this.menuItemFactsTools});
			this.menuItemEngine.Text = "Engine";
			// 
			// menuItemWM
			// 
			this.menuItemWM.Index = 0;
			this.menuItemWM.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemWMGlobal,
									this.menuItemWMIsolated,
									this.menuItemWMIsolatedEmpty,
									this.menuItemWMCommit});
			this.menuItemWM.Text = "Working Memory";
			// 
			// menuItemWMGlobal
			// 
			this.menuItemWMGlobal.Index = 0;
			this.menuItemWMGlobal.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
			this.menuItemWMGlobal.Text = "New Global";
			this.menuItemWMGlobal.Click += new System.EventHandler(this.MenuItemWMGlobalClick);
			// 
			// menuItemWMIsolated
			// 
			this.menuItemWMIsolated.Index = 1;
			this.menuItemWMIsolated.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.menuItemWMIsolated.Text = "New Isolated";
			this.menuItemWMIsolated.Click += new System.EventHandler(this.MenuItemWMIsolatedClick);
			// 
			// menuItemWMIsolatedEmpty
			// 
			this.menuItemWMIsolatedEmpty.Index = 2;
			this.menuItemWMIsolatedEmpty.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
			this.menuItemWMIsolatedEmpty.Text = "New Isolated Empty";
			this.menuItemWMIsolatedEmpty.Click += new System.EventHandler(this.MenuItemWMIsolatedEmptyClick);
			// 
			// menuItemWMCommit
			// 
			this.menuItemWMCommit.Index = 3;
			this.menuItemWMCommit.Text = "Commit";
			this.menuItemWMCommit.Click += new System.EventHandler(this.MenuItemWMCommitClick);
			// 
			// menuItemProcess
			// 
			this.menuItemProcess.Index = 1;
			this.menuItemProcess.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.menuItemProcess.Text = "Process";
			this.menuItemProcess.Click += new System.EventHandler(this.MenuItemProcessClick);
			// 
			// menuItemRunQuery
			// 
			this.menuItemRunQuery.Index = 2;
			this.menuItemRunQuery.Shortcut = System.Windows.Forms.Shortcut.F6;
			this.menuItemRunQuery.Text = "Run query";
			this.menuItemRunQuery.Click += new System.EventHandler(this.MenuItemRunQueryClick);
            // 
            // menuItemRunCustomQuery
            // 
            this.menuItemRunCustomQuery.Index = 3;
            this.menuItemRunCustomQuery.Shortcut = System.Windows.Forms.Shortcut.F7;
            this.menuItemRunCustomQuery.Text = "Run custom query";
            this.menuItemRunCustomQuery.Click += new System.EventHandler(this.MenuItemRunCustomQueryClick);
			// 
			// menuItemStatus
			// 
			this.menuItemStatus.Index = 4;
			this.menuItemStatus.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
			this.menuItemStatus.Text = "Dump status";
			this.menuItemStatus.Click += new System.EventHandler(this.MenuItemStatusClick);
			// 
			// menuItemFactsTools
			// 
			this.menuItemFactsTools.Index = 5;
			this.menuItemFactsTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemAssert,
									this.menuItemRetract,
									this.menuItemFacts});
			this.menuItemFactsTools.Text = "Facts";
			// 
			// menuItemAssert
			// 
			this.menuItemAssert.Index = 0;
			this.menuItemAssert.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.menuItemAssert.Text = "Assert";
			this.menuItemAssert.Click += new System.EventHandler(this.MenuItemAssertClick);
			// 
			// menuItemRetract
			// 
			this.menuItemRetract.Index = 1;
			this.menuItemRetract.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.menuItemRetract.Text = "Retract";
			this.menuItemRetract.Click += new System.EventHandler(this.MenuItemRetractClick);
			// 
			// menuItemFacts
			// 
			this.menuItemFacts.Index = 2;
			this.menuItemFacts.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.menuItemFacts.Text = "List";
			this.menuItemFacts.Click += new System.EventHandler(this.MenuItemFactsClick);
			// 
			// menuItemConsole
			// 
			this.menuItemConsole.Index = 2;
			this.menuItemConsole.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemConsoleClear,
									this.menuItemShowDeductions,
									this.menuItemShowDeletions,
									this.menuItemShowModifications,
									this.menuItemVerbosity});
			this.menuItemConsole.Text = "Console";
			// 
			// menuItemConsoleClear
			// 
			this.menuItemConsoleClear.Index = 0;
			this.menuItemConsoleClear.Shortcut = System.Windows.Forms.Shortcut.F12;
			this.menuItemConsoleClear.Text = "Clear";
			this.menuItemConsoleClear.Click += new System.EventHandler(this.MenuItemConsoleClearClick);
			// 
			// menuItemShowDeductions
			// 
			this.menuItemShowDeductions.Checked = true;
			this.menuItemShowDeductions.Index = 1;
			this.menuItemShowDeductions.Text = "Show deductions";
			this.menuItemShowDeductions.Click += new System.EventHandler(this.MenuItemShowDeductionsClick);
			// 
			// menuItemShowDeletions
			// 
			this.menuItemShowDeletions.Checked = true;
			this.menuItemShowDeletions.Index = 2;
			this.menuItemShowDeletions.Text = "Show deletions";
			this.menuItemShowDeletions.Click += new System.EventHandler(this.MenuItemShowDeletionsClick);
			// 
			// menuItemShowModifications
			// 
			this.menuItemShowModifications.Checked = true;
			this.menuItemShowModifications.Index = 3;
			this.menuItemShowModifications.Text = "Show modifcations";
			this.menuItemShowModifications.Click += new System.EventHandler(this.MenuItemShowModificationsClick);
			// 
			// menuItemVerbosity
			// 
			this.menuItemVerbosity.Index = 4;
			this.menuItemVerbosity.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemVerbositySilent,
									this.menuItemVerbosityLow,
									this.menuItemVerbosityMedium,
									this.menuItemVerbosityHigh});
			this.menuItemVerbosity.Text = "Engine verbosity";
			// 
			// menuItemVerbosityLow
			// 
			this.menuItemVerbosityLow.Index = 1;
			this.menuItemVerbosityLow.RadioCheck = true;
			this.menuItemVerbosityLow.Text = "Low";
			this.menuItemVerbosityLow.Click += new System.EventHandler(this.MenuItemVerbosityLowClick);
			// 
			// menuItemVerbosityMedium
			// 
			this.menuItemVerbosityMedium.Index = 2;
			this.menuItemVerbosityMedium.RadioCheck = true;
			this.menuItemVerbosityMedium.Text = "Medium";
			this.menuItemVerbosityMedium.Click += new System.EventHandler(this.MenuItemVerbosityMediumClick);
			// 
			// menuItemVerbosityHigh
			// 
			this.menuItemVerbosityHigh.Index = 3;
			this.menuItemVerbosityHigh.RadioCheck = true;
			this.menuItemVerbosityHigh.Text = "High";
			this.menuItemVerbosityHigh.Click += new System.EventHandler(this.MenuItemVerbosityHighClick);
			// 
			// menuItemSystem
			// 
			this.menuItemSystem.Index = 3;
			this.menuItemSystem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.menuItemListAssemblies});
			this.menuItemSystem.Text = "System";
			// 
			// menuItemListAssemblies
			// 
			this.menuItemListAssemblies.Index = 0;
			this.menuItemListAssemblies.Text = "List assemblies";
			this.menuItemListAssemblies.Click += new System.EventHandler(this.MenuItemListAssembliesClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "RuleML files|*.ruleml|Visio files|*.vdx|Human Readable files|*.hrf";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "RuleML files|*.ruleml|Human Readable files|*.hrf";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 542);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(816, 19);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "Loading...";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(816, 561);
			this.Controls.Add(this.textBoxConsole);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "NxBRE - Inference Engine Console";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainFormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.ComponentModel.IContainer components;
		#endregion
		void MenuItemExitClick(object sender, System.EventArgs e)
		{
			Action(ActionType.Exit);
		}
		
		void MainFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Action(ActionType.Exit);
		}
		
		void MenuItemLoadRuleBaseClick(object sender, System.EventArgs e)
		{
			Action(ActionType.LoadRuleBase);
		}
		
		void MenuItemConsoleClearClick(object sender, System.EventArgs e)
		{
			Action(ActionType.ClearConsole);
		}
		
		void MenuItemStatusClick(object sender, System.EventArgs e)
		{
			Action(ActionType.DumpStatus);
		}
		
		void MenuItemShowDeductionsClick(object sender, System.EventArgs e)
		{
			menuItemShowDeductions.Checked = !menuItemShowDeductions.Checked;
		}
		
		void MenuItemVerbositySilentClick(object sender, System.EventArgs e)
		{
			Action(ActionType.VerbositySilent);
		}
		
		void MenuItemVerbosityLowClick(object sender, System.EventArgs e)
		{
			Action(ActionType.VerbosityLow);
		}
		
		void MenuItemVerbosityMediumClick(object sender, System.EventArgs e)
		{
			Action(ActionType.VerbosityMedium);
		}
		
		void MenuItemVerbosityHighClick(object sender, System.EventArgs e)
		{
			Action(ActionType.VerbosityHigh);
		}
		
		void MenuItemLoadFactsClick(object sender, System.EventArgs e)
		{
			Action(ActionType.LoadFacts);
		}
		
		void MenuItemProcessClick(object sender, System.EventArgs e)
		{
			Action(ActionType.Process);
		}
		
		void MenuItemFactsClick(object sender, System.EventArgs e)
		{
			Action(ActionType.DumpFacts);
		}
		
		void MenuItemRunQueryClick(object sender, System.EventArgs e)
		{
			Action(ActionType.RunQuery);
		}

        void MenuItemRunCustomQueryClick(object sender, System.EventArgs e)
        {
            Action(ActionType.RunCustomQuery);
        }
		
		void MenuItemWMGlobalClick(object sender, System.EventArgs e)
		{
			Action(ActionType.WMGlobal);
		}
		
		void MenuItemWMIsolatedClick(object sender, System.EventArgs e)
		{
			Action(ActionType.WMIsolated);
		}
		
		void MenuItemWMIsolatedEmptyClick(object sender, System.EventArgs e)
		{
			Action(ActionType.WMIsolatedEmpty);
		}
		
		void MenuItemWMCommitClick(object sender, System.EventArgs e)
		{
			Action(ActionType.WMCommit);
		}
		
		void MenuItemListAssembliesClick(object sender, System.EventArgs e)
		{
			Action(ActionType.ListAssemblies);
		}
		
		void MenuItemSaveRuleBaseClick(object sender, System.EventArgs e)
		{
			Action(ActionType.SaveRuleBase);
		}
		
		void MenuItemSaveFactsClick(object sender, System.EventArgs e)
		{
			Action(ActionType.SaveFacts);
		}
		
		void MenuItemAssertClick(object sender, System.EventArgs e)
		{
			Action(ActionType.AssertFact);
		}
		
		void MenuItemShowDeletionsClick(object sender, System.EventArgs e)
		{
			menuItemShowDeletions.Checked = !menuItemShowDeletions.Checked;
		}
		
		void MenuItemRetractClick(object sender, System.EventArgs e)
		{
			Action(ActionType.RetractFact);
		}
		
		void MenuItemShowModificationsClick(object sender, System.EventArgs e)
		{
			menuItemShowModifications.Checked = !menuItemShowModifications .Checked;
		}

	}
	
}
