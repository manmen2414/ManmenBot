using Discord;
using Discord.WebSocket;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public enum blocks
{
    石,
    閃緑岩,
    磨かれた閃緑岩,
    花崗岩,
    磨かれた花崗岩,
    丸石,
    苔石,
    黒曜石,
    石レンガ,
    苔石レンガ,
    ひび割れた石レンガ,
    模様入りの石レンガ,
    滑らかな石,
    滑らかな石英,
    土,
    粗い土,
    ポドソル,
    菌糸,
    アカシアの木材,
    オークの木材,
    ジャングルの木材,
    シラカバの木材,
    ダークオークの木材,
    マツの木材,
    オークの原木,
    ジャングルの原木,
    シラカバの原木,
    マツの原木,
    アカシアの木材のハーフブロック,
    オークの木材のハーフブロック,
    ジャングルの木材のハーフブロック,
    シラカバの木材のハーフブロック,
    ダークオークの木材のハーフブロック,
    マツの木材のハーフブロック,
    マツの木材の階段,
    シラカバの木材の階段,
    ジャングルの木材の階段,
    アカシアの原木,
    ダークオークの原木,
    アカシアの木材の階段,
    ダークオークの木材の階段,
    オークの木材の階段,
    ダークオークの木,
    アカシアの木,
    ジャングルの木,
    シラカバの木,
    マツの木,
    オークの木,
    樹皮を剥いだダークオークの木,
    樹皮を剥いだアカシアの木,
    樹皮を剥いだジャングルの木,
    樹皮を剥いだシラカバの木,
    樹皮を剥いだマツの木,
    樹皮を剥いだオークの木,
    樹皮を剥いだオークの原木,
    樹皮を剥いだマツの原木,
    樹皮を剥いだシラカバの原木,
    樹皮を剥いだジャングルの原木,
    樹皮を剥いだアカシアの原木,
    樹皮を剥いだダークオークの原木
}
public enum omikuji
{
    大凶,
    凶,
    未吉,
    吉,
    小吉,
    中吉,
    大吉
}

namespace Main
{

    class Program
    {
        public static SocketGuild scktgid = null;
        long me = 0;
        public static bool[] okcommand = { true, true, true, true, true, true, true, true, true };
        ulong[] ulgkakko = { 0, 0, 0 };
        public static DiscordSocketClient _client;
        public static DiscordSocketClient cl2 = Manmenbot.Settngs._client;
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
        public Program()
        {
            Manmenbot.LoopClass.LoopMain();
            _client = new DiscordSocketClient();
            _client.Log += LogAsync;
            _client.Ready += onReady;
            _client.MessageReceived += OnMessage;
            _client.Disconnected += dis;
        }

        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Manmenbot.Settngs.token);
            await _client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            writeline($"{DateTime.Now:yyyy/MM/dd} {log}");
            return Task.CompletedTask;
        }

        public async Task onReady()
        {

            await _client.SetStatusAsync(UserStatus.Online);
            okcommand = Manmenbot.Settngs.commandswich;
            Console.Write($"\r\n[{_client.CurrentUser.Username}]botは接続されました");
            if (_client.GetChannel(Manmenbot.Settngs.notificationNotTextChannel) == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\r\nエラー:notificationChannelがnullです(入力値が間違っている可能性があります)");
                Console.ResetColor();
            }
            else
            {
                if (Manmenbot.Settngs.notificationcheck)
                {
                    var embed = new EmbedBuilder() // Embedのビルダーを宣言……面倒なのでBuild()まで含めてしまいます。
                .WithTitle("**起動**") // 起動
                .WithDescription($"{_client.CurrentUser.Username}が起動しました。") // (bot名)が起動しました。
                .WithColor(Color.Green) //サイドの色を設定
                .WithCurrentTimestamp() // 実行日時を追加
                .Build(); // ビルド
                    await (_client.GetChannel(Manmenbot.Settngs.notificationNotTextChannel) as SocketTextChannel).SendMessageAsync(null, false, embed);
                }
            }
        }

        private async Task OnMessage(SocketMessage message)
        {
            if (IsDMmessage(message))//DMで送ってる場合ログ表記変更
            {
                writeline($"{DateTime.Now} {message.Author.Username}({message.Channel.ToString()})|=|{message.ToString()}({message.Author.Id})");
            }
            else
            {
                scktgid = (message.Channel as SocketGuildChannel).Guild;
                writeline($"{DateTime.Now} {message.Author.Username}({scktgid.Name})|=|{message}({message.Author.Id})");
            }

            if (message.Author.Id == _client.CurrentUser.Id || message.Author.Id == ulgkakko[0] || message.Author.Id == ulgkakko[1] || message.Author.Id == ulgkakko[2])
            { //ブロックユーザーである・自分のメッセージだったら終了
                return;
            }
            else
            {
                if (message.ToString().StartsWith("m?"))//こっからコマンド探す
                {
                    try
                    {
                        switch (message.ToString())//case "この中にあるテキストがコマンド"
                        {
                            case "m?help":
                                await Manmenbot.Commands.Help(message);//System.csにあります
                                break;
                            case "m?こんにちは":
                                await Manmenbot.Commands.Konnitiha(message);
                                break;
                            case "m?サイコロ":
                                await Manmenbot.Commands.Saikoro(message);
                                break;
                            case "m?ブロック":
                                await Manmenbot.Commands.Block(message);
                                break;
                            case "m?おみくじ":
                                await Manmenbot.Commands.Omikuji(message);
                                break;
                            case "m?時間":
                                await Manmenbot.Commands.Time(message);
                                break;
                            case "m?終了":
                                await Manmenbot.Commands.End(message);
                                break;
                            case "m?おやすみ":
                                await Manmenbot.Commands.Joke_Oyasumi(message);
                                break;
                            default:
                                switch (message.ToString().Split(' ')[0])
                                {
                                    case "m?計算":
                                        await Manmenbot.Commands.Cal(message);
                                        break;
                                    case "m?ランダム":
                                        await Manmenbot.Commands.Randam(message);
                                        break;
                                    case "m?じゃんけん":
                                        await Manmenbot.Commands.Janken(message);
                                        break;
                                    case "m?クイックロール":
                                        await Manmenbot.Commands.QuickRole(message);
                                        break;
                                    default:
                                        if (message.ToString()[1] == '+' || message.ToString()[1] == '-' || message.ToString()[1] == '=')
                                            me = await Manmenbot.Commands.Caunter(message, me); //特殊コマンド
                                        break;
                                }
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        await message.Channel.SendMessageAsync("**超エラー！**\r\n引数が足りません。");
                    }
                    catch (OverflowException)
                    {
                        await message.Channel.SendMessageAsync("**超エラー！**\r\n数が極端すぎます。");
                    }
                }
            }

        }
        public void writeline(string write)
        {
            Console.Write("\r\n" + write);
            try
            {
                StreamWriter writer = new StreamWriter($"{Manmenbot.Settngs.path}manmen4.log", true, Encoding.GetEncoding("UTF-8"));
                writer.WriteLine(write);
                writer.Close();
            }
            catch (FileLoadException)
            {
                Console.Write("\r\nエラー:writer.Close();が実行されていません(たまに起こる原因不明エラー)");
            }
        }
        private Task dis(Exception exc)
        {
            Console.Write("\r\n!警告:botは切断されました \r\n原因:" + exc);
            return Task.CompletedTask;
        }
        public static bool IsDMmessage(SocketMessage message)
        {
            if (message.Channel.ToString().StartsWith('@'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
