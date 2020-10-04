﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using BongoCat.DJMAX.Common;
using BongoCat.DJMAX.Common.Input;
using BongoCat.DJMAX.Setting.Controls;
using BongoCat.DJMAX.Setting.Data;
using BongoCat.DJMAX.Setting.Input;
using BongoCat.DJMAX.Setting.Models;

namespace BongoCat.DJMAX.Setting
{
    public partial class SettingWindow
    {
        private const string actionTrack1 = "TRACK 1";
        private const string actionTrack2 = "TRACK 2";
        private const string actionTrack3 = "TRACK 3";
        private const string actionTrack4 = "TRACK 4";
        private const string actionTrack5 = "TRACK 5";
        private const string actionTrack6 = "TRACK 6";
        private const string actionTrackL = "TRACK L";
        private const string actionTrackR = "TRACK R";
        private const string actionLeftSideTrack = "L SIDE TRACK";
        private const string actionRightSideTrack = "R SIDE TRACK";

        private readonly SettingWindowModel _model;

        private InputKeysModel[][] _inputModels;
        private ITransaction _configurationTransaction;

        private IInputProvider _inputProvider;
        private InputKeysModel _targetInputModel;
        private InputOverlay _inputOverlay;
        private bool _skipSaveHint;

        public SettingWindow(Configuration configuration)
        {
            InitializeComponent();
            InitializeConfiguration(configuration);

            _model = new SettingWindowModel
            {
                SelectedButtons = Buttons._4,
                Keys = _inputModels[0],
                CancelCommand = new ActionCommand(OnCancelCommandExecuted),
                SaveCommand = new ActionCommand(OnSaveCommandExecuted)
            };

            _model.PropertyChanged += ModelOnPropertyChanged;
            DataContext = _model;

            radioBtn4.IsChecked = true;
            radioBtn4.Checked += (s, e) => _model.SelectedButtons = Buttons._4;
            radioBtn5.Checked += (s, e) => _model.SelectedButtons = Buttons._5;
            radioBtn6.Checked += (s, e) => _model.SelectedButtons = Buttons._6;
            radioBtn8.Checked += (s, e) => _model.SelectedButtons = Buttons._8;
        }

