using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Task_002
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cancelToken = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancelToken?.Cancel();

            Dispatcher.Invoke(() =>
            {
                TextBox.Text = "Cancel token has been activated!";
            });
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _cancelToken = new CancellationTokenSource();

            await DbConnectAsync(_cancelToken.Token);

            for (int i = 1; i < 100; i++)
            {
                try
                {
                    await OperationAsync(i, _cancelToken.Token);
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            _cancelToken = null!;
            TextBox.Text = "Completed!";
        }

        private async Task DbConnectAsync(CancellationToken token)
        {
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        TextBox.Text = "Connecting to DB. Please wait!";
                    });
                    Thread.Sleep(3000);
                }
                , token);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task OperationAsync(int number, CancellationToken token)
        {
            try
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(100);
                    Dispatcher.Invoke(() =>
                    {
                        TextBox.Text = $"File #{number} is Done!";
                    });
                }
                , token);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
