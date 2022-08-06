using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MyCustomTimer = System.Timers;

namespace Task_002
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cancelToken = null;
        private MyCustomTimer.Timer _timer = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _cancelToken?.Cancel();

            Dispatcher.Invoke(() =>
            {
                TextBox.Text = "Cancel token has been activated!";
            });
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _cancelToken = new CancellationTokenSource();

            _timer.Start();

            await DbConnectAsync(_cancelToken.Token);

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        await OperationAsync(i, _cancelToken.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        TextBox.Text = ex.ToString();
                        throw;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                TextBox.Text = ex.ToString();
                throw;
            }
            catch (Exception ex)
            {
                TextBox.Text = ex.ToString();
                throw;
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
                Dispatcher.Invoke(() =>
                {
                    TextBox.Text = ex.ToString();
                });
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
                Dispatcher.Invoke(() =>
                {
                    TextBox.Text = ex.ToString();
                });
                throw;
            }
        }
    }
}