        private void InitializeConfiguration(Configuration configuration)
        {
            var transactions = new List<PropertyTransaction<InputKeys>>(32)
            {
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.Track1)),
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.Track2)),
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.Track3)),
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.Track4)),
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.LeftSideTrack)),
                new PropertyTransaction<InputKeys>(configuration.Input4, nameof(InputSetting4.RightSideTrack)),

                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track1)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track2)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track3)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track4)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track5)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.Track6)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.LeftSideTrack)),
                new PropertyTransaction<InputKeys>(configuration.Input5, nameof(InputSetting5.RightSideTrack)),

                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track1)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track2)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track3)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track4)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track5)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.Track6)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.LeftSideTrack)),
                new PropertyTransaction<InputKeys>(configuration.Input6, nameof(InputSetting6.RightSideTrack)),

                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track1)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track2)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track3)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track4)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track5)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.Track6)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.TrackLeft)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.TrackRight)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.LeftSideTrack)),
                new PropertyTransaction<InputKeys>(configuration.Input8, nameof(InputSetting8.RightSideTrack))
            };

            _configurationTransaction = new TransactionGroup(transactions);

            _inputModels = new[]
            {
                new[]
                {
                    new InputKeysModel(actionTrack1, transactions[0], null),
                    new InputKeysModel(actionTrack2, transactions[1], null),
                    new InputKeysModel(actionTrack3, transactions[2], null),
                    new InputKeysModel(actionTrack4, transactions[3], null),
                    new InputKeysModel(actionLeftSideTrack, transactions[4], null),
                    new InputKeysModel(actionRightSideTrack, transactions[5], null)
                },
                new[]
                {
                    new InputKeysModel(actionTrack1, transactions[6], null),
                    new InputKeysModel(actionTrack2, transactions[7], null),
                    new InputKeysModel(actionTrack3, transactions[8], null),
                    new InputKeysModel(actionTrack4, transactions[9], null),
                    new InputKeysModel(actionTrack5, transactions[10], null),
                    new InputKeysModel(actionTrack6, transactions[11], null),
                    new InputKeysModel(actionLeftSideTrack, transactions[12], null),
                    new InputKeysModel(actionRightSideTrack, transactions[13], null)
                },
                new[]
                {
                    new InputKeysModel(actionTrack1, transactions[14], null),
                    new InputKeysModel(actionTrack2, transactions[15], null),
                    new InputKeysModel(actionTrack3, transactions[16], null),
                    new InputKeysModel(actionTrack4, transactions[17], null),
                    new InputKeysModel(actionTrack5, transactions[18], null),
                    new InputKeysModel(actionTrack6, transactions[19], null),
                    new InputKeysModel(actionLeftSideTrack, transactions[20], null),
                    new InputKeysModel(actionRightSideTrack, transactions[21], null)
                },
                new[]
                {
                    new InputKeysModel(actionTrack1, transactions[22], null),
                    new InputKeysModel(actionTrack2, transactions[23], null),
                    new InputKeysModel(actionTrack3, transactions[24], null),
                    new InputKeysModel(actionTrack4, transactions[25], null),
                    new InputKeysModel(actionTrack5, transactions[26], null),
                    new InputKeysModel(actionTrack6, transactions[27], null),
                    new InputKeysModel(actionTrackL, transactions[28], null),
                    new InputKeysModel(actionTrackR, transactions[29], null),
                    new InputKeysModel(actionLeftSideTrack, transactions[30], null),
                    new InputKeysModel(actionRightSideTrack, transactions[31], null)
                }
            };

            var keyboardCommand = new ActionCommand<InputKeysModel>(OnKeyboardCommandExecuted);
            var controllerCommand = new ActionCommand<InputKeysModel>(OnControllerCommandExecuted);

            foreach (var model in _inputModels.SelectMany(x => x))
            {
                model.KeyboardCommand = keyboardCommand;
                model.ControllerCommand = controllerCommand;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = _inputProvider != null;
            base.OnPreviewKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            StopInputRecording();

            if (!_skipSaveHint && _configurationTransaction.IsChanged)
            {
                if (MessageBox.Show("변경된 사항을 저장하시겠습니까?", Title, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    Save();
                }
            }

            base.OnClosing(e);
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SettingWindowModel.SelectedButtons))
                return;

            switch (_model.SelectedButtons)
            {
                case Buttons._4:
                    _model.Keys = _inputModels[0];
                    break;

                case Buttons._5:
                    _model.Keys = _inputModels[1];
                    break;

                case Buttons._6:
                    _model.Keys = _inputModels[2];
                    break;

                case Buttons._8:
                    _model.Keys = _inputModels[3];
                    break;
            }
        }

        private void OnKeyboardCommandExecuted(InputKeysModel model)
        {
            if (_inputProvider != null)
                return;

            StartInputRecording(new KeyboardProvider(), model);
        }

        private void OnControllerCommandExecuted(InputKeysModel model)
        {
            MessageBox.Show("Coming soon", Title, MessageBoxButton.OK, MessageBoxImage.Warning);

            // TODO: implement
            // StartInputRecording(new ControllerProvider(), model);
        }

        private void OnCancelCommandExecuted()
        {
            _skipSaveHint = true;

            if (ComponentDispatcher.IsThreadModal)
                DialogResult = false;

            Close();
        }

        private void OnSaveCommandExecuted()
        {
            Save();
            Close();
        }

        private void Save()
        {
            _configurationTransaction.Commit();
            _skipSaveHint = true;

            if (ComponentDispatcher.IsThreadModal)
                DialogResult = true;
        }

        private void StartInputRecording(IInputProvider provider, InputKeysModel target)
        {
            _targetInputModel = target;
            _model.Overlay = _inputOverlay ??= new InputOverlay();

            _inputProvider = provider;
            _inputProvider.KeyDown += OnInputProviderKeyDown;
        }

        private void StopInputRecording()
        {
            if (_inputProvider == null)
                return;

            _inputProvider.KeyDown -= OnInputProviderKeyDown;
            _inputProvider.Dispose();
            _inputProvider = null;

            _targetInputModel = null;
            _model.Overlay = null;
        }

        private void OnInputProviderKeyDown(object sender, InputKeys keys)
        {
            if (keys == InputKeys.None || !IsActive)
                return;

            if (keys != InputKeys.Escape)
            {
                if (keys.HasFlag(InputKeys.Keyboard))
                {
                    _targetInputModel.Keyboard.Value = keys;
                }
                else if (keys.HasFlag(InputKeys.Controller))
                {
                    _targetInputModel.Controller.Value = keys;
                }
                else
                {
                    return;
                }
            }

            StopInputRecording();
        }
    }
}
