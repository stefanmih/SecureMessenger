﻿using MediatR;
using SecureMessenger.Application.Auth.Commands;
using SecureMessenger.Application.Messages.Commands;
using SecureMessenger.Application.Messages.Queries;
using SecureMessenger.Infrastructure.Services;
using SecureMessenger.Workers;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using FluentValidation.Results;
using FluentValidation;

namespace SecureMessenger.UI
{
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        private readonly IMediator _mediator;
        private int? _loggedInUserId = null;
        private readonly AesEncryptionService _aesEncryptionService;

        public MainWindow(IMediator mediator, IHost host)
        {
            InitializeComponent();
            //AllocConsole(); // za debug

            var stream = Console.OpenStandardOutput();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            Console.SetOut(writer);
            Console.WriteLine("MainWindow initialized.");

            _mediator = mediator;
            _aesEncryptionService = new AesEncryptionService();

            var messageWorker = new MessageWorker(_mediator, LoadMessages);
            messageWorker.StartAsync(CancellationToken.None).ConfigureAwait(false);
        }

        public async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var command = new RegisterUserCommand
            {
                Username = RegisterUsernameTextBox.Text,
                Password = RegisterPasswordBox.Password,
                PublicIp = RegisterPublicIpTextBox.Text
            };

            var userId = await _mediator.Send(command);

            if (userId > 0)
            {
                System.Windows.MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RegistrationPanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
            }
            else
            {
                System.Windows.MessageBox.Show("Registration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var command = new UserLoginCommand
            {
                Username = UsernameTextBox.Text,
                Password = PasswordBox.Password
            };

            var userId = await _mediator.Send(command);

            if (userId != null)
            {
                _loggedInUserId = userId;
                ChatPanel.Visibility = Visibility.Visible;
                LoginPanel.Visibility = Visibility.Collapsed;
                RegistrationPanel.Visibility = Visibility.Collapsed;

                LoggedInUserIdTextBlock.Text = $"Your User ID: {_loggedInUserId}";

                System.Windows.MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int receiverId = int.Parse(ReceiverIdTextBox.Text);
                string encryptedMessage = _aesEncryptionService.Encrypt(MessageTextBox.Text, EncryptionKeyBox.Password);

                var command = new SendMessageCommand
                {
                    SenderId = _loggedInUserId.Value,
                    ReceiverId = receiverId,
                    PlainContent = encryptedMessage,
                    EncryptionKey = EncryptionKeyBox.Password
                };

                var validator = new SendMessageCommandValidator();
                ValidationResult validationResult = await validator.ValidateAsync(command);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var messageId = await _mediator.Send(command);

                System.Windows.MessageBox.Show($"Message sent with ID: {messageId}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadMessages()
        {
            if (_loggedInUserId == null) return;

            var query = new GetMessagesQuery { ReceiverId = _loggedInUserId.Value };
            var messages = await _mediator.Send(query);
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                MessagesListBox.Items.Clear();
                foreach (var message in messages)
                {
                    var userQuery = new GetUserNameQuery { UserId = message.SenderId };
                    var username = await _mediator.Send(userQuery);
                    string decryptedMessage = _aesEncryptionService.Decrypt(message.EncryptedContent, EncryptionKeyBox.Password);
                    MessagesListBox.Items.Add($"Username: {username}\n\t{decryptedMessage}");
                }
            });
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                tb.Text = "";
            }
        }
    }
}
