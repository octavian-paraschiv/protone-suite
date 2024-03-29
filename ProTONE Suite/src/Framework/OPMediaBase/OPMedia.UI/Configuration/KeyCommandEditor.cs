using OPMedia.Core;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.Shortcuts;
using OPMedia.UI.Themes;
using System;
using System.Windows.Forms;


namespace OPMedia.UI.Configuration
{
    public partial class KeyCommandEditor : ToolForm
    {
        OPMShortcut _cmd = OPMShortcut.CmdOutOfRange;
        bool _primary = true;

        public KeyCommandEditor(OPMShortcut cmd, bool primary) :
            base("TXT_EDIT_KEY")
        {
            InitializeComponent();
            _cmd = cmd;
            _primary = primary;

            this.TopMost = true;

            this.Load += new EventHandler(OnLoad);
            this.Shown += new EventHandler(KeyCommandEditor_Shown);
            this.KeyUp += new KeyEventHandler(KeyCommandEditor_KeyUp);

            Application.DoEvents();
        }

        void OnLoad(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(OnLoad), new object[] { sender, e });
                return;
            }

            Application.DoEvents();

            KeysConverter kc = new KeysConverter();
            string key = _primary ?
                kc.ConvertToInvariantString(ShortcutMapper.KeyCommands[(int)_cmd].KeyData) :
                kc.ConvertToInvariantString(ShortcutMapper.AltKeyCommands[(int)_cmd].KeyData);

            lblDesc.Text = Translator.Translate("TXT_EDITKEYDESC",
                _cmd.ToString().Replace("Cmd", string.Empty), key);
        }

        void KeyCommandEditor_Shown(object sender, EventArgs e)
        {
            User32.BringWindowToTop(this.Handle);

            if (_cmd == OPMShortcut.CmdOutOfRange)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        void KeyCommandEditor_KeyUp(object sender, KeyEventArgs e)
        {
            KeyEventArgs args = GetKeyArgs(e);
            if (args != null &&
                (
                    (args.KeyData != Keys.Enter &&
                    args.KeyData != Keys.Escape &&
                    args.KeyData != Keys.Tab &&
                    args.KeyCode != Keys.PrintScreen &&
                    args.KeyData != Keys.F1)
                    )
                    ||
                    (
                        (args.Control && (args.KeyData == Keys.F2 || args.KeyData == Keys.Left || args.KeyData == Keys.Right || args.KeyData == Keys.Up || args.KeyData == Keys.Down))
                    )
                )
            {
                if (VerifyShortcut(args))
                {
                    if (_primary)
                    {
                        ShortcutMapper.KeyCommands[(int)_cmd] = args;
                    }
                    else
                    {
                        ShortcutMapper.AltKeyCommands[(int)_cmd] = args;
                    }

                    EventDispatch.DispatchEvent(EventNames.KeymapChanged);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private bool VerifyShortcut(KeyEventArgs args)
        {
            OPMShortcut cmd = ShortcutMapper.MapCommand(args.KeyData);
            if (cmd == OPMShortcut.CmdOutOfRange)
            {
                // Key combination currently not assigned so it's OK to use it.
                return true;
            }

            if (cmd == _cmd)
            {
                // Same command => ok to reassign.
                return true;
            }

            string cmdOld = cmd.ToString().Replace("Cmd", string.Empty);
            string cmdNew = _cmd.ToString().Replace("Cmd", string.Empty);

            KeysConverter kc = new KeysConverter();
            string key = kc.ConvertToInvariantString(args.KeyData);

            if ((args.KeyData == Keys.Space && ShortcutMapper.IsPlayer) ||
                !ShortcutMapper.IsConfigurableShortcut(cmd))
            {
                // Key combination currently assigned 
                // to a non-configurable command (e.g. F1 = help)
                MessageDisplay.Show(Translator.Translate("TXT_DUP_SHORTCUT_FIXED", key, cmdOld),
                    Translator.Translate("TXT_DUPPLICATE_SHORTCUT"),
                    MessageBoxIcon.Warning);
                return false;
            }

            if (MessageDisplay.Query(Translator.Translate("TXT_DUP_SHORTCUT_CONFIRM", key, cmdOld, cmdNew),
                    Translator.Translate("TXT_DUPPLICATE_SHORTCUT"),
                    MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
            {
                // Key combination already assigned and the user did not want to change it use it.
                return false;
            }

            // Unassign old shortcut
            if (ShortcutMapper.KeyCommands[(int)cmd].KeyData == args.KeyData)
            {
                // Was used for primary shortcut
                ShortcutMapper.KeyCommands[(int)cmd] = new KeyEventArgs(Keys.None);
            }
            else if (ShortcutMapper.AltKeyCommands[(int)cmd].KeyData == args.KeyData)
            {
                // Was used for alternate shortcut
                ShortcutMapper.AltKeyCommands[(int)cmd] = new KeyEventArgs(Keys.None);
            }

            return true;
        }

        private KeyEventArgs GetKeyArgs(KeyEventArgs e)
        {
            KeyEventArgs retVal = null;
            if (e.Modifiers == Keys.None)
            {
                if (e.KeyData != Keys.Menu &&
                e.KeyData != Keys.ControlKey &&
                e.KeyData != Keys.ShiftKey)
                {
                    retVal = new KeyEventArgs(e.KeyData);
                }
            }
            else
            {
                if (e.KeyData != e.Modifiers)
                    retVal = new KeyEventArgs(e.KeyData);
            }

            return retVal;
        }


    }
}