global using Application.Service;
global using DTOs;
using Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        View view = new();
        await view.Start();
    }
}