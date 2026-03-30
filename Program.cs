using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Point d'entrée principal pour le bot Discord Nothing.
/// </summary>
public class Program
{
    private readonly DiscordSocketClient _client;
    private readonly string _token;

    /// <summary>
    /// Initialise une nouvelle instance de la classe Program.
    /// </summary>
    public Program()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
        });
        
        _token = Environment.GetEnvironmentVariable("DISCORD_TOKEN") ?? throw new InvalidOperationException("La variable d'environnement DISCORD_TOKEN est manquante.");
        _client.MessageReceived += OnMessageReceivedAsync;
    }

    /// <summary>
    /// Méthode d'exécution principale asynchrone.
    /// </summary>
    /// <param name="args">Arguments de ligne de commande.</param>
    /// <returns>Une tâche asynchrone.</returns>
    public static Task Main(string[] args)
    {
        return new Program().RunAsync();
    }

    /// <summary>
    /// Démarre le client Discord et bloque le thread principal.
    /// </summary>
    /// <returns>Une tâche asynchrone.</returns>
    public async Task RunAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    /// <summary>
    /// Gère l'événement de réception d'un message.
    /// Vérifie si le bot est mentionné et répond en conséquence.
    /// </summary>
    /// <param name="message">Le message reçu depuis Discord.</param>
    /// <returns>Une tâche asynchrone.</returns>
    private async Task OnMessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot)
        {
            return;
        }

        if (message.MentionedUsers.Any(u => u.Id == _client.CurrentUser.Id))
        {

        // if (new Random().Next(0, 100) < 40)
        // {
        //     return;
        // }
        string response = GetRandomResponse();
        await message.Channel.SendMessageAsync(response);
        }
    }

    /// <summary>
    /// Sélectionne une réponse aléatoire en fonction des probabilités relatives (poids) définies.
    /// </summary>
    /// <returns>La chaîne de caractères sélectionnée.</returns>
    private string GetRandomResponse()
    {
        var responses = new Dictionary<string, int>
        {
            { "Je t’ai manqué ?", 40 },
            { "Bienvenue chez toi", 30 },
            { "J’ai froids", 40 },
            { "J’ai faim", 40 },
            { "Tu sais quoi faire…", 30 },
            { "Je me sens seul", 20 },
            { "Enfin…", 10 },
            { "N’a tu jamais abandonner un ami ?", 4 },
            { "Toi ?", 20 },
            { "mh ?", 30 }

        };

        int totalWeight = responses.Sum(r => r.Value);
        int randomValue = new Random().Next(0, totalWeight);
        int currentWeight = 0;

        foreach (var response in responses)
        {
            currentWeight += response.Value;
            if (randomValue < currentWeight)
            {
                return response.Key;
            }
        }

        return "...";
    }
}