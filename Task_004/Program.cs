await OperationAsync();

static async Task OperationAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine("task 1 done!");
    });
    await Task.Run(() =>
    {
        Console.WriteLine("task 2 done!");
    });
    await Task.Run(() =>
    {
        Console.WriteLine("task 3 done!");
    });
}